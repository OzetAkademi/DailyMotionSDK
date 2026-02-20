using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Language model
/// </summary>
public class Language
{
    /// <summary>
    /// Language code
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Language name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
