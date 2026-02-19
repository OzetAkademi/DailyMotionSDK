using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Channel metadata
/// https://developer.dailymotion.com/api#channel-fields
/// </summary>
public class ChannelMetadata
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("avatar_120_url")]
    public string? Avatar120Url { get; set; }

    [JsonProperty("avatar_240_url")]
    public string? Avatar240Url { get; set; }

    [JsonProperty("avatar_360_url")]
    public string? Avatar360Url { get; set; }

    [JsonProperty("avatar_480_url")]
    public string? Avatar480Url { get; set; }

    [JsonProperty("avatar_720_url")]
    public string? Avatar720Url { get; set; }

    [JsonProperty("created_time")]
    public long CreatedTime { get; set; }

    [JsonProperty("videos_total")]
    public int VideosTotal { get; set; }

    [JsonProperty("playlists_total")]
    public int PlaylistsTotal { get; set; }

    [JsonProperty("subscribers_total")]
    public int SubscribersTotal { get; set; }
}
