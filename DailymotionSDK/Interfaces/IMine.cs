using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion "mine" operations (current user)
/// https://developer.dailymotion.com/api#user
/// </summary>
public interface IMine
{
    /// <summary>
    /// Gets current user information
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    Task<UserMetadata?> GetUserInfoAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's videos
    /// https://developer.dailymotion.com/api#user-videos
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetVideosAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the videos asynchronous.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;VideoListResponse&gt;&gt;.</returns>
    Task<VideoListResponse?> GetVideosAsync(VideoFilters? filters = null, VideoFields[]? fields = null, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's playlists
    /// https://developer.dailymotion.com/api#user-playlists
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> GetPlaylistsAsync(int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's followers
    /// https://developer.dailymotion.com/api#user-followers
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    Task<UserListResponse> GetFollowersAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's following
    /// https://developer.dailymotion.com/api#user-following
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    Task<UserListResponse> GetFollowingAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Follows a user
    /// https://developer.dailymotion.com/api#user-follow
    /// </summary>
    /// <param name="userId">User ID to follow</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> FollowUserAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unfollows a user
    /// https://developer.dailymotion.com/api#user-follow
    /// </summary>
    /// <param name="userId">User ID to unfollow</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> UnfollowUserAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches current user's playlists
    /// https://developer.dailymotion.com/api#user-playlists
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> SearchPlaylistsAsync(string keyword, int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches current user's videos
    /// https://developer.dailymotion.com/api#user-videos
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> SearchVideosAsync(string keyword, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's favorite videos
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetFavoritesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's watch history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetHistoryAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user's watch later videos
    /// https://developer.dailymotion.com/api#user-watchlater
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetWatchLaterAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a video using the /me/videos endpoint
    /// https://developer.dailymotion.com/api#user-videos
    /// </summary>
    /// <param name="videoUrl">Video URL to import</param>
    /// <param name="title">Video title</param>
    /// <param name="description">Video description</param>
    /// <param name="channel">Channel ID</param>
    /// <param name="tags">Video tags</param>
    /// <param name="isPrivate">Whether video is private</param>
    /// <param name="published">Whether video is published</param>
    /// <param name="isCreatedForKids">Whether video is created for kids</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created video metadata</returns>
    Task<VideoMetadata?> CreateVideoAsync(string videoUrl, string title, string? description = null, string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isCreatedForKids = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the video.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="fields">The fields.</param>
    /// <returns>System.Nullable&lt;VideoMetadata&gt;.</returns>
    VideoMetadata? GetVideo(string videoId, VideoFields[]? fields = null);
}
