using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetEmailTemplateResponseImpl : TypedProtobuf<CMsgGCGetEmailTemplateResponse>, CMsgGCGetEmailTemplateResponse
{
    public CMsgGCGetEmailTemplateResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Eresult
    { get => Accessor.GetUInt32("eresult"); set => Accessor.SetUInt32("eresult", value); }
    public bool TemplateExists
    { get => Accessor.GetBool("template_exists"); set => Accessor.SetBool("template_exists", value); }
    public string Template
    { get => Accessor.GetString("template"); set => Accessor.SetString("template", value); }
}