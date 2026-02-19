using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Locale detection response model
/// https://developers.dailymotion.com/api/platform-api/reference/#locale-detection-response
/// </summary>
public class LocaleDetectionResponse
{
    /// <summary>
    /// Detected locale
    /// </summary>
    [JsonProperty("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// Country code
    /// </summary>
    [JsonProperty("country")]
    public string? Country { get; set; }

    /// <summary>
    /// Language code
    /// </summary>
    [JsonProperty("language")]
    public string? Language { get; set; }
}
