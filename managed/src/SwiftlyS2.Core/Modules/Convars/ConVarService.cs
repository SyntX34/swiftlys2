using System.Runtime.CompilerServices;
using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.Scheduler;
using SwiftlyS2.Shared.Convars;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.Convars;

internal enum EConVarType : int
{
    EConVarType_Invalid = -1,
    EConVarType_Bool,
    EConVarType_Int16,
    EConVarType_UInt16,
    EConVarType_Int32,
    EConVarType_UInt32,
    EConVarType_Int64,
    EConVarType_UInt64,
    EConVarType_Float32,
    EConVarType_Float64,
    EConVarType_String,
    EConVarType_Color,
    EConVarType_Vector2,
    EConVarType_Vector3,
    EConVarType_Vector4,
    EConVarType_Qangle,
    EConVarType_VectorWS,
    EConVarType_MAX
};

internal static class ConVarTypes
{
    public static bool TryGet<T>( out EConVarType type )
    {
        var t = typeof(T);
        type =
            t == typeof(bool) ? EConVarType.EConVarType_Bool :
            t == typeof(short) ? EConVarType.EConVarType_Int16 :
            t == typeof(ushort) ? EConVarType.EConVarType_UInt16 :
            t == typeof(int) ? EConVarType.EConVarType_Int32 :
            t == typeof(uint) ? EConVarType.EConVarType_UInt32 :
            t == typeof(long) ? EConVarType.EConVarType_Int64 :
            t == typeof(ulong) ? EConVarType.EConVarType_UInt64 :
            t == typeof(float) ? EConVarType.EConVarType_Float32 :
            t == typeof(double) ? EConVarType.EConVarType_Float64 :
            t == typeof(string) ? EConVarType.EConVarType_String :
            t == typeof(Color) ? EConVarType.EConVarType_Color :
            t == typeof(Vector2D) ? EConVarType.EConVarType_Vector2 :
            t == typeof(Vector) ? EConVarType.EConVarType_Vector3 :
            t == typeof(Vector4D) ? EConVarType.EConVarType_Vector4 :
            t == typeof(QAngle) ? EConVarType.EConVarType_Qangle :
            t == typeof(VectorWS) ? EConVarType.EConVarType_VectorWS :
            EConVarType.EConVarType_Invalid;
        return type != EConVarType.EConVarType_Invalid;
    }
}

internal class ConVarService : IConVarService
{
    private static readonly Dictionary<(int ClientId, string Name), Queue<Action<string>>> _pendingQueries = [];
    private static readonly Lock _pendingQueriesLock = new();
    private INetMessageService _netMessageService;
    private CNETMsg_SetConVar? _setConVarMessage;

    public ConVarService( INetMessageService netMessageService )
    {
        _netMessageService = netMessageService;
    }

    public IConVar<T>? Find<T>( string name )
    {
        return !NativeConvars.ExistsConvar(name) ? null : (IConVar<T>)new ConVar<T>(name, this);
    }

    public IConVar? FindAsString( string name )
    {
        return !NativeConvars.ExistsConvar(name) ? null : (IConVar)new ConVar(name, this);
    }

    public unsafe IConVar<T> Create<T>( string name, string helpMessage, T defaultValue, ConvarFlags flags = ConvarFlags.NONE )
    {
        if (NativeConvars.ExistsConvar(name))
        {
            throw new Exception($"Convar {name} already exists.");
        }

        var type = GetConVarType<T>();

        if (defaultValue is string stringValue)
        {
            CreateStringConVar(name, helpMessage, stringValue, flags);
        }
        else
        {
            NativeConvars.CreateConvar(name, (int)type, (ulong)flags, helpMessage, (nint)Unsafe.AsPointer(ref defaultValue), 0, 0);
        }

        return new ConVar<T>(name, this, type);
    }

    public unsafe IConVar<T> Create<T>( string name, string helpMessage, T defaultValue, T? minValue, T? maxValue, ConvarFlags flags = ConvarFlags.NONE ) where T : unmanaged
    {
        if (NativeConvars.ExistsConvar(name))
        {
            throw new Exception($"Convar {name} already exists.");
        }

        var type = GetConVarType<T>();

        var min = minValue.GetValueOrDefault();
        var max = maxValue.GetValueOrDefault();

        NativeConvars.CreateConvar(name, (int)type, (ulong)flags, helpMessage, (nint)(&defaultValue), minValue.HasValue ? (nint)(&min) : 0, maxValue.HasValue ? (nint)(&max) : 0);

        return new ConVar<T>(name, this, type);
    }

    public IConVar<T> CreateOrFind<T>( string name, string helpMessage, T defaultValue, ConvarFlags flags = ConvarFlags.NONE )
    {
        return NativeConvars.ExistsConvar(name) ? new ConVar<T>(name, this) : Create(name, helpMessage, defaultValue, flags);
    }

    public IConVar<T> CreateOrFind<T>( string name, string helpMessage, T defaultValue, T? minValue, T? maxValue, ConvarFlags flags = ConvarFlags.NONE ) where T : unmanaged
    {
        return NativeConvars.ExistsConvar(name) ? new ConVar<T>(name, this) : Create(name, helpMessage, defaultValue, minValue, maxValue, flags);
    }

    private void Replicate( string name, string value, int? clientId )
    {
        if (NativeBinding.IsMainThread)
        {
            ReplicateNow(name, value, clientId);
            return;
        }

        _ = SchedulerManager.QueueOrNow(() => ReplicateNow(name, value, clientId));
    }

    private void ReplicateNow( string name, string value, int? clientId )
    {
        var msg = _setConVarMessage ??= _netMessageService.Create<CNETMsg_SetConVar>();

        msg.Convars.Cvars.Clear();
        var cvar = msg.Convars.Cvars.Add();
        cvar.Name = name;
        cvar.Value = value;

        msg.Recipients.RemoveAllPlayers();
        if (clientId is { } id)
        {
            msg.Recipients.AddRecipient(id);
        }
        else
        {
            msg.Recipients.AddAllPlayers();
        }

        msg.Send();
    }

    public void ReplicateToClient( int clientId, string name, string value ) => Replicate(name, value, clientId);

    public void ReplicateToAll( string name, string value ) => Replicate(name, value, null);

    private static unsafe void CreateStringConVar( string name, string helpMessage, string value, ConvarFlags flags )
    {
        using var valueString = new ScopedCString(value);
        fixed (byte* valuePtr = valueString)
        {
            NativeConvars.CreateConvar(name, (int)EConVarType.EConVarType_String, (ulong)flags, helpMessage, (nint)valuePtr, 0, 0);
        }
    }

    private static EConVarType GetConVarType<T>() =>
        ConVarTypes.TryGet<T>(out var type) ? type : throw new Exception($"Unsupported type {typeof(T)}.");

    public void QueryClient( int clientId, string name, Action<string> callback )
    {
        var key = (clientId, name);
        var shouldQuery = false;

        lock (_pendingQueriesLock)
        {
            if (!_pendingQueries.TryGetValue(key, out var queue))
            {
                queue = new Queue<Action<string>>();
                _pendingQueries[key] = queue;
                shouldQuery = true;
            }
            else
            {
                shouldQuery = false;
            }

            queue.Enqueue(callback);
        }

        if (shouldQuery)
        {
            _ = SchedulerManager.QueueOrNow(() => NativeConvars.QueryClientConvar(clientId, name));
        }
    }

    public static void ProcessConVarQueryCallback(int playerid, string convarName, string convarValue)
    {
        var key = (playerid, convarName);
        Queue<Action<string>>? queue;

        lock (_pendingQueriesLock)
        {
            if (!_pendingQueries.Remove(key, out queue))
            {
                return;
            }
        }

        while (queue.Count > 0)
        {
            queue.Dequeue()(convarValue);
        }
    }
}