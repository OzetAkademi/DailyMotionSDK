using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Token response from OAuth flow
/// https://developer.dailymotion.com/api#oauth-token
/// </summary>
public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("scope")]
    public string? Scope { get; set; }

    [JsonProperty("uid")]
    public string? Uid { get; set; }

    [JsonProperty("email_verified")]
    public bool? EmailVerified { get; set; }

    /// <summary>
    /// Gets whether this token response includes a refresh token
    /// </summary>
    public bool HasRefreshToken => !string.IsNullOrEmpty(RefreshToken);

    /// <summary>
    /// Gets whether this appears to be a user-level authentication (has UID and potentially refresh token)
    /// </summary>
    public bool IsUserAuthentication => !string.IsNullOrEmpty(Uid);

    /// <summary>
    /// Gets whether this appears to be an application-level authentication (no UID, no refresh token)
    /// </summary>
    public bool IsApplicationAuthentication => string.IsNullOrEmpty(Uid) && !HasRefreshToken;
}
