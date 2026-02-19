using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion favorites operations
/// https://developer.dailymotion.com/api#favorites
/// </summary>
public interface IFavorites
{
    /// <summary>
    /// Gets user's favorite videos
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetFavoritesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a video to favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to add to favorites</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddToFavoritesAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video from favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to remove from favorites</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveFromFavoritesAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video is in favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in favorites</returns>
    Task<bool> IsInFavoritesAsync(string videoId, CancellationToken cancellationToken = default);
}
