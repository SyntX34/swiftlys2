using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgSetOptions_MessageRangeImpl : TypedProtobuf<CMsgGCMsgSetOptions_MessageRange>, CMsgGCMsgSetOptions_MessageRange
{
    public CMsgGCMsgSetOptions_MessageRangeImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Low
    { get => Accessor.GetUInt32("low"); set => Accessor.SetUInt32("low", value); }
    public uint High
    { get => Accessor.GetUInt32("high"); set => Accessor.SetUInt32("high", value); }
}