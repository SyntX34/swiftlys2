using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCRoutingInfo_PolicyRuleImpl : TypedProtobuf<CMsgGCRoutingInfo_PolicyRule>, CMsgGCRoutingInfo_PolicyRule
{
    public CMsgGCRoutingInfo_PolicyRuleImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int AccountType
    { get => Accessor.GetInt32("account_type"); set => Accessor.SetInt32("account_type", value); }
    public int AddressMaskGroupId
    { get => Accessor.GetInt32("address_mask_group_id"); set => Accessor.SetInt32("address_mask_group_id", value); }
    public CMsgGCRoutingInfo_TokenBucketConfiguration TokenBucket
    { get => new CMsgGCRoutingInfo_TokenBucketConfigurationImpl(NativeNetMessages.GetNestedMessage(Address, "token_bucket"), false); }
}