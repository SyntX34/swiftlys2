using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgDPPartnerMicroTxns_PartnerMicroTxn : ITypedProtobuf<CMsgDPPartnerMicroTxns_PartnerMicroTxn>
{
    static CMsgDPPartnerMicroTxns_PartnerMicroTxn ITypedProtobuf<CMsgDPPartnerMicroTxns_PartnerMicroTxn>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgDPPartnerMicroTxns_PartnerMicroTxnImpl(handle, isManuallyAllocated);

    public uint InitTime { get; set; }
    public uint LastUpdateTime { get; set; }
    public ulong TxnId { get; set; }
    public uint AccountId { get; set; }
    public uint LineItem { get; set; }
    public ulong ItemId { get; set; }
    public uint DefIndex { get; set; }
    public ulong Price { get; set; }
    public ulong Tax { get; set; }
    public ulong PriceUsd { get; set; }
    public ulong TaxUsd { get; set; }
    public uint PurchaseType { get; set; }
    public uint SteamTxnType { get; set; }
    public string CountryCode { get; set; }
    public string RegionCode { get; set; }
    public int Quantity { get; set; }
    public ulong RefTransId { get; set; }
}