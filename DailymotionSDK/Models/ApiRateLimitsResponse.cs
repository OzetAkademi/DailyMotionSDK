using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// API rate limits response
/// https://developer.dailymotion.com/api#oauth-token-info
/// </summary>
public class ApiRateLimitsResponse
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    [JsonPropertyName("id")]
    public string? UserId { get; set; }
    /// <summary>
    /// Gets or sets the limits.
    /// </summary>
    /// <value>The limits.</value>
    public UserLimits? Limits { get; set; }
}
