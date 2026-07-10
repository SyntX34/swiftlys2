using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgNotificationOfSuspiciousActivity_MultipleGameInstancesImpl : TypedProtobuf<CMsgNotificationOfSuspiciousActivity_MultipleGameInstances>, CMsgNotificationOfSuspiciousActivity_MultipleGameInstances
{
    public CMsgNotificationOfSuspiciousActivity_MultipleGameInstancesImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint AppInstanceCount
    { get => Accessor.GetUInt32("app_instance_count"); set => Accessor.SetUInt32("app_instance_count", value); }
    public IProtobufRepeatedFieldValueType<ulong> OtherSteamids
    { get => new ProtobufRepeatedFieldValueType<ulong>(Accessor, "other_steamids"); }
}