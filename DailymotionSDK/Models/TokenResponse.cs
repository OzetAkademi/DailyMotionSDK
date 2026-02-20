using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Token response from OAuth flow
/// https://developer.dailymotion.com/api#oauth-token
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    /// <value>The access token.</value>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the token.
    /// </summary>
    /// <value>The type of the token.</value>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expires in.
    /// </summary>
    /// <value>The expires in.</value>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    /// <value>The refresh token.</value>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the scope.
    /// </summary>
    /// <value>The scope.</value>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// Gets or sets the uid.
    /// </summary>
    /// <value>The uid.</value>
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [email verified].
    /// </summary>
    /// <value><c>null</c> if [email verified] contains no value, <c>true</c> if [email verified]; otherwise, <c>false</c>.</value>
    [JsonPropertyName("email_verified")]
    public bool? EmailVerified { get; set; }

    /// <summary>
    /// Gets whether this token response includes a refresh token
    /// </summary>
    /// <value><c>true</c> if this instance has refresh token; otherwise, <c>false</c>.</value>
    public bool HasRefreshToken => !string.IsNullOrEmpty(RefreshToken);

    /// <summary>
    /// Gets whether this appears to be a user-level authentication (has UID and potentially refresh token)
    /// </summary>
    /// <value><c>true</c> if this instance is user authentication; otherwise, <c>false</c>.</value>
    public bool IsUserAuthentication => !string.IsNullOrEmpty(Uid);

    /// <summary>
    /// Gets whether this appears to be an application-level authentication (no UID, no refresh token)
    /// </summary>
    /// <value><c>true</c> if this instance is application authentication; otherwise, <c>false</c>.</value>
    public bool IsApplicationAuthentication => string.IsNullOrEmpty(Uid) && !HasRefreshToken;
}
