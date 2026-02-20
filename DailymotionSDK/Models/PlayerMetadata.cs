using System.Text.Json.Serialization;

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
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Player name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Player description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Player URL
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Player embed URL
    /// </summary>
    [JsonPropertyName("embed_url")]
    public string? EmbedUrl { get; set; }

    /// <summary>
    /// Player embed HTML
    /// </summary>
    [JsonPropertyName("embed_html")]
    public string? EmbedHtml { get; set; }
}
