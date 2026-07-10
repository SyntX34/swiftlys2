using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCMsgMasterSetDirectoryImpl : TypedProtobuf<CMsgGCMsgMasterSetDirectory>, CMsgGCMsgMasterSetDirectory
{
    public CMsgGCMsgMasterSetDirectoryImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint MasterDirIndex
    { get => Accessor.GetUInt32("master_dir_index"); set => Accessor.SetUInt32("master_dir_index", value); }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetDirectory_SubGC> Dir
    { get => new ProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetDirectory_SubGC>(Accessor, "dir"); }
}