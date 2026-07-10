using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCHAccountVacStatusChange : ITypedProtobuf<CMsgGCHAccountVacStatusChange>
{
    static CMsgGCHAccountVacStatusChange ITypedProtobuf<CMsgGCHAccountVacStatusChange>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCHAccountVacStatusChangeImpl(handle, isManuallyAllocated);

    public ulong SteamId { get; set; }
    public uint AppId { get; set; }
    public uint RtimeVacbanStarts { get; set; }
    public bool IsBannedNow { get; set; }
    public bool IsBannedFuture { get; set; }
}