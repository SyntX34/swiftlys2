using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGetUserGameStatsResponse_Achievement_BlocksImpl : TypedProtobuf<CMsgAMGetUserGameStatsResponse_Achievement_Blocks>, CMsgAMGetUserGameStatsResponse_Achievement_Blocks
{
    public CMsgAMGetUserGameStatsResponse_Achievement_BlocksImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint AchievementId
    { get => Accessor.GetUInt32("achievement_id"); set => Accessor.SetUInt32("achievement_id", value); }
    public uint AchievementBitId
    { get => Accessor.GetUInt32("achievement_bit_id"); set => Accessor.SetUInt32("achievement_bit_id", value); }
    public uint UnlockTime
    { get => Accessor.GetUInt32("unlock_time"); set => Accessor.SetUInt32("unlock_time", value); }
}