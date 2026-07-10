using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgHttpResponse : ITypedProtobuf<CMsgHttpResponse>
{
    static CMsgHttpResponse ITypedProtobuf<CMsgHttpResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgHttpResponseImpl(handle, isManuallyAllocated);

    public uint StatusCode { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpResponse_ResponseHeader> Headers { get; }
    public byte[] Body { get; set; }
}