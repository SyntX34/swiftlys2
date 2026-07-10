using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMSendEmail_ReplacementTokenImpl : TypedProtobuf<CMsgAMSendEmail_ReplacementToken>, CMsgAMSendEmail_ReplacementToken
{
    public CMsgAMSendEmail_ReplacementTokenImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public string TokenName
    { get => Accessor.GetString("token_name"); set => Accessor.SetString("token_name", value); }
    public string TokenValue
    { get => Accessor.GetString("token_value"); set => Accessor.SetString("token_value", value); }
}