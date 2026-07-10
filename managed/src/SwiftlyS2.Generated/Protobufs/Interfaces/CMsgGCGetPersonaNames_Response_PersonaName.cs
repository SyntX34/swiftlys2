using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetPersonaNames_Response_PersonaName : ITypedProtobuf<CMsgGCGetPersonaNames_Response_PersonaName>
{
    static CMsgGCGetPersonaNames_Response_PersonaName ITypedProtobuf<CMsgGCGetPersonaNames_Response_PersonaName>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetPersonaNames_Response_PersonaNameImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public string PersonaName { get; set; }
}