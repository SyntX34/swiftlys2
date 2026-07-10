using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgClothStiffenAnimEvent : ITypedProtobuf<CMsgClothStiffenAnimEvent>, INetMessage<CMsgClothStiffenAnimEvent>, IDisposable
{
    static int INetMessage<CMsgClothStiffenAnimEvent>.MessageId => 213;

    static string INetMessage<CMsgClothStiffenAnimEvent>.MessageName => "CMsgClothStiffenAnimEvent";

    static CMsgClothStiffenAnimEvent ITypedProtobuf<CMsgClothStiffenAnimEvent>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgClothStiffenAnimEventImpl(handle, isManuallyAllocated);

    public int SourceEntityIndex { get; set; }
    public int VertexSetHash { get; set; }
    public float Intensity { get; set; }
    public float Length { get; set; }
    public float SpeedIn { get; set; }
    public float SpeedOut { get; set; }
}