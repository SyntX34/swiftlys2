using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CCSPredictionEvent_PlayerTeleportImpl : TypedProtobuf<CCSPredictionEvent_PlayerTeleport>, CCSPredictionEvent_PlayerTeleport
{
    public CCSPredictionEvent_PlayerTeleportImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public bool Relative
    { get => Accessor.GetBool("relative"); set => Accessor.SetBool("relative", value); }
    public Vector Origin
    { get => Accessor.GetVector("origin"); set => Accessor.SetVector("origin", value); }
    public QAngle Angles
    { get => Accessor.GetQAngle("angles"); set => Accessor.SetQAngle("angles", value); }
    public Vector Velocity
    { get => Accessor.GetVector("velocity"); set => Accessor.SetVector("velocity", value); }
}