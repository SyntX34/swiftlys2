using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgClothStiffenAnimEventImpl : NetMessage<CMsgClothStiffenAnimEvent>, CMsgClothStiffenAnimEvent
{
    public CMsgClothStiffenAnimEventImpl(nint handle, bool isManuallyAllocated) : base(handle, isManuallyAllocated)
    {
    }

    public int SourceEntityIndex
    { get => Accessor.GetInt32("source_entity_index"); set => Accessor.SetInt32("source_entity_index", value); }
    public int VertexSetHash
    { get => Accessor.GetInt32("vertex_set_hash"); set => Accessor.SetInt32("vertex_set_hash", value); }
    public float Intensity
    { get => Accessor.GetFloat("intensity"); set => Accessor.SetFloat("intensity", value); }
    public float Length
    { get => Accessor.GetFloat("length"); set => Accessor.SetFloat("length", value); }
    public float SpeedIn
    { get => Accessor.GetFloat("speed_in"); set => Accessor.SetFloat("speed_in", value); }
    public float SpeedOut
    { get => Accessor.GetFloat("speed_out"); set => Accessor.SetFloat("speed_out", value); }
}