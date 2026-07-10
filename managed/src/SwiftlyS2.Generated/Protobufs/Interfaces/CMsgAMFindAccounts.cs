using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgAMFindAccounts : ITypedProtobuf<CMsgAMFindAccounts>
{
    static CMsgAMFindAccounts ITypedProtobuf<CMsgAMFindAccounts>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgAMFindAccountsImpl(handle, isManuallyAllocated);

    public uint SearchType { get; set; }
    public string SearchString { get; set; }
}