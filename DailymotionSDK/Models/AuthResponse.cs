using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Authentication response from DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#auth
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the created time.
    /// </summary>
    /// <value>The created time.</value>
    [JsonPropertyName("created_time")]
    public int CreatedTime { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    /// <value>The full name.</value>
    [JsonPropertyName("fullname")]
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="AuthResponse"/> is verified.
    /// </summary>
    /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    /// <summary>
    /// Gets or sets the videos total.
    /// </summary>
    /// <value>The videos total.</value>
    [JsonPropertyName("videos_total")]
    public int VideosTotal { get; set; }

    /// <summary>
    /// Gets or sets the views total.
    /// </summary>
    /// <value>The views total.</value>
    [JsonPropertyName("views_total")]
    public int ViewsTotal { get; set; }

    /// <summary>
    /// Gets or sets the playlists total.
    /// </summary>
    /// <value>The playlists total.</value>
    [JsonPropertyName("playlists_total")]
    public int PlaylistsTotal { get; set; }

    /// <summary>
    /// Gets a value indicating whether [email verified].
    /// </summary>
    /// <value><c>true</c> if [email verified]; otherwise, <c>false</c>.</value>
    public bool EmailVerified => Status != "pending-activation";
}
