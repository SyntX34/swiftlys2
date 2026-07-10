using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCCStrike15_v2_SetClanId : ITypedProtobuf<CMsgGCCStrike15_v2_SetClanId>
{
    static CMsgGCCStrike15_v2_SetClanId ITypedProtobuf<CMsgGCCStrike15_v2_SetClanId>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCCStrike15_v2_SetClanIdImpl(handle, isManuallyAllocated);

    public uint ClanId { get; set; }
}