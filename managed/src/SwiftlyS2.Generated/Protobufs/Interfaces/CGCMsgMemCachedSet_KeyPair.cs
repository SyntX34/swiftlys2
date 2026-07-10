using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedSet_KeyPair : ITypedProtobuf<CGCMsgMemCachedSet_KeyPair>
{
    static CGCMsgMemCachedSet_KeyPair ITypedProtobuf<CGCMsgMemCachedSet_KeyPair>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedSet_KeyPairImpl(handle, isManuallyAllocated);

    public string Name { get; set; }
    public byte[] Value { get; set; }
}