using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMAddFreeLicenseImpl : TypedProtobuf<CMsgAMAddFreeLicense>, CMsgAMAddFreeLicense
{
    public CMsgAMAddFreeLicenseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong Steamid
    { get => Accessor.GetUInt64("steamid"); set => Accessor.SetUInt64("steamid", value); }
    public uint IpPublic
    { get => Accessor.GetUInt32("ip_public"); set => Accessor.SetUInt32("ip_public", value); }
    public uint Packageid
    { get => Accessor.GetUInt32("packageid"); set => Accessor.SetUInt32("packageid", value); }
    public string StoreCountryCode
    { get => Accessor.GetString("store_country_code"); set => Accessor.SetString("store_country_code", value); }
}