using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class VideoListResponse.
/// </summary>
public class VideoListResponse
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>The data.</value>
    [JsonPropertyName("data")]
    public List<VideoItem> Data { get; init; } = [];

    /// <summary>
    /// Gets the pagination.
    /// </summary>
    /// <value>The pagination.</value>
    [JsonPropertyName("pagination")]
    public PaginationMetadata? Pagination { get; init; }
}

/// <summary>
/// Class VideoItem.
/// </summary>
public class VideoItem
{
    /// <summary>
    /// Gets the video identifier.
    /// </summary>
    /// <value>The video identifier.</value>
    [JsonPropertyName("video_id")]
    public string? VideoId { get; init; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>The title.</value>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// Gets the created at.
    /// </summary>
    /// <value>The created at.</value>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }
}

/// <summary>
/// Class PaginationMetadata.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Gets the page.
    /// </summary>
    /// <value>The page.</value>
    [JsonPropertyName("page")]
    public int Page { get; init; }

    /// <summary>
    /// Gets the size of the page.
    /// </summary>
    /// <value>The size of the page.</value>
    [JsonPropertyName("page_size")]
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total.
    /// </summary>
    /// <value>The total.</value>
    [JsonPropertyName("total")]
    public int Total { get; init; }

    /// <summary>
    /// Gets the next.
    /// </summary>
    /// <value>The next.</value>
    [JsonPropertyName("next")]
    public string? Next { get; init; }

    /// <summary>
    /// Gets the previous.
    /// </summary>
    /// <value>The previous.</value>
    [JsonPropertyName("previous")]
    public string? Previous { get; init; }

    /// <summary>
    /// Gets a value indicating whether this instance has next page.
    /// </summary>
    /// <value><c>true</c> if this instance has next page; otherwise, <c>false</c>.</value>
    public bool HasNextPage => !string.IsNullOrEmpty(Next);

    /// <summary>
    /// Gets a value indicating whether this instance has previous page.
    /// </summary>
    /// <value><c>true</c> if this instance has previous page; otherwise, <c>false</c>.</value>
    public bool HasPreviousPage => !string.IsNullOrEmpty(Previous);
}