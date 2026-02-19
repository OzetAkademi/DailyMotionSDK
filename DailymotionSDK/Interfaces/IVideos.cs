using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion video operations
/// https://developers.dailymotion.com/api/platform-api/reference/#video
/// </summary>
public interface IVideos
{
    /// <summary>
    /// Gets video metadata by ID (base function)
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// Note: This is a base function without mandatory parameter validation.
    /// Calling functions should handle their own parameter validation requirements.
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="fields">List of fields to retrieve</param>
    /// <param name="globalApiParameters"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video metadata</returns>
    Task<VideoMetadata?> GetVideoAsync(string videoId, VideoFields[]? fields = null,
        GlobalApiParameters? globalApiParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets videos with filters and fields (async)
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="filters">Video filters to apply</param>
    /// <param name="fields">List of fields to retrieve</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse?> GetVideosAsync(VideoFilters? filters = null, VideoFields[]? fields = null,
        VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets videos with filters and fields (sync wrapper)
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="filters">Video filters to apply</param>
    /// <param name="fields">List of fields to retrieve</param>
    /// <returns>Video list response</returns>
    VideoListResponse? GetVideos(VideoFilters? filters = null, VideoFields[]? fields = null);

    /// <summary>
    /// Gets video upload URL
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload URL response</returns>
    Task<UploadUrlResponse?> GetUploadUrlAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads a video from URL
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="videoUrl">Video URL</param>
    /// <param name="title">Video title</param>
    /// <param name="description">Video description</param>
    /// <param name="channel">Channel ID</param>
    /// <param name="tags">Video tags</param>
    /// <param name="isPrivate">Whether video is private</param>
    /// <param name="published">Whether video is published</param>
    /// <param name="isCreatedForKids">Whether video is created for kids</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload completion response</returns>
    Task<UploadCompletionResponse?> UploadFromUrlAsync(string videoUrl, string title, string? description = null, string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isCreatedForKids = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a video
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates video metadata
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
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
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, string? title = null, string? description = null, string? channel = null, string[]? tags = null, bool? isPrivate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update video as an asynchronous operation.
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Video ID cannot be null or empty, nameof(videoId)</exception>
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoFilters? filters = null, VideoFields[]? fields = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// Update video as an asynchronous operation.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.VideoMetadata?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">nameof(parameters)</exception>
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoUpdateParameters? parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates video embed settings
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="allowEmbed">Whether video can be embedded outside of Dailymotion</param>
    /// <param name="geoblocking">List of countries where video is accessible/blocked (e.g., ["allow", "us", "ca"] or ["deny", "fr", "de"])</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated video metadata</returns>
    Task<VideoMetadata?> UpdateVideoEmbedSettingsAsync(string videoId, bool? allowEmbed = null, List<string>? geoblocking = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a video from an uploaded file
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="fileUrl">Uploaded file URL</param>
    /// <param name="title">Video title</param>
    /// <param name="description">Video description</param>
    /// <param name="channel">Channel ID</param>
    /// <param name="tags">Video tags</param>
    /// <param name="isPrivate">Whether video is private</param>
    /// <param name="published">Whether video is published</param>
    /// <param name="isCreatedForKids">Whether video is created for kids</param>
    /// <param name="fields">List of fields to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created video metadata</returns>
    Task<VideoMetadata?> CreateVideoFromFileAsync(string fileUrl, string title, string? description = null,
        string? channel = null, string[]? tags = null, bool isPrivate = false, bool published = true,
        bool isCreatedForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a video from an uploaded file with custom parameters
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="parameters">Video creation parameters with all available fields</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created video metadata</returns>
    Task<VideoMetadata?> CreateVideoFromFileAsync(VideoCreationParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a video using the specified parameters (base function)
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// Note: This is a base function without mandatory parameter validation.
    /// Calling functions should handle their own parameter validation requirements.
    /// </summary>
    /// <param name="parameters">Video creation parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created video metadata</returns>
    Task<VideoMetadata?> CreateVideo(VideoCreationParameters? parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches videos with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="filters">Video filters to apply</param>
    /// <param name="limit">Maximum number of results to return</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="sort">Sort order for results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response with pagination information</returns>
    Task<VideoListResponse?> SearchVideosWithFiltersAsync(VideoFilters? filters, int limit = 20, int page = 1,
        VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets videos by channel with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="channel">Channel to search in</param>
    /// <param name="filters">Video filters to apply</param>
    /// <param name="limit">Maximum number of results to return</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="sort">Sort order for results</param>
    /// <param name="fields">Comma-separated list of fields to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response with pagination information</returns>
    Task<VideoListResponse?> GetChannelVideosWithFiltersAsync(Channel channel, VideoFilters filters, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, string? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets videos by user with filters
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="filters">Video filters to apply</param>
    /// <param name="limit">Maximum number of results to return</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="sort">Sort order for results</param>
    /// <param name="fields">Comma-separated list of fields to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response with pagination information</returns>
    Task<VideoListResponse?> GetUserVideosWithFiltersAsync(string userId, VideoFilters filters, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, string? fields = null, CancellationToken cancellationToken = default);
}
