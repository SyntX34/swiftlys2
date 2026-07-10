using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMSendEmailImpl : TypedProtobuf<CMsgAMSendEmail>, CMsgAMSendEmail
{
    public CMsgAMSendEmailImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong Steamid
    { get => Accessor.GetUInt64("steamid"); set => Accessor.SetUInt64("steamid", value); }
    public uint EmailMsgType
    { get => Accessor.GetUInt32("email_msg_type"); set => Accessor.SetUInt32("email_msg_type", value); }
    public uint EmailFormat
    { get => Accessor.GetUInt32("email_format"); set => Accessor.SetUInt32("email_format", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_PersonaNameReplacementToken> PersonaNameTokens
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_PersonaNameReplacementToken>(Accessor, "persona_name_tokens"); }
    public uint SourceGc
    { get => Accessor.GetUInt32("source_gc"); set => Accessor.SetUInt32("source_gc", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_ReplacementToken> Tokens
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_ReplacementToken>(Accessor, "tokens"); }
}