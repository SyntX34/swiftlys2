using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetClientMsgRouting_Entry : ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting_Entry>
{
    static CMsgGCMsgMasterSetClientMsgRouting_Entry ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting_Entry>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetClientMsgRouting_EntryImpl(handle, isManuallyAllocated);

    public uint MsgType { get; set; }
    public CMsgGCRoutingInfo Routing { get; }
}