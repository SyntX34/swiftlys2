using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMAddFreeLicenseResponse : ITypedProtobuf<CMsgAMAddFreeLicenseResponse>
{
    static CMsgAMAddFreeLicenseResponse ITypedProtobuf<CMsgAMAddFreeLicenseResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMAddFreeLicenseResponseImpl(handle, isManuallyAllocated);

    public int Eresult { get; set; }
    public int PurchaseResultDetail { get; set; }
    public ulong Transid { get; set; }
}