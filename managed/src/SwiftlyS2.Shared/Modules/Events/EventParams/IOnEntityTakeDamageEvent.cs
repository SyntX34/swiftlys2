using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Shared.Events;


/// <summary>
/// Called when an entity takes damage.
/// </summary>
[Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.TakeDamage instead.")]
public interface IOnEntityTakeDamageEvent
{
    /// <summary>
    /// The entity that took damage.
    /// </summary>
    public CEntityInstance Entity { get; }

    /// <summary>
    /// The damage info.
    /// </summary>
    public ref CTakeDamageInfo Info { get; }

    /// <summary>
    /// The damage result.
    /// Throws <see cref="InvalidOperationException"/> when the native call did not provide a result.
    /// Use <see cref="SwiftlyS2.Shared.GameHooks.IGameHooks.Entities"/> to access the nullable native result.
    /// </summary>
    public ref CTakeDamageResult DamageResult { get; }

    /// <summary>
    /// If return <see cref="HookResult.Stop"/> or <see cref="HookResult.CancelOriginal"/>, the damage will not be applied.
    /// </summary>
    public HookResult Result { get; set; }
}
