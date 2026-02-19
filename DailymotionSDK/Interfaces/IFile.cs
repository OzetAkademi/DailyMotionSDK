using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for file upload operations
/// https://developers.dailymotion.com/api/platform-api/reference/#file
/// </summary>
public interface IFile
{
    /// <summary>
    /// Uploads a file to DailyMotion
    /// https://developers.dailymotion.com/api/platform-api/reference/#file-upload
    /// </summary>
    /// <param name="filePath">Path to the file to upload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File upload response</returns>
    Task<FileUploadResponse> UploadAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads a file from a stream to DailyMotion
    /// https://developers.dailymotion.com/api/platform-api/reference/#file-upload
    /// </summary>
    /// <param name="stream">File stream</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File upload response</returns>
    Task<FileUploadResponse> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Monitors the upload progress using the progress URL
    /// </summary>
    /// <param name="progressUrl">The progress URL from the upload response</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload progress information</returns>
    Task<UploadProgressResponse?> MonitorUploadProgressAsync(string progressUrl, CancellationToken cancellationToken = default);
}
