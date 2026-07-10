using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCHAccountVacStatusChangeImpl : TypedProtobuf<CMsgGCHAccountVacStatusChange>, CMsgGCHAccountVacStatusChange
{
    public CMsgGCHAccountVacStatusChangeImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamId
    { get => Accessor.GetUInt64("steam_id"); set => Accessor.SetUInt64("steam_id", value); }
    public uint AppId
    { get => Accessor.GetUInt32("app_id"); set => Accessor.SetUInt32("app_id", value); }
    public uint RtimeVacbanStarts
    { get => Accessor.GetUInt32("rtime_vacban_starts"); set => Accessor.SetUInt32("rtime_vacban_starts", value); }
    public bool IsBannedNow
    { get => Accessor.GetBool("is_banned_now"); set => Accessor.SetBool("is_banned_now", value); }
    public bool IsBannedFuture
    { get => Accessor.GetBool("is_banned_future"); set => Accessor.SetBool("is_banned_future", value); }
}