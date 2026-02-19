namespace DailymotionSDK.Models;

/// <summary>
/// Auto quality setting for adaptive video streaming
/// Contains information about automatic quality selection for video playback
/// </summary>
public class AutoQuality
{
    /// <summary>
    /// Type of auto quality setting
    /// Indicates the quality selection algorithm or method used
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// URL for the auto quality stream
    /// Points to the adaptive streaming endpoint
    /// </summary>
    public string? Url { get; set; }
}
