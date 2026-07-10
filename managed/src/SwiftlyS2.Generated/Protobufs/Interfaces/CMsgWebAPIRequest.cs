using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CMsgWebAPIRequest : ITypedProtobuf<CMsgWebAPIRequest>
{
    static CMsgWebAPIRequest ITypedProtobuf<CMsgWebAPIRequest>.Wrap(nint handle, bool isManuallyAllocated) => new CMsgWebAPIRequestImpl(handle, isManuallyAllocated);

    public string InterfaceName { get; set; }
    public string MethodName { get; set; }
    public uint Version { get; set; }
    public CMsgWebAPIKey ApiKey { get; }
    public CMsgHttpRequest Request { get; }
    public uint RoutingAppId { get; set; }
}