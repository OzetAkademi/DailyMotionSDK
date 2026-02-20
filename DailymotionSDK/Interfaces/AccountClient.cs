using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion account operations
/// https://developer.dailymotion.com/api#account
/// </summary>
public class AccountClient : IAccount
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<AccountClient> _logger;
    /// <summary>
    /// The json options
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    /// <exception cref="System.ArgumentNullException">logger</exception>
    public AccountClient(IDailymotionHttpClient httpClient, ILogger<AccountClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Gets current user information
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    public async Task<UserMetadata?> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user info");

            var response = await _httpClient.GetAsync("/me", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<UserMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info");
            throw;
        }
    }

    /// <summary>
    /// Checks if the current access token is valid
    /// https://developer.dailymotion.com/api#oauth-token-validation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token validation response</returns>
    public async Task<TokenValidationResponse?> ValidateAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Validating access token");

            var response = await _httpClient.GetAsync("/oauth/token/info", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to validate access token: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<TokenValidationResponse>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating access token");
            throw;
        }
    }

    /// <summary>
    /// Revokes the current access token
    /// https://developer.dailymotion.com/api#oauth-token-revocation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> RevokeAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Revoking access token");

            var response = await _httpClient.PostAsync("/oauth/token/revoke", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to revoke access token: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking access token");
            throw;
        }
    }

    /// <summary>
    /// Gets API rate limits
    /// https://developer.dailymotion.com/api#rate-limits
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rate limits information</returns>
    public async Task<RateLimitsResponse?> GetRateLimitsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting rate limits");

            var response = await _httpClient.GetAsync("/rate_limits", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get rate limits: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<RateLimitsResponse>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rate limits");
            throw;
        }
    }
}