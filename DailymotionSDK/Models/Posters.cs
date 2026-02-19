using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Video posters in different resolutions
/// Contains thumbnail images in various sizes for different display contexts
/// </summary>
public class Posters
{
    /// <summary>
    /// 60x60 pixel poster image
    /// Small thumbnail suitable for lists and previews
    /// </summary>
    [JsonProperty("60")]
    public string? X60 { get; set; }
    
    /// <summary>
    /// 120x120 pixel poster image
    /// Small thumbnail for compact displays
    /// </summary>
    [JsonProperty("120")]
    public string? X120 { get; set; }
    
    /// <summary>
    /// 180x180 pixel poster image
    /// Medium-small thumbnail for mobile displays
    /// </summary>
    [JsonProperty("180")]
    public string? X180 { get; set; }
    
    /// <summary>
    /// 240x240 pixel poster image
    /// Medium thumbnail for standard displays
    /// </summary>
    [JsonProperty("240")]
    public string? X240 { get; set; }
    
    /// <summary>
    /// 360x360 pixel poster image
    /// Medium-large thumbnail for high-resolution displays
    /// </summary>
    [JsonProperty("360")]
    public string? X360 { get; set; }
    
    /// <summary>
    /// 480x480 pixel poster image
    /// Large thumbnail for detailed previews
    /// </summary>
    [JsonProperty("480")]
    public string? X480 { get; set; }
    
    /// <summary>
    /// 720x720 pixel poster image
    /// High-resolution thumbnail for large displays
    /// </summary>
    [JsonProperty("720")]
    public string? X720 { get; set; }
    
    /// <summary>
    /// 1080x1080 pixel poster image
    /// Full HD thumbnail for high-resolution displays
    /// </summary>
    [JsonProperty("1080")]
    public string? X1080 { get; set; }
    
    /// <summary>
    /// 1440x1440 pixel poster image
    /// 2K thumbnail for ultra-high-resolution displays
    /// </summary>
    [JsonProperty("1440")]
    public string? X1440 { get; set; }
    
    /// <summary>
    /// 2160x2160 pixel poster image
    /// 4K thumbnail for ultra-high-resolution displays
    /// </summary>
    [JsonProperty("2160")]
    public string? X2160 { get; set; }
}
