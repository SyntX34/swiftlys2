using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCCheckFriendshipImpl : TypedProtobuf<CMsgGCCheckFriendship>, CMsgGCCheckFriendship
{
    public CMsgGCCheckFriendshipImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamidLeft
    { get => Accessor.GetUInt64("steamid_left"); set => Accessor.SetUInt64("steamid_left", value); }
    public ulong SteamidRight
    { get => Accessor.GetUInt64("steamid_right"); set => Accessor.SetUInt64("steamid_right", value); }
}