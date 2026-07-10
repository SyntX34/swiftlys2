using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgGCGetPersonaNames : ITypedProtobuf<CMsgGCGetPersonaNames>
{
    static CMsgGCGetPersonaNames ITypedProtobuf<CMsgGCGetPersonaNames>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgGCGetPersonaNamesImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<ulong> Steamids { get; }
}