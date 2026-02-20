namespace DailymotionSDK.Models;

/// <summary>
/// OAuth response types
/// https://developer.dailymotion.com/api#oauth-authorization-code-flow
/// </summary>
public enum OAuthResponseType
{
    /// <summary>
    /// The token
    /// </summary>
    Token,
    /// <summary>
    /// The code
    /// </summary>
    Code,
    /// <summary>
    /// The authorization code
    /// </summary>
    AuthorizationCode,
    /// <summary>
    /// The password
    /// </summary>
    Password,
    /// <summary>
    /// The refresh token
    /// </summary>
    RefreshToken
}
