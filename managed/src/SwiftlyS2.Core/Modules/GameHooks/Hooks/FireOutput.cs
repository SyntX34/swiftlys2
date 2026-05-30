using SwiftlyS2.Core.EntitySystem;
using SwiftlyS2.Core.Extensions;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private delegate void CEntityIOOutputFireOutputInternal( nint pEntityIO, nint pActivator, nint pCaller, nint pVariant, float flDelay, nint unk1, nint unk2 );

    internal static unsafe Guid HookFireOutput()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var address = _core.GameData.GetSignature("CEntityIOOutput::FireOutputInternal");
        if (address == 0)
            throw new InvalidOperationException("Failed to find signature for CEntityIOOutput::FireOutputInternal.");

        var fn = _core.Memory.GetUnmanagedFunctionByAddress<CEntityIOOutputFireOutputInternal>(address);
        return fn.AddHook(next =>
        {
            return ( pEntityIO, pActivator, pCaller, pVariant, flDelay, unk1, unk2 ) =>
            {
                var entityIO = pEntityIO.AsRef<CEntityIOOutput>();
                var outputName = entityIO.Desc.Name.Value;
                var activator = pActivator != nint.Zero ? EntityManager.GetEntityByAddress(pActivator) : null;
                var caller = pCaller != nint.Zero ? EntityManager.GetEntityByAddress(pCaller) : null;

                var preCtx = new FireOutputEntityPreContext {
                    Params = new FireOutputEntityParams {
                        _entityIO = (CEntityIOOutput*)pEntityIO,
                        _variant = (CVariant<CVariantDefaultAllocator>*)pVariant,
                        DesignerName = caller?.DesignerName ?? string.Empty,
                        OutputName = outputName,
                        Activator = activator,
                        Caller = caller,
                        Delay = flDelay
                    }
                };

                InvokeFireOutputPre(ref preCtx);
                if (preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal) return;

                next()(pEntityIO, pActivator, pCaller, pVariant, flDelay, unk1, unk2);

                var postCtx = new FireOutputEntityPostContext { Params = preCtx.Params };
                InvokeFireOutputPost(ref postCtx);
            };
        });
    }

    internal static Guid UnhookFireOutput()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.FireOutput, out var hookId))
        {
            var address = _core.GameData.GetSignature("CEntityIOOutput::FireOutputInternal");
            if (address == 0)
                throw new InvalidOperationException("Failed to find signature for CEntityIOOutput::FireOutputInternal.");

            var fn = _core.Memory.GetUnmanagedFunctionByAddress<CEntityIOOutputFireOutputInternal>(address);
            fn.RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    internal static void InvokeFireOutputPre( ref FireOutputEntityPreContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeFireOutputPre(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }

    internal static void InvokeFireOutputPost( ref FireOutputEntityPostContext ctx )
    {
        lock (subscribersLock)
        {
            for (var i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].InvokeFireOutputPost(ref ctx);
                if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
            }
        }
    }
}
