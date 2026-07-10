using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedGetResponse_ValueTag : ITypedProtobuf<CGCMsgMemCachedGetResponse_ValueTag>
{
    static CGCMsgMemCachedGetResponse_ValueTag ITypedProtobuf<CGCMsgMemCachedGetResponse_ValueTag>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedGetResponse_ValueTagImpl(handle, isManuallyAllocated);

    public bool Found { get; set; }
    public byte[] Value { get; set; }
}