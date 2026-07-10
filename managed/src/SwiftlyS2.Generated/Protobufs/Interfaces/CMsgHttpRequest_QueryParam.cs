using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgHttpRequest_QueryParam : ITypedProtobuf<CMsgHttpRequest_QueryParam>
{
    static CMsgHttpRequest_QueryParam ITypedProtobuf<CMsgHttpRequest_QueryParam>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgHttpRequest_QueryParamImpl(handle, isManuallyAllocated);

    public string Name { get; set; }
    public byte[] Value { get; set; }
}