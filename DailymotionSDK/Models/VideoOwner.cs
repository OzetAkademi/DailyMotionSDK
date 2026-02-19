namespace DailymotionSDK.Models;

/// <summary>
/// Video owner information containing details about the user who created the video
/// https://developers.dailymotion.com/reference/video-fields
/// </summary>
public class VideoOwner
{
    /// <summary>
    /// Unique identifier of the video owner
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Parent object or organization that the owner belongs to
    /// Can be null if the owner is an individual user
    /// </summary>
    public object? Parent { get; set; }

    /// <summary>
    /// Display name or screen name of the video owner
    /// This is the name shown to other users
    /// </summary>
    public string? ScreenName { get; set; }

    /// <summary>
    /// Direct URL to the owner's profile page
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Username of the video owner
    /// This is the unique identifier used for login
    /// </summary>
    public string? Username { get; set; }
}
