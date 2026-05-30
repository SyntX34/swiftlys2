using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class GameHookEntities : IGameHookEntities
{
    internal readonly TakeDamageEntityHook TakeDamageHook = new();
    internal readonly StartTouchEntityHook StartTouchHook = new();
    internal readonly TouchEntityHook TouchHook = new();
    internal readonly EndTouchEntityHook EndTouchHook = new();
    internal readonly AcceptInputEntityHook AcceptInputHook = new();
    internal readonly FireOutputEntityHook FireOutputHook = new();

    public ITakeDamageEntityHook TakeDamage => TakeDamageHook;
    public IStartTouchEntityHook StartTouch => StartTouchHook;
    public ITouchEntityHook Touch => TouchHook;
    public IEndTouchEntityHook EndTouch => EndTouchHook;
    public IAcceptInputEntityHook AcceptInput => AcceptInputHook;
    public IFireOutputEntityHook FireOutput => FireOutputHook;
}
