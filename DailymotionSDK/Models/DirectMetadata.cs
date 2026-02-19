namespace DailymotionSDK.Models;

/// <summary>
/// Direct metadata for video containing essential video information for streaming
/// Used in direct link responses and video streaming contexts
/// </summary>
public class DirectMetadata
{
    /// <summary>
    /// URL to the video filmstrip (preview frames)
    /// </summary>
    public string? FilmstripUrl { get; set; }

    /// <summary>
    /// URL to the video poster image
    /// </summary>
    public string? PosterUrl { get; set; }

    /// <summary>
    /// Whether the video delivery is protected
    /// </summary>
    public bool ProtectedDelivery { get; set; }

    /// <summary>
    /// Channel where the video belongs
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    /// Timestamp when the video was created
    /// </summary>
    public int CreatedTime { get; set; }

    /// <summary>
    /// Duration of the video in seconds
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    /// List of tags associated with the video
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// Title of the video
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Direct URL to the video
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Mode of the video (e.g., "vod" for video on demand, "live" for live streaming)
    /// </summary>
    public string? Mode { get; set; }

    /// <summary>
    /// Whether the video is private
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Poster images in different sizes
    /// </summary>
    public Posters? Posters { get; set; }

    /// <summary>
    /// Owner information of the video
    /// </summary>
    public VideoOwner? Owner { get; set; }

    /// <summary>
    /// Type of streaming (e.g., "hls", "dash")
    /// </summary>
    public string? StreamType { get; set; }

    /// <summary>
    /// Available quality options for the video
    /// </summary>
    public Qualities? Qualities { get; set; }
}
