using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCRoutingInfo : ITypedProtobuf<CMsgGCRoutingInfo>
{
    static CMsgGCRoutingInfo ITypedProtobuf<CMsgGCRoutingInfo>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCRoutingInfoImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<uint> DirIndex { get; }
    public CMsgGCRoutingInfo_RoutingMethod Method { get; set; }
    public CMsgGCRoutingInfo_RoutingMethod Fallback { get; set; }
    public uint ProtobufField { get; set; }
    public string WebapiParam { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCRoutingInfo_PolicyRule> PolicyRules { get; }
}