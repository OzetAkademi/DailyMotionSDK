using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion history operations
/// https://developer.dailymotion.com/api#history
/// </summary>
public class HistoryClient : IHistory
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<HistoryClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public HistoryClient(IDailymotionHttpClient httpClient, ILogger<HistoryClient> logger)
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
    /// Gets user's watch history
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
            _logger.LogDebug("Getting user history. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/history", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user history: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, _jsonSettings) ?? new VideoListResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user history");
            throw;
        }
    }

    /// <summary>
    /// Adds a video to history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to add to history</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> AddToHistoryAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Adding video {VideoId} to history", videoId);

            var parameters = new Dictionary<string, string>
            {
                ["video_id"] = videoId
            };

            var response = await _httpClient.PostAsync("/me/history", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add video {VideoId} to history: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding video {VideoId} to history", videoId);
            throw;
        }
    }

    /// <summary>
    /// Removes a video from history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to remove from history</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> RemoveFromHistoryAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Removing video {VideoId} from history", videoId);

            var response = await _httpClient.DeleteAsync($"/me/history/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to remove video {VideoId} from history: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing video {VideoId} from history", videoId);
            throw;
        }
    }

    /// <summary>
    /// Checks if a video is in history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in history</returns>
    public async Task<bool> IsInHistoryAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Checking if video {VideoId} is in history", videoId);

            var response = await _httpClient.GetAsync($"/me/history/{videoId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if video {VideoId} is in history", videoId);
            throw;
        }
    }

    /// <summary>
    /// Clears all history
    /// https://developer.dailymotion.com/api#user-history
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> ClearHistoryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Clearing user history");

            var response = await _httpClient.DeleteAsync("/me/history", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to clear user history: {Error}", response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing user history");
            throw;
        }
    }
}