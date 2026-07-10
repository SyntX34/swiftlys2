using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgGetIPLocationResponse : ITypedProtobuf<CGCMsgGetIPLocationResponse>
{
    static CGCMsgGetIPLocationResponse ITypedProtobuf<CGCMsgGetIPLocationResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgGetIPLocationResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldSubMessageType<CIPLocationInfo> Infos { get; }
}