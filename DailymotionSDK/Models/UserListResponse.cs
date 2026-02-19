using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// User list response
/// https://developer.dailymotion.com/api#user-list
/// </summary>
public class UserListResponse
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("explicit")]
    public bool Explicit { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("has_more")]
    public bool HasMore { get; set; }

    [JsonProperty("list")]
    public List<UserMetadata> List { get; set; } = new();
}
