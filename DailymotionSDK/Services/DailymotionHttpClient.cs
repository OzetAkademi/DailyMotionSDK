using DailymotionSDK.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// Class DailymotionHttpClient.
/// Implements the <see cref="DailymotionSDK.Services.IDailymotionHttpClient" />
/// Implements the <see cref="System.IDisposable" />
/// </summary>
/// <param name="options">The options.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Services.IDailymotionHttpClient" />
/// <seealso cref="System.IDisposable" />
public class DailymotionHttpClient(DailymotionOptions options, ILogger<DailymotionHttpClient> logger) : IDailymotionHttpClient, IDisposable
{
    /// <summary>
    /// Gets the options.
    /// </summary>
    /// <value>The options.</value>
    public DailymotionOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    public RestClient Client { get; } = new(new RestClientOptions(options.ApiBaseUrl)
    {
        Timeout = options.Timeout,
        ThrowOnDeserializationError = true,
        ThrowOnAnyError = false
    });

    /// <summary>
    /// The access token
    /// </summary>
    private string? _accessToken;

    /// <summary>
    /// Sets the access token.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    public void SetAccessToken(string accessToken) => _accessToken = accessToken;

    /// <summary>
    /// Clears the access token.
    /// </summary>
    public void ClearAccessToken() => _accessToken = null;

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <returns>System.Nullable{System.String}.</returns>
    public string? GetAccessToken() => _accessToken;

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    public async Task<RestResponse> GetAsync(string resource, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = CreateAuthorizedRequest(resource, Method.Get);

            // 3. Modern Dictionary Deconstruction
            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    request.AddQueryParameter(key, value);
                }
            }

            logger.LogDebug("Making GET request to {Resource}. Params: {Params}", resource, parameters?.Count ?? 0);
            return await ExecuteWithLoggingAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making GET request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    public async Task<RestResponse> PostAsync(string resource, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = CreateAuthorizedRequest(resource, Method.Post);

            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    request.AddParameter(key, value, ParameterType.GetOrPost);
                }
            }

            logger.LogDebug("Making POST request to {Resource}. Params: {Params}", resource, parameters?.Count ?? 0);
            return await ExecuteWithLoggingAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making POST request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Posts the json asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    public async Task<RestResponse> PostJsonAsync<T>(string resource, T payload, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var request = CreateAuthorizedRequest(resource, Method.Post);

            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            logger.LogDebug("Making JSON POST request to {Resource}", resource);
            return await ExecuteWithLoggingAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making JSON POST request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    public async Task<RestResponse> DeleteAsync(string resource, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = CreateAuthorizedRequest(resource, Method.Delete);
            logger.LogDebug("Making DELETE request to {Resource}", resource);
            return await ExecuteWithLoggingAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making DELETE request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Uploads the file asynchronous.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="fileStream">The file stream.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    public async Task<RestResponse> UploadFileAsync(string resource, Stream fileStream, string fileName, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = CreateAuthorizedRequest(resource, Method.Post);

            request.AddFile("file", () => fileStream, fileName);

            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    request.AddParameter(key, value, ParameterType.RequestBody);
                }
            }

            logger.LogDebug("Making file upload request to {Resource} for file {FileName}", resource, fileName);
            return await ExecuteWithLoggingAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Client.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates the authorized request.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="method">The method.</param>
    /// <returns>RestRequest.</returns>
    private RestRequest CreateAuthorizedRequest(string resource, Method method)
    {
        var request = new RestRequest(resource.TrimStart('/'), method)
        {
            Timeout = Options.Timeout
        };

        if (!string.IsNullOrEmpty(_accessToken))
            request.AddHeader("Authorization", $"Bearer {_accessToken}");

        return request;
    }

    /// <summary>
    /// Execute with logging as an asynchronous operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;RestResponse&gt; representing the asynchronous operation.</returns>
    private async Task<RestResponse> ExecuteWithLoggingAsync(RestRequest request, CancellationToken cancellationToken)
    {
        var response = await Client.ExecuteAsync(request, cancellationToken);

        logger.LogDebug("{Method} request completed with status {StatusCode}", request.Method, response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("{Method} request failed with status {StatusCode}: {Content}\nEndpoint: {Endpoint}",
                request.Method, response.StatusCode, response.Content, $"{Client.Options.BaseUrl}{request.Resource}");
        }

        return response;
    }
}