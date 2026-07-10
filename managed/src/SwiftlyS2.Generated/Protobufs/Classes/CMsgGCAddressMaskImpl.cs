using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCAddressMaskImpl : TypedProtobuf<CMsgGCAddressMask>, CMsgGCAddressMask
{
    public CMsgGCAddressMaskImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Ipv4
    { get => Accessor.GetUInt32("ipv4"); set => Accessor.SetUInt32("ipv4", value); }
    public uint Maskbits
    { get => Accessor.GetUInt32("maskbits"); set => Accessor.SetUInt32("maskbits", value); }
}