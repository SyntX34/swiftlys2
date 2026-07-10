using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMAddFreeLicense : ITypedProtobuf<CMsgAMAddFreeLicense>
{
    static CMsgAMAddFreeLicense ITypedProtobuf<CMsgAMAddFreeLicense>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMAddFreeLicenseImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public uint IpPublic { get; set; }
    public uint Packageid { get; set; }
    public string StoreCountryCode { get; set; }
}