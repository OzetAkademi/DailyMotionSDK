using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion features operations
/// https://developer.dailymotion.com/api#features
/// </summary>
public class FeaturesClient : IFeatures
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<FeaturesClient> _logger;
    /// <summary>
    /// The json options
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeaturesClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public FeaturesClient(IDailymotionHttpClient httpClient, ILogger<FeaturesClient> logger)
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
    /// Gets user's featured videos
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    public async Task<VideoListResponse> GetFeaturesAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user features. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/features", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user features: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonSerializer.Deserialize<VideoListResponse>(response.Content!, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user features");
            throw;
        }
    }

    /// <summary>
    /// Adds a video to features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to add to features</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> AddToFeaturesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Adding video {VideoId} to features", videoId);

            var parameters = new Dictionary<string, string>
            {
                ["video_id"] = videoId
            };

            var response = await _httpClient.PostAsync("/me/features", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add video {VideoId} to features: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding video {VideoId} to features", videoId);
            throw;
        }
    }

    /// <summary>
    /// Removes a video from features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to remove from features</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> RemoveFromFeaturesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Removing video {VideoId} from features", videoId);

            var response = await _httpClient.DeleteAsync($"/me/features/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to remove video {VideoId} from features: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing video {VideoId} from features", videoId);
            throw;
        }
    }

    /// <summary>
    /// Checks if a video is in features
    /// https://developer.dailymotion.com/api#user-features
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in features</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> IsInFeaturesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Checking if video {VideoId} is in features", videoId);

            var response = await _httpClient.GetAsync($"/me/features/{videoId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if video {VideoId} is in features", videoId);
            throw;
        }
    }
}