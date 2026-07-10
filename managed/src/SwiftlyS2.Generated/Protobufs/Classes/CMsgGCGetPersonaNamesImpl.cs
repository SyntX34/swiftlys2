using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetPersonaNamesImpl : TypedProtobuf<CMsgGCGetPersonaNames>, CMsgGCGetPersonaNames
{
    public CMsgGCGetPersonaNamesImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public IProtobufRepeatedFieldValueType<ulong> Steamids
    { get => new ProtobufRepeatedFieldValueType<ulong>(Accessor, "steamids"); }
}