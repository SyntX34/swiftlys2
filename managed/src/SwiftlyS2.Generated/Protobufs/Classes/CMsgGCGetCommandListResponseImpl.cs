using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetCommandListResponseImpl : TypedProtobuf<CMsgGCGetCommandListResponse>, CMsgGCGetCommandListResponse
{
    public CMsgGCGetCommandListResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<string> CommandName
    { get => new ProtobufRepeatedFieldValueType<string>(Accessor, "command_name"); }
}