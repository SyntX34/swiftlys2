using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCMsgSystemStatsSchema : ITypedProtobuf<CGCMsgSystemStatsSchema>
{
    static CGCMsgSystemStatsSchema ITypedProtobuf<CGCMsgSystemStatsSchema>.Wrap(nint handle, bool isManuallyAllocated) => new CGCMsgSystemStatsSchemaImpl(handle, isManuallyAllocated);

    public uint GcAppId { get; set; }
    public byte[] SchemaKv { get; set; }
}