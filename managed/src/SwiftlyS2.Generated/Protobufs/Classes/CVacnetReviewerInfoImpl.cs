using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CVacnetReviewerInfoImpl : TypedProtobuf<CVacnetReviewerInfo>, CVacnetReviewerInfo
{
    public CVacnetReviewerInfoImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<string> Permissions
    { get => new ProtobufRepeatedFieldValueType<string>(Accessor, "permissions"); }
}