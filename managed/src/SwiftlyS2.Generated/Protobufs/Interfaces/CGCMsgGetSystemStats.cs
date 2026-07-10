using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgGetSystemStats : ITypedProtobuf<CGCMsgGetSystemStats>
{
    static CGCMsgGetSystemStats ITypedProtobuf<CGCMsgGetSystemStats>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgGetSystemStatsImpl(handle, isManuallyAllocated);

}