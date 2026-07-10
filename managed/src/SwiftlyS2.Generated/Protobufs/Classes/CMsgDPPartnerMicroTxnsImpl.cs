using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgDPPartnerMicroTxnsImpl : TypedProtobuf<CMsgDPPartnerMicroTxns>, CMsgDPPartnerMicroTxns
{
    public CMsgDPPartnerMicroTxnsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Appid
    { get => Accessor.GetUInt32("appid"); set => Accessor.SetUInt32("appid", value); }
    public string GcName
    { get => Accessor.GetString("gc_name"); set => Accessor.SetString("gc_name", value); }
    public CMsgDPPartnerMicroTxns_PartnerInfo Partner
    { get => new CMsgDPPartnerMicroTxns_PartnerInfoImpl(NativeNetMessages.GetNestedMessage(Address, "partner"), false); }
    public IProtobufRepeatedFieldSubMessageType<CMsgDPPartnerMicroTxns_PartnerMicroTxn> Transactions
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgDPPartnerMicroTxns_PartnerMicroTxn>(Accessor, "transactions"); }
}