using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMSendEmail_PersonaNameReplacementToken : ITypedProtobuf<CMsgAMSendEmail_PersonaNameReplacementToken>
{
    static CMsgAMSendEmail_PersonaNameReplacementToken ITypedProtobuf<CMsgAMSendEmail_PersonaNameReplacementToken>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMSendEmail_PersonaNameReplacementTokenImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public string TokenName { get; set; }
}