using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion general operations
/// https://developer.dailymotion.com/api#general
/// </summary>
public class GeneralClient : IGeneral
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<GeneralClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the GeneralClient
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public GeneralClient(IDailymotionHttpClient httpClient, ILogger<GeneralClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Searches for videos
    /// https://developer.dailymotion.com/api#video-search
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="ArgumentException">Keyword cannot be null or empty - keyword</exception>
    public async Task<VideoListResponse> SearchVideosAsync(string keyword, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            _logger.LogDebug("Searching videos. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync("/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching videos");
            throw;
        }
    }

    /// <summary>
    /// Searches for users
    /// https://developer.dailymotion.com/api#user-search
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    /// <exception cref="ArgumentException">Keyword cannot be null or empty - keyword</exception>
    public async Task<UserListResponse> SearchUsersAsync(string keyword, int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            _logger.LogDebug("Searching users. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync("/users", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search users: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users");
            throw;
        }
    }

    /// <summary>
    /// Searches for playlists
    /// https://developer.dailymotion.com/api#playlist-search
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    /// <exception cref="ArgumentException">Keyword cannot be null or empty - keyword</exception>
    public async Task<PlaylistListResponse> SearchPlaylistsAsync(string keyword, int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            _logger.LogDebug("Searching playlists. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching playlists");
            throw;
        }
    }

    /// <summary>
    /// Gets trending videos
    /// https://developer.dailymotion.com/api#video-trending
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetTrendingVideosAsync(int limit = 100, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting trending videos. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString()
            };

            var response = await _httpClient.GetPublicAsync("/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get trending videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trending videos");
            throw;
        }
    }

    /// <summary>
    /// Gets featured videos
    /// https://developer.dailymotion.com/api#video-featured
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetFeaturedVideosAsync(int limit = 100, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting featured videos. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString()
            };

            var response = await _httpClient.GetPublicAsync("/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get featured videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting featured videos");
            throw;
        }
    }
}
