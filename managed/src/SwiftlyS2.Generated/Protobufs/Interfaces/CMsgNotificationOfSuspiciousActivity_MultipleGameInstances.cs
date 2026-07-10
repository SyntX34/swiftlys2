using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgNotificationOfSuspiciousActivity_MultipleGameInstances : ITypedProtobuf<CMsgNotificationOfSuspiciousActivity_MultipleGameInstances>
{
    static CMsgNotificationOfSuspiciousActivity_MultipleGameInstances ITypedProtobuf<CMsgNotificationOfSuspiciousActivity_MultipleGameInstances>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgNotificationOfSuspiciousActivity_MultipleGameInstancesImpl(handle, isManuallyAllocated);

    public uint AppInstanceCount { get; set; }
    public IProtobufRepeatedFieldValueType<ulong> OtherSteamids { get; }
}