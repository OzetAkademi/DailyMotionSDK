using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for DailyMotion video operations
/// https://developers.dailymotion.com/api/platform-api/reference/#video
/// </summary>
public class VideosClient : IVideos
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<VideosClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerSettings _jsonSettings;

    /// <summary>
    /// Extracts the user ID from the current access token
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User ID or null if not found</returns>
    private async Task<string?> GetUserIdFromMeEndpointAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user ID from /me endpoint");
            var response = await _httpClient.GetAsync("/me", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get user info from /me endpoint: {StatusCode}", response.StatusCode);
                return null;
            }

            if (string.IsNullOrEmpty(response.Content))
            {
                _logger.LogWarning("Empty response from /me endpoint");
                return null;
            }

            var userInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content, _jsonSettings);
            if (userInfo != null && userInfo.TryGetValue("id", out var idValue))
            {
                var userId = idValue.ToString();
                _logger.LogDebug("Retrieved user ID from /me endpoint: {UserId}", userId);
                return userId;
            }

            _logger.LogWarning("No user ID found in /me endpoint response");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user ID from /me endpoint");
            return null;
        }
    }


    /// <summary>
    /// Initializes a new instance of the VideosClient
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    public VideosClient(IDailymotionHttpClient httpClient, ILogger<VideosClient> logger)
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
    /// Get video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="globalApiParameters"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    public async Task<VideoMetadata?> GetVideoAsync(string videoId, VideoFields[]? fields = null, GlobalApiParameters? globalApiParameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Getting video metadata for {VideoId} with fields: {Fields}", videoId, fields?.Length > 0 ? string.Join(",", fields.Select(f => f.GetApiFieldName())) : "all");

            var parameters = new Dictionary<string, string>();
            if (fields is { Length: > 0 })
            {
                parameters["fields"] = string.Join(",", fields.ToApiFieldNames());
            }

            var response = await _httpClient.GetAsync($"/video/{videoId}", parameters, globalApiParameters, cancellationToken);

            if (response.IsSuccessStatusCode) return VideoMetadata.FromJson(response.Content!, fields);

            _logger.LogError("Failed to get video metadata: {Error}", response.ErrorMessage);
            return null;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting video metadata for {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Converts VideoFilters to query string parameters
    /// </summary>
    /// <param name="filters">Video filters to convert</param>
    /// <returns>Dictionary of query string parameters</returns>
    public static Dictionary<string, string> ConvertVideoFiltersToParameters(VideoFilters filters)
    {
        var parameters = new Dictionary<string, string>();
        var flags = new List<string>();

        // Boolean flag filters
        if (filters.ThreeSixtyDegree == true) flags.Add("360_degree");
        if (filters.AdvertisingInstreamBlocked == true) flags.Add("advertising_instream_blocked");
        if (filters.AllowedInPlaylists == true) flags.Add("allowed_in_playlists");
        if (filters.Availability == true) flags.Add("availability");
        if (filters.Exportable == true) flags.Add("exportable");
        if (filters.Featured == true) flags.Add("featured");
        if (filters.HasGame == true) flags.Add("has_game");
        if (filters.Hd == true) flags.Add("hd");
        if (filters.InHistory == true) flags.Add("in_history");
        if (filters.Live == true) flags.Add("live");
        if (filters.LiveOffair == true) flags.Add("live_offair");
        if (filters.LiveOnair == true) flags.Add("live_onair");
        if (filters.LiveUpcoming == true) flags.Add("live_upcoming");
        if (filters.NoLive == true) flags.Add("no_live");
        if (filters.NoLiveRecording == true) flags.Add("no_live_recording");
        if (filters.NoPremium == true) flags.Add("no_premium");
        if (filters.Partner == true) flags.Add("partner");
        if (filters.Premium == true) flags.Add("premium");
        if (filters.Ugc == true) flags.Add("ugc");
        if (filters.UgcPartner == true) flags.Add("ugc_partner");
        if (filters.Verified == true) flags.Add("verified");

        // Add custom flags if provided (filter out empty/null values)
        if (filters.Flags is { Length: > 0 })
        {
            var validCustomFlags = filters.Flags
                .Where(flag => !string.IsNullOrWhiteSpace(flag))
                .ToArray();

            if (validCustomFlags.Length > 0)
            {
                flags.AddRange(validCustomFlags);
            }
        }

        // Add flags parameter if any flags are set (remove duplicates)
        if (flags.Count > 0)
        {
            var uniqueFlags = flags.Distinct().ToArray();
            parameters["flags"] = string.Join(",", uniqueFlags);
        }

        // Regular parameter filters
        if (!string.IsNullOrWhiteSpace(filters.Channel)) parameters["channel"] = filters.Channel;
        if (!string.IsNullOrWhiteSpace(filters.Country)) parameters["country"] = filters.Country;
        if (filters.CreatedAfter.HasValue) parameters["created_after"] = ((DateTimeOffset)filters.CreatedAfter.Value).ToUnixTimeSeconds().ToString();
        if (filters.CreatedBefore.HasValue) parameters["created_before"] = ((DateTimeOffset)filters.CreatedBefore.Value).ToUnixTimeSeconds().ToString();
        if (filters.ExcludeChannelIds is { Length: > 0 }) parameters["exclude_channel_ids"] = string.Join(",", filters.ExcludeChannelIds);
        if (filters.ExcludeIds is { Length: > 0 }) parameters["exclude_ids"] = string.Join(",", filters.ExcludeIds);
        if (filters.Explicit.HasValue) parameters["explicit"] = filters.Explicit.Value.ToString().ToLowerInvariant();
        if (filters.Ids is { Length: > 0 }) parameters["ids"] = string.Join(",", filters.Ids);
        if (filters.IsCreatedForKids.HasValue) parameters["is_created_for_kids"] = filters.IsCreatedForKids.Value.ToString().ToLowerInvariant();
        if (filters.Languages is { Length: > 0 }) parameters["languages"] = string.Join(",", filters.Languages);
        if (!string.IsNullOrWhiteSpace(filters.List)) parameters["list"] = filters.List;
        if (filters.LongerThan.HasValue) parameters["longer_than"] = filters.LongerThan.Value.ToString();
        if (!string.IsNullOrWhiteSpace(filters.Mode)) parameters["mode"] = filters.Mode;
        if (!string.IsNullOrWhiteSpace(filters.NoGenre)) parameters["nogenre"] = filters.NoGenre;
        if (filters.Owners is { Length: > 0 }) parameters["owners"] = string.Join(",", filters.Owners);
        if (filters.PasswordProtected.HasValue) parameters["password_protected"] = filters.PasswordProtected.Value.ToString().ToLowerInvariant();
        if (filters.Private.HasValue) parameters["private"] = filters.Private.Value.ToString().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(filters.RelatedVideosAlgorithm)) parameters["related_videos_algorithm"] = filters.RelatedVideosAlgorithm;
        if (!string.IsNullOrWhiteSpace(filters.Search)) parameters["search"] = filters.Search;
        if (filters.ShorterThan.HasValue) parameters["shorter_than"] = filters.ShorterThan.Value.ToString();
        if (!string.IsNullOrWhiteSpace(filters.Sort)) parameters["sort"] = filters.Sort;
        if (filters.Tags is { Length: > 0 }) parameters["tags"] = string.Join(",", filters.Tags);
        if (filters.Timeframe.HasValue) parameters["timeframe"] = filters.Timeframe.Value.ToString();
        if (filters.Unpublished.HasValue) parameters["unpublished"] = filters.Unpublished.Value.ToString().ToLowerInvariant();
        if (filters.Page.HasValue) parameters["page"] = filters.Page.Value.ToString();
        if (filters.Limit.HasValue) parameters["limit"] = filters.Limit.Value.ToString();

        return parameters;
    }


    /// <summary>
    /// Get upload URL as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.UploadUrlResponse?&gt; representing the asynchronous operation.</returns>
    public async Task<UploadUrlResponse?> GetUploadUrlAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting video upload URL");

            var response = await _httpClient.GetAsync("/file/upload", null, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get video upload URL: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonConvert.DeserializeObject<UploadUrlResponse>(response.Content!, _jsonSettings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting video upload URL");
            throw;
        }
    }


    /// <summary>
    /// Upload from URL as an asynchronous operation.
    /// </summary>
    /// <param name="videoUrl">The video URL.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="channel">The channel.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">The is private.</param>
    /// <param name="published">The published.</param>
    /// <param name="isCreatedForKids">The is created for kids.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.UploadCompletionResponse?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video URL cannot be null or empty, nameof(videoUrl)</exception>
    /// <exception cref="ArgumentException">Video title cannot be null or empty, nameof(title)</exception>
    public async Task<UploadCompletionResponse?> UploadFromUrlAsync(string videoUrl, string title, string? description = null, string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isCreatedForKids = false, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                throw new ArgumentException("Video URL cannot be null or empty", nameof(videoUrl));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Video title cannot be null or empty", nameof(title));

            _logger.LogDebug("Uploading video from URL. Title: {Title}, URL: {Url}", title, videoUrl);

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

            if (tags is { Length: > 0 })
                parameters["tags"] = string.Join(",", tags);

            parameters["private"] = isPrivate.ToString().ToLowerInvariant();

            // Get user ID from /me endpoint for password authentication
            var userId = await GetUserIdFromMeEndpointAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("Could not get user ID from /me endpoint for video creation");
                return null;
            }

            var response = await _httpClient.PostPublicAsync($"/user/{userId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to upload video from URL: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonConvert.DeserializeObject<UploadCompletionResponse>(response.Content!, _jsonSettings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading video from URL");
            throw;
        }
    }


    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;bool&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    public async Task<bool> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Deleting video {VideoId}", videoId);

            var response = await _httpClient.DeleteAsync($"/video/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to delete video {VideoId}: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting video {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Update video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoFilters? filters = null, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(videoId))
            throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

        _logger.LogDebug("Updating video {VideoId}", videoId);

        var parameters = new Dictionary<string, string>();

        // Add fields parameter
        if (fields is { Length: > 0 })
        {
            parameters["fields"] = string.Join(",", fields.ToApiFieldNames());
        }

        // Add filter parameters
        if (filters != null)
        {
            var filterParams = ConvertVideoFiltersToParameters(filters);
            foreach (var filter in filterParams)
            {
                parameters[filter.Key] = filter.Value;
            }
        }

        var response = await _httpClient.PostAsync($"/video/{videoId}", parameters, null, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
            return null;
        }

        return VideoMetadata.FromJson(response.Content!);
    }

    /// <summary>
    /// Update video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">nameof(parameters)</exception>
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoUpdateParameters? parameters, CancellationToken cancellationToken = default)
    {

        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        _logger.LogDebug("Creating video with parameters. Title: {Title}, URL: {Url}", parameters.Title ?? "null", parameters.Url ?? "null");

        var requestParameters = ConvertVideoCreationParametersToDictionary(parameters);

        var response = await _httpClient.PostAsync($"/video/{videoId}", requestParameters, null, cancellationToken);
        if (response.IsSuccessStatusCode) return VideoMetadata.FromJson(response.Content!);
        _logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
        return null;

    }


    /// <summary>
    /// Update video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="channel">The channel.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">The is private.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, string? title = null, string? description = null, string? channel = null, string[]? tags = null, bool? isPrivate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Updating video {VideoId}", videoId);

            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(title))
                parameters["title"] = title;

            if (!string.IsNullOrWhiteSpace(description))
                parameters["description"] = description;

            if (!string.IsNullOrWhiteSpace(channel))
                parameters["channel"] = channel;

            if (tags is { Length: > 0 })
                parameters["tags"] = string.Join(",", tags);

            if (isPrivate.HasValue)
                parameters["private"] = isPrivate.Value.ToString().ToLowerInvariant();

            var response = await _httpClient.PostAsync($"/video/{videoId}", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
                return null;
            }

            return VideoMetadata.FromJson(response.Content!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating video {VideoId}", videoId);
            throw;
        }
    }


    /// <summary>
    /// Update video embed settings as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="allowEmbed">The allow embed.</param>
    /// <param name="geoblocking">The geoblocking.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    public async Task<VideoMetadata?> UpdateVideoEmbedSettingsAsync(string videoId, bool? allowEmbed = null, List<string>? geoblocking = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

            _logger.LogDebug("Updating video embed settings for {VideoId}", videoId);

            var parameters = new Dictionary<string, string>();

            if (allowEmbed.HasValue)
                parameters["allow_embed"] = allowEmbed.Value.ToString().ToLowerInvariant();

            if (geoblocking is { Count: > 0 })
                parameters["geoblocking"] = string.Join(",", geoblocking);

            // Use POST method instead of PUT for video updates
            var response = await _httpClient.PostAsync($"/video/{videoId}", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to update video embed settings for {VideoId}: {Error}", videoId, response.ErrorMessage);
                return null;
            }

            return VideoMetadata.FromJson(response.Content!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating video embed settings for {VideoId}", videoId);
            throw;
        }
    }


    /// <summary>
    /// Gets the videos.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <returns>DailymotionSDK.Models.VideoListResponse?.</returns>
    public VideoListResponse? GetVideos(VideoFilters? filters = null, VideoFields[]? fields = null)
    {
        return GetVideosAsync(filters, fields).GetAwaiter().GetResult();
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
                var filterParams = ConvertVideoFiltersToParameters(filters);
                foreach (var filter in filterParams)
                {
                    parameters[filter.Key] = filter.Value;
                }
            }

            parameters["sort"] = sort.ToApiSortString();

            var response = await _httpClient.GetPublicAsync("/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get videos: {Error}", response.ErrorMessage);
                return null;
            }

            // Use custom converter for VideoMetadata with original field selection (including restricted fields)
            // This ensures the converter knows about all requested fields, even if they weren't in the API response
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new VideoMetadataJsonConverter(fields) },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting videos");
            throw;
        }
    }

    /// <summary>
    /// Shorthand method for GetVideosAsync with filters as an asynchronous operation.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="limit">The limit.</param>
    /// <param name="page">The page.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoListResponse?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">nameof(filters)</exception>
    public async Task<VideoListResponse?> SearchVideosWithFiltersAsync(VideoFilters? filters, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(filters);

            _logger.LogDebug("Searching videos with filters. Limit: {Limit}, Page: {Page}, Sort: {Sort}", limit, page, sort);
            
            filters.Limit = limit;
            filters.Page = page;
            filters.Sort = sort.ToApiSortString();

            return await GetVideosAsync(filters, [], sort, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching videos with filters");
            throw;
        }
    }


    /// <summary>
    /// Get channel videos with filters as an asynchronous operation.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="limit">The limit.</param>
    /// <param name="page">The page.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoListResponse?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">nameof(filters)</exception>
    public async Task<VideoListResponse?> GetChannelVideosWithFiltersAsync(Channel channel, VideoFilters filters, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, string? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));


            _logger.LogDebug("Getting channel videos with filters. Channel: {Channel}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", channel, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;

            // Add filter parameters
            var filterParams = ConvertVideoFiltersToParameters(filters);
            foreach (var filter in filterParams)
            {
                parameters[filter.Key] = filter.Value;
            }

            var response = await _httpClient.GetPublicAsync($"/channel/{channel.ToString().ToLowerInvariant()}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get channel videos with filters: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, _jsonSettings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel videos with filters for channel {Channel}", channel);
            throw;
        }
    }


    /// <summary>
    /// Get user videos with filters as an asynchronous operation.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="limit">The limit.</param>
    /// <param name="page">The page.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoListResponse?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">User ID cannot be null or empty, nameof(userId)</exception>
    /// <exception cref="ArgumentNullException">nameof(filters)</exception>
    public async Task<VideoListResponse?> GetUserVideosWithFiltersAsync(string userId, VideoFilters filters, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, string? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _logger.LogDebug("Getting user videos with filters. User: {UserId}, Limit: {Limit}, Page: {Page}, Sort: {Sort}", userId, limit, page, sort);

            var parameters = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString(),
                ["page"] = page.ToString(),
                ["sort"] = sort.ToApiSortString()
            };

            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;

            // Add filter parameters
            var filterParams = ConvertVideoFiltersToParameters(filters);
            foreach (var filter in filterParams)
            {
                parameters[filter.Key] = filter.Value;
            }

            var response = await _httpClient.GetPublicAsync($"/user/{userId}/videos", parameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user videos with filters: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonConvert.DeserializeObject<VideoListResponse>(response.Content!, _jsonSettings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user videos with filters for user {UserId}", userId);
            throw;
        }
    }


    /// <summary>
    /// Create video from file as an asynchronous operation.
    /// </summary>
    /// <param name="fileUrl">The file URL.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="channel">The channel.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">The is private.</param>
    /// <param name="published">The published.</param>
    /// <param name="isCreatedForKids">The is created for kids.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">File URL cannot be null or empty, nameof(fileUrl)</exception>
    /// <exception cref="ArgumentException">Video title cannot be null or empty, nameof(title)</exception>
    public async Task<VideoMetadata?> CreateVideoFromFileAsync(string fileUrl, string title, string? description = null, string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isCreatedForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File URL cannot be null or empty", nameof(fileUrl));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Video title cannot be null or empty", nameof(title));

            _logger.LogDebug("Creating video from file. Title: {Title}, File URL: {FileUrl}", title, fileUrl);

            var parameters = new VideoCreationParameters
            {
                Url = fileUrl,
                Title = title,
                Description = description,
                Channel = channel,
                Tags = tags,
                Private = isPrivate,
                Published = published,
                IsCreatedForKids = isCreatedForKids,
                Fields = fields
            };

            return await CreateVideo(parameters, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video from file");
            throw;
        }
    }


    /// <summary>
    /// Create video from file as an asynchronous operation.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">nameof(parameters)</exception>
    /// <exception cref="ArgumentException">URL cannot be null or empty, nameof(parameters.Url)</exception>
    /// <exception cref="ArgumentException">Title cannot be null or empty, nameof(parameters.Title)</exception>
    public async Task<VideoMetadata?> CreateVideoFromFileAsync(VideoCreationParameters parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(parameters.Url))
                throw new ArgumentException("URL cannot be null or empty", nameof(parameters.Url));
            if (string.IsNullOrWhiteSpace(parameters.Title))
                throw new ArgumentException("Title cannot be null or empty", nameof(parameters.Title));

            _logger.LogDebug("Creating video from file with custom parameters. Title: {Title}, URL: {Url}", parameters.Title, parameters.Url);

            return await CreateVideo(parameters, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video from file with custom parameters");
            throw;
        }
    }

    /// <summary>
    /// Converts the video creation parameters to dictionary.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>System.Collections.Generic.Dictionary&lt;string, string&gt;.</returns>
    private Dictionary<string, string> ConvertVideoCreationParametersToDictionary(VideoCreationParameters parameters)
    {
        var requestParameters = new Dictionary<string, string>();

        // Add title only if provided
        if (!string.IsNullOrWhiteSpace(parameters.Title))
            requestParameters["title"] = parameters.Title;

        // Add URL only if provided (not required for live events)
        if (!string.IsNullOrWhiteSpace(parameters.Url))
            requestParameters["url"] = parameters.Url;

        // Add optional parameters only if they are explicitly set (not null)
        // Based on the official Dailymotion API documentation for video creation

        // Basic video information
        if (!string.IsNullOrWhiteSpace(parameters.Description))
            requestParameters["description"] = parameters.Description;

        if (!string.IsNullOrWhiteSpace(parameters.Channel))
            requestParameters["channel"] = parameters.Channel;

        if (parameters.Tags is { Length: > 0 })
            requestParameters["tags"] = string.Join(",", parameters.Tags);

        if (parameters.Private.HasValue)
            requestParameters["private"] = parameters.Private.Value.ToString().ToLowerInvariant();

        if (parameters.Published.HasValue)
            requestParameters["published"] = parameters.Published.Value.ToString().ToLowerInvariant();

        if (parameters.IsCreatedForKids.HasValue)
            requestParameters["is_created_for_kids"] = parameters.IsCreatedForKids.Value.ToString().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(parameters.Mode))
            requestParameters["mode"] = parameters.Mode;

        if (!string.IsNullOrWhiteSpace(parameters.Language))
            requestParameters["language"] = parameters.Language;

        if (!string.IsNullOrWhiteSpace(parameters.Country))
            requestParameters["country"] = parameters.Country;

        // Advertising and monetization
        if (!string.IsNullOrWhiteSpace(parameters.AdvertisingCustomTarget))
            requestParameters["advertising_custom_target"] = parameters.AdvertisingCustomTarget;

        if (parameters.AdvertisingInstreamBlocked.HasValue)
            requestParameters["advertising_instream_blocked"] = parameters.AdvertisingInstreamBlocked.Value.ToString().ToLowerInvariant();

        // AI and automation
        if (parameters.AiChapterGenerationRequired.HasValue)
            requestParameters["ai_chapter_generation_required"] = parameters.AiChapterGenerationRequired.Value.ToString().ToLowerInvariant();

        if (parameters.StreamAlteredWithAi.HasValue)
            requestParameters["stream_altered_with_ai"] = parameters.StreamAlteredWithAi.Value.ToString().ToLowerInvariant();

        // Embedding and sharing
        if (parameters.AllowEmbed.HasValue)
            requestParameters["allow_embed"] = parameters.AllowEmbed.Value.ToString().ToLowerInvariant();

        if (parameters.AllowedInPlaylists.HasValue)
            requestParameters["allowed_in_playlists"] = parameters.AllowedInPlaylists.Value.ToString().ToLowerInvariant();

        // Content provider
        if (!string.IsNullOrWhiteSpace(parameters.ContentProviderId))
            requestParameters["content_provider_id"] = parameters.ContentProviderId;

        // Custom classification
        if (parameters.CustomClassification is { Length: > 0 })
            requestParameters["custom_classification"] = string.Join(",", parameters.CustomClassification);

        // Scheduling and availability
        if (parameters.PublishDate.HasValue)
            requestParameters["publish_date"] = ((DateTimeOffset)parameters.PublishDate.Value).ToUnixTimeSeconds().ToString();

        if (parameters.ExpiryDate.HasValue)
            requestParameters["expiry_date"] = ((DateTimeOffset)parameters.ExpiryDate.Value).ToUnixTimeSeconds().ToString();

        if (parameters.ExpiryDateAvailability.HasValue)
            requestParameters["expiry_date_availability"] = parameters.ExpiryDateAvailability.Value.ToString().ToLowerInvariant();

        if (parameters.ExpiryDateDeletion.HasValue)
            requestParameters["expiry_date_deletion"] = parameters.ExpiryDateDeletion.Value.ToString().ToLowerInvariant();

        if (parameters.PublishDateKeepPrivate.HasValue)
            requestParameters["publish_date_keep_private"] = parameters.PublishDateKeepPrivate.Value.ToString().ToLowerInvariant();

        // Content flags
        if (parameters.Explicit.HasValue)
            requestParameters["explicit"] = parameters.Explicit.Value.ToString().ToLowerInvariant();

        // Geoblocking
        if (parameters.Geoblocking is { Length: > 0 })
            requestParameters["geoblocking"] = string.Join(",", parameters.Geoblocking);

        // Geolocation
        if (parameters.Geoloc is { Length: 2 })
            requestParameters["geoloc"] = $"[{parameters.Geoloc[0]},{parameters.Geoloc[1]}]";

        // Hashtags
        if (parameters.Hashtags is { Length: > 0 })
            requestParameters["hashtags"] = string.Join(",", parameters.Hashtags);

        // Live streaming
        if (parameters.EndTime.HasValue)
            requestParameters["end_time"] = ((DateTimeOffset)parameters.EndTime.Value).ToUnixTimeSeconds().ToString();

        if (parameters.LiveAdBreakLaunch.HasValue)
            requestParameters["live_ad_break_launch"] = parameters.LiveAdBreakLaunch.Value.ToString();

        if (parameters.LiveAutoRecord.HasValue)
            requestParameters["live_auto_record"] = parameters.LiveAutoRecord.Value.ToString().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(parameters.LiveBackupVideo))
            requestParameters["live_backup_video"] = parameters.LiveBackupVideo;

        if (parameters.StartTime.HasValue)
            requestParameters["start_time"] = ((DateTimeOffset)parameters.StartTime.Value).ToUnixTimeSeconds().ToString();

        // Password protection
        if (!string.IsNullOrWhiteSpace(parameters.Password))
            requestParameters["password"] = parameters.Password;

        // Player settings
        if (!string.IsNullOrWhiteSpace(parameters.PlayerNextVideo))
            requestParameters["player_next_video"] = parameters.PlayerNextVideo;

        // Recording status
        if (!string.IsNullOrWhiteSpace(parameters.RecordStatus))
            requestParameters["record_status"] = parameters.RecordStatus;

        // Soundtrack
        if (!string.IsNullOrWhiteSpace(parameters.SoundtrackIsrc))
            requestParameters["soundtrack_isrc"] = parameters.SoundtrackIsrc;

        if (parameters.SoundtrackPopularity.HasValue)
            requestParameters["soundtrack_popularity"] = parameters.SoundtrackPopularity.Value.ToString();

        // Thumbnail
        if (!string.IsNullOrWhiteSpace(parameters.ThumbnailUrl))
            requestParameters["thumbnail_url"] = parameters.ThumbnailUrl;

        //Password Protected
        if(parameters.PasswordProtected.HasValue) 
            requestParameters["password_protected"] = parameters.PasswordProtected.Value.ToString().ToLowerInvariant();

        // Audience URL
        if (!string.IsNullOrWhiteSpace(parameters.AudienceUrl))
            requestParameters["audience_url"] = parameters.AudienceUrl;

        return requestParameters;
    }


    /// <summary>
    /// Creates the video.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>DailymotionSDK.Models.VideoMetadata?.</returns>
    /// <exception cref="ArgumentNullException">nameof(parameters)</exception>
    public async Task<VideoMetadata?> CreateVideo(VideoCreationParameters? parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            _logger.LogDebug("Creating video with parameters. Title: {Title}, URL: {Url}", parameters.Title ?? "null", parameters.Url ?? "null");

            var requestParameters = ConvertVideoCreationParametersToDictionary(parameters);

            // Get user ID from /me endpoint for password authentication
            var userId = await GetUserIdFromMeEndpointAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("Could not get user ID from /me endpoint for video creation");
                return null;
            }

            // Build endpoint, appending fields as query string if provided
            var endpoint = $"/user/{userId}/videos";
            if (parameters.Fields is { Length: > 0 })
            {
                var fieldsCsv = string.Join(",", parameters.Fields.ToApiFieldNames());
                endpoint += $"?fields={fieldsCsv}";
            }

            var response = await _httpClient.PostPublicAsync(endpoint, requestParameters, null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create video: {Error}\nResponse Status: {StatusCode}, Content: {Content}\nRequest Parameters: {Parameters}", response.ErrorMessage, response.StatusCode, response.Content, string.Join(", ", requestParameters.Select(p => $"{p.Key}={p.Value}")));
                return null;
            }

            return VideoMetadata.FromJson(response.Content!, parameters.Fields);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video");
            throw;
        }
    }
}
