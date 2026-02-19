using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for language operations
/// https://developers.dailymotion.com/api/platform-api/reference/#languages
/// </summary>
public class LanguagesClient : ILanguages
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<LanguagesClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public LanguagesClient(IDailymotionHttpClient httpClient, ILogger<LanguagesClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }

    public async Task<LanguagesResponse> GetLanguagesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving available languages");

            var response = await _httpClient.GetAsync("/languages", new Dictionary<string, string>(), null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<LanguagesResponse>(response.Content, _jsonSettings);
                _logger.LogDebug("Languages retrieved successfully");
                return result ?? new LanguagesResponse();
            }

            _logger.LogWarning("Languages request failed with status: {StatusCode}", response.StatusCode);
            throw new Exception($"Languages request failed: {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving languages");
            throw;
        }
    }
}

/// <summary>
/// Client for locale operations
/// https://developers.dailymotion.com/api/platform-api/reference/#locale
/// </summary>
public class LocaleClient : ILocale
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<LocaleClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public LocaleClient(IDailymotionHttpClient httpClient, ILogger<LocaleClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }

    public async Task<LocaleDetectionResponse> DetectLocaleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Detecting locale");

            var response = await _httpClient.GetAsync("/locale", new Dictionary<string, string>(), null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<LocaleDetectionResponse>(response.Content, _jsonSettings);
                _logger.LogDebug("Locale detected successfully");
                return result ?? new LocaleDetectionResponse();
            }

            _logger.LogWarning("Locale detection failed with status: {StatusCode}", response.StatusCode);
            throw new Exception($"Locale detection failed: {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting locale");
            throw;
        }
    }
}

/// <summary>
/// Client for logout operations
/// https://developers.dailymotion.com/api/platform-api/reference/#logout
/// </summary>
public class LogoutClient : ILogout
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<LogoutClient> _logger;

    public LogoutClient(IDailymotionHttpClient httpClient, ILogger<LogoutClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> LogoutAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Logging out user");

            var response = await _httpClient.GetPublicAsync("/logout", new Dictionary<string, string>(), null, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Logout completed successfully");
                return true;
            }

            _logger.LogWarning("Logout failed with status: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            throw;
        }
    }
}

/// <summary>
/// Client for player operations
/// https://developers.dailymotion.com/api/platform-api/reference/#player
/// </summary>
public class PlayerClient : IPlayer
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<PlayerClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public PlayerClient(IDailymotionHttpClient httpClient, ILogger<PlayerClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }

    public async Task<PlayerMetadata?> GetPlayerAsync(string playerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(playerId))
            throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));

        try
        {
            _logger.LogDebug("Getting player: {PlayerId}", playerId);

            var response = await _httpClient.GetPublicAsync($"/player/{playerId}", new Dictionary<string, string>(), null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<PlayerMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Player retrieved successfully");
                return result;
            }

            _logger.LogWarning("Player request failed with status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player: {PlayerId}", playerId);
            throw;
        }
    }

    public async Task<PlayerMetadata?> CreatePlayerAsync(Dictionary<string, string> playerData, CancellationToken cancellationToken = default)
    {
        if (playerData == null)
            throw new ArgumentNullException(nameof(playerData));

        try
        {
            _logger.LogDebug("Creating player");

            var response = await _httpClient.PostAsync("/player", playerData, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<PlayerMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Player created successfully");
                return result;
            }

            _logger.LogWarning("Player creation failed with status: {StatusCode}", response.StatusCode);
            throw new Exception($"Player creation failed: {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating player");
            throw;
        }
    }

    public async Task<PlayerMetadata?> UpdatePlayerAsync(string playerId, Dictionary<string, string> playerData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(playerId))
            throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));

        if (playerData == null)
            throw new ArgumentNullException(nameof(playerData));

        try
        {
            _logger.LogDebug("Updating player: {PlayerId}", playerId);

            var response = await _httpClient.PostAsync($"/player/{playerId}", playerData, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<PlayerMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Player updated successfully");
                return result;
            }

            _logger.LogWarning("Player update failed with status: {StatusCode}", response.StatusCode);
            throw new Exception($"Player update failed: {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating player: {PlayerId}", playerId);
            throw;
        }
    }

    public async Task<bool> DeletePlayerAsync(string playerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(playerId))
            throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));

        try
        {
            _logger.LogDebug("Deleting player: {PlayerId}", playerId);

            var response = await _httpClient.DeleteAsync($"/player/{playerId}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Player deleted successfully");
                return true;
            }

            _logger.LogWarning("Player deletion failed with status: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting player: {PlayerId}", playerId);
            throw;
        }
    }
}

/// <summary>
/// Client for subtitle operations
/// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
/// </summary>
public class SubtitlesClient : ISubtitles
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<SubtitlesClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public SubtitlesClient(IDailymotionHttpClient httpClient, ILogger<SubtitlesClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }

    /// <summary>
    /// Creates a subtitle for a video
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="subtitleData">Subtitle data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created subtitle metadata</returns>
    public async Task<SubtitleMetadata?> CreateSubtitleAsync(Dictionary<string, string> subtitleData, CancellationToken cancellationToken = default)
    {
        try
        {
            if (subtitleData == null)
                throw new ArgumentNullException(nameof(subtitleData));

            if (!subtitleData.ContainsKey("video_id"))
                throw new ArgumentException("video_id is required for subtitle creation");

            var videoId = subtitleData["video_id"];
            subtitleData.Remove("video_id"); // Remove video_id from parameters as it's used in URL

            _logger.LogDebug("Creating subtitle for video {VideoId}", videoId);

            // Use the correct endpoint: POST /video/{id}/subtitles
            var response = await _httpClient.PostAsync($"video/{videoId}/subtitles", subtitleData, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<SubtitleMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Subtitle created successfully for video {VideoId}", videoId);
                return result;
            }

            _logger.LogError("Failed to create subtitle for video {VideoId}: {Error}", videoId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subtitle");
            throw;
        }
    }

    /// <summary>
    /// Creates a subtitle for a specific video (convenience method)
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="url">Subtitle file URL</param>
    /// <param name="language">Language code (optional)</param>
    /// <param name="format">Format (default: SRT)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created subtitle metadata</returns>
    public async Task<SubtitleMetadata?> CreateSubtitleForVideoAsync(string videoId, string url, string? language = null, string format = "SRT", CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Subtitle URL cannot be null or empty", nameof(url));

            _logger.LogDebug("Creating subtitle for video {VideoId} with URL {Url}", videoId, url);

            var parameters = new Dictionary<string, string>
            {
                ["url"] = url,
                ["format"] = format
            };

            if (!string.IsNullOrWhiteSpace(language))
                parameters["language"] = language;

            // Use the correct endpoint: POST /video/{id}/subtitles
            var response = await _httpClient.PostAsync($"video/{videoId}/subtitles", parameters, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<SubtitleMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Subtitle created successfully for video {VideoId}", videoId);
                return result;
            }

            _logger.LogError("Failed to create subtitle for video {VideoId}: {Error}", videoId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subtitle for video {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Gets a subtitle by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Subtitle metadata</returns>
    public async Task<SubtitleMetadata?> GetSubtitleAsync(string subtitleId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(subtitleId))
                throw new ArgumentException("Subtitle ID cannot be null or empty", nameof(subtitleId));

            _logger.LogDebug("Getting subtitle {SubtitleId}", subtitleId);

            // Use the correct endpoint: GET /subtitle/{id}
            var response = await _httpClient.GetAsync($"subtitle/{subtitleId}", null, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<SubtitleMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Subtitle retrieved successfully: {SubtitleId}", subtitleId);
                return result;
            }

            _logger.LogError("Failed to get subtitle {SubtitleId}: {Error}", subtitleId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subtitle {SubtitleId}", subtitleId);
            throw;
        }
    }

    /// <summary>
    /// Updates a subtitle
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="subtitleData">Subtitle data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated subtitle metadata</returns>
    public async Task<SubtitleMetadata?> UpdateSubtitleAsync(string subtitleId, Dictionary<string, string> subtitleData, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(subtitleId))
                throw new ArgumentException("Subtitle ID cannot be null or empty", nameof(subtitleId));

            if (subtitleData == null)
                throw new ArgumentNullException(nameof(subtitleData));

            _logger.LogDebug("Updating subtitle {SubtitleId}", subtitleId);

            // Use the correct endpoint: POST /subtitle/{id} (not PUT)
            var response = await _httpClient.PostAsync($"subtitle/{subtitleId}", subtitleData, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<SubtitleMetadata>(response.Content, _jsonSettings);
                _logger.LogDebug("Subtitle updated successfully: {SubtitleId}", subtitleId);
                return result;
            }

            _logger.LogError("Failed to update subtitle {SubtitleId}: {Error}", subtitleId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subtitle {SubtitleId}", subtitleId);
            throw;
        }
    }

    /// <summary>
    /// Deletes a subtitle
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    public async Task<bool> DeleteSubtitleAsync(string subtitleId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(subtitleId))
                throw new ArgumentException("Subtitle ID cannot be null or empty", nameof(subtitleId));

            _logger.LogDebug("Deleting subtitle {SubtitleId}", subtitleId);

            // Use the correct endpoint: DELETE /subtitle/{id}
            var response = await _httpClient.DeleteAsync($"subtitle/{subtitleId}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Subtitle deleted successfully: {SubtitleId}", subtitleId);
                return true;
            }

            _logger.LogError("Failed to delete subtitle {SubtitleId}: {Error}", subtitleId, response.ErrorMessage);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subtitle {SubtitleId}", subtitleId);
            throw;
        }
    }
}
