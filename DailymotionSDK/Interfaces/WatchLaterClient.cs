using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion watch later operations
/// https://developer.dailymotion.com/api#watch-later
/// </summary>
public class WatchLaterClient : IWatchLater
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<WatchLaterClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public WatchLaterClient(IDailymotionHttpClient httpClient, ILogger<WatchLaterClient> logger)
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
    /// Gets user's watch later videos
    /// https://developer.dailymotion.com/api#user-watch-later
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
            _logger.LogDebug("Getting user watch later. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            var response = await _httpClient.GetAsync("/me/watchlater", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user watch later: {Error}", response.ErrorMessage);
                return new VideoListResponse();
            }

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, _jsonSettings) ?? new VideoListResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user watch later");
            throw;
        }
    }

    /// <summary>
    /// Adds a video to watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to add to watch later</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> AddToWatchLaterAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Adding video {VideoId} to watch later", videoId);

            var parameters = new Dictionary<string, string>
            {
                ["video_id"] = videoId
            };

            var response = await _httpClient.PostAsync("/me/watchlater", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to add video {VideoId} to watch later: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding video {VideoId} to watch later", videoId);
            throw;
        }
    }

    /// <summary>
    /// Removes a video from watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to remove from watch later</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> RemoveFromWatchLaterAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Removing video {VideoId} from watch later", videoId);

            var response = await _httpClient.DeleteAsync($"/me/watchlater/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to remove video {VideoId} from watch later: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing video {VideoId} from watch later", videoId);
            throw;
        }
    }

    /// <summary>
    /// Checks if a video is in watch later
    /// https://developer.dailymotion.com/api#user-watch-later
    /// </summary>
    /// <param name="videoId">Video ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if video is in watch later</returns>
    public async Task<bool> IsInWatchLaterAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Checking if video {VideoId} is in watch later", videoId);

            var response = await _httpClient.GetAsync($"/me/watchlater/{videoId}", null, null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if video {VideoId} is in watch later", videoId);
            throw;
        }
    }
}
