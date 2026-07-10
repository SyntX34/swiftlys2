using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetWebAPIRouting_Response : ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Response>
{
    static CMsgGCMsgMasterSetWebAPIRouting_Response ITypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetWebAPIRouting_ResponseImpl(handle, isManuallyAllocated);

    public int Eresult { get; set; }
}