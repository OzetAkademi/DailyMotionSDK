using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Languages response model
/// https://developers.dailymotion.com/api/platform-api/reference/#languages-response
/// </summary>
public class LanguagesResponse
{
    /// <summary>
    /// List of available languages
    /// </summary>
    [JsonPropertyName("list")]
    public List<Language>? List { get; set; }
}
