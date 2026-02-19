using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// API rate limits response
/// https://developer.dailymotion.com/api#oauth-token-info
/// </summary>
public class ApiRateLimitsResponse
{
    [JsonProperty("id")]
    public string? UserId { get; set; }
    public UserLimits? Limits { get; set; }
}
