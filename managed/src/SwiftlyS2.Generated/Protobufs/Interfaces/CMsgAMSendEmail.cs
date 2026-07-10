using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMSendEmail : ITypedProtobuf<CMsgAMSendEmail>
{
    static CMsgAMSendEmail ITypedProtobuf<CMsgAMSendEmail>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMSendEmailImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public uint EmailMsgType { get; set; }
    public uint EmailFormat { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_PersonaNameReplacementToken> PersonaNameTokens { get; }
    public uint SourceGc { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgAMSendEmail_ReplacementToken> Tokens { get; }
}