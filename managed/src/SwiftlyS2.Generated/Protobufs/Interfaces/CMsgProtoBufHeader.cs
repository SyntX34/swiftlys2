using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgProtoBufHeader : ITypedProtobuf<CMsgProtoBufHeader>
{
    static CMsgProtoBufHeader ITypedProtobuf<CMsgProtoBufHeader>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgProtoBufHeaderImpl(handle, isManuallyAllocated);

    public ulong ClientSteamId { get; set; }
    public int ClientSessionId { get; set; }
    public uint SourceAppId { get; set; }
    public ulong JobIdSource { get; set; }
    public ulong JobIdTarget { get; set; }
    public string TargetJobName { get; set; }
    public int Eresult { get; set; }
    public string ErrorMessage { get; set; }
    public uint Ip { get; set; }
    public GCProtoBufMsgSrc GcMsgSrc { get; set; }
    public uint GcDirIndexSource { get; set; }
}