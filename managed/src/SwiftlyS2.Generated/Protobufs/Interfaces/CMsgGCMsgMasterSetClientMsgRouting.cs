using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetClientMsgRouting : ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting>
{
    static CMsgGCMsgMasterSetClientMsgRouting ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetClientMsgRoutingImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetClientMsgRouting_Entry> Entries { get; }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCAddressMaskGroup> AddressMaskGroups { get; }
}