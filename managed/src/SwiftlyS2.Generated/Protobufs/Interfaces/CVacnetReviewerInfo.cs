using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CVacnetReviewerInfo : ITypedProtobuf<CVacnetReviewerInfo>
{
    static CVacnetReviewerInfo ITypedProtobuf<CVacnetReviewerInfo>.Wrap(nint handle, bool isManuallyAllocated) => new CVacnetReviewerInfoImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<string> Permissions { get; }
}