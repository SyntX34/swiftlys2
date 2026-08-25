using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgProtoBufHeaderImpl : TypedProtobuf<CMsgProtoBufHeader>, CMsgProtoBufHeader
{
    public CMsgProtoBufHeaderImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong ClientSteamId
    { get => Accessor.GetUInt64("client_steam_id"); set => Accessor.SetUInt64("client_steam_id", value); }
    public int ClientSessionId
    { get => Accessor.GetInt32("client_session_id"); set => Accessor.SetInt32("client_session_id", value); }
    public uint SourceAppId
    { get => Accessor.GetUInt32("source_app_id"); set => Accessor.SetUInt32("source_app_id", value); }
    public ulong JobIdSource
    { get => Accessor.GetUInt64("job_id_source"); set => Accessor.SetUInt64("job_id_source", value); }
    public ulong JobIdTarget
    { get => Accessor.GetUInt64("job_id_target"); set => Accessor.SetUInt64("job_id_target", value); }
    public string TargetJobName
    { get => Accessor.GetString("target_job_name"); set => Accessor.SetString("target_job_name", value); }
    public int Eresult
    { get => Accessor.GetInt32("eresult"); set => Accessor.SetInt32("eresult", value); }
    public string ErrorMessage
    { get => Accessor.GetString("error_message"); set => Accessor.SetString("error_message", value); }
    public uint Ip
    { get => Accessor.GetUInt32("ip"); set => Accessor.SetUInt32("ip", value); }
    public GCProtoBufMsgSrc GcMsgSrc
    { get => (GCProtoBufMsgSrc)Accessor.GetInt32("gc_msg_src"); set => Accessor.SetInt32("gc_msg_src", (int)value); }
    public uint GcDirIndexSource
    { get => Accessor.GetUInt32("gc_dir_index_source"); set => Accessor.SetUInt32("gc_dir_index_source", value); }
}