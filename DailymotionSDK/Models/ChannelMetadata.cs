using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Channel metadata
/// https://developer.dailymotion.com/api#channel-fields
/// </summary>
public class ChannelMetadata
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("avatar_120_url")]
    public string? Avatar120Url { get; set; }

    [JsonPropertyName("avatar_240_url")]
    public string? Avatar240Url { get; set; }

    [JsonPropertyName("avatar_360_url")]
    public string? Avatar360Url { get; set; }

    [JsonPropertyName("avatar_480_url")]
    public string? Avatar480Url { get; set; }

    [JsonPropertyName("avatar_720_url")]
    public string? Avatar720Url { get; set; }

    [JsonPropertyName("created_time")]
    public long CreatedTime { get; set; }

    [JsonPropertyName("videos_total")]
    public int VideosTotal { get; set; }

    [JsonPropertyName("playlists_total")]
    public int PlaylistsTotal { get; set; }

    [JsonPropertyName("subscribers_total")]
    public int SubscribersTotal { get; set; }
}
