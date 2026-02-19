namespace DailymotionSDK.Models;

/// <summary>
/// OAuth response types
/// https://developer.dailymotion.com/api#oauth-authorization-code-flow
/// </summary>
public enum OAuthResponseType
{
    Token,
    Code,
    AuthorizationCode,
    Password,
    RefreshToken
}
