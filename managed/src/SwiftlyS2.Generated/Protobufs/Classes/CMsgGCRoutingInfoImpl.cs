using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCRoutingInfoImpl : TypedProtobuf<CMsgGCRoutingInfo>, CMsgGCRoutingInfo
{
    public CMsgGCRoutingInfoImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<uint> DirIndex
    { get => new ProtobufRepeatedFieldValueType<uint>(Accessor, "dir_index"); }
    public CMsgGCRoutingInfo_RoutingMethod Method
    { get => (CMsgGCRoutingInfo_RoutingMethod)Accessor.GetInt32("method"); set => Accessor.SetInt32("method", (int)value); }
    public CMsgGCRoutingInfo_RoutingMethod Fallback
    { get => (CMsgGCRoutingInfo_RoutingMethod)Accessor.GetInt32("fallback"); set => Accessor.SetInt32("fallback", (int)value); }
    public uint ProtobufField
    { get => Accessor.GetUInt32("protobuf_field"); set => Accessor.SetUInt32("protobuf_field", value); }
    public string WebapiParam
    { get => Accessor.GetString("webapi_param"); set => Accessor.SetString("webapi_param", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCRoutingInfo_PolicyRule> PolicyRules
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCRoutingInfo_PolicyRule>(Accessor, "policy_rules"); }
}