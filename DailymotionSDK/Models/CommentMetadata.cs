namespace DailymotionSDK.Models;

/// <summary>
/// Comment metadata containing information about video comments
/// Represents a comment on a video with all associated metadata
/// https://developer.dailymotion.com/api#comment-fields
/// </summary>
public class CommentMetadata
{
    /// <summary>
    /// Unique identifier of the comment
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Text content of the comment
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Timestamp when the comment was created
    /// </summary>
    public string? CreatedTime { get; set; }

    /// <summary>
    /// Language of the comment
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Parent comment ID if this is a reply
    /// Null if this is a top-level comment
    /// </summary>
    public string? Parent { get; set; }

    /// <summary>
    /// ID of the video this comment belongs to
    /// </summary>
    public string? Video { get; set; }

    /// <summary>
    /// Owner information of the comment author
    /// </summary>
    public VideoOwner? Owner { get; set; }

    /// <summary>
    /// Number of replies to this comment
    /// </summary>
    public int? Replies { get; set; }

    /// <summary>
    /// Number of votes (likes/dislikes) for this comment
    /// </summary>
    public int? Votes { get; set; }

    /// <summary>
    /// Whether the comment has been flagged as spam
    /// </summary>
    public bool? IsSpam { get; set; }

    /// <summary>
    /// Whether the comment is hidden from public view
    /// </summary>
    public bool? IsHidden { get; set; }

    /// <summary>
    /// Whether the comment has been deleted
    /// </summary>
    public bool? IsDeleted { get; set; }
}
