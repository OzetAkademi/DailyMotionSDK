using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class Me.
/// </summary>
public class Me
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the profiles.
    /// </summary>
    /// <value>The profiles.</value>
    [JsonPropertyName("profiles")]
    public List<MeProfiles>? Profiles { get; set; }
}