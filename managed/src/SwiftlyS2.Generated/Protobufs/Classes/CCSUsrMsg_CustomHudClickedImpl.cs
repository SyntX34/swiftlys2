using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CCSUsrMsg_CustomHudClickedImpl : NetMessage<CCSUsrMsg_CustomHudClicked>, CCSUsrMsg_CustomHudClicked
{
    public CCSUsrMsg_CustomHudClickedImpl(nint handle, bool isManuallyAllocated) : base(handle, isManuallyAllocated)
    {
    }

    public uint CustomHudLayout
    { get => Accessor.GetUInt32("custom_hud_layout"); set => Accessor.SetUInt32("custom_hud_layout", value); }
    public string ButtonId
    { get => Accessor.GetString("button_id"); set => Accessor.SetString("button_id", value); }
}