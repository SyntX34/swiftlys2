using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgNotifyWatchdogImpl : TypedProtobuf<CMsgNotifyWatchdog>, CMsgNotifyWatchdog
{
    public CMsgNotifyWatchdogImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Source
    { get => Accessor.GetUInt32("source"); set => Accessor.SetUInt32("source", value); }
    public uint AlertType
    { get => Accessor.GetUInt32("alert_type"); set => Accessor.SetUInt32("alert_type", value); }
    public uint AlertDestination
    { get => Accessor.GetUInt32("alert_destination"); set => Accessor.SetUInt32("alert_destination", value); }
    public bool Critical
    { get => Accessor.GetBool("critical"); set => Accessor.SetBool("critical", value); }
    public uint Time
    { get => Accessor.GetUInt32("time"); set => Accessor.SetUInt32("time", value); }
    public uint Appid
    { get => Accessor.GetUInt32("appid"); set => Accessor.SetUInt32("appid", value); }
    public string Text
    { get => Accessor.GetString("text"); set => Accessor.SetString("text", value); }
}