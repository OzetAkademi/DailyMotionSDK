using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Playlist list response
/// https://developer.dailymotion.com/api#playlist-list
/// </summary>
public class PlaylistListResponse
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("explicit")]
    public bool Explicit { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("has_more")]
    public bool HasMore { get; set; }

    [JsonProperty("list")]
    public List<PlaylistMetadata> List { get; set; } = new();
}
