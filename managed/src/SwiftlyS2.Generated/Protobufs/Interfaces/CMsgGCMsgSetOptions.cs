using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgSetOptions : ITypedProtobuf<CMsgGCMsgSetOptions>
{
    static CMsgGCMsgSetOptions ITypedProtobuf<CMsgGCMsgSetOptions>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgSetOptionsImpl(handle, isManuallyAllocated);

    public CMsgGCMsgSetOptions_Option Options { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgSetOptions_MessageRange> ClientMsgRanges { get; }
}