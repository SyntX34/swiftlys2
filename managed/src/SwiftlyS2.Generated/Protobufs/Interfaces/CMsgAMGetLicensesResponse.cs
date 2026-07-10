using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMGetLicensesResponse : ITypedProtobuf<CMsgAMGetLicensesResponse>
{
    static CMsgAMGetLicensesResponse ITypedProtobuf<CMsgAMGetLicensesResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMGetLicensesResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CMsgPackageLicense> License { get; }
    public uint Result { get; set; }
}