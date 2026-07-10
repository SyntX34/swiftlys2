using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetUserGameStatsResponse_Stats : ITypedProtobuf<CMsgAMGetUserGameStatsResponse_Stats>
{
    static CMsgAMGetUserGameStatsResponse_Stats ITypedProtobuf<CMsgAMGetUserGameStatsResponse_Stats>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetUserGameStatsResponse_StatsImpl(handle, isManuallyAllocated);

    public uint StatId { get; set; }
    public uint StatValue { get; set; }
}