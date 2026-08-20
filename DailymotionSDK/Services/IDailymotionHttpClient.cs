using DailymotionSDK.Configuration;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// Interface IDailymotionHttpClient
/// Extends the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IDailymotionHttpClient : IDisposable
{
    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    RestClient Client { get; }

    /// <summary>
    /// Gets the options.
    /// </summary>
    /// <value>The options.</value>
    DailymotionOptions Options { get; }

    /// <summary>
    /// Sets the access token.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    void SetAccessToken(string accessToken);

    /// <summary>
    /// Clears the access token.
    /// </summary>
    void ClearAccessToken();

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <returns>System.Nullable{System.String}.</returns>
    string? GetAccessToken();

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{RestResponse}.</returns>
    Task<RestResponse> GetAsync(string resource, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{RestResponse}.</returns>
    Task<RestResponse> PostAsync(string resource, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts the json asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{RestResponse}.</returns>
    Task<RestResponse> PostJsonAsync<T>(string resource, T payload, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{RestResponse}.</returns>
    Task<RestResponse> DeleteAsync(string resource, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads the file asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="fileStream">The file stream.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{RestResponse}.</returns>
    Task<RestResponse> UploadFileAsync(string resource, Stream fileStream, string fileName, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default);
}