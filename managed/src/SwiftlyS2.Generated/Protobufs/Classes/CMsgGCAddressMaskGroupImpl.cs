using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCAddressMaskGroupImpl : TypedProtobuf<CMsgGCAddressMaskGroup>, CMsgGCAddressMaskGroup
{
    public CMsgGCAddressMaskGroupImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CMsgGCAddressMask> Addrs
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCAddressMask>(Accessor, "addrs"); }
}