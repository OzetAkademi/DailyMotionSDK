namespace DailymotionSDK.Models;

/// <summary>
/// Video direct link response containing metadata and streaming URLs
/// Provides direct access to video content and metadata for streaming
/// https://developer.dailymotion.com/api#video-direct-link
/// </summary>
public class VideoDirectLinkResponse
{
    /// <summary>
    /// Direct metadata containing video information and streaming details
    /// Includes title, duration, owner, and other video properties
    /// </summary>
    public DirectMetadata? Metadata { get; set; }

    /// <summary>
    /// Video resolution URLs for different quality levels
    /// Contains direct streaming URLs for various video resolutions (144p, 240p, 480p, 720p, 1080p, etc.)
    /// </summary>
    public VideoResolutionUrls? VideoResolutionUrls { get; set; }
}
