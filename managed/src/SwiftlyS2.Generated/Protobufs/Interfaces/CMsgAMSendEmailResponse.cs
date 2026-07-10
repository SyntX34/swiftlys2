using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMSendEmailResponse : ITypedProtobuf<CMsgAMSendEmailResponse>
{
    static CMsgAMSendEmailResponse ITypedProtobuf<CMsgAMSendEmailResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMSendEmailResponseImpl(handle, isManuallyAllocated);

    public uint Eresult { get; set; }
}