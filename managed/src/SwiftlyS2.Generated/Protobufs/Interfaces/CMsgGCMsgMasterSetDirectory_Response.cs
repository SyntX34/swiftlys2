using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetDirectory_Response : ITypedProtobuf<CMsgGCMsgMasterSetDirectory_Response>
{
    static CMsgGCMsgMasterSetDirectory_Response ITypedProtobuf<CMsgGCMsgMasterSetDirectory_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetDirectory_ResponseImpl(handle, isManuallyAllocated);

    public int Eresult { get; set; }
    public string Message { get; set; }
}