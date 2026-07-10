using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetEmailTemplateImpl : TypedProtobuf<CMsgGCGetEmailTemplate>, CMsgGCGetEmailTemplate
{
    public CMsgGCGetEmailTemplateImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint AppId
    { get => Accessor.GetUInt32("app_id"); set => Accessor.SetUInt32("app_id", value); }
    public uint EmailMsgType
    { get => Accessor.GetUInt32("email_msg_type"); set => Accessor.SetUInt32("email_msg_type", value); }
    public int EmailLang
    { get => Accessor.GetInt32("email_lang"); set => Accessor.SetInt32("email_lang", value); }
    public int EmailFormat
    { get => Accessor.GetInt32("email_format"); set => Accessor.SetInt32("email_format", value); }
}