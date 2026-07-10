using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetDirectory : ITypedProtobuf<CMsgGCMsgMasterSetDirectory>
{
    static CMsgGCMsgMasterSetDirectory ITypedProtobuf<CMsgGCMsgMasterSetDirectory>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetDirectoryImpl(handle, isManuallyAllocated);

    public uint MasterDirIndex { get; set; }
    public IProtobufRepeatedFieldSubMessageType<CMsgGCMsgMasterSetDirectory_SubGC> Dir { get; }
}