using System.Text.Json.Serialization;

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
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Short descriptive name of this playlist
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Comprehensive description of this playlist
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Graph type of this object (hopefully playlist)
    /// </summary>
    [JsonPropertyName("item_type")]
    public string? ItemType { get; set; }

    /// <summary>
    /// Date and time when this playlist was created
    /// </summary>
    [JsonPropertyName("created_time")]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// Date and time when this playlist was last updated
    /// </summary>
    [JsonPropertyName("updated_time")]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// True if this playlist is private
    /// </summary>
    [JsonPropertyName("private")]
    public bool Private { get; set; }

    /// <summary>
    /// Number of videos in this playlist
    /// </summary>
    [JsonPropertyName("videos_total")]
    public int VideosTotal { get; set; }

    /// <summary>
    /// Owner of the playlist
    /// </summary>
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    /// <summary>
    /// URL of this playlist's first video thumbnail image (60px height)
    /// </summary>
    [JsonPropertyName("thumbnail_60_url")]
    public string? Thumbnail60Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (120px height)
    /// </summary>
    [JsonPropertyName("thumbnail_120_url")]
    public string? Thumbnail120Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (180px height)
    /// </summary>
    [JsonPropertyName("thumbnail_180_url")]
    public string? Thumbnail180Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (240px height)
    /// </summary>
    [JsonPropertyName("thumbnail_240_url")]
    public string? Thumbnail240Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (360px height)
    /// </summary>
    [JsonPropertyName("thumbnail_360_url")]
    public string? Thumbnail360Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (480px height)
    /// </summary>
    [JsonPropertyName("thumbnail_480_url")]
    public string? Thumbnail480Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (720px height)
    /// </summary>
    [JsonPropertyName("thumbnail_720_url")]
    public string? Thumbnail720Url { get; set; }

    /// <summary>
    /// URL of this playlist's first video's thumbnail image (1080px height)
    /// </summary>
    [JsonPropertyName("thumbnail_1080_url")]
    public string? Thumbnail1080Url { get; set; }

    /// <summary>
    /// URL of the thumbnail of this playlist's first video (raw, respecting full size ratio)
    /// </summary>
    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }
}
