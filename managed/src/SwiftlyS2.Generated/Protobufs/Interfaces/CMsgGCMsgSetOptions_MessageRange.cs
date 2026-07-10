using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgSetOptions_MessageRange : ITypedProtobuf<CMsgGCMsgSetOptions_MessageRange>
{
    static CMsgGCMsgSetOptions_MessageRange ITypedProtobuf<CMsgGCMsgSetOptions_MessageRange>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgSetOptions_MessageRangeImpl(handle, isManuallyAllocated);

    public uint Low { get; set; }
    public uint High { get; set; }
}