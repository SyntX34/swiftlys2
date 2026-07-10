using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgNotifyWatchdog : ITypedProtobuf<CMsgNotifyWatchdog>
{
    static CMsgNotifyWatchdog ITypedProtobuf<CMsgNotifyWatchdog>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgNotifyWatchdogImpl(handle, isManuallyAllocated);

    public uint Source { get; set; }
    public uint AlertType { get; set; }
    public uint AlertDestination { get; set; }
    public bool Critical { get; set; }
    public uint Time { get; set; }
    public uint Appid { get; set; }
    public string Text { get; set; }
}