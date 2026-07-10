using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedDelete : ITypedProtobuf<CGCMsgMemCachedDelete>
{
    static CGCMsgMemCachedDelete ITypedProtobuf<CGCMsgMemCachedDelete>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedDeleteImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<string> Keys { get; }
}