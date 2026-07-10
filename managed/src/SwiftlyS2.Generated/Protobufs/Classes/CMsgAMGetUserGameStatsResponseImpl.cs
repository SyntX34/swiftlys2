using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGetUserGameStatsResponseImpl : TypedProtobuf<CMsgAMGetUserGameStatsResponse>, CMsgAMGetUserGameStatsResponse
{
    public CMsgAMGetUserGameStatsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamId
    { get => Accessor.GetUInt64("steam_id"); set => Accessor.SetUInt64("steam_id", value); }
    public ulong GameId
    { get => Accessor.GetUInt64("game_id"); set => Accessor.SetUInt64("game_id", value); }
    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Stats> Stats
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Stats>(Accessor, "stats"); }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Achievement_Blocks> AchievementBlocks
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgAMGetUserGameStatsResponse_Achievement_Blocks>(Accessor, "achievement_blocks"); }
}