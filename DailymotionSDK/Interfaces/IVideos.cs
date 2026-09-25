using DailymotionSDK.Models;
using DailymotionSDK.Models.Enums;
using DailymotionSDK.Models.Requests;
using Microsoft.Extensions.Options;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IVideos
/// </summary>
public interface IVideos
{
    /// <summary>
    /// Gets the video asynchronous.
    /// </summary>
    /// <param name="videoGetRequest">The video get request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{Video}}.</returns>
    Task<Video?> GetVideoAsync(VideoGetRequest videoGetRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the video HLS asynchronous.
    /// </summary>
    /// <param name="videoHLSGetRequest">The video HLS get request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoStreamUrls}}.</returns>
    Task<VideoStreamUrls?> GetVideoHLSAsync(VideoHLSGetRequest videoHLSGetRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="videoDeleteRequest">The video delete request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Boolean}.</returns>
    Task<bool> DeleteVideoAsync(VideoDeleteRequest videoDeleteRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the video asynchronous. 
    /// Partially update a video (PATCH). 
    /// Send only fields to change; omitted fields stay unchanged. 
    /// Updates return 204 No Content. 
    /// Requires video.manage scope
    /// </summary>
    /// <param name="videoUpdateRequest">The video update request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoMetadata}}.</returns>
    Task<VideoMetadata?> UpdateVideoAsync(VideoUpdateRequest videoUpdateRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the videos asynchronous. 
    /// List videos for the given profile_id with optional sort, optional filters(visibility, tags, date range, etc.), and fields for sparse responses. 
    /// Default sort is created_at descending. 
    /// Requires video.read scope.
    /// </summary>
    /// <param name="videoListRequest">The video list request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoListResponse}}.</returns>
    Task<VideoListResponse?> GetVideosAsync(VideoListRequest videoListRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video from file asynchronous.
    /// </summary>
    /// <param name="profileId">The profile ID.</param>
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
    Task<VideoCreateResponse?> CreateVideoFromFileAsync(string profileId, string fileUrl, string title, string? description = null, Category? category = null, string[]? tags = null, bool isPrivate = false, bool published = true, bool isForKids = false, VideoFields[]? fields = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video from file asynchronous.
    /// </summary>
    /// <param name="videoCreateRequest">The video create request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoCreateResponse}}.</returns>
    Task<VideoCreateResponse?> CreateVideoFromFileAsync(VideoCreateRequest videoCreateRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the video. 
    /// Create a new video under this profile. 
    /// Required body fields are title, visibility, category, is_for_kids, and source (use source.file_url for upload workflows). 
    /// Returns 201 with the created representation. 
    /// Requires video.manage scope.
    /// </summary>
    /// <param name="videoCreateRequest">The video create request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{VideoCreateResponse}}.</returns>
    Task<VideoCreateResponse?> CreateVideo(VideoCreateRequest videoCreateRequest, CancellationToken cancellationToken = default);
}