using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgWebAPIJobRequestForwardResponseImpl : TypedProtobuf<CMsgGCMsgWebAPIJobRequestForwardResponse>, CMsgGCMsgWebAPIJobRequestForwardResponse
{
    public CMsgGCMsgWebAPIJobRequestForwardResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint DirIndex
    { get => Accessor.GetUInt32("dir_index"); set => Accessor.SetUInt32("dir_index", value); }
}