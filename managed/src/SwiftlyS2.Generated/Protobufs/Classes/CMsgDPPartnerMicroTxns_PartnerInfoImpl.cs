using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgDPPartnerMicroTxns_PartnerInfoImpl : TypedProtobuf<CMsgDPPartnerMicroTxns_PartnerInfo>, CMsgDPPartnerMicroTxns_PartnerInfo
{
    public CMsgDPPartnerMicroTxns_PartnerInfoImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint PartnerId
    { get => Accessor.GetUInt32("partner_id"); set => Accessor.SetUInt32("partner_id", value); }
    public string PartnerName
    { get => Accessor.GetString("partner_name"); set => Accessor.SetString("partner_name", value); }
    public string CurrencyCode
    { get => Accessor.GetString("currency_code"); set => Accessor.SetString("currency_code", value); }
    public string CurrencyName
    { get => Accessor.GetString("currency_name"); set => Accessor.SetString("currency_name", value); }
}