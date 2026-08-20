using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Class UploadClient.
/// Implements the <see cref="DailymotionSDK.Interfaces.IUpload" />
/// </summary>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Interfaces.IUpload" />
public class UploadClient(IDailymotionHttpClient httpClient, ILogger<UploadClient> logger) : IUpload
{
    /// <summary>
    /// Uploads the asynchronous.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;FileUpload&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.IO.FileNotFoundException">File not found</exception>
    public async Task<FileUpload> UploadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        try
        {
            logger.LogDebug("Uploading file: {FilePath}", filePath);

            await using var fileStream = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);

            return await UploadAsync(fileStream, fileName, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Uploads the asynchronous.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;FileUpload&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.InvalidOperationException">Failed to get upload URL: {uploadUrlResponse.ErrorMessage}</exception>
    /// <exception cref="System.InvalidOperationException">Invalid upload URL response</exception>
    /// <exception cref="System.InvalidOperationException">File upload failed: {uploadResponse.ErrorMessage}</exception>
    public async Task<FileUpload> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        try
        {
            logger.LogDebug("Starting file upload process for: {FileName}", fileName);

            logger.LogDebug("Step 1: Getting upload URL from /files/upload_sessions");
            var uploadUrlResponse = await httpClient.PostAsync("/files/upload_sessions", null, cancellationToken);

            if (!uploadUrlResponse.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get upload URL: {StatusCode} - {Content}", uploadUrlResponse.StatusCode, uploadUrlResponse.Content);
                throw new InvalidOperationException($"Failed to get upload URL: {uploadUrlResponse.ErrorMessage}");
            }

            var uploadUrlData = JsonHandler.Deserialize<UploadSession>(uploadUrlResponse.Content!);
            if (string.IsNullOrEmpty(uploadUrlData?.UploadUrl))
            {
                logger.LogError("Invalid upload URL response: {Content}", uploadUrlResponse.Content);
                throw new InvalidOperationException("Invalid upload URL response");
            }

            logger.LogDebug("Received upload URL: {UploadUrl}", uploadUrlData.UploadUrl);

            logger.LogDebug("Step 2: Uploading file to the provided URL");

            using var uploadClient = new RestClient();

            var uploadRequest = new RestRequest(uploadUrlData.UploadUrl, Method.Post);
            uploadRequest.AddHeader("Accept", "application/json");

            uploadRequest.AddFile("file", () => stream, fileName);

            var uploadResponse = await uploadClient.ExecuteAsync(uploadRequest, cancellationToken);

            if (uploadResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(uploadResponse.Content))
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Upload response content: {Content}", uploadResponse.Content);
                }

                var result = JsonSerializer.Deserialize<FileUpload>(uploadResponse.Content);
                logger.LogDebug("File upload completed successfully. Parsed URL: {Url}", result?.Url);

                return result ?? new();
            }

            logger.LogWarning("File upload failed with status: {StatusCode} - {Content}", uploadResponse.StatusCode, uploadResponse.Content);
            throw new InvalidOperationException($"File upload failed: {uploadResponse.ErrorMessage}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file stream: {FileName}", fileName);
            throw;
        }
    }
}