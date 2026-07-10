using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetClientMsgRouting_Response : ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting_Response>
{
    static CMsgGCMsgMasterSetClientMsgRouting_Response ITypedProtobuf<CMsgGCMsgMasterSetClientMsgRouting_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetClientMsgRouting_ResponseImpl(handle, isManuallyAllocated);

    public int Eresult { get; set; }
}