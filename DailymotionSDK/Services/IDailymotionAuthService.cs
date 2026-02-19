using DailymotionSDK.Models;
using DailymotionSDK.Configuration;

namespace DailymotionSDK.Services;

/// <summary>
/// Interface for DailyMotion authentication service
/// Provides OAuth 2.0 authentication flows for DailyMotion API
/// </summary>
public interface IDailymotionAuthService
{
    /// <summary>
    /// Gets the current access token
    /// </summary>
    string? AccessToken { get; }

    /// <summary>
    /// Gets the current refresh token
    /// </summary>
    string? RefreshToken { get; }

    /// <summary>
    /// Gets whether the current token is expired
    /// </summary>
    bool IsTokenExpired { get; }

    /// <summary>
    /// Gets the current API key type used for authentication
    /// </summary>
    ApiKeyType CurrentApiKeyType { get; }

    /// <summary>
    /// Authenticates using client credentials grant type
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#client-credentials
    /// </summary>
    /// <param name="apiKey">API Key for client credentials</param>
    /// <param name="apiSecret">API Secret for client credentials</param>
    /// <param name="apiKeyType">Type of API key (Public or Private) - determines which endpoints to use</param>
    /// <param name="scopes">Required scopes for the access token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<TokenResponse> AuthenticateWithClientCredentialsAsync(string apiKey, string apiSecret, ApiKeyType apiKeyType, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates using username and password (Resource Owner Password Credentials flow)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="scopes">Optional scopes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<TokenResponse> AuthenticateWithPasswordAsync(string username, string password, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates using username and password with specific API keys (Resource Owner Password Credentials flow)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="apiKey">Public API Key</param>
    /// <param name="apiSecret">Public API Secret</param>
    /// <param name="scopes">Optional scopes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<TokenResponse> AuthenticateWithPasswordAsync(string username, string password, string apiKey, string apiSecret, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exchanges authorization code for access token
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#authorization-code
    /// </summary>
    /// <param name="authorizationCode">Authorization code from OAuth flow</param>
    /// <param name="redirectUri">Redirect URI</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token exchange result</returns>
    Task<TokenResponse> ExchangeCodeForTokenAsync(string authorizationCode, string redirectUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the access token using refresh token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token refresh result</returns>
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates the current access token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token validation result</returns>
    Task<TokenValidationResponse?> ValidateTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the access token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="accessToken">Access token to revoke</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RevokeTokenAsync(string accessToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears all stored tokens
    /// </summary>
    void ClearTokens();
}