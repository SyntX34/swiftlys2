using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMSendEmail_PersonaNameReplacementTokenImpl : TypedProtobuf<CMsgAMSendEmail_PersonaNameReplacementToken>, CMsgAMSendEmail_PersonaNameReplacementToken
{
    public CMsgAMSendEmail_PersonaNameReplacementTokenImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong Steamid
    { get => Accessor.GetUInt64("steamid"); set => Accessor.SetUInt64("steamid", value); }
    public string TokenName
    { get => Accessor.GetString("token_name"); set => Accessor.SetString("token_name", value); }
}