namespace SwiftlyS2.Shared.ProtobufDefinitions;

public enum CMsgGCRoutingInfo_RoutingMethod
{
    RANDOM = 0,
    DISCARD = 1,
    CLIENT_STEAMID = 2,
    PROTOBUF_FIELD_UINT64 = 3,
    WEBAPI_PARAM_UINT64 = 4,
}