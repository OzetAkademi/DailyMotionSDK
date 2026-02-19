using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion general operations
/// https://developer.dailymotion.com/api#general
/// </summary>
public interface IGeneral
{
    /// <summary>
    /// Searches for videos
    /// https://developer.dailymotion.com/api#video-search
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> SearchVideosAsync(string query, int limit = 20, int page = 1, VideoSort sort = VideoSort.Relevance, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for users
    /// https://developer.dailymotion.com/api#user-search
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    Task<UserListResponse> SearchUsersAsync(string query, int limit = 20, int page = 1, UserSort sort = UserSort.Relevance, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for playlists
    /// https://developer.dailymotion.com/api#playlist-search
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> SearchPlaylistsAsync(string query, int limit = 20, int page = 1, PlaylistSort sort = PlaylistSort.Relevance, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets trending videos
    /// https://developer.dailymotion.com/api#video-list
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetTrendingVideosAsync(int limit = 20, int page = 1, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets featured videos
    /// https://developer.dailymotion.com/api#video-list
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetFeaturedVideosAsync(int limit = 20, int page = 1, CancellationToken cancellationToken = default);
}
