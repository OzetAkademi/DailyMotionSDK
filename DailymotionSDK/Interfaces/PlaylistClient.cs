using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion playlist operations
/// https://developer.dailymotion.com/api#playlist
/// </summary>
public class PlaylistClient : IPlaylist
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<PlaylistClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;
    /// <summary>
    /// The playlist identifier
    /// </summary>
    private readonly string _playlistId;

    /// <summary>
    /// Initializes a new instance of the PlaylistClient
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <exception cref="ArgumentNullException">playlistId</exception>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public PlaylistClient(string playlistId, IDailymotionHttpClient httpClient, ILogger<PlaylistClient> logger)
    {
        _playlistId = playlistId ?? throw new ArgumentNullException(nameof(playlistId));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Gets playlist metadata
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    public async Task<PlaylistMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting playlist metadata for {PlaylistId}", _playlistId);

            var response = await _httpClient.GetAsync($"/playlist/{_playlistId}", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get playlist metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<PlaylistMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist metadata for {PlaylistId}", _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Gets playlist by ID
    /// https://developer.dailymotion.com/api#playlist-fields
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    /// <exception cref="ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public async Task<PlaylistMetadata?> GetPlaylistAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playlistId))
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

            _logger.LogDebug("Getting playlist metadata for {PlaylistId}", playlistId);

            var response = await _httpClient.GetAsync($"/playlist/{playlistId}", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get playlist metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<PlaylistMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist metadata for {PlaylistId}", playlistId);
            throw;
        }
    }

    /// <summary>
    /// Updates playlist metadata
    /// </summary>
    /// <param name="name">Playlist name</param>
    /// <param name="description">Playlist description</param>
    /// <param name="isPrivate">Whether playlist is private</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated playlist metadata</returns>
    public async Task<PlaylistMetadata?> UpdateMetadataAsync(string? name = null, string? description = null, bool? isPrivate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating playlist metadata for {PlaylistId}. Name: {Name}, Description: {Description}, IsPrivate: {IsPrivate}", _playlistId, name, description, isPrivate);

            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(name))
                parameters["name"] = name;

            if (!string.IsNullOrWhiteSpace(description))
                parameters["description"] = description;

            if (isPrivate.HasValue)
                parameters["private"] = isPrivate.Value.ToString().ToLowerInvariant();

            // Use POST instead of PUT for playlist updates
            var response = await _httpClient.PostAsync($"/playlist/{_playlistId}", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to update playlist metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<PlaylistMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating playlist metadata for {PlaylistId}", _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Deletes the playlist
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> DeleteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting playlist {PlaylistId}", _playlistId);

            var response = await _httpClient.DeleteAsync($"/playlist/{_playlistId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to delete playlist: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting playlist {PlaylistId}", _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Gets videos in the playlist
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetVideosAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting playlist videos for {PlaylistId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _playlistId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/playlist/{_playlistId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get playlist videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist videos for {PlaylistId}", _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Adds a video to the playlist
    /// </summary>
    /// <param name="videoId">Video ID to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> AddVideoAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Adding video {VideoId} to playlist {PlaylistId}", videoId, _playlistId);

            // Use the correct endpoint: POST /playlist/{id}/videos/{video}
            var response = await _httpClient.PostAsync($"/playlist/{_playlistId}/videos/{videoId}", [], null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add video {VideoId} to playlist: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding video {VideoId} to playlist {PlaylistId}", videoId, _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Adds multiple videos to the playlist
    /// </summary>
    /// <param name="videoIds">Array of video IDs to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="ArgumentException">Video IDs cannot be null or empty - videoIds</exception>
    public async Task<bool> AddVideosAsync(string[] videoIds, CancellationToken cancellationToken = default)
    {
        try
        {
            if (videoIds == null || videoIds.Length == 0)
                throw new ArgumentException("Video IDs cannot be null or empty", nameof(videoIds));

            _logger.LogDebug("Adding {Count} videos to playlist {PlaylistId}", videoIds.Length, _playlistId);

            var parameters = new Dictionary<string, string>
            {
                ["video_ids"] = string.Join(",", videoIds)
            };

            var response = await _httpClient.PostAsync($"/playlist/{_playlistId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add videos to playlist: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding videos to playlist {PlaylistId}", _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Removes a video from the playlist
    /// </summary>
    /// <param name="videoId">Video ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> RemoveVideoAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Removing video {VideoId} from playlist {PlaylistId}", videoId, _playlistId);

            var response = await _httpClient.DeleteAsync($"/playlist/{_playlistId}/videos/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to remove video {VideoId} from playlist: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing video {VideoId} from playlist {PlaylistId}", videoId, _playlistId);
            throw;
        }
    }

    /// <summary>
    /// Checks if a video exists in the playlist
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video exists in playlist</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> VideoExistsAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Checking if video {VideoId} exists in playlist {PlaylistId}", videoId, _playlistId);

            var response = await _httpClient.GetAsync($"/playlist/{_playlistId}/videos/{videoId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if video {VideoId} exists in playlist {PlaylistId}", videoId, _playlistId);
            throw;
        }
    }
}
