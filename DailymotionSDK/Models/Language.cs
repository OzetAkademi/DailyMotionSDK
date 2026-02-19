using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Language model
/// </summary>
public class Language
{
    /// <summary>
    /// Language code
    /// </summary>
    [JsonProperty("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Language name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }
}
