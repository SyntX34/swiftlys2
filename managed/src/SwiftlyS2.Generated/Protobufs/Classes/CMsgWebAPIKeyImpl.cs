using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgWebAPIKeyImpl : TypedProtobuf<CMsgWebAPIKey>, CMsgWebAPIKey
{
    public CMsgWebAPIKeyImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Status
    { get => Accessor.GetUInt32("status"); set => Accessor.SetUInt32("status", value); }
    public uint AccountId
    { get => Accessor.GetUInt32("account_id"); set => Accessor.SetUInt32("account_id", value); }
    public uint PublisherGroupId
    { get => Accessor.GetUInt32("publisher_group_id"); set => Accessor.SetUInt32("publisher_group_id", value); }
    public uint KeyId
    { get => Accessor.GetUInt32("key_id"); set => Accessor.SetUInt32("key_id", value); }
    public string Domain
    { get => Accessor.GetString("domain"); set => Accessor.SetString("domain", value); }
}