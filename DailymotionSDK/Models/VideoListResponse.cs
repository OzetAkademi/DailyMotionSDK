using Newtonsoft.Json;

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
    [JsonProperty("page")]
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Total number of items available
    /// </summary>
    [JsonProperty("total")]
    public int Total { get; set; }

    /// <summary>
    /// Whether there are more pages available
    /// </summary>
    [JsonProperty("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// List of videos
    /// </summary>
    [JsonProperty("list")]
    public List<VideoMetadata>? List { get; set; }
}
