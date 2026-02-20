using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion channel operations
/// https://developer.dailymotion.com/api#channel
/// </summary>
public class ChannelsClient : IChannels
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ChannelsClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the ChannelsClient
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public ChannelsClient(IDailymotionHttpClient httpClient, ILogger<ChannelsClient> logger)
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
    /// Gets channel metadata
    /// https://developer.dailymotion.com/api#channel-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel metadata</returns>
    public async Task<ChannelMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting channel metadata");

            var response = await _httpClient.GetAsync("/channel", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel metadata: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<ChannelMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel metadata");
            throw;
        }
    }

    /// <summary>
    /// Gets channel subscribers
    /// https://developer.dailymotion.com/api#channel-subscribers
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    public async Task<UserListResponse> GetSubscribersAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting channel subscribers. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/channel/subscribers", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel subscribers: {Error}", response.ErrorMessage);
                return new UserListResponse();
            }

            return JsonSerializer.Deserialize<UserListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel subscribers");
            throw;
        }
    }

    /// <summary>
    /// Gets channel videos
    /// https://developer.dailymotion.com/api#channel-videos
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
            _logger.LogDebug("Getting channel videos. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync("/channel/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel videos");
            throw;
        }
    }

    /// <summary>
    /// Gets channel videos by channel ID
    /// https://developer.dailymotion.com/api#channel-videos
    /// </summary>
    /// <param name="channelId">Channel ID</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="ArgumentException">Channel ID cannot be null or empty - channelId</exception>
    public async Task<VideoListResponse> GetChannelVideosAsync(string channelId, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(channelId))
                throw new ArgumentException("Channel ID cannot be null or empty", nameof(channelId));

            _logger.LogDebug("Getting channel videos for {ChannelId}. Limit: {Limit}, Page: {Page}, Sort: {Sort}", channelId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync($"/channel/{channelId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel videos for {ChannelId}: {Error}", channelId, response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel videos for {ChannelId}", channelId);
            throw;
        }
    }

    /// <summary>
    /// Gets channel playlists
    /// https://developer.dailymotion.com/api#channel-playlists
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
            _logger.LogDebug("Getting channel playlists. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/channel/playlists", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel playlists: {Error}", response.ErrorMessage);
                return new PlaylistListResponse();
            }

            return JsonSerializer.Deserialize<PlaylistListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel playlists");
            throw;
        }
    }

    /// <summary>
    /// Searches for videos in the channel
    /// https://developer.dailymotion.com/api#channel-videos
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

            _logger.LogDebug("Searching channel videos. Keyword: {Keyword}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", keyword, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["search"] = keyword,
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetPublicAsync("/channel/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to search channel videos: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching channel videos");
            throw;
        }
    }

    /// <summary>
    /// Gets channel metadata by channel ID
    /// https://developer.dailymotion.com/api#channel-fields
    /// </summary>
    /// <param name="channelId">Channel ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel metadata</returns>
    /// <exception cref="ArgumentException">Channel ID cannot be null or empty - channelId</exception>
    public async Task<ChannelMetadata?> GetChannelMetadataAsync(string channelId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(channelId))
                throw new ArgumentException("Channel ID cannot be null or empty", nameof(channelId));

            _logger.LogDebug("Getting channel metadata for {ChannelId}", channelId);

            var response = await _httpClient.GetPublicAsync($"/channel/{channelId}", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel metadata for {ChannelId}: {Error}", channelId, response.ErrorMessage);
                return null;
            }

            return JsonSerializer.Deserialize<ChannelMetadata>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel metadata for {ChannelId}", channelId);
            throw;
        }
    }

    /// <summary>
    /// Gets a list of all available channels
    /// https://developer.dailymotion.com/api#channel
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel list response</returns>
    public async Task<ChannelListResponse> GetChannelsAsync(int limit = 100, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting channels list. Limit: {Limit}, Page: {Page}", limit, page);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString()
            };

            var response = await _httpClient.GetPublicAsync("/channels", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channels list: {Error}", response.ErrorMessage);
                return new ChannelListResponse();
            }

            return JsonSerializer.Deserialize<ChannelListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channels list");
            throw;
        }
    }
}
