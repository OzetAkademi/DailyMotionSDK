using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Subtitle metadata model
/// https://developers.dailymotion.com/api/platform-api/reference/#subtitle-fields
/// </summary>
public class SubtitleMetadata
{
    /// <summary>
    /// Subtitle ID
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Subtitle format
    /// </summary>
    [JsonProperty("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Subtitle language
    /// </summary>
    [JsonProperty("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Subtitle URL
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }
}
