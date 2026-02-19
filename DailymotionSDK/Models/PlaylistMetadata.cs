using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Playlist metadata
/// https://developers.dailymotion.com/api/platform-api/reference/#playlist
/// </summary>
public class PlaylistMetadata
{
    /// <summary>
    /// Unique object identifier (unique among all playlists)
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Short descriptive name of this playlist
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Comprehensive description of this playlist
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Graph type of this object (hopefully playlist)
    /// </summary>
    [JsonProperty("item_type")]
    public string? ItemType { get; set; }

    /// <summary>
    /// Date and time when this playlist was created
    /// </summary>
    [JsonProperty("created_time")]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// Date and time when this playlist was last updated
    /// </summary>
    [JsonProperty("updated_time")]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// True if this playlist is private
    /// </summary>
    [JsonProperty("private")]
    public bool Private { get; set; }

    /// <summary>
    /// Number of videos in this playlist
    /// </summary>
    [JsonProperty("videos_total")]
    public int VideosTotal { get; set; }

    /// <summary>
    /// Owner of the playlist
    /// </summary>
    [JsonProperty("owner")]
    public string? Owner { get; set; }

    /// <summary>
    /// URL of this playlist's first video thumbnail image (60px height)
    /// </summary>
    [JsonProperty("thumbnail_60_url")]
    public string? Thumbnail60Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (120px height)
    /// </summary>
    [JsonProperty("thumbnail_120_url")]
    public string? Thumbnail120Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (180px height)
    /// </summary>
    [JsonProperty("thumbnail_180_url")]
    public string? Thumbnail180Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (240px height)
    /// </summary>
    [JsonProperty("thumbnail_240_url")]
    public string? Thumbnail240Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (360px height)
    /// </summary>
    [JsonProperty("thumbnail_360_url")]
    public string? Thumbnail360Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (480px height)
    /// </summary>
    [JsonProperty("thumbnail_480_url")]
    public string? Thumbnail480Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (720px height)
    /// </summary>
    [JsonProperty("thumbnail_720_url")]
    public string? Thumbnail720Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (1080px height)
    /// </summary>
    [JsonProperty("thumbnail_1080_url")]
    public string? Thumbnail1080Url { get; set; }

    /// <summary>
    /// URL of the thumbnail of this playlist's first video (raw, respecting full size ratio)
    /// </summary>
    [JsonProperty("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }
}
