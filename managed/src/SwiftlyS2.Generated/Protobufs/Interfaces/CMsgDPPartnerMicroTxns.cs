using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgDPPartnerMicroTxns : ITypedProtobuf<CMsgDPPartnerMicroTxns>
{
    static CMsgDPPartnerMicroTxns ITypedProtobuf<CMsgDPPartnerMicroTxns>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgDPPartnerMicroTxnsImpl(handle, isManuallyAllocated);

    public uint Appid { get; set; }
    public string GcName { get; set; }
    public CMsgDPPartnerMicroTxns_PartnerInfo Partner { get; }
    public IProtobufRepeatedFieldSubMessageType<CMsgDPPartnerMicroTxns_PartnerMicroTxn> Transactions { get; }
}