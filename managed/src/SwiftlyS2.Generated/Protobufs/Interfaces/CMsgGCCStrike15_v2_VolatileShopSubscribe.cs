using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCCStrike15_v2_VolatileShopSubscribe : ITypedProtobuf<CMsgGCCStrike15_v2_VolatileShopSubscribe>
{
    static CMsgGCCStrike15_v2_VolatileShopSubscribe ITypedProtobuf<CMsgGCCStrike15_v2_VolatileShopSubscribe>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCCStrike15_v2_VolatileShopSubscribeImpl(handle, isManuallyAllocated);

    public uint Defidx { get; set; }
    public ulong Psid { get; set; }
    public uint Upnext { get; set; }
    public uint Gctime { get; set; }
    public byte[] Payload { get; set; }
}