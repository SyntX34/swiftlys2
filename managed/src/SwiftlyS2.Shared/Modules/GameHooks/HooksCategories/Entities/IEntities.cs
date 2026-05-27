namespace SwiftlyS2.Shared.GameHooks;

public interface IGameHookEntities
{
    /// <summary>
    /// Hook triggered when an entity takes damage.
    /// </summary>
    public ITakeDamageEntityHook TakeDamage { get; }
}
