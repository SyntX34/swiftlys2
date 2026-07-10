using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgNotificationOfSuspiciousActivity : ITypedProtobuf<CMsgNotificationOfSuspiciousActivity>
{
    static CMsgNotificationOfSuspiciousActivity ITypedProtobuf<CMsgNotificationOfSuspiciousActivity>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgNotificationOfSuspiciousActivityImpl(handle, isManuallyAllocated);

    public ulong Steamid { get; set; }
    public uint Appid { get; set; }
    public CMsgNotificationOfSuspiciousActivity_MultipleGameInstances MultipleInstances { get; }
}