using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgHttpResponse_ResponseHeader : ITypedProtobuf<CMsgHttpResponse_ResponseHeader>
{
    static CMsgHttpResponse_ResponseHeader ITypedProtobuf<CMsgHttpResponse_ResponseHeader>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgHttpResponse_ResponseHeaderImpl(handle, isManuallyAllocated);

    public string Name { get; set; }
    public string Value { get; set; }
}