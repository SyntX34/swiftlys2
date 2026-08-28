using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CVacNet_GetReviewerInfo_ResponseImpl : TypedProtobuf<CVacNet_GetReviewerInfo_Response>, CVacNet_GetReviewerInfo_Response
{
    public CVacNet_GetReviewerInfo_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public CVacnetReviewerInfo ReviewerInfo
    { get => new CVacnetReviewerInfoImpl(NativeNetMessages.GetNestedMessage(Address, "reviewer_info"), false); }
}