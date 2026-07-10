using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetWebAPIRouting : ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting>
{
    static CMsgGCMsgMasterSetWebAPIRouting ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetWebAPIRoutingImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetWebAPIRouting_Entry> Entries { get; }
}