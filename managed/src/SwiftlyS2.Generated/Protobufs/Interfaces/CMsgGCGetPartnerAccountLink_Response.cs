using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetPartnerAccountLink_Response : ITypedProtobuf<CMsgGCGetPartnerAccountLink_Response>
{
    static CMsgGCGetPartnerAccountLink_Response ITypedProtobuf<CMsgGCGetPartnerAccountLink_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetPartnerAccountLink_ResponseImpl(handle, isManuallyAllocated);

    public uint Pwid { get; set; }
    public uint Nexonid { get; set; }
    public int Ageclass { get; set; }
    public bool IdVerified { get; set; }
    public bool IsAdult { get; set; }
}