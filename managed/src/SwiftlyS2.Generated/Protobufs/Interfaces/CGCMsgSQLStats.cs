using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgSQLStats : ITypedProtobuf<CGCMsgSQLStats>
{
    static CGCMsgSQLStats ITypedProtobuf<CGCMsgSQLStats>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgSQLStatsImpl(handle, isManuallyAllocated);

    public uint SchemaCatalog { get; set; }
}