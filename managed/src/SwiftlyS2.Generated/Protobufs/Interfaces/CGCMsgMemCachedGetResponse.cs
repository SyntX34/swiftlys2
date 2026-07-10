using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedGetResponse : ITypedProtobuf<CGCMsgMemCachedGetResponse>
{
    static CGCMsgMemCachedGetResponse ITypedProtobuf<CGCMsgMemCachedGetResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedGetResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedGetResponse_ValueTag> Values { get; }
}