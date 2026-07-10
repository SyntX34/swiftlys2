using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetCommandListImpl : TypedProtobuf<CMsgGCGetCommandList>, CMsgGCGetCommandList
{
    public CMsgGCGetCommandListImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint AppId
    { get => Accessor.GetUInt32("app_id"); set => Accessor.SetUInt32("app_id", value); }
    public string CommandPrefix
    { get => Accessor.GetString("command_prefix"); set => Accessor.SetString("command_prefix", value); }
}