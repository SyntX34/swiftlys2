using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMAddFreeLicenseResponseImpl : TypedProtobuf<CMsgAMAddFreeLicenseResponse>, CMsgAMAddFreeLicenseResponse
{
    public CMsgAMAddFreeLicenseResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
    public int PurchaseResultDetail
    { get => Accessor.GetInt32("purchase_result_detail"); set => Accessor.SetInt32("purchase_result_detail", value); }
    public ulong Transid
    { get => Accessor.GetUInt64("transid"); set => Accessor.SetUInt64("transid", value); }
}