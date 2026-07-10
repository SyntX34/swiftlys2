using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetUserGameStats : ITypedProtobuf<CMsgAMGetUserGameStats>
{
    static CMsgAMGetUserGameStats ITypedProtobuf<CMsgAMGetUserGameStats>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetUserGameStatsImpl(handle, isManuallyAllocated);

    public ulong SteamId { get; set; }
    public ulong GameId { get; set; }
    public IProtobufRepeatedFieldValueType<uint> Stats { get; }
}