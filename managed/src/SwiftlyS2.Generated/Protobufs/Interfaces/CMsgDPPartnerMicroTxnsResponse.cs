using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgDPPartnerMicroTxnsResponse : ITypedProtobuf<CMsgDPPartnerMicroTxnsResponse>
{
    static CMsgDPPartnerMicroTxnsResponse ITypedProtobuf<CMsgDPPartnerMicroTxnsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgDPPartnerMicroTxnsResponseImpl(handle, isManuallyAllocated);

    public uint Eresult { get; set; }
    public CMsgDPPartnerMicroTxnsResponse_EErrorCode Eerrorcode { get; set; }
}