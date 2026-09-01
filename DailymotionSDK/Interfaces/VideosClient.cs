using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Class VideosClient.
/// Implements the <see cref="DailymotionSDK.Interfaces.IVideos" />
/// </summary>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Interfaces.IVideos" />
public class VideosClient(IMe meClient, IDailymotionHttpClient httpClient, ILogger<VideosClient> logger) : IVideos
{
    /// <summary>
    /// Get video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.Video?&gt; representing the asynchronous operation.</returns>
    public async Task<Video?> GetVideoAsync(string videoId, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoId);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                var fieldsLog = fields is { Length: > 0 } ? string.Join(",", fields.Select(f => f.GetApiFieldName())) : "all";
                logger.LogDebug("Getting video metadata for {VideoId} with fields: {Fields}", videoId, fieldsLog);
            }

            Dictionary<string, string> parameters = [];
            if (fields is { Length: > 0 })
            {
                parameters["fields"] = string.Join(',', fields.ToApiFieldNames());
            }

            var response = await httpClient.GetAsync($"/videos/{videoId}", parameters, cancellationToken);

            if (response.IsSuccessStatusCode)
                return JsonHandler.Deserialize<Video>(response.Content);

            logger.LogError("Failed to get video metadata for {VideoId}: {Error}", videoId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting video metadata for {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Get video HLS as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="clientIp">The client ip.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoStreamUrls?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoStreamUrls?> GetVideoHLSAsync(string videoId, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoId);

            object requestBody = !string.IsNullOrEmpty(clientIp)
                ? new { protocol = "hls", client_ip = clientIp }
                : new { protocol = "hls", no_ip_lock = true, no_expire = true };

            var response = await httpClient.PostJsonAsync($"/videos/{videoId}/streams", requestBody, cancellationToken);

            if (response.IsSuccessStatusCode)
                return JsonHandler.Deserialize<VideoStreamUrls>(response.Content);

            logger.LogError("Failed to get video HLS metadata for {VideoId}: {Error}", videoId, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting video HLS metadata for {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Converts the video filters to parameters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <returns>System.Collections.Generic.Dictionary{string, string}.</returns>
    public static Dictionary<string, string> ConvertVideoFiltersToParameters(VideoFilters filters)
    {
        Dictionary<string, string> parameters = [];

        if (filters.Page.HasValue) parameters["page"] = filters.Page.Value.ToString();
        if (filters.PageSize.HasValue) parameters["page_size"] = filters.PageSize.Value.ToString();
        if (!string.IsNullOrWhiteSpace(filters.Visibility)) parameters["visibility"] = filters.Visibility.ToLowerInvariant();
        if (filters.EnableAdvertising.HasValue) parameters["enable_advertising"] = filters.EnableAdvertising.Value.ToString().ToLowerInvariant();
        if (filters.IsExplicit.HasValue) parameters["is_explicit"] = filters.IsExplicit.Value.ToString().ToLowerInvariant();
        if (filters.IsForKids.HasValue) parameters["is_for_kids"] = filters.IsForKids.Value.ToString().ToLowerInvariant();
        if (filters.CreatedAfter.HasValue) parameters["created_after"] = ((DateTimeOffset)filters.CreatedAfter.Value).ToUnixTimeSeconds().ToString();
        if (filters.CreatedBefore.HasValue) parameters["created_before"] = ((DateTimeOffset)filters.CreatedBefore.Value).ToUnixTimeSeconds().ToString();
        if (!string.IsNullOrWhiteSpace(filters.Tags)) parameters["tags"] = filters.Tags;

        return parameters;
    }

    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;bool&gt; representing the asynchronous operation.</returns>
    public async Task<bool> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoId);

            logger.LogDebug("Deleting video {VideoId}", videoId);

            var response = await httpClient.DeleteAsync($"/videos/{videoId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to delete video {VideoId}: {Error}", videoId, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting video {VideoId}", videoId);
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
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoFilters? filters = null, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(videoId);

        logger.LogDebug("Updating video {VideoId}", videoId);

        Dictionary<string, string> parameters = [];

        if (fields is { Length: > 0 })
        {
            parameters["fields"] = string.Join(",", fields.ToApiFieldNames());
        }

        if (filters != null)
        {
            foreach (var (key, value) in ConvertVideoFiltersToParameters(filters))
            {
                parameters[key] = value;
            }
        }

        var response = await httpClient.PostAsync($"/video/{videoId}", parameters, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
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
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(videoId);
        ArgumentNullException.ThrowIfNull(parameters);

        var response = await httpClient.PostJsonAsync($"/video/{videoId}", parameters, cancellationToken);

        if (response.IsSuccessStatusCode)
            return VideoMetadata.FromJson(response.Content!);

        logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
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
    public async Task<VideoMetadata?> UpdateVideoAsync(string videoId, string? title = null, string? description = null, string? channel = null, string[]? tags = null, bool? isPrivate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoId);
            logger.LogDebug("Updating video {VideoId}", videoId);

            Dictionary<string, string> parameters = [];

            if (!string.IsNullOrWhiteSpace(title)) parameters["title"] = title;
            if (!string.IsNullOrWhiteSpace(description)) parameters["description"] = description;
            if (!string.IsNullOrWhiteSpace(channel)) parameters["channel"] = channel;
            if (tags is { Length: > 0 }) parameters["tags"] = string.Join(",", tags);
            if (isPrivate.HasValue) parameters["private"] = isPrivate.Value.ToString().ToLowerInvariant();

            var response = await httpClient.PostAsync($"/video/{videoId}", parameters, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to update video {VideoId}: {Error}", videoId, response.ErrorMessage);
                return null;
            }

            return VideoMetadata.FromJson(response.Content!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating video {VideoId}", videoId);
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
    public async Task<VideoMetadata?> UpdateVideoEmbedSettingsAsync(string videoId, bool? allowEmbed = null, List<string>? geoblocking = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoId);
            logger.LogDebug("Updating video embed settings for {VideoId}", videoId);

            Dictionary<string, string> parameters = [];

            if (allowEmbed.HasValue)
                parameters["allow_embed"] = allowEmbed.Value.ToString().ToLowerInvariant();

            if (geoblocking is { Count: > 0 })
                parameters["geoblocking"] = string.Join(",", geoblocking);

            var response = await httpClient.PostAsync($"/video/{videoId}", parameters, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to update video embed settings for {VideoId}: {Error}", videoId, response.ErrorMessage);
                return null;
            }

            return VideoMetadata.FromJson(response.Content!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating video embed settings for {VideoId}", videoId);
            throw;
        }
    }

    /// <summary>
    /// Get videos as an asynchronous operation.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoListResponse?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoListResponse?> GetVideosAsync(VideoFilters? filters = null, VideoFields[]? fields = null, VideoSort sort = VideoSort.CreatedAt, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Getting videos with filters and fields");

            var me = await meClient.GetMeAsync(cancellationToken);

            Dictionary<string, string> parameters = new()
            {
                ["fields"] = string.Join(",", fields?.ToApiFieldNames() ?? [])
            };

            if (filters != null)
            {
                foreach (var (key, value) in ConvertVideoFiltersToParameters(filters))
                {
                    parameters[key] = value;
                }
            }

            parameters["sort"] = sort.ToApiSortString();

            var response = await httpClient.PostAsync($"/profiles/{me?.Profiles?[0].ProfileId}/videos", parameters, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get videos: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonHandler.Deserialize<VideoListResponse>(response.Content!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting videos");
            throw;
        }
    }

    /// <summary>
    /// Create video from file as an asynchronous operation.
    /// </summary>
    /// <param name="fileUrl">The file URL.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="category">The category.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">The is private.</param>
    /// <param name="published">The published.</param>
    /// <param name="isForKids">The is for kids.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoCreateResponse?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoCreateResponse?> CreateVideoFromFileAsync(string fileUrl, string title, string? description = null, string? category = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            logger.LogDebug("Creating video from file. Title: {Title}, File URL: {FileUrl}", title, fileUrl);

            var parameters = new VideoCreationParameters()
            {
                Source = new() { FileUrl = fileUrl },
                Title = title,
                Description = description,
                Category = category,
                Visibility = isPrivate ? "private" : "public",
                IsForKids = isForKids
            };

            return await CreateVideo(parameters, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating video from file");
            throw;
        }
    }

    /// <summary>
    /// Create video from file as an asynchronous operation.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoCreateResponse?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoCreateResponse?> CreateVideoFromFileAsync(VideoCreationParameters parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(parameters);
            return await CreateVideo(parameters, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating video from file with custom parameters");
            throw;
        }
    }

    /// <summary>
    /// Creates the video.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>DailymotionSDK.Models.VideoCreateResponse?.</returns>
    public async Task<VideoCreateResponse?> CreateVideo(VideoCreationParameters parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(parameters);

            var me = await meClient.GetMeAsync(cancellationToken);
            if (string.IsNullOrEmpty(me?.UserId))
            {
                logger.LogError("Could not get user ID from /me endpoint for video creation");
                return null;
            }

            if (!(me.Profiles?.Count > 0))
            {
                logger.LogError("Could not get profiles from /me endpoint for video creation");
                return null;
            }

            var endpoint = $"/profiles/{me.Profiles[0].ProfileId}/videos";
            var response = await httpClient.PostJsonAsync(endpoint, parameters, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("Failed to create video: {Error}\nResponse Status: {StatusCode}, Content: {Content}\nRequest Parameters: {Parameters}",
                        response.ErrorMessage,
                        response.StatusCode,
                        response.Content,
                        JsonHandler.Serialize(parameters));
                }
                return null;
            }

            return JsonHandler.Deserialize<VideoCreateResponse>(response.Content!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating video");
            throw;
        }
    }
}