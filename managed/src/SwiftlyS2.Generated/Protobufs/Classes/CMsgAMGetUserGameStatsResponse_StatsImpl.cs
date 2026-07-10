using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGetUserGameStatsResponse_StatsImpl : TypedProtobuf<CMsgAMGetUserGameStatsResponse_Stats>, CMsgAMGetUserGameStatsResponse_Stats
{
    public CMsgAMGetUserGameStatsResponse_StatsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint StatId
    { get => Accessor.GetUInt32("stat_id"); set => Accessor.SetUInt32("stat_id", value); }
    public uint StatValue
    { get => Accessor.GetUInt32("stat_value"); set => Accessor.SetUInt32("stat_value", value); }
}