using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Text.Json;

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
    /// Start an upload session (no JSON body).
    /// The response contains upload_url(POST the file here) and progress_url(poll until complete).
    /// Call this before attaching the file to a video record.
    /// Requires video.manage scope and a valid Bearer token.
    /// </summary>
    /// <param name="fileUploadRequest">The file upload request.</param>
    /// <returns>A Task&lt;FileUpload&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.IO.FileNotFoundException">File not found</exception>
    public async Task<FileUpload> UploadAsync(FileUploadRequest fileUploadRequest)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileUploadRequest.FilePath);

        if (!File.Exists(fileUploadRequest.FilePath))
            throw new FileNotFoundException("File not found", fileUploadRequest.FilePath);

        try
        {
            logger.LogDebug("Uploading file: {FilePath}", fileUploadRequest.FilePath);

            await using var fileStream = File.OpenRead(fileUploadRequest.FilePath);
            var fileName = Path.GetFileName(fileUploadRequest.FilePath);

            return await UploadAsync(
                new FileStreamUploadRequest()
                {
                    Stream = fileStream,
                    FileName = fileName
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file: {FilePath}", fileUploadRequest.FilePath);
            throw;
        }
    }

    /// <summary>
    /// Upload using multipart/form-data.
    /// </summary>
    /// <param name="streamUploadRequest">The stream upload request.</param>
    /// <returns>A Task&lt;FileUpload&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.InvalidOperationException">Failed to get upload URL: {uploadUrlResponse.ErrorMessage}</exception>
    /// <exception cref="System.InvalidOperationException">Invalid upload URL response</exception>
    /// <exception cref="System.InvalidOperationException">File upload failed: {uploadResponse.ErrorMessage}</exception>
    public async Task<FileUpload> UploadAsync(FileStreamUploadRequest streamUploadRequest)
    {
        ArgumentNullException.ThrowIfNull(streamUploadRequest);
        ArgumentNullException.ThrowIfNull(streamUploadRequest.Stream);
        ArgumentException.ThrowIfNullOrEmpty(streamUploadRequest.FileName);

        try
        {
            logger.LogDebug("Starting file upload process for: {FileName}", streamUploadRequest.FileName);

            logger.LogDebug("Step 1: Getting upload URL from /files/upload_sessions");
            var uploadUrlResponse = await httpClient.PostAsync("/files/upload_sessions");

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

            uploadRequest.AddFile("file", () => streamUploadRequest.Stream, streamUploadRequest.FileName);

            var uploadResponse = await uploadClient.ExecuteAsync(uploadRequest);

            if (uploadResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(uploadResponse.Content))
            {
                if (logger.IsEnabled(LogLevel.Debug))                
                    logger.LogDebug("Upload response content: {Content}", uploadResponse.Content);                

                var result = JsonSerializer.Deserialize<FileUpload>(uploadResponse.Content);
                logger.LogDebug("File upload completed successfully. Parsed URL: {Url}", result?.Url);

                return result ?? new();
            }

            logger.LogWarning("File upload failed with status: {StatusCode} - {Content}", uploadResponse.StatusCode, uploadResponse.Content);
            throw new InvalidOperationException($"File upload failed: {uploadResponse.ErrorMessage}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file stream: {FileName}", streamUploadRequest.FileName);
            throw;
        }
    }
}