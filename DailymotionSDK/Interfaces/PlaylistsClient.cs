using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion playlist management operations
/// https://developers.dailymotion.com/api/platform-api/reference/#playlist
/// </summary>
public class PlaylistsClient : IPlaylists
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<PlaylistsClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;
    /// <summary>
    /// The logger factory
    /// </summary>
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the PlaylistsClient
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <param name="loggerFactory">Logger factory for creating playlist client loggers</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    /// <exception cref="ArgumentNullException">loggerFactory</exception>
    public PlaylistsClient(IDailymotionHttpClient httpClient, ILogger<PlaylistsClient> logger, ILoggerFactory loggerFactory)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Creates a new playlist
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="name">Playlist name</param>
    /// <param name="description">Playlist description</param>
    /// <param name="isPrivate">Whether playlist is private</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created playlist metadata</returns>
    /// <exception cref="ArgumentException">Playlist name cannot be null or empty - name</exception>
    public async Task<PlaylistMetadata?> CreatePlaylistAsync(string name, string? description = null, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Playlist name cannot be null or empty", nameof(name));

            _logger.LogDebug("Creating playlist: {Name}", name);

            var parameters = new Dictionary<string, string>
            {
                ["name"] = name
            };

            if (!string.IsNullOrWhiteSpace(description))
                parameters["description"] = description;

            parameters["private"] = isPrivate.ToString().ToLowerInvariant();

            _logger.LogDebug("Playlist creation parameters: {Parameters}", string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")));

            var response = await _httpClient.PostAsync("/me/playlists", parameters, null, cancellationToken);

            _logger.LogDebug("Playlist creation response status: {StatusCode}, Content length: {ContentLength}",
                response.StatusCode, response.Content?.Length ?? 0);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<PlaylistMetadata>(response.Content, _jsonOptions);
                _logger.LogDebug("Playlist created successfully: {PlaylistId}", result?.Id);
                return result;
            }

            _logger.LogError("Failed to create playlist. Status: {StatusCode}, Error: {Error}, Content: {Content}",
                response.StatusCode, response.ErrorMessage, response.Content);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating playlist: {Name}", name);
            throw;
        }
    }

    /// <summary>
    /// Gets a playlist by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
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

            _logger.LogDebug("Getting playlist: {PlaylistId}", playlistId);

            var response = await _httpClient.GetAsync($"playlist/{playlistId}", null, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<PlaylistMetadata>(response.Content, _jsonOptions);
                _logger.LogDebug("Playlist retrieved successfully: {PlaylistId}", playlistId);
                return result;
            }

            _logger.LogError("Failed to get playlist {PlaylistId}: {Error}", playlistId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist: {PlaylistId}", playlistId);
            throw;
        }
    }

    /// <summary>
    /// Gets a playlist client for operations on a specific playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns>Playlist client</returns>
    /// <exception cref="ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public IPlaylist GetPlaylist(string playlistId)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
            throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

        // Create a new PlaylistClient instance with the required parameters
        var logger = _loggerFactory.CreateLogger<PlaylistClient>();

        return new PlaylistClient(playlistId, _httpClient, logger);
    }

    /// <summary>
    /// Searches playlists with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="filters">Playlist filters</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    /// <exception cref="ArgumentNullException">filters</exception>
    public async Task<PlaylistListResponse> SearchPlaylistsAsync(PlaylistFilters filters, int limit = 100, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _logger.LogDebug("Searching playlists with filters. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = filters.ToDictionary();
            parameters["limit"] = limit.ToString();
            parameters["page"] = page.ToString();

            var response = await _httpClient.GetAsync("/playlists", parameters, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<PlaylistListResponse>(response.Content, _jsonOptions);
                _logger.LogDebug("Playlist search completed successfully. Found {Count} playlists", result?.List.Count ?? 0);
                return result ?? new();
            }

            _logger.LogError("Failed to search playlists. Status: {StatusCode}, Error: {Error}", response.StatusCode, response.ErrorMessage);
            return new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching playlists");
            throw;
        }
    }

    /// <summary>
    /// Gets playlists with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="filters">Playlist filters</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    /// <exception cref="ArgumentNullException">filters</exception>
    public async Task<PlaylistListResponse> GetPlaylistsAsync(PlaylistFilters filters, int limit = 100, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(filters);

            _logger.LogDebug("Getting playlists with filters. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = filters.ToDictionary();
            parameters["limit"] = limit.ToString();
            parameters["page"] = page.ToString();

            var response = await _httpClient.GetAsync("/playlists", parameters, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<PlaylistListResponse>(response.Content, _jsonOptions);
                _logger.LogDebug("Playlist retrieval completed successfully. Found {Count} playlists", result?.List.Count ?? 0);
                return result ?? new();
            }

            _logger.LogError("Failed to get playlists. Status: {StatusCode}, Error: {Error}", response.StatusCode, response.ErrorMessage);
            return new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlists");
            throw;
        }
    }
}
