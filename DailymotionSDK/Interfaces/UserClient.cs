using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion user operations
/// https://developer.dailymotion.com/api#user
/// </summary>
public class UserClient : IUser
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<UserClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;
    /// <summary>
    /// The user identifier
    /// </summary>
    private readonly string _userId;

    /// <summary>
    /// Initializes a new instance of the UserClient
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <exception cref="ArgumentNullException">userId</exception>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public UserClient(string userId, IDailymotionHttpClient httpClient, ILogger<UserClient> logger)
    {
        _userId = userId ?? throw new ArgumentNullException(nameof(userId));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Gets user metadata
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    public async Task<UserMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user metadata for {UserId}", _userId);

            var response = await _httpClient.GetPublicAsync($"/user/{_userId}", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<UserMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user metadata for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets user metadata by providing user page URL
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="userPageUrl">User page URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    /// <exception cref="ArgumentException">User page URL cannot be null or empty - userPageUrl</exception>
    /// <exception cref="ArgumentException">Could not extract user ID from URL - userPageUrl</exception>
    public async Task<UserMetadata?> GetUserAsync(string userPageUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userPageUrl))
                throw new ArgumentException("User page URL cannot be null or empty", nameof(userPageUrl));

            var userId = ExtractUserIdFromUrl(userPageUrl);
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Could not extract user ID from URL", nameof(userPageUrl));

            _logger.LogDebug("Getting user metadata for URL {UserPageUrl}, extracted ID: {UserId}", userPageUrl, userId);

            var response = await _httpClient.GetPublicAsync($"/user/{userId}", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<UserMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user metadata for URL {UserPageUrl}", userPageUrl);
            throw;
        }
    }

    /// <summary>
    /// Gets user videos
    /// https://developer.dailymotion.com/api#user-videos
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
            _logger.LogDebug("Getting user videos for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync($"/user/{_userId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user videos for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets user playlists
    /// https://developer.dailymotion.com/api#user-playlists
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    public async Task<PlaylistListResponse> GetPlaylistsAsync(int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user playlists for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user playlists for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets user followers
    /// https://developer.dailymotion.com/api#user-followers
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    public async Task<UserListResponse> GetFollowersAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user followers for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/followers", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user followers: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user followers for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets users that this user is following
    /// https://developer.dailymotion.com/api#user-following
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    public async Task<UserListResponse> GetFollowingAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user following for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/following", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user following: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user following for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Checks if the current user is following this user
    /// https://developer.dailymotion.com/api#user-following
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if following</returns>
    public async Task<bool> IsFollowingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if following user {UserId}", _userId);

            var response = await _httpClient.GetAsync($"/me/following/{_userId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if following user {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Follows this user
    /// https://developer.dailymotion.com/api#user-following
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> FollowAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Following user {UserId}", _userId);

            var parameters = new Dictionary<string, string>
            {
                ["user_id"] = _userId
            };

            var response = await _httpClient.PostAsync("/me/following", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to follow user: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error following user {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Unfollows this user
    /// https://developer.dailymotion.com/api#user-following
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> UnfollowAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Unfollowing user {UserId}", _userId);

            var response = await _httpClient.DeleteAsync($"/me/following/{_userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to unfollow user: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unfollowing user {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets playlist videos
    /// https://developer.dailymotion.com/api#playlist-videos
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public async Task<VideoListResponse> GetPlaylistVideosAsync(string playlistId, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playlistId))
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

            _logger.LogDebug("Getting playlist videos for {PlaylistId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", playlistId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/playlist/{playlistId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get playlist videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist videos for {PlaylistId}", playlistId);
            throw;
        }
    }

    /// <summary>
    /// Searches user playlists
    /// https://developer.dailymotion.com/api#user-playlists
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

            _logger.LogDebug("Searching user playlists for {UserId}. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search user playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user playlists for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Searches user videos
    /// https://developer.dailymotion.com/api#user-videos
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

            _logger.LogDebug("Searching user videos for {UserId}. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync($"/user/{_userId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search user videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user videos for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets user likes
    /// https://developer.dailymotion.com/api#user-likes
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetLikesAsync(int limit = 10, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user likes for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/likes", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user likes: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user likes for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Gets user featured videos
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetFeaturesAsync(int limit = 10, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user features for {UserId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", _userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync($"/user/{_userId}/features", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user features: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user features for {UserId}", _userId);
            throw;
        }
    }

    /// <summary>
    /// Extracts user ID from a DailyMotion user URL
    /// </summary>
    /// <param name="url">DailyMotion user URL</param>
    /// <returns>User ID or null if not found</returns>
    public static string? ExtractUserIdFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        // Handle various URL formats
        var patterns = new[]
        {
            @"dailymotion\.com/user/([^/?]+)",
            @"dailymotion\.com/([^/?]+)",
            @"dailymotion\.com/channel/([^/?]+)"
        };

        foreach (var pattern in patterns)
        {
            var match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value;
        }

        return null;
    }
}
