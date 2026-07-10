using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CIPLocationInfoImpl : TypedProtobuf<CIPLocationInfo>, CIPLocationInfo
{
    public CIPLocationInfoImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Ip
    { get => Accessor.GetUInt32("ip"); set => Accessor.SetUInt32("ip", value); }
    public float Latitude
    { get => Accessor.GetFloat("latitude"); set => Accessor.SetFloat("latitude", value); }
    public float Longitude
    { get => Accessor.GetFloat("longitude"); set => Accessor.SetFloat("longitude", value); }
    public string Country
    { get => Accessor.GetString("country"); set => Accessor.SetString("country", value); }
    public string State
    { get => Accessor.GetString("state"); set => Accessor.SetString("state", value); }
    public string City
    { get => Accessor.GetString("city"); set => Accessor.SetString("city", value); }
}