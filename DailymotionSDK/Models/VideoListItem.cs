namespace DailymotionSDK.Models;

/// <summary>
/// Video list item containing essential video information for playlists and collections
/// Used in playlist responses and video list endpoints
/// https://developers.dailymotion.com/reference/video-fields
/// </summary>
public class VideoListItem
{
    /// <summary>
    /// Duration of the video in seconds
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    /// Embed URL for the video player
    /// Used for embedding videos in external websites
    /// </summary>
    public string? EmbedUrl { get; set; }

    /// <summary>
    /// Height of the video in pixels
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Unique identifier of the video
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// URL to a 60x60 pixel thumbnail image
    /// Small thumbnail suitable for lists and previews
    /// </summary>
    public string? Thumbnail60Url { get; set; }

    /// <summary>
    /// URL to the main thumbnail image
    /// Standard thumbnail used for video previews
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Title of the video
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Direct URL to the video page
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Total number of views the video has received
    /// </summary>
    public int ViewsTotal { get; set; }

    /// <summary>
    /// Width of the video in pixels
    /// </summary>
    public int Width { get; set; }
}
