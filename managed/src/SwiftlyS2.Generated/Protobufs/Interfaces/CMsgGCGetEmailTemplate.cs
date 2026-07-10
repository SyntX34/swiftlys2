using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetEmailTemplate : ITypedProtobuf<CMsgGCGetEmailTemplate>
{
    static CMsgGCGetEmailTemplate ITypedProtobuf<CMsgGCGetEmailTemplate>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetEmailTemplateImpl(handle, isManuallyAllocated);

    public uint AppId { get; set; }
    public uint EmailMsgType { get; set; }
    public int EmailLang { get; set; }
    public int EmailFormat { get; set; }
}