using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for subtitle operations
/// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
/// </summary>
public interface ISubtitles
{
    /// <summary>
    /// Gets subtitle information
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitle
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Subtitle metadata</returns>
    Task<SubtitleMetadata?> GetSubtitleAsync(string subtitleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new subtitle
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-subtitles
    /// </summary>
    /// <param name="subtitleData">Subtitle data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created subtitle metadata</returns>
    Task<SubtitleMetadata?> CreateSubtitleAsync(Dictionary<string, string> subtitleData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new subtitle for a video
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="url">URL pointing to the subtitle data (SRT format)</param>
    /// <param name="language">Language of the subtitles (optional)</param>
    /// <param name="format">Data format (SRT only is supported)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created subtitle metadata</returns>
    Task<SubtitleMetadata?> CreateSubtitleForVideoAsync(string videoId, string url, string? language = null, string format = "SRT", CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a subtitle
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-subtitles
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="subtitleData">Subtitle data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated subtitle metadata</returns>
    Task<SubtitleMetadata?> UpdateSubtitleAsync(string subtitleId, Dictionary<string, string> subtitleData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a subtitle
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-subtitles
    /// </summary>
    /// <param name="subtitleId">Subtitle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeleteSubtitleAsync(string subtitleId, CancellationToken cancellationToken = default);
}
