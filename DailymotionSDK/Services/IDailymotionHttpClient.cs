using DailymotionSDK.Configuration;
using DailymotionSDK.Models;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// Interface for DailyMotion HTTP client service
/// </summary>
public interface IDailymotionHttpClient
{
    /// <summary>
    /// Gets the RestClient instance
    /// </summary>
    /// <value>The client.</value>
    RestClient Client { get; }

    /// <summary>
    /// Gets the configuration options
    /// </summary>
    /// <value>The options.</value>
    DailymotionOptions Options { get; }

    /// <summary>
    /// Sets the access token for authenticated requests
    /// </summary>
    /// <param name="accessToken">The OAuth access token</param>
    void SetAccessToken(string accessToken);

    /// <summary>
    /// Clears the access token
    /// </summary>
    void ClearAccessToken();

    /// <summary>
    /// Sets the API key type and updates the base URL accordingly
    /// </summary>
    /// <param name="apiKeyType">The API key type</param>
    void SetApiKeyType(ApiKeyType apiKeyType);

    /// <summary>
    /// Gets the current access token
    /// </summary>
    /// <returns>Current access token or null if not set</returns>
    string? GetAccessToken();

    /// <summary>
    /// Checks if the current authentication is using client credentials (no user context)
    /// </summary>
    /// <returns>True if using client credentials authentication</returns>
    bool IsUsingClientCredentials();

    /// <summary>
    /// Executes a GET request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional query parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> GetAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GET request using the public API base URL (https://api.dailymotion.com)
    /// regardless of the current API key type
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional query parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> GetPublicAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a POST request using the public API base URL (https://api.dailymotion.com)
    /// regardless of the current API key type
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional form parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> PostPublicAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a POST request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional form parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> PostAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a DELETE request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> DeleteAsync(string resource, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a file upload request with multipart form data
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="fileStream">File stream to upload</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="parameters">Optional additional parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    Task<RestResponse> UploadFileAsync(string resource, Stream fileStream, string fileName, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disposes the HTTP client
    /// </summary>
    void Dispose();
}