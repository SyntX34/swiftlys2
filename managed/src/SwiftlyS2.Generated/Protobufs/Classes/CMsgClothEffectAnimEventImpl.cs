using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgClothEffectAnimEventImpl : NetMessage<CMsgClothEffectAnimEvent>, CMsgClothEffectAnimEvent
{
    public CMsgClothEffectAnimEventImpl(nint handle, bool isManuallyAllocated) : base(handle, isManuallyAllocated)
    {
    }

    public int SourceEntityIndex
    { get => Accessor.GetInt32("source_entity_index"); set => Accessor.SetInt32("source_entity_index", value); }
    public int EffectNameHash
    { get => Accessor.GetInt32("effect_name_hash"); set => Accessor.SetInt32("effect_name_hash", value); }
    public int Operation
    { get => Accessor.GetInt32("operation"); set => Accessor.SetInt32("operation", value); }
    public int Flags
    { get => Accessor.GetInt32("flags"); set => Accessor.SetInt32("flags", value); }
    public string Tags
    { get => Accessor.GetString("tags"); set => Accessor.SetString("tags", value); }
    public Vector Pte
    { get => Accessor.GetVector("pte"); set => Accessor.SetVector("pte", value); }
}