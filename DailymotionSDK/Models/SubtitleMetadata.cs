using System.Text.Json.Serialization;

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
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Subtitle format
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Subtitle language
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Subtitle URL
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
