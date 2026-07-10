using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCAddressMask : ITypedProtobuf<CMsgGCAddressMask>
{
    static CMsgGCAddressMask ITypedProtobuf<CMsgGCAddressMask>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCAddressMaskImpl(handle, isManuallyAllocated);

    public uint Ipv4 { get; set; }
    public uint Maskbits { get; set; }
}