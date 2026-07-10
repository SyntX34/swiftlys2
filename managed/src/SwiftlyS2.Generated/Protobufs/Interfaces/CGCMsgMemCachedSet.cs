using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedSet : ITypedProtobuf<CGCMsgMemCachedSet>
{
    static CGCMsgMemCachedSet ITypedProtobuf<CGCMsgMemCachedSet>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedSetImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedSet_KeyPair> Keys { get; }
}