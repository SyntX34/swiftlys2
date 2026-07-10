using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgDPPartnerMicroTxnsResponseImpl : TypedProtobuf<CMsgDPPartnerMicroTxnsResponse>, CMsgDPPartnerMicroTxnsResponse
{
    public CMsgDPPartnerMicroTxnsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Eresult
    { get => Accessor.GetUInt32("eresult"); set => Accessor.SetUInt32("eresult", value); }
    public CMsgDPPartnerMicroTxnsResponse_EErrorCode Eerrorcode
    { get => (CMsgDPPartnerMicroTxnsResponse_EErrorCode)Accessor.GetInt32("eerrorcode"); set => Accessor.SetInt32("eerrorcode", (int)value); }
}