using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Locale detection response model
/// https://developers.dailymotion.com/api/platform-api/reference/#locale-detection-response
/// </summary>
public class LocaleDetectionResponse
{
    /// <summary>
    /// Gets or sets the locale.
    /// </summary>
    /// <value>The locale.</value>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>The country.</value>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the language.
    /// </summary>
    /// <value>The language.</value>
    [JsonPropertyName("language")]
    public string? Language { get; set; }
}
