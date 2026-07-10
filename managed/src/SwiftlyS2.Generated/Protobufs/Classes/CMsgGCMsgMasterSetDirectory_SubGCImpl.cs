using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetDirectory_SubGCImpl : TypedProtobuf<CMsgGCMsgMasterSetDirectory_SubGC>, CMsgGCMsgMasterSetDirectory_SubGC
{
    public CMsgGCMsgMasterSetDirectory_SubGCImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint DirIndex
    { get => Accessor.GetUInt32("dir_index"); set => Accessor.SetUInt32("dir_index", value); }
    public string Name
    { get => Accessor.GetString("name"); set => Accessor.SetString("name", value); }
    public string Box
    { get => Accessor.GetString("box"); set => Accessor.SetString("box", value); }
    public string CommandLine
    { get => Accessor.GetString("command_line"); set => Accessor.SetString("command_line", value); }
    public string GcBinary
    { get => Accessor.GetString("gc_binary"); set => Accessor.SetString("gc_binary", value); }
}