using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Shared.GameHooks;

public struct AcceptInputEntityParams
{
    /// <summary>
    /// The entity identity whose AcceptInput function was called.
    /// </summary>
    public required CEntityIdentity Identity { get; init; }

    /// <summary>
    /// The entity instance the input is being sent to.
    /// </summary>
    public required CEntityInstance EntityInstance { get; init; }

    /// <summary>
    /// The designer name of the entity.
    /// </summary>
    public required string DesignerName { get; init; }

    /// <summary>
    /// The name of the input being accepted.
    /// </summary>
    public required string InputName { get; init; }

    /// <summary>
    /// The activator of the input, or null when none was provided.
    /// </summary>
    public required CEntityInstance? Activator { get; init; }

    /// <summary>
    /// The caller of the input, or null when none was provided.
    /// </summary>
    public required CEntityInstance? Caller { get; init; }

    internal unsafe CVariant<CVariantDefaultAllocator>* _variant;

    /// <summary>
    /// The variant value of the input. Modifications are written to native memory.
    /// </summary>
    public ref CVariant<CVariantDefaultAllocator> VariantValue {
        get {
            unsafe
            {
                if (_variant == null) throw new InvalidOperationException("Variant pointer is null.");
                return ref *_variant;
            }
        }
    }

    /// <summary>
    /// The output ID of the input being accepted.
    /// </summary>
    public required int OutputId { get; init; }
}

public ref struct AcceptInputEntityPreContext
{
    public AcceptInputEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct AcceptInputEntityPostContext
{
    public AcceptInputEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public delegate void OnAcceptInputEntityPreDelegate( ref AcceptInputEntityPreContext ctx );
public delegate void OnAcceptInputEntityPostDelegate( ref AcceptInputEntityPostContext ctx );

public interface IAcceptInputEntityHook
{
    public event OnAcceptInputEntityPreDelegate Pre;
    public event OnAcceptInputEntityPostDelegate Post;
}
