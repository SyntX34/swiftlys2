using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CVacNet_GetReviewerInfo_Request : ITypedProtobuf<CVacNet_GetReviewerInfo_Request>
{
    static CVacNet_GetReviewerInfo_Request ITypedProtobuf<CVacNet_GetReviewerInfo_Request>.Wrap(nint handle, bool isManuallyAllocated) => new CVacNet_GetReviewerInfo_RequestImpl(handle, isManuallyAllocated);

    public uint Appid { get; set; }
}