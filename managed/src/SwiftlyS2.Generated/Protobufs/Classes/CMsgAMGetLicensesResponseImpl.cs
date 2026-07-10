using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGetLicensesResponseImpl : TypedProtobuf<CMsgAMGetLicensesResponse>, CMsgAMGetLicensesResponse
{
    public CMsgAMGetLicensesResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CMsgPackageLicense> License
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgPackageLicense>(Accessor, "license"); }
    public uint Result
    { get => Accessor.GetUInt32("result"); set => Accessor.SetUInt32("result", value); }
}