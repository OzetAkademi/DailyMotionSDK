using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion favorites operations
/// https://developer.dailymotion.com/api#favorites
/// </summary>
public class FavoritesClient : IFavorites
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<FavoritesClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerSettings _jsonSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoritesClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    /// <exception cref="System.ArgumentNullException">logger</exception>
    public FavoritesClient(IDailymotionHttpClient httpClient, ILogger<FavoritesClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }

    /// <summary>
    /// Gets user's favorite videos
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

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, _jsonSettings) ?? new VideoListResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user favorites");
            throw;
        }
    }

    /// <summary>
    /// Adds a video to favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to add to favorites</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="System.ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> AddToFavoritesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Adding video {VideoId} to favorites", videoId);

            var parameters = new Dictionary<string, string>
            {
                ["video_id"] = videoId
            };

            var response = await _httpClient.PostAsync("/me/favorites", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add video {VideoId} to favorites: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding video {VideoId} to favorites", videoId);
            throw;
        }
    }

    /// <summary>
    /// Removes a video from favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to remove from favorites</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="System.ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> RemoveFromFavoritesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Removing video {VideoId} from favorites", videoId);

            var response = await _httpClient.DeleteAsync($"/me/favorites/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to remove video {VideoId} from favorites: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing video {VideoId} from favorites", videoId);
            throw;
        }
    }

    /// <summary>
    /// Checks if a video is in favorites
    /// https://developer.dailymotion.com/api#user-favorites
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in favorites</returns>
    /// <exception cref="System.ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<bool> IsInFavoritesAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Checking if video {VideoId} is in favorites", videoId);

            var response = await _httpClient.GetAsync($"/me/favorites/{videoId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if video {VideoId} is in favorites", videoId);
            throw;
        }
    }
}

