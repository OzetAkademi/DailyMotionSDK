using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for file upload operations
/// https://developers.dailymotion.com/api/platform-api/reference/#file
/// </summary>
public class FileClient : IFile
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<FileClient> _logger;
    /// <summary>
    /// The json settings
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the FileClient
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public FileClient(IDailymotionHttpClient httpClient, ILogger<FileClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Uploads a file to DailyMotion
    /// https://developers.dailymotion.com/api/platform-api/reference/#file-upload
    /// </summary>
    /// <param name="filePath">Path to the file to upload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File upload response</returns>
    /// <exception cref="ArgumentException">File path cannot be null or empty - filePath</exception>
    /// <exception cref="FileNotFoundException">File not found</exception>
    public async Task<FileUploadResponse> UploadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        try
        {
            _logger.LogDebug("Uploading file: {FilePath}", filePath);

            await using var fileStream = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);
            return await UploadAsync(fileStream, fileName, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Uploads a file from a stream to DailyMotion
    /// https://developers.dailymotion.com/api/platform-api/reference/#file-upload
    /// </summary>
    /// <param name="stream">File stream</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File upload response</returns>
    /// <exception cref="ArgumentNullException">stream</exception>
    /// <exception cref="ArgumentException">File name cannot be null or empty - fileName</exception>
    /// <exception cref="Exception">Failed to get upload URL: {uploadUrlResponse.ErrorMessage}</exception>
    /// <exception cref="Exception">Invalid upload URL response</exception>
    /// <exception cref="Exception">File upload failed: {uploadResponse.ErrorMessage}</exception>
    public async Task<FileUploadResponse> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

        try
        {
            _logger.LogDebug("Starting file upload process for: {FileName}", fileName);

            // Step 1: Get the upload URL by making a GET request to /file/upload
            _logger.LogDebug("Step 1: Getting upload URL from /file/upload");
            var uploadUrlResponse = await _httpClient.GetPublicAsync("/file/upload", null, null, cancellationToken);

            if (!uploadUrlResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get upload URL: {StatusCode} - {Content}", uploadUrlResponse.StatusCode, uploadUrlResponse.Content);
                throw new Exception($"Failed to get upload URL: {uploadUrlResponse.ErrorMessage}");
            }

            var uploadUrlData = JsonSerializer.Deserialize<UploadUrlResponse>(uploadUrlResponse.Content!, _jsonOptions);
            if (uploadUrlData == null || string.IsNullOrEmpty(uploadUrlData.UploadUrl))
            {
                _logger.LogError("Invalid upload URL response: {Content}", uploadUrlResponse.Content);
                throw new Exception("Invalid upload URL response");
            }

            _logger.LogDebug("Received upload URL: {UploadUrl}", uploadUrlData.UploadUrl);

            // Step 2: Upload the file to the provided URL
            _logger.LogDebug("Step 2: Uploading file to the provided URL");

            // Create a new HTTP client for the upload (since it's a different domain)
            var uploadClient = new RestClient();
            var uploadRequest = new RestRequest(uploadUrlData.UploadUrl, Method.Post);

            // Add the file as multipart form data
            uploadRequest.AddFile("file", () => stream, fileName);

            // Add Authorization header if we have an access token
            // Note: The upload URL might not require authorization, but we'll add it if available
            // This should be handled by the HTTP client's access token management

            var uploadResponse = await uploadClient.ExecuteAsync(uploadRequest, cancellationToken);

            if (uploadResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(uploadResponse.Content))
            {
                _logger.LogDebug("Upload response content: {Content}", uploadResponse.Content);
                var result = JsonSerializer.Deserialize<FileUploadResponse>(uploadResponse.Content, _jsonOptions);
                _logger.LogDebug("File upload completed successfully");
                _logger.LogDebug("Parsed result - URL: {Url}, ID: {Id}",
                    result?.Url, result?.Id);
                return result ?? new FileUploadResponse();
            }

            _logger.LogWarning("File upload failed with status: {StatusCode} - {Content}", uploadResponse.StatusCode, uploadResponse.Content);
            throw new Exception($"File upload failed: {uploadResponse.ErrorMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file stream: {FileName}", fileName);
            throw;
        }
    }

    /// <summary>
    /// Monitors the upload progress using the progress URL
    /// </summary>
    /// <param name="progressUrl">The progress URL from the upload response</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload progress information</returns>
    /// <exception cref="ArgumentException">Progress URL cannot be null or empty - progressUrl</exception>
    public async Task<UploadProgressResponse?> MonitorUploadProgressAsync(string progressUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(progressUrl))
            throw new ArgumentException("Progress URL cannot be null or empty", nameof(progressUrl));

        try
        {
            _logger.LogDebug("Monitoring upload progress: {ProgressUrl}", progressUrl);

            // Create a new HTTP client for the progress check (since it's a different domain)
            var progressClient = new RestClient();
            var progressRequest = new RestRequest(progressUrl);

            var progressResponse = await progressClient.ExecuteAsync(progressRequest, cancellationToken);

            if (progressResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(progressResponse.Content))
            {
                var result = JsonSerializer.Deserialize<UploadProgressResponse>(progressResponse.Content, _jsonOptions);
                _logger.LogDebug("Progress check completed: {Status}", result?.Status ?? "Unknown");
                return result;
            }

            _logger.LogWarning("Progress check failed with status: {StatusCode} - {Content}", progressResponse.StatusCode, progressResponse.Content);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error monitoring upload progress: {ProgressUrl}", progressUrl);
            throw;
        }
    }
}
