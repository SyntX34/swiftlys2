using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgPackageLicense : ITypedProtobuf<CMsgPackageLicense>
{
    static CMsgPackageLicense ITypedProtobuf<CMsgPackageLicense>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgPackageLicenseImpl(handle, isManuallyAllocated);

    public uint PackageId { get; set; }
    public uint TimeCreated { get; set; }
    public uint OwnerId { get; set; }
}