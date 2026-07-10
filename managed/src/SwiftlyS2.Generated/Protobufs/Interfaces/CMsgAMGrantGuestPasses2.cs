using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGrantGuestPasses2 : ITypedProtobuf<CMsgAMGrantGuestPasses2>
{
    static CMsgAMGrantGuestPasses2 ITypedProtobuf<CMsgAMGrantGuestPasses2>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGrantGuestPasses2Impl(handle, isManuallyAllocated);

    public ulong SteamId { get; set; }
    public uint PackageId { get; set; }
    public int PassesToGrant { get; set; }
    public int DaysToExpiration { get; set; }
    public int Action { get; set; }
}