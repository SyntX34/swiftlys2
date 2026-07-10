using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgDPPartnerMicroTxns_PartnerInfo : ITypedProtobuf<CMsgDPPartnerMicroTxns_PartnerInfo>
{
    static CMsgDPPartnerMicroTxns_PartnerInfo ITypedProtobuf<CMsgDPPartnerMicroTxns_PartnerInfo>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgDPPartnerMicroTxns_PartnerInfoImpl(handle, isManuallyAllocated);

    public uint PartnerId { get; set; }
    public string PartnerName { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
}