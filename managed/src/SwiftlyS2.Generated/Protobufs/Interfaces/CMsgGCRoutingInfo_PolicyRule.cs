using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCRoutingInfo_PolicyRule : ITypedProtobuf<CMsgGCRoutingInfo_PolicyRule>
{
    static CMsgGCRoutingInfo_PolicyRule ITypedProtobuf<CMsgGCRoutingInfo_PolicyRule>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCRoutingInfo_PolicyRuleImpl(handle, isManuallyAllocated);

    public int AccountType { get; set; }
    public int AddressMaskGroupId { get; set; }
    public CMsgGCRoutingInfo_TokenBucketConfiguration TokenBucket { get; }
}