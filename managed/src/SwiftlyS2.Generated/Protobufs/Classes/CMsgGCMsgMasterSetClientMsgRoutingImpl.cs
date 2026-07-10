using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetClientMsgRoutingImpl : TypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting>, CMsgGCMsgMasterSetClientMsgRouting
{
    public CMsgGCMsgMasterSetClientMsgRoutingImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetClientMsgRouting_Entry> Entries
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetClientMsgRouting_Entry>(Accessor, "entries"); }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCAddressMaskGroup> AddressMaskGroups
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCAddressMaskGroup>(Accessor, "address_mask_groups"); }
}