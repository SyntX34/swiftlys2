using SwiftlyS2.Core.EntitySystem;
using SwiftlyS2.Core.Events;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private unsafe delegate void CBaseEntityTakeDamage( nint entity, CTakeDamageInfo* info, CTakeDamageResult* damageResult );

    internal static unsafe Guid HookTakeDamage()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var ptr = _core.GameData.GetSignature("CBaseEntity::TakeDamage");
        if (ptr == 0)
            throw new InvalidOperationException("Failed to find signature for CBaseEntity::TakeDamage.");

        var unmanagedFunction = _core.Memory.GetUnmanagedFunctionByAddress<CBaseEntityTakeDamage>(ptr);
        return unmanagedFunction.AddHook(next =>
        {
            return ( entity, info, damageResult ) =>
            {
                if (hookListeners.TryGetValue(HookListener.TakeDamage, out var count) && count == 1 && !EventPublisher.ListensToTakeDamage)
                {
                    next()(entity, info, damageResult);
                    return;
                }

                var baseEntity = EntityManager.GetEntityByAddress(entity) as CBaseEntity
                    ?? _core.Memory.ToSchemaClass<CBaseEntity>(entity);

                var preCtx = new TakeDamageEntityPreContext {
                    Params = new TakeDamageEntityParams {
                        Entity = baseEntity,
                        _infoPtr = info,
                        _resultPtr = damageResult
                    }
                };

                InvokeTakeDamagePre(ref preCtx);
                if (preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal) return;

                next()(entity, info, damageResult);

                var postCtx = new TakeDamageEntityPostContext { Params = preCtx.Params };
                InvokeTakeDamagePost(ref postCtx);
            };
        });
    }

    internal static unsafe Guid UnhookTakeDamage()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.TakeDamage, out var hookId))
        {
            var ptr = _core.GameData.GetSignature("CBaseEntity::TakeDamage");
            if (ptr == 0)
                throw new InvalidOperationException("Failed to find signature for CBaseEntity::TakeDamage.");

            var unmanagedFunction = _core.Memory.GetUnmanagedFunctionByAddress<CBaseEntityTakeDamage>(ptr);
            unmanagedFunction.RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    internal static void InvokeTakeDamagePre( ref TakeDamageEntityPreContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeTakeDamagePre(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }

    internal static void InvokeTakeDamagePost( ref TakeDamageEntityPostContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeTakeDamagePost(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }
}
