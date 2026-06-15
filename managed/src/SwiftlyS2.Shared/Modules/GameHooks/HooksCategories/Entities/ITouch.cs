using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Shared.GameHooks;

public struct EntityTouchParams
{
    /// <summary>
    /// The entity whose touch function was called.
    /// </summary>
    public required CBaseEntity Entity { get; init; }

    /// <summary>
    /// The other entity involved in the touch.
    /// </summary>
    public required CBaseEntity OtherEntity { get; init; }
}

public ref struct StartTouchEntityPreContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct StartTouchEntityPostContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct TouchEntityPreContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct TouchEntityPostContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct EndTouchEntityPreContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public ref struct EndTouchEntityPostContext
{
    public EntityTouchParams Params;
    private HookResult _hookResult;

    public void SetHookResult( HookResult result ) => _hookResult = result;
    internal HookResult HookResult => _hookResult;
}

public delegate void OnStartTouchEntityPreDelegate( ref StartTouchEntityPreContext ctx );
public delegate void OnStartTouchEntityPostDelegate( ref StartTouchEntityPostContext ctx );
public delegate void OnTouchEntityPreDelegate( ref TouchEntityPreContext ctx );
public delegate void OnTouchEntityPostDelegate( ref TouchEntityPostContext ctx );
public delegate void OnEndTouchEntityPreDelegate( ref EndTouchEntityPreContext ctx );
public delegate void OnEndTouchEntityPostDelegate( ref EndTouchEntityPostContext ctx );

public interface IStartTouchEntityHook
{
    public event OnStartTouchEntityPreDelegate Pre;
    public event OnStartTouchEntityPostDelegate Post;
}

public interface ITouchEntityHook
{
    public event OnTouchEntityPreDelegate Pre;
    public event OnTouchEntityPostDelegate Post;
}

public interface IEndTouchEntityHook
{
    public event OnEndTouchEntityPreDelegate Pre;
    public event OnEndTouchEntityPostDelegate Post;
}
