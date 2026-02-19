using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Token validation response
/// https://developer.dailymotion.com/api#oauth-token-validation
/// </summary>
public class TokenValidationResponse
{
    [JsonProperty("valid")]
    public bool Valid { get; set; }

    [JsonProperty("uid")]
    public string? Uid { get; set; }

    [JsonProperty("scope")]
    public string? Scope { get; set; }

    [JsonProperty("expires_in")]
    public int? ExpiresIn { get; set; }
}
