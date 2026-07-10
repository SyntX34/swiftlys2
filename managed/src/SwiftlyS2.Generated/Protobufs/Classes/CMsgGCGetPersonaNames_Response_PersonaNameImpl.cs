using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetPersonaNames_Response_PersonaNameImpl : TypedProtobuf<CMsgGCGetPersonaNames_Response_PersonaName>, CMsgGCGetPersonaNames_Response_PersonaName
{
    public CMsgGCGetPersonaNames_Response_PersonaNameImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong Steamid
    { get => Accessor.GetUInt64("steamid"); set => Accessor.SetUInt64("steamid", value); }
    public string PersonaName
    { get => Accessor.GetString("persona_name"); set => Accessor.SetString("persona_name", value); }
}