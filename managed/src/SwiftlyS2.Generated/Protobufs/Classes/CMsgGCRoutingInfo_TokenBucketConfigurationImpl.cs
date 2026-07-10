using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCRoutingInfo_TokenBucketConfigurationImpl : TypedProtobuf<CMsgGCRoutingInfo_TokenBucketConfiguration>, CMsgGCRoutingInfo_TokenBucketConfiguration
{
    public CMsgGCRoutingInfo_TokenBucketConfigurationImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int TokensStart
    { get => Accessor.GetInt32("tokens_start"); set => Accessor.SetInt32("tokens_start", value); }
    public int TokensGrant
    { get => Accessor.GetInt32("tokens_grant"); set => Accessor.SetInt32("tokens_grant", value); }
    public int GrantSeconds
    { get => Accessor.GetInt32("grant_seconds"); set => Accessor.SetInt32("grant_seconds", value); }
}