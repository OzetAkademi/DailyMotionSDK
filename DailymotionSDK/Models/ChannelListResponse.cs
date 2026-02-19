using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Channel list response
/// https://developer.dailymotion.com/api#channel-list
/// </summary>
public class ChannelListResponse
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public bool Explicit { get; set; }
    public int Total { get; set; }
    public bool HasMore { get; set; }
    
    [JsonProperty("list")]
    public List<ChannelMetadata>? ChannelsList { get; set; }
}
