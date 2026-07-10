using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgMemCachedStatsResponseImpl : TypedProtobuf<CGCMsgMemCachedStatsResponse>, CGCMsgMemCachedStatsResponse
{
    public CGCMsgMemCachedStatsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong CurrConnections
    { get => Accessor.GetUInt64("curr_connections"); set => Accessor.SetUInt64("curr_connections", value); }
    public ulong CmdGet
    { get => Accessor.GetUInt64("cmd_get"); set => Accessor.SetUInt64("cmd_get", value); }
    public ulong CmdSet
    { get => Accessor.GetUInt64("cmd_set"); set => Accessor.SetUInt64("cmd_set", value); }
    public ulong CmdFlush
    { get => Accessor.GetUInt64("cmd_flush"); set => Accessor.SetUInt64("cmd_flush", value); }
    public ulong GetHits
    { get => Accessor.GetUInt64("get_hits"); set => Accessor.SetUInt64("get_hits", value); }
    public ulong GetMisses
    { get => Accessor.GetUInt64("get_misses"); set => Accessor.SetUInt64("get_misses", value); }
    public ulong DeleteHits
    { get => Accessor.GetUInt64("delete_hits"); set => Accessor.SetUInt64("delete_hits", value); }
    public ulong DeleteMisses
    { get => Accessor.GetUInt64("delete_misses"); set => Accessor.SetUInt64("delete_misses", value); }
    public ulong BytesRead
    { get => Accessor.GetUInt64("bytes_read"); set => Accessor.SetUInt64("bytes_read", value); }
    public ulong BytesWritten
    { get => Accessor.GetUInt64("bytes_written"); set => Accessor.SetUInt64("bytes_written", value); }
    public ulong LimitMaxbytes
    { get => Accessor.GetUInt64("limit_maxbytes"); set => Accessor.SetUInt64("limit_maxbytes", value); }
    public ulong CurrItems
    { get => Accessor.GetUInt64("curr_items"); set => Accessor.SetUInt64("curr_items", value); }
    public ulong Evictions
    { get => Accessor.GetUInt64("evictions"); set => Accessor.SetUInt64("evictions", value); }
    public ulong Bytes
    { get => Accessor.GetUInt64("bytes"); set => Accessor.SetUInt64("bytes", value); }
}