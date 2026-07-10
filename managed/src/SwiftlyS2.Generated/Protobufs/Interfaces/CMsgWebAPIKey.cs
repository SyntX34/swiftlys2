using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgWebAPIKey : ITypedProtobuf<CMsgWebAPIKey>
{
    static CMsgWebAPIKey ITypedProtobuf<CMsgWebAPIKey>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgWebAPIKeyImpl(handle, isManuallyAllocated);

    public uint Status { get; set; }
    public uint AccountId { get; set; }
    public uint PublisherGroupId { get; set; }
    public uint KeyId { get; set; }
    public string Domain { get; set; }
}