using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Response model for video list API calls
/// https://developers.dailymotion.com/api/platform-api/reference/#video
/// </summary>
public class VideoListResponse
{
    /// <summary>
    /// Current page number
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Total number of items available
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// Whether there are more pages available
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// List of videos
    /// </summary>
    [JsonPropertyName("list")]
    public List<VideoMetadata>? List { get; set; }
}
