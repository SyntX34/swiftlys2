using SwiftlyS2.Shared.Natives;

namespace SwiftlyS2.Shared.Memory;

public interface IMidHook
{
    /// <summary>
    /// Gets the context information associated with the current instruction hook.
    /// </summary>
    public MidHookContext Context { get; }
}