using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Token validation response
/// https://developer.dailymotion.com/api#oauth-token-validation
/// </summary>
public class TokenValidationResponse
{
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }
}
