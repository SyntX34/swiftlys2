namespace SwiftlyS2.Shared.Memory;

public interface IUnmanagedAddress
{
    /// <summary>
    /// The address pointing to the unmanaged code.
    /// </summary>
    nint Address { get; }

    /// <summary>
    /// Hook the specified address with a managed callback.
    /// The <paramref name="callback"/> receives a wrapper for the context of the registries.
    /// </summary>
    /// <param name="callback">Context Wrapper.</param>
    /// <returns>a guid for the hook.</returns>
    Guid AddHook(Action<IMidHook> callback);

    /// <summary>
    /// Unhook a hook by its id.
    /// </summary>
    /// <param name="id">The id of the hook to unhook.</param>
    void RemoveHook(Guid id);
}