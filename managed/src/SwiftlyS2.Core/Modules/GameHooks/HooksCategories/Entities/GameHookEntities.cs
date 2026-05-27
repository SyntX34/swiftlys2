using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class GameHookEntities : IGameHookEntities
{
    internal readonly TakeDamageEntityHook TakeDamageHook = new();

    public ITakeDamageEntityHook TakeDamage => TakeDamageHook;
}
