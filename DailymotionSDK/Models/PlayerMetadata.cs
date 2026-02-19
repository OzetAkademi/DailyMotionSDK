using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Player metadata model
/// https://developers.dailymotion.com/api/platform-api/reference/#player-fields
/// </summary>
public class PlayerMetadata
{
    /// <summary>
    /// Player ID
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Player name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Player description
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Player URL
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Player embed URL
    /// </summary>
    [JsonProperty("embed_url")]
    public string? EmbedUrl { get; set; }

    /// <summary>
    /// Player embed HTML
    /// </summary>
    [JsonProperty("embed_html")]
    public string? EmbedHtml { get; set; }
}
