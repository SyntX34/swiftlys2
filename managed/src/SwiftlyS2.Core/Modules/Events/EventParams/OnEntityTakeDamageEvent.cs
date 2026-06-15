using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Core.Extensions;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Core.Events;

internal class OnEntityTakeDamageEvent : IOnEntityTakeDamageEvent
{
    public required CEntityInstance Entity { get; set; }
    public nint _infoPtr;
    public nint _resultPtr;
    public ref CTakeDamageInfo Info => ref _infoPtr.AsRef<CTakeDamageInfo>();
    public ref CTakeDamageResult DamageResult {
        get {
            if (_resultPtr == nint.Zero)
                throw new InvalidOperationException("The native TakeDamage call did not provide a DamageResult. Use GameHooks.Entities.TakeDamage and check its nullable DamageResult pointer.");

            return ref _resultPtr.AsRef<CTakeDamageResult>();
        }
    }

    public HookResult Result { get; set; } = HookResult.Continue;
}
