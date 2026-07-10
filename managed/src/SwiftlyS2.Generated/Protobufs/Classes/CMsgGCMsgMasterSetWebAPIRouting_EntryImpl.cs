using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetWebAPIRouting_EntryImpl : TypedProtobuf<CMsgGCMsgMasterSetWebAPIRouting_Entry>, CMsgGCMsgMasterSetWebAPIRouting_Entry
{
    public CMsgGCMsgMasterSetWebAPIRouting_EntryImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public string InterfaceName
    { get => Accessor.GetString("interface_name"); set => Accessor.SetString("interface_name", value); }
    public string MethodName
    { get => Accessor.GetString("method_name"); set => Accessor.SetString("method_name", value); }
    public CMsgGCRoutingInfo Routing
    { get => new CMsgGCRoutingInfoImpl(NativeNetMessages.GetNestedMessage(Address, "routing"), false); }
}