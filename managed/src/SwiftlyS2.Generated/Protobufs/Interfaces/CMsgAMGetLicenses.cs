using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetLicenses : ITypedProtobuf<CMsgAMGetLicenses>
{
    static CMsgAMGetLicenses ITypedProtobuf<CMsgAMGetLicenses>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetLicensesImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
}