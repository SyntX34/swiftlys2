using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgGetIPLocationImpl : TypedProtobuf<CGCMsgGetIPLocation>, CGCMsgGetIPLocation
{
    public CGCMsgGetIPLocationImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<uint> Ips
    { get => new ProtobufRepeatedFieldValueType<uint>(Accessor, "ips"); }
}