namespace DailymotionSDK.Models;

/// <summary>
/// Video resolution URLs for different quality levels
/// Contains direct streaming URLs for various video resolutions
/// https://developers.dailymotion.com/reference/video-fields
/// </summary>
public class VideoResolutionUrls
{
    /// <summary>
    /// Name or identifier for this set of resolution URLs
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Direct streaming URL for 144p resolution (very low quality)
    /// Typically used for very slow connections or mobile data conservation
    /// </summary>
    public string? R144 { get; set; }

    /// <summary>
    /// Direct streaming URL for 240p resolution (low quality)
    /// Suitable for slow internet connections or mobile devices
    /// </summary>
    public string? R240 { get; set; }

    /// <summary>
    /// Direct streaming URL for 380p resolution (medium-low quality)
    /// Good balance between quality and bandwidth usage
    /// </summary>
    public string? R380 { get; set; }

    /// <summary>
    /// Direct streaming URL for 480p resolution (standard definition)
    /// Standard quality for most viewing scenarios
    /// </summary>
    public string? R480 { get; set; }

    /// <summary>
    /// Direct streaming URL for 720p resolution (high definition)
    /// High quality suitable for larger screens and good internet connections
    /// </summary>
    public string? R720 { get; set; }

    /// <summary>
    /// Direct streaming URL for 1080p resolution (full HD)
    /// High quality suitable for large screens and fast internet connections
    /// </summary>
    public string? R1080 { get; set; }

    /// <summary>
    /// Direct streaming URL for 1440p resolution (2K/Quad HD)
    /// Very high quality for professional displays and fast internet connections
    /// </summary>
    public string? R1440 { get; set; }

    /// <summary>
    /// Direct streaming URL for 2160p resolution (4K/Ultra HD)
    /// Ultra high quality for 4K displays and very fast internet connections
    /// </summary>
    public string? R2160 { get; set; }
}
