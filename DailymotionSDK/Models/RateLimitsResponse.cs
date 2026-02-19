using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Rate limits response
/// https://developer.dailymotion.com/api#rate-limits
/// </summary>
public class RateLimitsResponse
{
    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("remaining")]
    public int Remaining { get; set; }

    [JsonProperty("reset")]
    public long Reset { get; set; }

    [JsonProperty("window")]
    public string Window { get; set; } = string.Empty;
}
