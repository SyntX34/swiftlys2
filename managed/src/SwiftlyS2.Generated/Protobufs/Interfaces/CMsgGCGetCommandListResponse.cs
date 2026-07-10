using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetCommandListResponse : ITypedProtobuf<CMsgGCGetCommandListResponse>
{
    static CMsgGCGetCommandListResponse ITypedProtobuf<CMsgGCGetCommandListResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetCommandListResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<string> CommandName { get; }
}