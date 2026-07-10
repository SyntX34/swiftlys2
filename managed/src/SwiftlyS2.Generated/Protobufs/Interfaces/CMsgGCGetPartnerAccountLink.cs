using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetPartnerAccountLink : ITypedProtobuf<CMsgGCGetPartnerAccountLink>
{
    static CMsgGCGetPartnerAccountLink ITypedProtobuf<CMsgGCGetPartnerAccountLink>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetPartnerAccountLinkImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
}