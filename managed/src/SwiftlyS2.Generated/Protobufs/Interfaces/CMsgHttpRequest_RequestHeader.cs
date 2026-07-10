using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgHttpRequest_RequestHeader : ITypedProtobuf<CMsgHttpRequest_RequestHeader>
{
    static CMsgHttpRequest_RequestHeader ITypedProtobuf<CMsgHttpRequest_RequestHeader>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgHttpRequest_RequestHeaderImpl(handle, isManuallyAllocated);

    public string Name { get; set; }
    public string Value { get; set; }
}