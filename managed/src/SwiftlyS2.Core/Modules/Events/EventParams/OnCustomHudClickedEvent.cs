using SwiftlyS2.Core.EntitySystem;
using SwiftlyS2.Core.SchemaDefinitions;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.ProtobufDefinitions;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace SwiftlyS2.Core.Events;

internal class OnCustomHudClickedEvent : IOnCustomHudClickedEvent
{
    public required CCSUsrMsg_CustomHudClicked Message { get; set; }

    public uint CustomHudLayoutEntityIndex => Message.CustomHudLayout;

    public CCSCustomHudLayout? CustomHudLayout {
        get {
            var entity = EntityManager.GetEntityByIndex(Message.CustomHudLayout);
            return entity is CCSCustomHudLayoutImpl layout ? layout : null;
        }
    }

    public string ButtonId => Message.ButtonId;
}