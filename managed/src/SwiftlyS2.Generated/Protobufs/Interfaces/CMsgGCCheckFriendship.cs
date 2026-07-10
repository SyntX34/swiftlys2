using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCCheckFriendship : ITypedProtobuf<CMsgGCCheckFriendship>
{
    static CMsgGCCheckFriendship ITypedProtobuf<CMsgGCCheckFriendship>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCCheckFriendshipImpl(handle, isManuallyAllocated);

    public ulong SteamidLeft { get; set; }
    public ulong SteamidRight { get; set; }
}