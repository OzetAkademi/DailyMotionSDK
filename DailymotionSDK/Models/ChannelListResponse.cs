using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Channel list response
/// https://developer.dailymotion.com/api#channel-list
/// </summary>
public class ChannelListResponse
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    public int Page { get; set; }
    /// <summary>
    /// Gets or sets the limit.
    /// </summary>
    /// <value>The limit.</value>
    public int Limit { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ChannelListResponse"/> is explicit.
    /// </summary>
    /// <value><c>true</c> if explicit; otherwise, <c>false</c>.</value>
    public bool Explicit { get; set; }
    /// <summary>
    /// Gets or sets the total.
    /// </summary>
    /// <value>The total.</value>
    public int Total { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has more.
    /// </summary>
    /// <value><c>true</c> if this instance has more; otherwise, <c>false</c>.</value>
    public bool HasMore { get; set; }

    /// <summary>
    /// Gets or sets the channels list.
    /// </summary>
    /// <value>The channels list.</value>
    [JsonPropertyName("list")]
    public List<ChannelMetadata>? ChannelsList { get; set; }
}
