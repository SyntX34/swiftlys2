using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgDPPartnerMicroTxns_PartnerMicroTxnImpl : TypedProtobuf<CMsgDPPartnerMicroTxns_PartnerMicroTxn>, CMsgDPPartnerMicroTxns_PartnerMicroTxn
{
    public CMsgDPPartnerMicroTxns_PartnerMicroTxnImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint InitTime
    { get => Accessor.GetUInt32("init_time"); set => Accessor.SetUInt32("init_time", value); }
    public uint LastUpdateTime
    { get => Accessor.GetUInt32("last_update_time"); set => Accessor.SetUInt32("last_update_time", value); }
    public ulong TxnId
    { get => Accessor.GetUInt64("txn_id"); set => Accessor.SetUInt64("txn_id", value); }
    public uint AccountId
    { get => Accessor.GetUInt32("account_id"); set => Accessor.SetUInt32("account_id", value); }
    public uint LineItem
    { get => Accessor.GetUInt32("line_item"); set => Accessor.SetUInt32("line_item", value); }
    public ulong ItemId
    { get => Accessor.GetUInt64("item_id"); set => Accessor.SetUInt64("item_id", value); }
    public uint DefIndex
    { get => Accessor.GetUInt32("def_index"); set => Accessor.SetUInt32("def_index", value); }
    public ulong Price
    { get => Accessor.GetUInt64("price"); set => Accessor.SetUInt64("price", value); }
    public ulong Tax
    { get => Accessor.GetUInt64("tax"); set => Accessor.SetUInt64("tax", value); }
    public ulong PriceUsd
    { get => Accessor.GetUInt64("price_usd"); set => Accessor.SetUInt64("price_usd", value); }
    public ulong TaxUsd
    { get => Accessor.GetUInt64("tax_usd"); set => Accessor.SetUInt64("tax_usd", value); }
    public uint PurchaseType
    { get => Accessor.GetUInt32("purchase_type"); set => Accessor.SetUInt32("purchase_type", value); }
    public uint SteamTxnType
    { get => Accessor.GetUInt32("steam_txn_type"); set => Accessor.SetUInt32("steam_txn_type", value); }
    public string CountryCode
    { get => Accessor.GetString("country_code"); set => Accessor.SetString("country_code", value); }
    public string RegionCode
    { get => Accessor.GetString("region_code"); set => Accessor.SetString("region_code", value); }
    public int Quantity
    { get => Accessor.GetInt32("quantity"); set => Accessor.SetInt32("quantity", value); }
    public ulong RefTransId
    { get => Accessor.GetUInt64("ref_trans_id"); set => Accessor.SetUInt64("ref_trans_id", value); }
}