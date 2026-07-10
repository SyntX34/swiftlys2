using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgGetIPLocationResponseImpl : TypedProtobuf<CGCMsgGetIPLocationResponse>, CGCMsgGetIPLocationResponse
{
    public CGCMsgGetIPLocationResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldSubMessageType<CIPLocationInfo> Infos
    { get => new ProtobufRepeatedFieldSubMessageType<CIPLocationInfo>(Accessor, "infos"); }
}