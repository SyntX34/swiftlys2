using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class FireOutputEntityHook : IFireOutputEntityHook
{
    internal event OnFireOutputEntityPreDelegate? _Pre;
    internal event OnFireOutputEntityPostDelegate? _Post;

    public event OnFireOutputEntityPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.FireOutput);
            _Pre += value;
        }
        remove
        {
            _Pre -= value;
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
        }
    }

    public event OnFireOutputEntityPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.FireOutput);
            _Post += value;
        }
        remove
        {
            _Post -= value;
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
        }
    }

    public void InvokePre( ref FireOutputEntityPreContext ctx ) => _Pre?.Invoke(ref ctx);
    public void InvokePost( ref FireOutputEntityPostContext ctx ) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
    }
}
