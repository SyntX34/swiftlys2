using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCSystemMsg_GetPurchaseTrust_Response : ITypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Response>
{
    static CGCSystemMsg_GetPurchaseTrust_Response ITypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CGCSystemMsg_GetPurchaseTrust_ResponseImpl(handle, isManuallyAllocated);

    public bool HasPriorPurchaseHistory { get; set; }
    public bool HasNoRecentPasswordResets { get; set; }
    public bool IsWalletCashTrusted { get; set; }
    public uint TimeAllTrusted { get; set; }
}