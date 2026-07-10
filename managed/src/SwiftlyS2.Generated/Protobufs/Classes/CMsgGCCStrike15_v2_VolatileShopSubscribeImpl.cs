using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCCStrike15_v2_VolatileShopSubscribeImpl : TypedProtobuf<CMsgGCCStrike15_v2_VolatileShopSubscribe>, CMsgGCCStrike15_v2_VolatileShopSubscribe
{
    public CMsgGCCStrike15_v2_VolatileShopSubscribeImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Defidx
    { get => Accessor.GetUInt32("defidx"); set => Accessor.SetUInt32("defidx", value); }
    public ulong Psid
    { get => Accessor.GetUInt64("psid"); set => Accessor.SetUInt64("psid", value); }
    public uint Upnext
    { get => Accessor.GetUInt32("upnext"); set => Accessor.SetUInt32("upnext", value); }
    public uint Gctime
    { get => Accessor.GetUInt32("gctime"); set => Accessor.SetUInt32("gctime", value); }
    public byte[] Payload
    { get => Accessor.GetBytes("payload"); set => Accessor.SetBytes("payload", value); }
}