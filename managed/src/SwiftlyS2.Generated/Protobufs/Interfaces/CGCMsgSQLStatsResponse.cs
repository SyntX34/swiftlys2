using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgSQLStatsResponse : ITypedProtobuf<CGCMsgSQLStatsResponse>
{
    static CGCMsgSQLStatsResponse ITypedProtobuf<CGCMsgSQLStatsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgSQLStatsResponseImpl(handle, isManuallyAllocated);

    public uint Threads { get; set; }
    public uint ThreadsConnected { get; set; }
    public uint ThreadsActive { get; set; }
    public uint OperationsSubmitted { get; set; }
    public uint PreparedStatementsExecuted { get; set; }
    public uint NonPreparedStatementsExecuted { get; set; }
    public uint DeadlockRetries { get; set; }
    public uint OperationsTimedOutInQueue { get; set; }
    public uint Errors { get; set; }
}