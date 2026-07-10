using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCCStrike15_v2_SetClanIdImpl : TypedProtobuf<CMsgGCCStrike15_v2_SetClanId>, CMsgGCCStrike15_v2_SetClanId
{
    public CMsgGCCStrike15_v2_SetClanIdImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint ClanId
    { get => Accessor.GetUInt32("clan_id"); set => Accessor.SetUInt32("clan_id", value); }
}