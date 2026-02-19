using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion account operations
/// https://developer.dailymotion.com/api#account
/// </summary>
public interface IAccount
{
    /// <summary>
    /// Gets current user information
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    Task<UserMetadata?> GetUserInfoAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the current access token is valid
    /// https://developer.dailymotion.com/api#oauth-token-validation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token validation response</returns>
    Task<TokenValidationResponse?> ValidateAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the current access token
    /// https://developer.dailymotion.com/api#oauth-token-revocation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RevokeAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets API rate limits
    /// https://developer.dailymotion.com/api#rate-limits
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rate limits information</returns>
    Task<RateLimitsResponse?> GetRateLimitsAsync(CancellationToken cancellationToken = default);
}