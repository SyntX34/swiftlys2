using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgMemCachedDeleteImpl : TypedProtobuf<CGCMsgMemCachedDelete>, CGCMsgMemCachedDelete
{
    public CGCMsgMemCachedDeleteImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<string> Keys
    { get => new ProtobufRepeatedFieldValueType<string>(Accessor, "keys"); }
}