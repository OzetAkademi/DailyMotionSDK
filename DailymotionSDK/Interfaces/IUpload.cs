using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IUpload
/// </summary>
public interface IUpload
{
    /// <summary>
    /// Uploads the asynchronous.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{FileUpload}.</returns>
    Task<FileUpload> UploadAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads the asynchronous.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{FileUpload}.</returns>
    Task<FileUpload> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
}