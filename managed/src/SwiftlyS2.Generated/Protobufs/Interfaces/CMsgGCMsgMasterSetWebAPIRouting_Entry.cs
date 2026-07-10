using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetWebAPIRouting_Entry : ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Entry>
{
    static CMsgGCMsgMasterSetWebAPIRouting_Entry ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Entry>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetWebAPIRouting_EntryImpl(handle, isManuallyAllocated);

    public string InterfaceName { get; set; }
    public string MethodName { get; set; }
    public CMsgGCRoutingInfo Routing { get; }
}