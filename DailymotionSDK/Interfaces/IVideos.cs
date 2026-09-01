using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IVideos
/// </summary>
public interface IVideos
{
    /// <summary>
    /// Gets the video asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{Video}}.</returns>
    Task<Video?> GetVideoAsync(string videoId, VideoFields[]? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the video HLS asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="clientIp">The client ip.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoStreamUrls}}.</returns>
    Task<VideoStreamUrls?> GetVideoHLSAsync(string videoId, string? clientIp = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Boolean}.</returns>
    Task<bool> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the video asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoMetadata}}.</returns>
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoFilters? filters = null, VideoFields[]? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the video asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoMetadata}}.</returns>
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, VideoUpdateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the video asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="channel">The channel.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">if set to <c>true</c> [is private].</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoMetadata}}.</returns>
    Task<VideoMetadata?> UpdateVideoAsync(string videoId, string? title = null, string? description = null, string? channel = null, string[]? tags = null, bool? isPrivate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the video embed settings asynchronous.
    /// </summary>
    /// <param name="videoId">The video identifier.</param>
    /// <param name="allowEmbed">if set to <c>true</c> [allow embed].</param>
    /// <param name="geoblocking">The geoblocking.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoMetadata}}.</returns>
    Task<VideoMetadata?> UpdateVideoEmbedSettingsAsync(string videoId, bool? allowEmbed = null, List<string>? geoblocking = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the videos asynchronous.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoListResponse}}.</returns>
    Task<VideoListResponse?> GetVideosAsync(VideoFilters? filters = null, VideoFields[]? fields = null, VideoSort sort = VideoSort.CreatedAt, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video from file asynchronous.
    /// </summary>
    /// <param name="fileUrl">The file URL.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="category">The category.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="isPrivate">if set to <c>true</c> [is private].</param>
    /// <param name="published">if set to <c>true</c> [published].</param>
    /// <param name="isForKids">if set to <c>true</c> [is for kids].</param>
    /// <param name="fields">The fields.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoCreateResponse}}.</returns>
    Task<VideoCreateResponse?> CreateVideoFromFileAsync(string fileUrl, string title, string? description = null, string? category = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video from file asynchronous.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoCreateResponse}}.</returns>
    Task<VideoCreateResponse?> CreateVideoFromFileAsync(VideoCreationParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoCreateResponse}}.</returns>
    Task<VideoCreateResponse?> CreateVideo(VideoCreationParameters parameters, CancellationToken cancellationToken = default);
}