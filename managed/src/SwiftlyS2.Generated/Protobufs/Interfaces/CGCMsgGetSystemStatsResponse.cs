using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgGetSystemStatsResponse : ITypedProtobuf<CGCMsgGetSystemStatsResponse>
{
    static CGCMsgGetSystemStatsResponse ITypedProtobuf<CGCMsgGetSystemStatsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgGetSystemStatsResponseImpl(handle, isManuallyAllocated);

    public uint GcAppId { get; set; }
    public byte[] StatsKv { get; set; }
    public uint ActiveJobs { get; set; }
    public uint YieldingJobs { get; set; }
    public uint UserSessions { get; set; }
    public uint GameServerSessions { get; set; }
    public uint Socaches { get; set; }
    public uint SocachesToUnload { get; set; }
    public uint SocachesLoading { get; set; }
    public uint WritebackQueue { get; set; }
    public uint SteamidLocks { get; set; }
    public uint LogonQueue { get; set; }
    public uint LogonJobs { get; set; }
}