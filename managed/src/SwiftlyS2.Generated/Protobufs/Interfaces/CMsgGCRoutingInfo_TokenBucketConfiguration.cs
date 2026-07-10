using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCRoutingInfo_TokenBucketConfiguration : ITypedProtobuf<CMsgGCRoutingInfo_TokenBucketConfiguration>
{
    static CMsgGCRoutingInfo_TokenBucketConfiguration ITypedProtobuf<CMsgGCRoutingInfo_TokenBucketConfiguration>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCRoutingInfo_TokenBucketConfigurationImpl(handle, isManuallyAllocated);

    public int TokensStart { get; set; }
    public int TokensGrant { get; set; }
    public int GrantSeconds { get; set; }
}