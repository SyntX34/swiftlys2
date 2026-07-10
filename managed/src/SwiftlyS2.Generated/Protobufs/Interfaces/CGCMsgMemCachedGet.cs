using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedGet : ITypedProtobuf<CGCMsgMemCachedGet>
{
    static CGCMsgMemCachedGet ITypedProtobuf<CGCMsgMemCachedGet>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedGetImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<string> Keys { get; }
}