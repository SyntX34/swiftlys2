using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetDirectory_ResponseImpl : TypedProtobuf<CMsgGCMsgMasterSetDirectory_Response>, CMsgGCMsgMasterSetDirectory_Response
{
    public CMsgGCMsgMasterSetDirectory_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
    public string Message
    { get => Accessor.GetString("message"); set => Accessor.SetString("message", value); }
}