using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Playlist extractor response
/// </summary>
public class PlaylistExtractorResponse
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    public int Page { get; set; }
    /// <summary>
    /// Gets or sets the limit.
    /// </summary>
    /// <value>The limit.</value>
    public int Limit { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PlaylistExtractorResponse"/> is explicit.
    /// </summary>
    /// <value><c>true</c> if explicit; otherwise, <c>false</c>.</value>
    public bool Explicit { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has more.
    /// </summary>
    /// <value><c>true</c> if this instance has more; otherwise, <c>false</c>.</value>
    public bool HasMore { get; set; }

    /// <summary>
    /// Gets or sets the videos list.
    /// </summary>
    /// <value>The videos list.</value>
    [JsonPropertyName("list")]
    public List<VideoListItem>? VideosList { get; set; }
}
