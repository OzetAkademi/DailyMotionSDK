using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion playlist operations
/// https://developer.dailymotion.com/api#playlist
/// </summary>
public interface IPlaylist
{
    /// <summary>
    /// Gets playlist metadata
    /// https://developer.dailymotion.com/api#playlist-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    Task<PlaylistMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets playlist by ID
    /// https://developer.dailymotion.com/api#playlist-fields
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    Task<PlaylistMetadata?> GetPlaylistAsync(string playlistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates playlist metadata
    /// https://developer.dailymotion.com/api#playlist-edit
    /// </summary>
    /// <param name="name">Playlist name</param>
    /// <param name="description">Playlist description</param>
    /// <param name="isPrivate">Whether playlist is private</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated playlist metadata</returns>
    Task<PlaylistMetadata?> UpdateMetadataAsync(string? name = null, string? description = null, bool? isPrivate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the playlist
    /// https://developer.dailymotion.com/api#playlist-delete
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DeleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets videos in the playlist
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetVideosAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a video to the playlist
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="videoId">Video ID to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddVideoAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple videos to the playlist
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="videoIds">Array of video IDs to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AddVideosAsync(string[] videoIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video from the playlist
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="videoId">Video ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveVideoAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a video exists in the playlist
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video exists in playlist</returns>
    Task<bool> VideoExistsAsync(string videoId, CancellationToken cancellationToken = default);
}
