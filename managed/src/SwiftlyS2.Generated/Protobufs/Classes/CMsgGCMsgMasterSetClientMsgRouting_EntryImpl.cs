using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetClientMsgRouting_EntryImpl : TypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting_Entry>, CMsgGCMsgMasterSetClientMsgRouting_Entry
{
    public CMsgGCMsgMasterSetClientMsgRouting_EntryImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint MsgType
    { get => Accessor.GetUInt32("msg_type"); set => Accessor.SetUInt32("msg_type", value); }
    public CMsgGCRoutingInfo Routing
    { get => new CMsgGCRoutingInfoImpl(NativeNetMessages.GetNestedMessage(Address, "routing"), false); }
}