using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class User.
/// </summary>
public class User
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
    /// Gets or sets the display name.
    /// </summary>
    /// <value>The display name.</value>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the created at.
    /// </summary>
    /// <value>The created at.</value>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the updated at.
    /// </summary>
    /// <value>The updated at.</value>
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}