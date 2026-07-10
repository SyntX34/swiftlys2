using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetPersonaNames_Response : ITypedProtobuf<CMsgGCGetPersonaNames_Response>
{
    static CMsgGCGetPersonaNames_Response ITypedProtobuf<CMsgGCGetPersonaNames_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetPersonaNames_ResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CMsgGCGetPersonaNames_Response_PersonaName> SucceededLookups { get; }
    public IProtobufRepeatedFieldValueType<ulong> FailedLookupSteamids { get; }
}