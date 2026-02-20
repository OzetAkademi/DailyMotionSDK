namespace DailymotionSDK.Models;

/// <summary>
/// Comment list response
/// https://developer.dailymotion.com/api#comment-list
/// </summary>
public class CommentListResponse
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    public int? Page { get; set; }
    /// <summary>
    /// Gets or sets the limit.
    /// </summary>
    /// <value>The limit.</value>
    public int? Limit { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has more.
    /// </summary>
    /// <value><c>null</c> if [has more] contains no value, <c>true</c> if [has more]; otherwise, <c>false</c>.</value>
    public bool? HasMore { get; set; }
    /// <summary>
    /// Gets or sets the list.
    /// </summary>
    /// <value>The list.</value>
    public List<CommentMetadata>? List { get; set; }
    /// <summary>
    /// Gets or sets the total.
    /// </summary>
    /// <value>The total.</value>
    public int? Total { get; set; }
}
