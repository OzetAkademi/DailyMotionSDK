using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion history operations
/// https://developer.dailymotion.com/api#history
/// </summary>
public interface IHistory
{
    /// <summary>
    /// Gets user's watch history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetHistoryAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a video to history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to add to history</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddToHistoryAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video from history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to remove from history</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveFromHistoryAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video is in history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in history</returns>
    Task<bool> IsInHistoryAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears all history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ClearHistoryAsync(CancellationToken cancellationToken = default);
}
