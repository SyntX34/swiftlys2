using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMFindAccountsImpl : TypedProtobuf<CMsgAMFindAccounts>, CMsgAMFindAccounts
{
    public CMsgAMFindAccountsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint SearchType
    { get => Accessor.GetUInt32("search_type"); set => Accessor.SetUInt32("search_type", value); }
    public string SearchString
    { get => Accessor.GetString("search_string"); set => Accessor.SetString("search_string", value); }
}