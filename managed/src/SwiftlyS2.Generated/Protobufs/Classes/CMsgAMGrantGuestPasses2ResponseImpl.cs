using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGrantGuestPasses2ResponseImpl : TypedProtobuf<CMsgAMGrantGuestPasses2Response>, CMsgAMGrantGuestPasses2Response
{
    public CMsgAMGrantGuestPasses2ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
    public int PassesGranted
    { get => Accessor.GetInt32("passes_granted"); set => Accessor.SetInt32("passes_granted", value); }
}