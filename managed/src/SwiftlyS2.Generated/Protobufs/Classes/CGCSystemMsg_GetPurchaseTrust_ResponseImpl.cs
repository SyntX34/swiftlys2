using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCSystemMsg_GetPurchaseTrust_ResponseImpl : TypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Response>, CGCSystemMsg_GetPurchaseTrust_Response
{
    public CGCSystemMsg_GetPurchaseTrust_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public bool HasPriorPurchaseHistory
    { get => Accessor.GetBool("has_prior_purchase_history"); set => Accessor.SetBool("has_prior_purchase_history", value); }
    public bool HasNoRecentPasswordResets
    { get => Accessor.GetBool("has_no_recent_password_resets"); set => Accessor.SetBool("has_no_recent_password_resets", value); }
    public bool IsWalletCashTrusted
    { get => Accessor.GetBool("is_wallet_cash_trusted"); set => Accessor.SetBool("is_wallet_cash_trusted", value); }
    public uint TimeAllTrusted
    { get => Accessor.GetUInt32("time_all_trusted"); set => Accessor.SetUInt32("time_all_trusted", value); }
}