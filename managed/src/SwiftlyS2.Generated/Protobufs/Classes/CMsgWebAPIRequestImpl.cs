using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgWebAPIRequestImpl : TypedProtobuf<CMsgWebAPIRequest>, CMsgWebAPIRequest
{
    public CMsgWebAPIRequestImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public string InterfaceName
    { get => Accessor.GetString("interface_name"); set => Accessor.SetString("interface_name", value); }
    public string MethodName
    { get => Accessor.GetString("method_name"); set => Accessor.SetString("method_name", value); }
    public uint Version
    { get => Accessor.GetUInt32("version"); set => Accessor.SetUInt32("version", value); }
    public CMsgWebAPIKey ApiKey
    { get => new CMsgWebAPIKeyImpl(NativeNetMessages.GetNestedMessage(Address, "api_key"), false); }
    public CMsgHttpRequest Request
    { get => new CMsgHttpRequestImpl(NativeNetMessages.GetNestedMessage(Address, "request"), false); }
    public uint RoutingAppId
    { get => Accessor.GetUInt32("routing_app_id"); set => Accessor.SetUInt32("routing_app_id", value); }
}