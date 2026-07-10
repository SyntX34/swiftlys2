using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CIPLocationInfo : ITypedProtobuf<CIPLocationInfo>
{
    static CIPLocationInfo ITypedProtobuf<CIPLocationInfo>.Wrap(nint handle, bool isManuallyAllocated) => new CIPLocationInfoImpl(handle, isManuallyAllocated);

    public uint Ip { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
}