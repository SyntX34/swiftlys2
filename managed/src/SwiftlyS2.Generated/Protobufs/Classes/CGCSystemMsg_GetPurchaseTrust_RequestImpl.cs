using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCSystemMsg_GetPurchaseTrust_RequestImpl : TypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Request>, CGCSystemMsg_GetPurchaseTrust_Request
{
    public CGCSystemMsg_GetPurchaseTrust_RequestImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public ulong Steamid
    { get => Accessor.GetUInt64("steamid"); set => Accessor.SetUInt64("steamid", value); }
}