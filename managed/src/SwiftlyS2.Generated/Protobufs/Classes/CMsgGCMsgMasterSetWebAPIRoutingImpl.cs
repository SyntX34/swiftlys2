using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetWebAPIRoutingImpl : TypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting>, CMsgGCMsgMasterSetWebAPIRouting
{
    public CMsgGCMsgMasterSetWebAPIRoutingImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetWebAPIRouting_Entry> Entries
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetWebAPIRouting_Entry>(Accessor, "entries"); }
}