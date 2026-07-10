using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetUserGameStatsResponse : ITypedProtobuf<CMsgAMGetUserGameStatsResponse>
{
    static CMsgAMGetUserGameStatsResponse ITypedProtobuf<CMsgAMGetUserGameStatsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetUserGameStatsResponseImpl(handle, isManuallyAllocated);

    public ulong SteamId { get; set; }
    public ulong GameId { get; set; }
    public int Eresult { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Stats> Stats { get; }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Achievement_Blocks> AchievementBlocks { get; }
}