using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgGetSystemStatsResponseImpl : TypedProtobuf<CGCMsgGetSystemStatsResponse>, CGCMsgGetSystemStatsResponse
{
    public CGCMsgGetSystemStatsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint GcAppId
    { get => Accessor.GetUInt32("gc_app_id"); set => Accessor.SetUInt32("gc_app_id", value); }
    public byte[] StatsKv
    { get => Accessor.GetBytes("stats_kv"); set => Accessor.SetBytes("stats_kv", value); }
    public uint ActiveJobs
    { get => Accessor.GetUInt32("active_jobs"); set => Accessor.SetUInt32("active_jobs", value); }
    public uint YieldingJobs
    { get => Accessor.GetUInt32("yielding_jobs"); set => Accessor.SetUInt32("yielding_jobs", value); }
    public uint UserSessions
    { get => Accessor.GetUInt32("user_sessions"); set => Accessor.SetUInt32("user_sessions", value); }
    public uint GameServerSessions
    { get => Accessor.GetUInt32("game_server_sessions"); set => Accessor.SetUInt32("game_server_sessions", value); }
    public uint Socaches
    { get => Accessor.GetUInt32("socaches"); set => Accessor.SetUInt32("socaches", value); }
    public uint SocachesToUnload
    { get => Accessor.GetUInt32("socaches_to_unload"); set => Accessor.SetUInt32("socaches_to_unload", value); }
    public uint SocachesLoading
    { get => Accessor.GetUInt32("socaches_loading"); set => Accessor.SetUInt32("socaches_loading", value); }
    public uint WritebackQueue
    { get => Accessor.GetUInt32("writeback_queue"); set => Accessor.SetUInt32("writeback_queue", value); }
    public uint SteamidLocks
    { get => Accessor.GetUInt32("steamid_locks"); set => Accessor.SetUInt32("steamid_locks", value); }
    public uint LogonQueue
    { get => Accessor.GetUInt32("logon_queue"); set => Accessor.SetUInt32("logon_queue", value); }
    public uint LogonJobs
    { get => Accessor.GetUInt32("logon_jobs"); set => Accessor.SetUInt32("logon_jobs", value); }
}