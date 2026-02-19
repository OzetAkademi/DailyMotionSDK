using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Playlist extractor response
/// </summary>
public class PlaylistExtractorResponse
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public bool Explicit { get; set; }
    public bool HasMore { get; set; }
    
    [JsonProperty("list")]
    public List<VideoListItem>? VideosList { get; set; }
}
