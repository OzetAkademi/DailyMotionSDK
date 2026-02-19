namespace DailymotionSDK.Models;

/// <summary>
/// Video qualities containing available quality options for video streaming
/// Provides information about different quality levels and adaptive streaming options
/// </summary>
public class Qualities
{
    /// <summary>
    /// List of auto quality settings for adaptive streaming
    /// Contains automatic quality selection options for optimal viewing experience
    /// </summary>
    public List<AutoQuality>? Auto { get; set; }
}
