namespace DailymotionSDK.Models;

/// <summary>
/// User status enumeration representing the current state of a user account
/// https://developer.dailymotion.com/api#user-fields
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// User account is pending activation
    /// Account has been created but not yet activated via email verification
    /// </summary>
    [System.Runtime.Serialization.EnumMember(Value = "pending-activation")]
    PendingActivation,

    /// <summary>
    /// User account has been disabled
    /// Account is temporarily or permanently disabled by administrators
    /// </summary>
    Disabled,

    /// <summary>
    /// User account is active and fully functional
    /// User can log in and use all available features
    /// </summary>
    Active,

    /// <summary>
    /// User status is unknown or could not be determined
    /// Used as a fallback when status cannot be determined
    /// </summary>
    Unknown
}
