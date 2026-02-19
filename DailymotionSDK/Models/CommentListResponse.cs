namespace DailymotionSDK.Models;

/// <summary>
/// Comment list response
/// https://developer.dailymotion.com/api#comment-list
/// </summary>
public class CommentListResponse
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public bool? HasMore { get; set; }
    public List<CommentMetadata>? List { get; set; }
    public int? Total { get; set; }
}
