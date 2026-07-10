using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgMemCachedSetImpl : TypedProtobuf<CGCMsgMemCachedSet>, CGCMsgMemCachedSet
{
    public CGCMsgMemCachedSetImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedSet_KeyPair> Keys
    { get => new ProtobufRepeatedFieldSubMessageType<CGCMsgMemCachedSet_KeyPair>(Accessor, "keys"); }
}