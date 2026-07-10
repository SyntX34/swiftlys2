using SwiftlyS2.Core.Natives;
using SwiftlyS2.Core.NetMessages;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace SwiftlyS2.Core.ProtobufDefinitions;

internal class CGCSystemMsg_GetAccountDetails_ResponseImpl : TypedProtobuf<CGCSystemMsg_GetAccountDetails_Response>, CGCSystemMsg_GetAccountDetails_Response
{
    public CGCSystemMsg_GetAccountDetails_ResponseImpl(nint handle, bool isManuallyAllocated) : base(handle)
    {
    }

    public uint EresultDeprecated
    { get => Accessor.GetUInt32("eresult_deprecated"); set => Accessor.SetUInt32("eresult_deprecated", value); }
    public string AccountName
    { get => Accessor.GetString("account_name"); set => Accessor.SetString("account_name", value); }
    public string PersonaName
    { get => Accessor.GetString("persona_name"); set => Accessor.SetString("persona_name", value); }
    public bool IsProfilePublic
    { get => Accessor.GetBool("is_profile_public"); set => Accessor.SetBool("is_profile_public", value); }
    public bool IsInventoryPublic
    { get => Accessor.GetBool("is_inventory_public"); set => Accessor.SetBool("is_inventory_public", value); }
    public bool IsVacBanned
    { get => Accessor.GetBool("is_vac_banned"); set => Accessor.SetBool("is_vac_banned", value); }
    public bool IsCyberCafe
    { get => Accessor.GetBool("is_cyber_cafe"); set => Accessor.SetBool("is_cyber_cafe", value); }
    public bool IsSchoolAccount
    { get => Accessor.GetBool("is_school_account"); set => Accessor.SetBool("is_school_account", value); }
    public bool IsLimited
    { get => Accessor.GetBool("is_limited"); set => Accessor.SetBool("is_limited", value); }
    public bool IsSubscribed
    { get => Accessor.GetBool("is_subscribed"); set => Accessor.SetBool("is_subscribed", value); }
    public uint Package
    { get => Accessor.GetUInt32("package"); set => Accessor.SetUInt32("package", value); }
    public bool IsFreeTrialAccount
    { get => Accessor.GetBool("is_free_trial_account"); set => Accessor.SetBool("is_free_trial_account", value); }
    public uint FreeTrialExpiration
    { get => Accessor.GetUInt32("free_trial_expiration"); set => Accessor.SetUInt32("free_trial_expiration", value); }
    public bool IsLowViolence
    { get => Accessor.GetBool("is_low_violence"); set => Accessor.SetBool("is_low_violence", value); }
    public bool IsAccountLockedDown
    { get => Accessor.GetBool("is_account_locked_down"); set => Accessor.SetBool("is_account_locked_down", value); }
    public bool IsCommunityBanned
    { get => Accessor.GetBool("is_community_banned"); set => Accessor.SetBool("is_community_banned", value); }
    public bool IsTradeBanned
    { get => Accessor.GetBool("is_trade_banned"); set => Accessor.SetBool("is_trade_banned", value); }
    public uint TradeBanExpiration
    { get => Accessor.GetUInt32("trade_ban_expiration"); set => Accessor.SetUInt32("trade_ban_expiration", value); }
    public uint Accountid
    { get => Accessor.GetUInt32("accountid"); set => Accessor.SetUInt32("accountid", value); }
    public uint SuspensionEndTime
    { get => Accessor.GetUInt32("suspension_end_time"); set => Accessor.SetUInt32("suspension_end_time", value); }
    public string Currency
    { get => Accessor.GetString("currency"); set => Accessor.SetString("currency", value); }
    public uint SteamLevel
    { get => Accessor.GetUInt32("steam_level"); set => Accessor.SetUInt32("steam_level", value); }
    public uint FriendCount
    { get => Accessor.GetUInt32("friend_count"); set => Accessor.SetUInt32("friend_count", value); }
    public uint AccountCreationTime
    { get => Accessor.GetUInt32("account_creation_time"); set => Accessor.SetUInt32("account_creation_time", value); }
    public bool IsSteamguardEnabled
    { get => Accessor.GetBool("is_steamguard_enabled"); set => Accessor.SetBool("is_steamguard_enabled", value); }
    public bool IsPhoneVerified
    { get => Accessor.GetBool("is_phone_verified"); set => Accessor.SetBool("is_phone_verified", value); }
    public bool IsTwoFactorAuthEnabled
    { get => Accessor.GetBool("is_two_factor_auth_enabled"); set => Accessor.SetBool("is_two_factor_auth_enabled", value); }
    public uint TwoFactorEnabledTime
    { get => Accessor.GetUInt32("two_factor_enabled_time"); set => Accessor.SetUInt32("two_factor_enabled_time", value); }
    public uint PhoneVerificationTime
    { get => Accessor.GetUInt32("phone_verification_time"); set => Accessor.SetUInt32("phone_verification_time", value); }
    public ulong PhoneId
    { get => Accessor.GetUInt64("phone_id"); set => Accessor.SetUInt64("phone_id", value); }
    public bool IsPhoneIdentifying
    { get => Accessor.GetBool("is_phone_identifying"); set => Accessor.SetBool("is_phone_identifying", value); }
    public uint RtIdentityLinked
    { get => Accessor.GetUInt32("rt_identity_linked"); set => Accessor.SetUInt32("rt_identity_linked", value); }
    public uint RtBirthDate
    { get => Accessor.GetUInt32("rt_birth_date"); set => Accessor.SetUInt32("rt_birth_date", value); }
    public string TxnCountryCode
    { get => Accessor.GetString("txn_country_code"); set => Accessor.SetString("txn_country_code", value); }
    public bool HasAcceptedChinaSsa
    { get => Accessor.GetBool("has_accepted_china_ssa"); set => Accessor.SetBool("has_accepted_china_ssa", value); }
    public bool IsBannedSteamChina
    { get => Accessor.GetBool("is_banned_steam_china"); set => Accessor.SetBool("is_banned_steam_china", value); }
    public ulong ExtSpend
    { get => Accessor.GetUInt64("ext_spend"); set => Accessor.SetUInt64("ext_spend", value); }
}