using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMFindAccountsResponseImpl : TypedProtobuf<CMsgAMFindAccountsResponse>, CMsgAMFindAccountsResponse
{
    public CMsgAMFindAccountsResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<ulong> SteamId
    { get => new ProtobufRepeatedFieldValueType<ulong>(Accessor, "steam_id"); }
}