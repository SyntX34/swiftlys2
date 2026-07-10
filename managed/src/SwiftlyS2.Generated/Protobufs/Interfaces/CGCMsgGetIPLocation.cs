using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgGetIPLocation : ITypedProtobuf<CGCMsgGetIPLocation>
{
    static CGCMsgGetIPLocation ITypedProtobuf<CGCMsgGetIPLocation>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgGetIPLocationImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<uint> Ips { get; }
}