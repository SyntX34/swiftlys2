namespace SwiftlyS2.Shared.GameHooks;

public interface IGameHookEntities
{
    /// <summary>
    /// Hook triggered when an entity takes damage.
    /// </summary>
    public ITakeDamageEntityHook TakeDamage { get; }

    /// <summary>
    /// Hook triggered when an entity starts touching another entity.
    /// </summary>
    public IStartTouchEntityHook StartTouch { get; }

    /// <summary>
    /// Hook triggered while an entity is touching another entity.
    /// </summary>
    public ITouchEntityHook Touch { get; }

    /// <summary>
    /// Hook triggered when an entity stops touching another entity.
    /// </summary>
    public IEndTouchEntityHook EndTouch { get; }

    /// <summary>
    /// Hook triggered when an entity accepts an input.
    /// </summary>
    public IAcceptInputEntityHook AcceptInput { get; }

    /// <summary>
    /// Hook triggered when an entity fires an output.
    /// </summary>
    public IFireOutputEntityHook FireOutput { get; }
}
