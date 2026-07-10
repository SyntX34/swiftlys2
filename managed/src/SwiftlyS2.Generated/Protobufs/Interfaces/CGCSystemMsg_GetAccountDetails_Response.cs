using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CGCSystemMsg_GetAccountDetails_Response : ITypedProtobuf<CGCSystemMsg_GetAccountDetails_Response>
{
    static CGCSystemMsg_GetAccountDetails_Response ITypedProtobuf<CGCSystemMsg_GetAccountDetails_Response>.Wrap(nint handle, bool isManuallyAllocated) => new CGCSystemMsg_GetAccountDetails_ResponseImpl(handle, isManuallyAllocated);

    public uint EresultDeprecated { get; set; }
    public string AccountName { get; set; }
    public string PersonaName { get; set; }
    public bool IsProfilePublic { get; set; }
    public bool IsInventoryPublic { get; set; }
    public bool IsVacBanned { get; set; }
    public bool IsCyberCafe { get; set; }
    public bool IsSchoolAccount { get; set; }
    public bool IsLimited { get; set; }
    public bool IsSubscribed { get; set; }
    public uint Package { get; set; }
    public bool IsFreeTrialAccount { get; set; }
    public uint FreeTrialExpiration { get; set; }
    public bool IsLowViolence { get; set; }
    public bool IsAccountLockedDown { get; set; }
    public bool IsCommunityBanned { get; set; }
    public bool IsTradeBanned { get; set; }
    public uint TradeBanExpiration { get; set; }
    public uint Accountid { get; set; }
    public uint SuspensionEndTime { get; set; }
    public string Currency { get; set; }
    public uint SteamLevel { get; set; }
    public uint FriendCount { get; set; }
    public uint AccountCreationTime { get; set; }
    public bool IsSteamguardEnabled { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsTwoFactorAuthEnabled { get; set; }
    public uint TwoFactorEnabledTime { get; set; }
    public uint PhoneVerificationTime { get; set; }
    public ulong PhoneId { get; set; }
    public bool IsPhoneIdentifying { get; set; }
    public uint RtIdentityLinked { get; set; }
    public uint RtBirthDate { get; set; }
    public string TxnCountryCode { get; set; }
    public bool HasAcceptedChinaSsa { get; set; }
    public bool IsBannedSteamChina { get; set; }
    public ulong ExtSpend { get; set; }
}