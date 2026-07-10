using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgSetOptionsImpl : TypedProtobuf<CMsgGCMsgSetOptions>, CMsgGCMsgSetOptions
{
    public CMsgGCMsgSetOptionsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public CMsgGCMsgSetOptions_Option Options
    { get => (CMsgGCMsgSetOptions_Option)Accessor.GetInt32("options"); set => Accessor.SetInt32("options", (int)value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgSetOptions_MessageRange> ClientMsgRanges
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCMsgSetOptions_MessageRange>(Accessor, "client_msg_ranges"); }
}