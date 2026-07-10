using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgMemCachedStatsResponse : ITypedProtobuf<CGCMsgMemCachedStatsResponse>
{
    static CGCMsgMemCachedStatsResponse ITypedProtobuf<CGCMsgMemCachedStatsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgMemCachedStatsResponseImpl(handle, isManuallyAllocated);

    public ulong CurrConnections { get; set; }
    public ulong CmdGet { get; set; }
    public ulong CmdSet { get; set; }
    public ulong CmdFlush { get; set; }
    public ulong GetHits { get; set; }
    public ulong GetMisses { get; set; }
    public ulong DeleteHits { get; set; }
    public ulong DeleteMisses { get; set; }
    public ulong BytesRead { get; set; }
    public ulong BytesWritten { get; set; }
    public ulong LimitMaxbytes { get; set; }
    public ulong CurrItems { get; set; }
    public ulong Evictions { get; set; }
    public ulong Bytes { get; set; }
}