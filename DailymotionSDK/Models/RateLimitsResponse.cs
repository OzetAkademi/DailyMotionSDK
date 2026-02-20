using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Rate limits response
/// https://developer.dailymotion.com/api#rate-limits
/// </summary>
public class RateLimitsResponse
{
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("remaining")]
    public int Remaining { get; set; }

    [JsonPropertyName("reset")]
    public long Reset { get; set; }

    [JsonPropertyName("window")]
    public string Window { get; set; } = string.Empty;
}
