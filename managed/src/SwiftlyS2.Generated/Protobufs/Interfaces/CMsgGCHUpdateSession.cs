using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCHUpdateSession : ITypedProtobuf<CMsgGCHUpdateSession>
{
    static CMsgGCHUpdateSession ITypedProtobuf<CMsgGCHUpdateSession>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCHUpdateSessionImpl(handle, isManuallyAllocated);

    public ulong SteamId { get; set; }
    public uint AppId { get; set; }
    public bool Online { get; set; }
    public ulong ServerSteamId { get; set; }
    public uint ServerAddr { get; set; }
    public uint ServerPort { get; set; }
    public uint OsType { get; set; }
    public uint ClientAddr { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCHUpdateSession_ExtraField> ExtraFields { get; }
    public ulong OwnerId { get; set; }
    public uint CmSessionSysid { get; set; }
    public uint CmSessionIdentifier { get; set; }
    public IProtobufRepeatedFieldValueType<uint> DepotIds { get; }
}