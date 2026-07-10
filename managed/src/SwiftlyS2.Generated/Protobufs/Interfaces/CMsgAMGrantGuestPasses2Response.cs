using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGrantGuestPasses2Response : ITypedProtobuf<CMsgAMGrantGuestPasses2Response>
{
    static CMsgAMGrantGuestPasses2Response ITypedProtobuf<CMsgAMGrantGuestPasses2Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGrantGuestPasses2ResponseImpl(handle, isManuallyAllocated);

    public int Eresult { get; set; }
    public int PassesGranted { get; set; }
}