using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// User list response
/// https://developer.dailymotion.com/api#user-list
/// </summary>
public class UserListResponse
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the limit.
    /// </summary>
    /// <value>The limit.</value>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="UserListResponse"/> is explicit.
    /// </summary>
    /// <value><c>true</c> if explicit; otherwise, <c>false</c>.</value>
    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }

    /// <summary>
    /// Gets or sets the total.
    /// </summary>
    /// <value>The total.</value>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has more.
    /// </summary>
    /// <value><c>true</c> if this instance has more; otherwise, <c>false</c>.</value>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// Gets or sets the list.
    /// </summary>
    /// <value>The list.</value>
    [JsonPropertyName("list")]
    public List<UserMetadata> List { get; set; } = [];
}
