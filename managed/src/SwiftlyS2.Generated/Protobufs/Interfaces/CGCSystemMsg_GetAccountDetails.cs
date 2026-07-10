using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCSystemMsg_GetAccountDetails : ITypedProtobuf<CGCSystemMsg_GetAccountDetails>
{
    static CGCSystemMsg_GetAccountDetails ITypedProtobuf<CGCSystemMsg_GetAccountDetails>.Wrap(nint handle, bool isManuallyAllocated) => new CGCSystemMsg_GetAccountDetailsImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public uint Appid { get; set; }
}