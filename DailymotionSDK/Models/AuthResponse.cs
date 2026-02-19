using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Authentication response from DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#auth
/// </summary>
public class AuthResponse
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("created_time")]
    public int CreatedTime { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("fullname")]
    public string? FullName { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("verified")]
    public bool Verified { get; set; }

    [JsonProperty("videos_total")]
    public int VideosTotal { get; set; }

    [JsonProperty("views_total")]
    public int ViewsTotal { get; set; }

    [JsonProperty("playlists_total")]
    public int PlaylistsTotal { get; set; }

    public bool EmailVerified => Status != "pending-activation";
}
