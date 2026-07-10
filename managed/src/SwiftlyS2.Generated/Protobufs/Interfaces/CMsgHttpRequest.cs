using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgHttpRequest : ITypedProtobuf<CMsgHttpRequest>
{
    static CMsgHttpRequest ITypedProtobuf<CMsgHttpRequest>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgHttpRequestImpl(handle, isManuallyAllocated);

    public uint RequestMethod { get; set; }
    public string Hostname { get; set; }
    public string Url { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_RequestHeader> Headers { get; }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam> GetParams { get; }
    public IProtobufRepeatedFieldSubMessageType<CMsgHttpRequest_QueryParam> PostParams { get; }
    public byte[] Body { get; set; }
    public uint AbsoluteTimeout { get; set; }
}