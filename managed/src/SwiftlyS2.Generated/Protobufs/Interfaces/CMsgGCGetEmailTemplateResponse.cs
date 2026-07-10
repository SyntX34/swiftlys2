using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetEmailTemplateResponse : ITypedProtobuf<CMsgGCGetEmailTemplateResponse>
{
    static CMsgGCGetEmailTemplateResponse ITypedProtobuf<CMsgGCGetEmailTemplateResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetEmailTemplateResponseImpl(handle, isManuallyAllocated);

    public uint Eresult { get; set; }
    public bool TemplateExists { get; set; }
    public string Template { get; set; }
}