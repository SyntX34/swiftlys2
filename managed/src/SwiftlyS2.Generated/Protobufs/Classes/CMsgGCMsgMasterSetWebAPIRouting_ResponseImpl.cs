using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetWebAPIRouting_ResponseImpl : TypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Response>, CMsgGCMsgMasterSetWebAPIRouting_Response
{
    public CMsgGCMsgMasterSetWebAPIRouting_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
}