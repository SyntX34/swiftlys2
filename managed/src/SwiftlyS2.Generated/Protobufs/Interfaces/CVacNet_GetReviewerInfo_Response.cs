using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CVacNet_GetReviewerInfo_Response : ITypedProtobuf<CVacNet_GetReviewerInfo_Response>
{
    static CVacNet_GetReviewerInfo_Response ITypedProtobuf<CVacNet_GetReviewerInfo_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CVacNet_GetReviewerInfo_ResponseImpl(handle, isManuallyAllocated);

    public CVacnetReviewerInfo ReviewerInfo { get; }
}