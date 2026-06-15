using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Shared.GameHooks;

public struct FireOutputEntityParams
{
    internal unsafe CEntityIOOutput* _entityIO;

    /// <summary>
    /// The entity IO output that is being fired. Modifications are written to native memory.
    /// </summary>
    public ref CEntityIOOutput EntityIO {
        get {
            unsafe
            {
                if (_entityIO == null) throw new InvalidOperationException("EntityIO pointer is null.");
                return ref *_entityIO;
            }
        }
    }

    /// <summary>
    /// The designer name of the caller.
    /// </summary>
    public required string DesignerName { get; init; }

    /// <summary>
    /// The name of the output being fired.
    /// </summary>
    public required string OutputName { get; init; }

    /// <summary>
    /// The activator of the output, or null when none was provided.
    /// </summary>
    public required CEntityInstance? Activator { get; init; }

    /// <summary>
    /// The caller of the output, or null when none was provided.
    /// </summary>
    public required CEntityInstance? Caller { get; init; }

    internal unsafe CVariant<CVariantDefaultAllocator>* _variant;

    /// <summary>
    /// The variant value of the output. Modifications are written to native memory.
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
    /// The delay of this IO event, in seconds.
    /// </summary>
    public required float Delay { get; init; }
}

public ref struct FireOutputEntityPreContext
{
    public FireOutputEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct FireOutputEntityPostContext
{
    public FireOutputEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public delegate void OnFireOutputEntityPreDelegate( ref FireOutputEntityPreContext ctx );
public delegate void OnFireOutputEntityPostDelegate( ref FireOutputEntityPostContext ctx );

public interface IFireOutputEntityHook
{
    public event OnFireOutputEntityPreDelegate Pre;
    public event OnFireOutputEntityPostDelegate Post;
}
