using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// User metadata containing detailed user profile information and statistics
/// Represents comprehensive user data including avatars, statistics, and profile information
/// https://developer.dailymotion.com/api#user-fields
/// </summary>
public class UserMetadata
{
    /// <summary>
    /// Unique identifier of the user
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Username of the user (unique identifier for login)
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Display name or screen name of the user
    /// This is the name shown to other users
    /// </summary>
    [JsonProperty("screenname")]
    public string? ScreenName { get; set; }

    /// <summary>
    /// User's profile description or bio
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// URL to 120x120 pixel avatar image
    /// Small avatar suitable for lists and compact displays
    /// </summary>
    [JsonProperty("avatar_120_url")]
    public string? Avatar120Url { get; set; }

    /// <summary>
    /// URL to 240x240 pixel avatar image
    /// Medium-small avatar for standard displays
    /// </summary>
    [JsonProperty("avatar_240_url")]
    public string? Avatar240Url { get; set; }

    /// <summary>
    /// URL to 360x360 pixel avatar image
    /// Medium avatar for high-resolution displays
    /// </summary>
    [JsonProperty("avatar_360_url")]
    public string? Avatar360Url { get; set; }

    /// <summary>
    /// URL to 480x480 pixel avatar image
    /// Large avatar for detailed profile views
    /// </summary>
    [JsonProperty("avatar_480_url")]
    public string? Avatar480Url { get; set; }

    /// <summary>
    /// URL to 720x720 pixel avatar image
    /// High-resolution avatar for large displays
    /// </summary>
    [JsonProperty("avatar_720_url")]
    public string? Avatar720Url { get; set; }

    /// <summary>
    /// Timestamp when the user account was created
    /// </summary>
    [JsonProperty("created_time")]
    public long CreatedTime { get; set; }

    /// <summary>
    /// Current status of the user account
    /// </summary>
    [JsonProperty("status")]
    public UserStatus Status { get; set; }

    /// <summary>
    /// Total number of videos uploaded by the user
    /// </summary>
    [JsonProperty("videos_total")]
    public int VideosTotal { get; set; }

    /// <summary>
    /// Total number of playlists created by the user
    /// </summary>
    [JsonProperty("playlists_total")]
    public int PlaylistsTotal { get; set; }

    /// <summary>
    /// Total number of followers the user has
    /// </summary>
    [JsonProperty("followers_total")]
    public int FollowersTotal { get; set; }

    /// <summary>
    /// Total number of users the user is following
    /// </summary>
    [JsonProperty("following_total")]
    public int FollowingTotal { get; set; }

    /// <summary>
    /// Total number of likes the user has received
    /// </summary>
    [JsonProperty("likes_total")]
    public int LikesTotal { get; set; }

    /// <summary>
    /// Total number of views across all user's videos
    /// </summary>
    [JsonProperty("views_total")]
    public long ViewsTotal { get; set; }
}
