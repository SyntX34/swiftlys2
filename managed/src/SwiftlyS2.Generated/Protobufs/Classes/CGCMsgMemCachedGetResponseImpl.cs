using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgMemCachedGetResponseImpl : TypedProtobuf<CGCMsgMemCachedGetResponse>, CGCMsgMemCachedGetResponse
{
    public CGCMsgMemCachedGetResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedGetResponse_ValueTag> Values
    { get => new ProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedGetResponse_ValueTag>(Accessor, "values"); }
}