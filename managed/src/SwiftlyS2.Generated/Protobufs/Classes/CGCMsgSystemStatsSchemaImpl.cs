using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCMsgSystemStatsSchemaImpl : TypedProtobuf<CGCMsgSystemStatsSchema>, CGCMsgSystemStatsSchema
{
    public CGCMsgSystemStatsSchemaImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint GcAppId
    { get => Accessor.GetUInt32("gc_app_id"); set => Accessor.SetUInt32("gc_app_id", value); }
    public byte[] SchemaKv
    { get => Accessor.GetBytes("schema_kv"); set => Accessor.SetBytes("schema_kv", value); }
}