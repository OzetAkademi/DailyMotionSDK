using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion "mine" operations (current user)
/// https://developer.dailymotion.com/api#user
/// </summary>
public class MineClient : IMine
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<MineClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="MineClient" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    public MineClient(IDailymotionHttpClient httpClient, ILogger<MineClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        //_httpClient.SetApiKeyType(ApiKeyType.Private);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Gets current user information
    /// https://developer.dailymotion.com/api#user-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    public async Task<UserMetadata?> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting current user info");

            var response = await _httpClient.GetAsync("/me", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<UserMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's videos
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
            _logger.LogDebug("Getting user videos. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user videos");
            throw;
        }
    }

    /// <summary>
    /// Gets the video.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="fields">The fields.</param>
    /// <returns>System.Nullable&lt;VideoMetadata&gt;.</returns>
    public VideoMetadata? GetVideo(string videoId, VideoFields[]? fields = null)
    {
        var vids = GetVideosAsync(new VideoFilters()
        {
            Ids = [videoId]
        }, fields).ConfigureAwait(false).GetAwaiter().GetResult();
        return vids?.List?.FirstOrDefault();
    }

    /// <summary>
    /// Get videos as an asynchronous operation.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoListResponse?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoListResponse?> GetVideosAsync(VideoFilters? filters = null, VideoFields[]? fields = null, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting videos with filters and fields");

            var parameters = new Dictionary<string, string>();

            // Filter out fields that require special permissions and cannot be used in list endpoints
            VideoFields[]? filteredFields = null;
            VideoFields[]? restrictedFields = null;

            if (fields is { Length: > 0 })
            {
                restrictedFields = fields.GetRestrictedFields();
                filteredFields = fields.FilterRestrictedFields();

                if (restrictedFields.Length > 0)
                {
                    _logger.LogWarning(
                        "The following fields require special permissions (can-read-video-streams, can-read-my-video-streams) " +
                        "and cannot be requested from list endpoints: {RestrictedFields}. " +
                        "These fields have been filtered out. To retrieve them, use GetVideoAsync() for individual videos. " +
                        "Requested fields: {RequestedFields}",
                        string.Join(", ", restrictedFields.Select(f => f.GetApiFieldName())),
                        string.Join(", ", fields.Select(f => f.GetApiFieldName()))
                    );
                }

                // Use filtered fields for the API request
                if (filteredFields.Length > 0)
                {
                    parameters["fields"] = string.Join(",", filteredFields.ToApiFieldNames());
                }
            }

            // Add filter parameters
            if (filters != null)
            {
                var filterParams = VideosClient.ConvertVideoFiltersToParameters(filters);
                foreach (var filter in filterParams)
                {
                    parameters[filter.Key] = filter.Value;
                }
            }

            parameters["sort"] = sort.ToApiSortString();

            var response = await _httpClient.GetAsync("/me/videos", parameters, null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get videos: {Error}", response.ErrorMessage);
                return null;
            }

            // Use custom converter for VideoMetadata with original field selection (including restricted fields)
            // This ensures the converter knows about all requested fields, even if they weren't in the API response
            var settings = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
            };

            settings.Converters.Add(new VideoMetadataJsonConverter(fields));

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting videos");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's playlists
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
            _logger.LogDebug("Getting user playlists. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user playlists");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's followers
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
            _logger.LogDebug("Getting user followers. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/followers", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user followers: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user followers");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's following
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
            _logger.LogDebug("Getting user following. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/following", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user following: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user following");
            throw;
        }
    }

    /// <summary>
    /// Follows a user
    /// https://developer.dailymotion.com/api#user-follow
    /// </summary>
    /// <param name="userId">User ID to follow</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> FollowUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            _logger.LogDebug("Following user {UserId}", userId);

            var parameters = new Dictionary<string, string>
            {
                ["user_id"] = userId
            };

            var response = await _httpClient.PostAsync("/me/following", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to follow user {UserId}: {Error}", userId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error following user {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Unfollows a user
    /// https://developer.dailymotion.com/api#user-follow
    /// </summary>
    /// <param name="userId">User ID to unfollow</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> UnfollowUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            _logger.LogDebug("Unfollowing user {UserId}", userId);

            var response = await _httpClient.DeleteAsync($"/me/following/{userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to unfollow user {UserId}: {Error}", userId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unfollowing user {UserId}", userId);
            throw;
        }
    }

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
    public async Task<PlaylistListResponse> SearchPlaylistsAsync(string keyword, int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            _logger.LogDebug("Searching user playlists. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search user playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user playlists");
            throw;
        }
    }

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
    public async Task<VideoListResponse> SearchVideosAsync(string keyword, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            _logger.LogDebug("Searching user videos. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search user videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user videos");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's favorite videos
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetFavoritesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user favorites. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/favorites", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user favorites: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user favorites");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's watch history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetHistoryAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user history. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString()
            };

            var response = await _httpClient.GetAsync("/me/history", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user history: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user history");
            throw;
        }
    }

    /// <summary>
    /// Gets current user's watch later videos
    /// https://developer.dailymotion.com/api#user-watchlater
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetWatchLaterAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user watch later. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString()
            };

            var response = await _httpClient.GetAsync("/me/watchlater", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user watch later: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user watch later");
            throw;
        }
    }

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
    public async Task<VideoMetadata?> CreateVideoAsync(string videoUrl, string title, string? description = null, string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isCreatedForKids = false, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                throw new ArgumentException("Video URL cannot be null or empty", nameof(videoUrl));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty", nameof(title));

            _logger.LogDebug("Creating video via /me/videos. Title: {Title}, URL: {Url}", title, videoUrl);

            var parameters = new Dictionary<string, string>
            {
                ["url"] = videoUrl,
                ["title"] = title,
                ["published"] = published.ToString().ToLowerInvariant(),
                ["is_created_for_kids"] = isCreatedForKids.ToString().ToLowerInvariant()
            };

            if (!string.IsNullOrWhiteSpace(description))
                parameters["description"] = description;

            if (!string.IsNullOrWhiteSpace(channel))
                parameters["channel"] = channel;

            if (tags != null && tags.Length > 0)
                parameters["tags"] = string.Join(",", tags);

            parameters["private"] = isPrivate.ToString().ToLowerInvariant();

            var response = await _httpClient.PostAsync("/me/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create video via /me/videos: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<VideoMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video via /me/videos");
            throw;
        }
    }
}
