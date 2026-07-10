using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetUserGameStatsResponse_Achievement_Blocks : ITypedProtobuf<CMsgAMGetUserGameStatsResponse_Achievement_Blocks>
{
    static CMsgAMGetUserGameStatsResponse_Achievement_Blocks ITypedProtobuf<CMsgAMGetUserGameStatsResponse_Achievement_Blocks>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetUserGameStatsResponse_Achievement_BlocksImpl(handle, isManuallyAllocated);

    public uint AchievementId { get; set; }
    public uint AchievementBitId { get; set; }
    public uint UnlockTime { get; set; }
}