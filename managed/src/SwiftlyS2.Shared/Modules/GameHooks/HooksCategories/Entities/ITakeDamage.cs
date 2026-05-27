using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Shared.GameHooks;

public unsafe struct TakeDamageEntityParams
{
    /// <summary>
    /// The entity that is taking damage.
    /// </summary>
    public required CBaseEntity Entity { get; init; }

    internal CTakeDamageInfo* _infoPtr;
    internal CTakeDamageResult* _resultPtr;

    /// <summary>
    /// The damage information passed to the original function. Modifications are written to native memory.
    /// </summary>
    public ref CTakeDamageInfo Info => ref *_infoPtr;

    /// <summary>
    /// The optional native damage result. It may be null when the caller did not provide a result object.
    /// </summary>
    public CTakeDamageResult* DamageResult => _resultPtr;
}

public ref struct TakeDamageEntityPreContext
{
    public TakeDamageEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct TakeDamageEntityPostContext
{
    public TakeDamageEntityParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public delegate void OnTakeDamageEntityPreDelegate( ref TakeDamageEntityPreContext ctx );
public delegate void OnTakeDamageEntityPostDelegate( ref TakeDamageEntityPostContext ctx );

public interface ITakeDamageEntityHook
{
    public event OnTakeDamageEntityPreDelegate Pre;
    public event OnTakeDamageEntityPostDelegate Post;
}
