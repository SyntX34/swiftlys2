using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCMsgMasterSetDirectory_SubGC : ITypedProtobuf<CMsgGCMsgMasterSetDirectory_SubGC>
{
    static CMsgGCMsgMasterSetDirectory_SubGC ITypedProtobuf<CMsgGCMsgMasterSetDirectory_SubGC>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCMsgMasterSetDirectory_SubGCImpl(handle, isManuallyAllocated);

    public uint DirIndex { get; set; }
    public string Name { get; set; }
    public string Box { get; set; }
    public string CommandLine { get; set; }
    public string GcBinary { get; set; }
}