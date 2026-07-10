using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCCheckFriendship_ResponseImpl : TypedProtobuf<CMsgGCCheckFriendship_Response>, CMsgGCCheckFriendship_Response
{
    public CMsgGCCheckFriendship_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public bool Success
    { get => Accessor.GetBool("success"); set => Accessor.SetBool("success", value); }
    public bool FoundFriendship
    { get => Accessor.GetBool("found_friendship"); set => Accessor.SetBool("found_friendship", value); }
}