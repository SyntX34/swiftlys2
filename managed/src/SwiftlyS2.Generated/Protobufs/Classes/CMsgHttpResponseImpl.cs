using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgHttpResponseImpl : TypedProtobuf<CMsgHttpResponse>, CMsgHttpResponse
{
    public CMsgHttpResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint StatusCode
    { get => Accessor.GetUInt32("status_code"); set => Accessor.SetUInt32("status_code", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpResponse_ResponseHeader> Headers
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgHttpResponse_ResponseHeader>(Accessor, "headers"); }
    public byte[] Body
    { get => Accessor.GetBytes("body"); set => Accessor.SetBytes("body", value); }
}