using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCSystemMsg_GetPurchaseTrust_Request : ITypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Request>
{
    static CGCSystemMsg_GetPurchaseTrust_Request ITypedProtobuf<CGCSystemMsg_GetPurchaseTrust_Request>.Wrap(nint handle, bool isManuallyAllocated) => new CGCSystemMsg_GetPurchaseTrust_RequestImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
}