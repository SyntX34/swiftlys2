using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgPackageLicenseImpl : TypedProtobuf<CMsgPackageLicense>, CMsgPackageLicense
{
    public CMsgPackageLicenseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint PackageId
    { get => Accessor.GetUInt32("package_id"); set => Accessor.SetUInt32("package_id", value); }
    public uint TimeCreated
    { get => Accessor.GetUInt32("time_created"); set => Accessor.SetUInt32("time_created", value); }
    public uint OwnerId
    { get => Accessor.GetUInt32("owner_id"); set => Accessor.SetUInt32("owner_id", value); }
}