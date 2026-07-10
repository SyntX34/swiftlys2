using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgAMGrantGuestPasses2Impl : TypedProtobuf<CMsgAMGrantGuestPasses2>, CMsgAMGrantGuestPasses2
{
    public CMsgAMGrantGuestPasses2Impl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong SteamId
    { get => Accessor.GetUInt64("steam_id"); set => Accessor.SetUInt64("steam_id", value); }
    public uint PackageId
    { get => Accessor.GetUInt32("package_id"); set => Accessor.SetUInt32("package_id", value); }
    public int PassesToGrant
    { get => Accessor.GetInt32("passes_to_grant"); set => Accessor.SetInt32("passes_to_grant", value); }
    public int DaysToExpiration
    { get => Accessor.GetInt32("days_to_expiration"); set => Accessor.SetInt32("days_to_expiration", value); }
    public int Action
    { get => Accessor.GetInt32("action"); set => Accessor.SetInt32("action", value); }
}