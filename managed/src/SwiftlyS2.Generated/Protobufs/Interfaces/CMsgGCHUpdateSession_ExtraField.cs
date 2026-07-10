using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCHUpdateSession_ExtraField : ITypedProtobuf<CMsgGCHUpdateSession_ExtraField>
{
    static CMsgGCHUpdateSession_ExtraField ITypedProtobuf<CMsgGCHUpdateSession_ExtraField>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCHUpdateSession_ExtraFieldImpl(handle, isManuallyAllocated);

    public string Name { get; set; }
    public string Value { get; set; }
}