using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetCommandList : ITypedProtobuf<CMsgGCGetCommandList>
{
    static CMsgGCGetCommandList ITypedProtobuf<CMsgGCGetCommandList>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetCommandListImpl(handle, isManuallyAllocated);

    public uint AppId { get; set; }
    public string CommandPrefix { get; set; }
}