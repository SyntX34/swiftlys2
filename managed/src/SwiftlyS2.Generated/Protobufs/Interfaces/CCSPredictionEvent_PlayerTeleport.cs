using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CCSPredictionEvent_PlayerTeleport : ITypedProtobuf<CCSPredictionEvent_PlayerTeleport>
{
    static CCSPredictionEvent_PlayerTeleport ITypedProtobuf<CCSPredictionEvent_PlayerTeleport>.Wrap(nint handle, bool isManuallyAllocated) => new CCSPredictionEvent_PlayerTeleportImpl(handle, isManuallyAllocated);

    public bool Relative { get; set; }
    public Vector Origin { get; set; }
    public QAngle Angles { get; set; }
    public Vector Velocity { get; set; }
}