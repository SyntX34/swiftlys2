using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgHttpRequestImpl : TypedProtobuf<CMsgHttpRequest>, CMsgHttpRequest
{
    public CMsgHttpRequestImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint RequestMethod
    { get => Accessor.GetUInt32("request_method"); set => Accessor.SetUInt32("request_method", value); }
    public string Hostname
    { get => Accessor.GetString("hostname"); set => Accessor.SetString("hostname", value); }
    public string Url
    { get => Accessor.GetString("url"); set => Accessor.SetString("url", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_RequestHeader> Headers
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_RequestHeader>(Accessor, "headers"); }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam> GetParams
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam>(Accessor, "get_params"); }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam> PostParams
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam>(Accessor, "post_params"); }
    public byte[] Body
    { get => Accessor.GetBytes("body"); set => Accessor.SetBytes("body", value); }
    public uint AbsoluteTimeout
    { get => Accessor.GetUInt32("absolute_timeout"); set => Accessor.SetUInt32("absolute_timeout", value); }
}