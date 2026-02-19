using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion playlist management operations
/// https://developers.dailymotion.com/api/platform-api/reference/#playlist
/// </summary>
public interface IPlaylists
{
    /// <summary>
    /// Creates a new playlist
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="name">Playlist name</param>
    /// <param name="description">Playlist description</param>
    /// <param name="isPrivate">Whether playlist is private</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created playlist metadata</returns>
    Task<PlaylistMetadata?> CreatePlaylistAsync(string name, string? description = null, bool isPrivate = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a playlist by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    Task<PlaylistMetadata?> GetPlaylistAsync(string playlistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a playlist client for operations on a specific playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns>Playlist client</returns>
    IPlaylist GetPlaylist(string playlistId);

    /// <summary>
    /// Searches playlists with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="filters">Playlist filters</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> SearchPlaylistsAsync(PlaylistFilters filters, int limit = 100, int page = 1, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets playlists with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="filters">Playlist filters</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> GetPlaylistsAsync(PlaylistFilters filters, int limit = 100, int page = 1, CancellationToken cancellationToken = default);
}
