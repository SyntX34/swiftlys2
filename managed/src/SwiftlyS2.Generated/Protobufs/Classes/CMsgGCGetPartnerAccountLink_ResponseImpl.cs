using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CMsgGCGetPartnerAccountLink_ResponseImpl : TypedProtobuf<CMsgGCGetPartnerAccountLink_Response>, CMsgGCGetPartnerAccountLink_Response
{
    public CMsgGCGetPartnerAccountLink_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint Pwid
    { get => Accessor.GetUInt32("pwid"); set => Accessor.SetUInt32("pwid", value); }
    public uint Nexonid
    { get => Accessor.GetUInt32("nexonid"); set => Accessor.SetUInt32("nexonid", value); }
    public int Ageclass
    { get => Accessor.GetInt32("ageclass"); set => Accessor.SetInt32("ageclass", value); }
    public bool IdVerified
    { get => Accessor.GetBool("id_verified"); set => Accessor.SetBool("id_verified", value); }
    public bool IsAdult
    { get => Accessor.GetBool("is_adult"); set => Accessor.SetBool("is_adult", value); }
}