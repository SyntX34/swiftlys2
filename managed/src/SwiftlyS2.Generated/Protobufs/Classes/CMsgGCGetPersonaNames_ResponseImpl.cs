using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetPersonaNames_ResponseImpl : TypedProtobuf<CMsgGCGetPersonaNames_Response>, CMsgGCGetPersonaNames_Response
{
    public CMsgGCGetPersonaNames_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CMsgGCGetPersonaNames_Response_PersonaName> SucceededLookups
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCGetPersonaNames_Response_PersonaName>(Accessor, "succeeded_lookups"); }
    public IProtobufRepeatedFieldValueType<ulong> FailedLookupSteamids
    { get => new ProtobufRepeatedFieldValueType<ulong>(Accessor, "failed_lookup_steamids"); }
}