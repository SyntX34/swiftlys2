using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCCheckFriendship_Response : ITypedProtobuf<CMsgGCCheckFriendship_Response>
{
    static CMsgGCCheckFriendship_Response ITypedProtobuf<CMsgGCCheckFriendship_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCCheckFriendship_ResponseImpl(handle, isManuallyAllocated);

    public bool Success { get; set; }
    public bool FoundFriendship { get; set; }
}