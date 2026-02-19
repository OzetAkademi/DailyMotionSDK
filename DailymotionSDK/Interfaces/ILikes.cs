using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion likes operations
/// https://developer.dailymotion.com/api#likes
/// </summary>
public interface ILikes
{
    /// <summary>
    /// Gets user's liked videos
    /// https://developer.dailymotion.com/api#user-likes
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetLikesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video is liked
    /// https://developer.dailymotion.com/api#user-likes
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is liked</returns>
    Task<bool> IsLikedAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Likes a video
    /// https://developer.dailymotion.com/api#user-likes
    /// </summary>
    /// <param name="videoId">Video ID to like</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> LikeVideoAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unlikes a video
    /// https://developer.dailymotion.com/api#user-likes
    /// </summary>
    /// <param name="videoId">Video ID to unlike</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> UnlikeVideoAsync(string videoId, CancellationToken cancellationToken = default);
}
