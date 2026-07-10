using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgHttpRequest_QueryParamImpl : TypedProtobuf<CMsgHttpRequest_QueryParam>, CMsgHttpRequest_QueryParam
{
    public CMsgHttpRequest_QueryParamImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public string Name
    { get => Accessor.GetString("name"); set => Accessor.SetString("name", value); }
    public byte[] Value
    { get => Accessor.GetBytes("value"); set => Accessor.SetBytes("value", value); }
}