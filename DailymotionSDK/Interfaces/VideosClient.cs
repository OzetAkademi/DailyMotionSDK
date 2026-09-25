using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Models.Enums;
using DailymotionSDK.Models.Requests;
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
public class VideosClient(IDailymotionHttpClient httpClient, ILogger<VideosClient> logger) : IVideos
{
    /// <summary>
    /// Get video as an asynchronous operation.
    /// </summary>
    /// <param name="videoGetRequest">The video get request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task{System.Nullable{Video}}. representing the asynchronous operation.</returns>
    public async Task<Video?> GetVideoAsync(VideoGetRequest videoGetRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(videoGetRequest.Id);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                var fieldsLog = videoGetRequest.Fields is { Length: > 0 } ? string.Join(",", videoGetRequest.Fields.Select(f => f.GetApiFieldName())) : "all";
                logger.LogDebug("Getting video metadata for {VideoId} with fields: {Fields}", videoGetRequest.Id, fieldsLog);
            }

            Dictionary<string, string> parameters = [];
            if (videoGetRequest.Fields is { Length: > 0 })
            {
                parameters["fields"] = string.Join(',', videoGetRequest.Fields.ToApiFieldNames());
            }

            var response = await httpClient.GetAsync($"/videos/{videoGetRequest.Id}", parameters, cancellationToken);

            if (response.IsSuccessStatusCode)
                return JsonHandler.Deserialize<Video>(response.Content);

            logger.LogError("Failed to get video metadata for {VideoId}: {Error}", videoGetRequest.Id, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting video metadata for {VideoId}", videoGetRequest.Id);
            throw;
        }
    }

    /// <summary>
    /// Get video HLS as an asynchronous operation.
    /// </summary>
    /// <param name="videoHLSGetRequest">The video HLS get request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoStreamUrls?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoStreamUrls?> GetVideoHLSAsync(VideoHLSGetRequest videoHLSGetRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoHLSGetRequest.Id);

            object requestBody = !string.IsNullOrEmpty(videoHLSGetRequest.ClientIp)
                ? new { protocol = "hls", client_ip = videoHLSGetRequest.ClientIp }
                : new { protocol = "hls", no_ip_lock = true, no_expire = true };

            var response = await httpClient.PostJsonAsync($"/videos/{videoHLSGetRequest.Id}/streams", requestBody, cancellationToken);

            if (response.IsSuccessStatusCode)
                return JsonHandler.Deserialize<VideoStreamUrls>(response.Content);

            logger.LogError("Failed to get video HLS metadata for {VideoId}: {Error}", videoHLSGetRequest.Id, response.ErrorMessage);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting video HLS metadata for {VideoId}", videoHLSGetRequest.Id);
            throw;
        }
    }

    /// <summary>
    /// Converts the video filters to parameters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <returns>System.Collections.Generic.Dictionary{string, string}.</returns>
    private static Dictionary<string, string> ConvertVideoFiltersToParameters(VideoQueryParameters filters)
    {
        var parameters = new Dictionary<string, string>();

        if (filters.Page.HasValue)
            parameters["page"] = filters.Page.Value.ToString();

        if (filters.PageSize.HasValue)
            parameters["page_size"] = filters.PageSize.Value.ToString();

        if (!string.IsNullOrWhiteSpace(filters.Sort))
            parameters["sort"] = filters.Sort;

        if (filters.Visibility.HasValue)
            parameters["visibility"] = filters.Visibility.Value.ToString().ToLowerInvariant();

        if (filters.EnableAdvertising.HasValue)
            parameters["enable_advertising"] = filters.EnableAdvertising.Value.ToString().ToLowerInvariant();

        if (filters.IsExplicit.HasValue)
            parameters["is_explicit"] = filters.IsExplicit.Value.ToString().ToLowerInvariant();

        if (filters.IsForKids.HasValue)
            parameters["is_for_kids"] = filters.IsForKids.Value.ToString().ToLowerInvariant();

        if (filters.CreatedAfter.HasValue)
            parameters["created_after"] = ((DateTimeOffset)filters.CreatedAfter.Value).ToUnixTimeSeconds().ToString();

        if (filters.CreatedBefore.HasValue)
            parameters["created_before"] = ((DateTimeOffset)filters.CreatedBefore.Value).ToUnixTimeSeconds().ToString();

        if (!string.IsNullOrWhiteSpace(filters.Tags))
            parameters["tags"] = filters.Tags;

        return parameters;
    }

    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="videoDeleteRequest">The video delete request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task<bool> representing the asynchronous operation.</returns>
    public async Task<bool> DeleteVideoAsync(VideoDeleteRequest videoDeleteRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(videoDeleteRequest?.Id);

            logger.LogDebug("Deleting video {VideoId}", videoDeleteRequest.Id);

            var response = await httpClient.DeleteAsync($"/videos/{videoDeleteRequest.Id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to delete video {VideoId}: {Error}", videoDeleteRequest.Id, response.ErrorMessage);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting video {VideoId}", videoDeleteRequest.Id);
            throw;
        }
    }

    /// <summary>
    /// Update video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    public async Task<VideoMetadata?> UpdateVideoAsync(VideoUpdateRequest videoUpdateRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(videoUpdateRequest);
        ArgumentException.ThrowIfNullOrEmpty(videoUpdateRequest.Id);

        var response = await httpClient.PostJsonAsync($"/videos/{videoUpdateRequest.Id}", videoUpdateRequest, cancellationToken);

        if (response.IsSuccessStatusCode)
            return VideoMetadata.FromJson(response.Content!);

        logger.LogError("Failed to update video {VideoId}: {Error}", videoUpdateRequest.Id, response.ErrorMessage);
        return null;
    }

    /// <summary>
    /// Get videos as an asynchronous operation.
    /// </summary>
    /// <param name="videoListRequest">The video list request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task{System.Nullable{VideoListResponse?}} representing the asynchronous operation.</returns>
    public async Task<VideoListResponse?> GetVideosAsync(VideoListRequest videoListRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(videoListRequest?.ProfileId);

            logger.LogDebug("Getting videos with filters and fields");

            Dictionary<string, string> parameters = new()
            {
                ["fields"] = string.Join(',', videoListRequest.VideoQueryParameters?.Fields?.ToApiFieldNames() ?? [])
            };

            if (videoListRequest.VideoQueryParameters is not null)
            {
                foreach (var (key, value) in ConvertVideoFiltersToParameters(videoListRequest.VideoQueryParameters))
                {
                    parameters[key] = value;
                }
            }

            var response = await httpClient.PostAsync($"/profiles/{videoListRequest.ProfileId}/videos", parameters, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get videos: {Error}", response.ErrorMessage);
                return null;
            }

            return JsonHandler.Deserialize<VideoListResponse>(response.Content);
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
    /// <param name="profileId">The profile ID.</param>
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
    public async Task<VideoCreateResponse?> CreateVideoFromFileAsync(string profileId, string fileUrl, string title, string? description = null, Category? category = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(profileId);
            ArgumentException.ThrowIfNullOrEmpty(fileUrl);
            ArgumentException.ThrowIfNullOrEmpty(title);

            logger.LogDebug("Creating video from file. Title: {Title}, File URL: {FileUrl}", title, fileUrl);

            var videoCreateRequest = new VideoCreateRequest()
            {
                ProfileId = profileId,
                Source = new()
                {
                    FileUrl = fileUrl
                },
                Title = title,
                Description = description,
                Category = category,
                Visibility = isPrivate ? "private" : "public",
                IsForKids = isForKids
            };

            return await CreateVideo(videoCreateRequest, cancellationToken);
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
    public async Task<VideoCreateResponse?> CreateVideoFromFileAsync(VideoCreateRequest videoCreateRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(videoCreateRequest);
            return await CreateVideo(videoCreateRequest, cancellationToken);
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
    /// <param name="videoCreateRequest">The video create request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>DailymotionSDK.Models.VideoCreateResponse?.</returns>
    public async Task<VideoCreateResponse?> CreateVideo(VideoCreateRequest videoCreateRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(videoCreateRequest);
            ArgumentException.ThrowIfNullOrEmpty(videoCreateRequest.ProfileId);

            var endpoint = $"/profiles/{videoCreateRequest.ProfileId}/videos";
            var response = await httpClient.PostJsonAsync(endpoint, videoCreateRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("Failed to create video: {Error}\nResponse Status: {StatusCode}, Content: {Content}\nRequest Parameters: {Parameters}",
                        response.ErrorMessage,
                        response.StatusCode,
                        response.Content,
                        JsonHandler.Serialize(videoCreateRequest));
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