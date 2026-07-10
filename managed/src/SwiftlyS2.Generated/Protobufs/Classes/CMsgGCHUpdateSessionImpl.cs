using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCHUpdateSessionImpl : TypedProtobuf<CMsgGCHUpdateSession>, CMsgGCHUpdateSession
{
    public CMsgGCHUpdateSessionImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamId
    { get => Accessor.GetUInt64("steam_id"); set => Accessor.SetUInt64("steam_id", value); }
    public uint AppId
    { get => Accessor.GetUInt32("app_id"); set => Accessor.SetUInt32("app_id", value); }
    public bool Online
    { get => Accessor.GetBool("online"); set => Accessor.SetBool("online", value); }
    public ulong ServerSteamId
    { get => Accessor.GetUInt64("server_steam_id"); set => Accessor.SetUInt64("server_steam_id", value); }
    public uint ServerAddr
    { get => Accessor.GetUInt32("server_addr"); set => Accessor.SetUInt32("server_addr", value); }
    public uint ServerPort
    { get => Accessor.GetUInt32("server_port"); set => Accessor.SetUInt32("server_port", value); }
    public uint OsType
    { get => Accessor.GetUInt32("os_type"); set => Accessor.SetUInt32("os_type", value); }
    public uint ClientAddr
    { get => Accessor.GetUInt32("client_addr"); set => Accessor.SetUInt32("client_addr", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCHUpdateSession_ExtraField> ExtraFields
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCHUpdateSession_ExtraField>(Accessor, "extra_fields"); }
    public ulong OwnerId
    { get => Accessor.GetUInt64("owner_id"); set => Accessor.SetUInt64("owner_id", value); }
    public uint CmSessionSysid
    { get => Accessor.GetUInt32("cm_session_sysid"); set => Accessor.SetUInt32("cm_session_sysid", value); }
    public uint CmSessionIdentifier
    { get => Accessor.GetUInt32("cm_session_identifier"); set => Accessor.SetUInt32("cm_session_identifier", value); }
    public IProtobufRepeatedFieldValueType<uint> DepotIds
    { get => new ProtobufRepeatedFieldValueType<uint>(Accessor, "depot_ids"); }
}