using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGetUserGameStatsImpl : TypedProtobuf<CMsgAMGetUserGameStats>, CMsgAMGetUserGameStats
{
    public CMsgAMGetUserGameStatsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamId
    { get => Accessor.GetUInt64("steam_id"); set => Accessor.SetUInt64("steam_id", value); }
    public ulong GameId
    { get => Accessor.GetUInt64("game_id"); set => Accessor.SetUInt64("game_id", value); }
    public IProtobufRepeatedFieldValueType<uint> Stats
    { get => new ProtobufRepeatedFieldValueType<uint>(Accessor, "stats"); }
}