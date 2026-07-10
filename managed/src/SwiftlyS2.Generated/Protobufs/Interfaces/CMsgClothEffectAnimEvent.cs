using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgClothEffectAnimEvent : ITypedProtobuf<CMsgClothEffectAnimEvent>, INetMessage<CMsgClothEffectAnimEvent>, IDisposable
{
    static int INetMessage<CMsgClothEffectAnimEvent>.MessageId => 214;

    static string INetMessage<CMsgClothEffectAnimEvent>.MessageName => "CMsgClothEffectAnimEvent";

    static CMsgClothEffectAnimEvent ITypedProtobuf<CMsgClothEffectAnimEvent>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgClothEffectAnimEventImpl(handle, isManuallyAllocated);

    public int SourceEntityIndex { get; set; }
    public int EffectNameHash { get; set; }
    public int Operation { get; set; }
    public int Flags { get; set; }
    public string Tags { get; set; }
    public Vector Pte { get; set; }
}