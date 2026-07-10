using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgSQLStatsResponseImpl : TypedProtobuf<CGCMsgSQLStatsResponse>, CGCMsgSQLStatsResponse
{
    public CGCMsgSQLStatsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Threads
    { get => Accessor.GetUInt32("threads"); set => Accessor.SetUInt32("threads", value); }
    public uint ThreadsConnected
    { get => Accessor.GetUInt32("threads_connected"); set => Accessor.SetUInt32("threads_connected", value); }
    public uint ThreadsActive
    { get => Accessor.GetUInt32("threads_active"); set => Accessor.SetUInt32("threads_active", value); }
    public uint OperationsSubmitted
    { get => Accessor.GetUInt32("operations_submitted"); set => Accessor.SetUInt32("operations_submitted", value); }
    public uint PreparedStatementsExecuted
    { get => Accessor.GetUInt32("prepared_statements_executed"); set => Accessor.SetUInt32("prepared_statements_executed", value); }
    public uint NonPreparedStatementsExecuted
    { get => Accessor.GetUInt32("non_prepared_statements_executed"); set => Accessor.SetUInt32("non_prepared_statements_executed", value); }
    public uint DeadlockRetries
    { get => Accessor.GetUInt32("deadlock_retries"); set => Accessor.SetUInt32("deadlock_retries", value); }
    public uint OperationsTimedOutInQueue
    { get => Accessor.GetUInt32("operations_timed_out_in_queue"); set => Accessor.SetUInt32("operations_timed_out_in_queue", value); }
    public uint Errors
    { get => Accessor.GetUInt32("errors"); set => Accessor.SetUInt32("errors", value); }
}