using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgMemCachedGetResponse_ValueTagImpl : TypedProtobuf<CGCMsgMemCachedGetResponse_ValueTag>, CGCMsgMemCachedGetResponse_ValueTag
{
    public CGCMsgMemCachedGetResponse_ValueTagImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public bool Found
    { get => Accessor.GetBool("found"); set => Accessor.SetBool("found", value); }
    public byte[] Value
    { get => Accessor.GetBytes("value"); set => Accessor.SetBytes("value", value); }
}