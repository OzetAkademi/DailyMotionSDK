namespace DailymotionSDK.Models;

/// <summary>
/// User information response containing comprehensive user profile data
/// Represents a user's profile information and statistics
/// https://developer.dailymotion.com/api#user-fields
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Unique identifier of the user
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Timestamp when the user account was created
    /// </summary>
    public int CreatedTime { get; set; }

    /// <summary>
    /// Email address of the user
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Full name of the user
    /// </summary>
    public object? FullName { get; set; }

    /// <summary>
    /// User limits and restrictions
    /// </summary>
    public UserLimits? Limits { get; set; }

    /// <summary>
    /// Current status of the user account
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Direct URL to the user's profile page
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Whether the user account is verified
    /// </summary>
    public bool Verified { get; set; }

    /// <summary>
    /// Total number of videos uploaded by the user
    /// </summary>
    public int VideosTotal { get; set; }

    /// <summary>
    /// Total number of views across all user's videos
    /// </summary>
    public int ViewsTotal { get; set; }

    /// <summary>
    /// Total number of playlists created by the user
    /// </summary>
    public int PlaylistsTotal { get; set; }
    
    /// <summary>
    /// Convenience property to check if the user's email is verified
    /// Returns true if status is not "pending-activation"
    /// </summary>
    public bool EmailVerified => Status != "pending-activation";
}
