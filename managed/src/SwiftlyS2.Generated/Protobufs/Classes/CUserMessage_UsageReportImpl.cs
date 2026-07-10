using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CUserMessage_UsageReportImpl : NetMessage<CUserMessage_UsageReport>, CUserMessage_UsageReport
{
    public CUserMessage_UsageReportImpl(nint handle, bool isManuallyAllocated) : base(handle, isManuallyAllocated)
    {
    }

    public string Usage
    { get => Accessor.GetString("usage"); set => Accessor.SetString("usage", value); }
}