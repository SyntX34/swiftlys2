using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMFindAccountsResponse : ITypedProtobuf<CMsgAMFindAccountsResponse>
{
    static CMsgAMFindAccountsResponse ITypedProtobuf<CMsgAMFindAccountsResponse>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMFindAccountsResponseImpl(handle, isManuallyAllocated);

    public IProtobufRepeatedFieldValueType<ulong> SteamId { get; }
}