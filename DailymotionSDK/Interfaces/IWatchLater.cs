using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion watch later operations
/// https://developer.dailymotion.com/api#watch-later
/// </summary>
public interface IWatchLater
{
    /// <summary>
    /// Gets user's watch later videos
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetWatchLaterAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a video to watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to add to watch later</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddToWatchLaterAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video from watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to remove from watch later</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveFromWatchLaterAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video is in watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in watch later</returns>
    Task<bool> IsInWatchLaterAsync(string videoId, CancellationToken cancellationToken = default);
}