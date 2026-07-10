using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMSendEmail_ReplacementToken : ITypedProtobuf<CMsgAMSendEmail_ReplacementToken>
{
    static CMsgAMSendEmail_ReplacementToken ITypedProtobuf<CMsgAMSendEmail_ReplacementToken>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMSendEmail_ReplacementTokenImpl(handle, isManuallyAllocated);

    public string TokenName { get; set; }
    public string TokenValue { get; set; }
}