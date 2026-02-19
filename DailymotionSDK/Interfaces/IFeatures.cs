using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion features operations
/// https://developer.dailymotion.com/api#features
/// </summary>
public interface IFeatures
{
    /// <summary>
    /// Gets user's featured videos
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetFeaturesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a video to features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to add to features</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddToFeaturesAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video from features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to remove from features</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveFromFeaturesAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video is in features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in features</returns>
    Task<bool> IsInFeaturesAsync(string videoId, CancellationToken cancellationToken = default);
}
