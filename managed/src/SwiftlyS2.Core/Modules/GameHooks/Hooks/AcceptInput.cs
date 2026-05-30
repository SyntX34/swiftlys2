using SwiftlyS2.Core.EntitySystem;
using SwiftlyS2.Core.Extensions;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private delegate void CEntityIdentityAcceptInput( nint pEntityIdentity, nint inputName, nint activator, nint caller, nint variant, int outputId, nint unk1, nint unk2 );

    internal static unsafe Guid HookAcceptInput()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var address = _core.GameData.GetSignature("CEntityIdentity::AcceptInput");
        if (address == 0)
            throw new InvalidOperationException("Failed to find signature for CEntityIdentity::AcceptInput.");

        var fn = _core.Memory.GetUnmanagedFunctionByAddress<CEntityIdentityAcceptInput>(address);
        return fn.AddHook(next =>
        {
            return ( pEntityIdentity, pInputName, pActivator, pCaller, pVariant, outputId, unk1, unk2 ) =>
            {
                var entityIdentity = _core.Memory.ToSchemaClass<CEntityIdentity>(pEntityIdentity);
                if (!entityIdentity.IsValid || !entityIdentity.EntityInstance.IsValid)
                {
                    next()(pEntityIdentity, pInputName, pActivator, pCaller, pVariant, outputId, unk1, unk2);
                    return;
                }

                var inputName = pInputName != nint.Zero ? pInputName.AsRef<CUtlSymbolLarge>().Value : "";
                var activator = pActivator != nint.Zero ? EntityManager.GetEntityByAddress(pActivator) : null;
                var caller = pCaller != nint.Zero ? EntityManager.GetEntityByAddress(pCaller) : null;

                var preCtx = new AcceptInputEntityPreContext {
                    Params = new AcceptInputEntityParams {
                        Identity = entityIdentity,
                        EntityInstance = entityIdentity.EntityInstance,
                        DesignerName = entityIdentity.DesignerName,
                        InputName = inputName,
                        Activator = activator,
                        Caller = caller,
                        _variant = (CVariant<CVariantDefaultAllocator>*)pVariant,
                        OutputId = outputId
                    }
                };

                InvokeAcceptInputPre(ref preCtx);
                if (preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal) return;

                next()(pEntityIdentity, pInputName, pActivator, pCaller, pVariant, outputId, unk1, unk2);

                var postCtx = new AcceptInputEntityPostContext { Params = preCtx.Params };
                InvokeAcceptInputPost(ref postCtx);
            };
        });
    }

    internal static Guid UnhookAcceptInput()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.AcceptInput, out var hookId))
        {
            var address = _core.GameData.GetSignature("CEntityIdentity::AcceptInput");
            if (address == 0)
                throw new InvalidOperationException("Failed to find signature for CEntityIdentity::AcceptInput.");

            var fn = _core.Memory.GetUnmanagedFunctionByAddress<CEntityIdentityAcceptInput>(address);
            fn.RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    internal static void InvokeAcceptInputPre( ref AcceptInputEntityPreContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeAcceptInputPre(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }

    internal static void InvokeAcceptInputPost( ref AcceptInputEntityPostContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeAcceptInputPost(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }
}
