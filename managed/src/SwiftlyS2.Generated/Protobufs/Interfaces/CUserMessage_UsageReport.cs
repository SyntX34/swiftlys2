using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CUserMessage_UsageReport : ITypedProtobuf<CUserMessage_UsageReport>, INetMessage<CUserMessage_UsageReport>, IDisposable
{
    static int INetMessage<CUserMessage_UsageReport>.MessageId => 168;

    static string INetMessage<CUserMessage_UsageReport>.MessageName => "CUserMessage_UsageReport";

    static CUserMessage_UsageReport ITypedProtobuf<CUserMessage_UsageReport>.Wrap(nint handle, bool isManuallyAllocated) => new CUserMessage_UsageReportImpl(handle, isManuallyAllocated);

    public string Usage { get; set; }
}