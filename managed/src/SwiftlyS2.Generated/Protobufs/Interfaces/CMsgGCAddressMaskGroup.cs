using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCAddressMaskGroup : ITypedProtobuf<CMsgGCAddressMaskGroup>
{
    static CMsgGCAddressMaskGroup ITypedProtobuf<CMsgGCAddressMaskGroup>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCAddressMaskGroupImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CMsgGCAddressMask> Addrs { get; }
}