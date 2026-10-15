using SwiftlyS2.Core.SchemaDefinitions;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Memory;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private delegate nint CCSPlayerInventoryGetItemInLoadout( nint pInventory, int team, int slot );

    private static IUnmanagedFunction<CCSPlayerInventoryGetItemInLoadout> GetItemInLoadoutFunction()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var getItemInLoadoutPtr = _core.GameData.GetSignature("CCSPlayerInventory::GetItemInLoadout");
        if (getItemInLoadoutPtr == 0)
            throw new InvalidOperationException("Failed to find signature for CCSPlayerInventory::GetItemInLoadout.");

        return _core.Memory.GetUnmanagedFunctionByAddress<CCSPlayerInventoryGetItemInLoadout>(getItemInLoadoutPtr);
    }

    /// <summary>
    /// The CCSPlayerInventory is embedded in CCSPlayerController_InventoryServices at this offset.
    /// </summary>
    private static nint InventoryOffset()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (!_core.GameData.TryGetOffset("CCSPlayerController_InventoryServices::m_pInventory", out var offset))
            throw new InvalidOperationException("Failed to find offset for CCSPlayerController_InventoryServices::m_pInventory.");

        return offset;
    }

    /// <summary>
    /// Looks up a loadout slot through the original function, so GetItemInLoadout hooks don't run
    /// (and a hook can read other slots without calling itself).
    /// </summary>
    internal static nint GetItemInLoadoutOriginal( CCSPlayerController_InventoryServices inventoryServices, Team team, loadout_slot_t slot )
    {
        var pInventory = inventoryServices.Address + InventoryOffset();
        return GetItemInLoadoutFunction().CallOriginal(pInventory, (int)team, (int)slot);
    }

    internal static Guid HookGetItemInLoadout()
    {
        var getItemInLoadoutFunction = GetItemInLoadoutFunction();
        var inventoryOffset = InventoryOffset();

        return getItemInLoadoutFunction.AddHook(next =>
        {
            return ( pInventory, team, slot ) =>
            {
                var player = FindInventoryOwner(pInventory, inventoryOffset);
                if (player == null) return next()(pInventory, team, slot);

                var preCtx = new GetItemInLoadoutPreContext {
                    Params = new GetItemInLoadoutParams {
                        Player = player,
                        Team = (Team)team,
                        Slot = (loadout_slot_t)slot
                    }
                };

                InvokeGetItemInLoadoutPre(ref preCtx);
                if ((preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal) && preCtx.IsReturnSet)
                {
                    return preCtx.Return?.Address ?? 0;
                }

                var result = next()(pInventory, team, slot);

                var dummyEconItemView = _econItemViewPool.Rent();
                dummyEconItemView.DangerousSetHandle(result);

                var postCtx = new GetItemInLoadoutPostContext { Params = preCtx.Params, Return = result == 0 ? null : dummyEconItemView };

                InvokeGetItemInLoadoutPost(ref postCtx);

                var postResult = postCtx.Return?.Address ?? 0;
                _econItemViewPool.Return(dummyEconItemView);

                return postResult;
            };
        });
    }

    internal static Guid UnhookGetItemInLoadout()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.GetItemInLoadout, out var hookId))
        {
            GetItemInLoadoutFunction().RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    /// <summary>
    /// Matches the inventory against each player's instead of reading through the pointer, so an
    /// inventory that isn't a player's is never dereferenced.
    /// </summary>
    private static IPlayer? FindInventoryOwner( nint pInventory, nint inventoryOffset )
    {
        foreach (var player in _core!.PlayerManager.GetAllPlayers())
        {
            if (!player.IsValid) continue;

            var inventoryServices = player.Controller?.InventoryServices;
            if (inventoryServices != null && inventoryServices.IsValid && inventoryServices.Address + inventoryOffset == pInventory)
                return player;
        }

        return null;
    }

    internal static void InvokeGetItemInLoadoutPre( ref GetItemInLoadoutPreContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeGetItemInLoadoutPre(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }

    internal static void InvokeGetItemInLoadoutPost( ref GetItemInLoadoutPostContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeGetItemInLoadoutPost(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }
}
