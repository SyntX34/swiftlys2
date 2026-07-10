using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgSQLStatsImpl : TypedProtobuf<CGCMsgSQLStats>, CGCMsgSQLStats
{
    public CGCMsgSQLStatsImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint SchemaCatalog
    { get => Accessor.GetUInt32("schema_catalog"); set => Accessor.SetUInt32("schema_catalog", value); }
}