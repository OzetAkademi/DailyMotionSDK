using DailymotionSDK.Configuration;
using DailymotionSDK.Models;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// HTTP client service for DailyMotion API using RestSharp
/// </summary>
public class DailymotionHttpClient : IDailymotionHttpClient
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<DailymotionHttpClient> _logger;
    /// <summary>
    /// The client
    /// </summary>
    private RestClient RestClient { get; set; }
    /// <summary>
    /// The access token
    /// </summary>
    private string? AccessToken { get; set; }
    /// <summary>
    /// The current API key type
    /// </summary>
    private ApiKeyType ApiKeyType { get; set; }

    /// <summary>
    /// Gets the RestClient instance
    /// </summary>
    /// <value>The client.</value>
    public RestClient Client => RestClient;

    /// <summary>
    /// Gets the configuration options
    /// </summary>
    /// <value>The options.</value>
    public DailymotionOptions Options { get; }

    /// <summary>
    /// Initializes a new instance of the DailymotionHttpClient
    /// </summary>
    /// <param name="options">Configuration options</param>
    /// <param name="logger">Logger instance</param>
    /// <exception cref="System.ArgumentNullException">options</exception>
    /// <exception cref="System.ArgumentNullException">logger</exception>
    public DailymotionHttpClient(DailymotionOptions options, ILogger<DailymotionHttpClient> logger)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Initialize with the base URL from options (this will be updated by SetApiKeyType if needed)
        var clientOptions = new RestClientOptions(options.ApiBaseUrl)
        {
            Timeout = options.Timeout,
            ThrowOnDeserializationError = true,
            ThrowOnAnyError = false
        };

        RestClient = new RestClient(clientOptions);

        // Set default headers
        RestClient.AddDefaultHeader("User-Agent", options.UserAgent);
        RestClient.AddDefaultHeader("Accept", "application/json");
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");

        // Set the API key type based on configuration - this will update the base URL accordingly
        SetApiKeyType(options.ApiKeyType);
    }

    /// <summary>
    /// Sets the access token for authenticated requests
    /// </summary>
    /// <param name="accessToken">The OAuth access token</param>
    public void SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
        // Note: We'll set the Authorization header on each request instead of as a default header
    }

    /// <summary>
    /// Clears the access token
    /// </summary>
    public void ClearAccessToken()
    {
        AccessToken = null;
    }

    /// <summary>
    /// Gets the current access token
    /// </summary>
    /// <returns>Current access token or null if not set</returns>
    public string? GetAccessToken()
    {
        return AccessToken;
    }

    /// <summary>
    /// Checks if the current authentication is using client credentials (no user context)
    /// This is determined by checking if we have a token but no refresh token
    /// </summary>
    /// <returns>True if using client credentials authentication</returns>
    public bool IsUsingClientCredentials()
    {
        // Client credentials flow doesn't provide refresh tokens
        // We can also check the token payload for the 'sub' field which indicates user context
        if (string.IsNullOrEmpty(AccessToken))
            return false;

        try
        {
            // Parse JWT token to check for user context
            var parts = AccessToken.Split('.');
            if (parts.Length != 3) return false;

            // Decode the payload (second part)
            var payload = parts[1];
            // Add padding if needed
            while (payload.Length % 4 != 0)
                payload += "=";

            var payloadBytes = Convert.FromBase64String(payload);
            var payloadJson = System.Text.Encoding.UTF8.GetString(payloadBytes);

            // Parse JSON to check for user context indicators
            var payloadObj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);

            // Client credentials tokens typically don't have 'sub' (subject) field
            // or have 'sub' but it represents the application, not a user
            // We can also check for 'oid' (organization ID) which is present in client credentials
            return payloadObj != null &&
                   (!payloadObj.ContainsKey("sub") ||
                    (payloadObj.ContainsKey("oid") && payloadObj.ContainsKey("our")));
        }
        catch
        {
            // If we can't parse the token, assume it's not client credentials
            return false;
        }
    }

    /// <summary>
    /// Checks if a resource path contains /me endpoints that require user context
    /// </summary>
    /// <param name="resource">The resource path to check</param>
    /// <returns>True if the path contains /me endpoints</returns>
    private bool IsMeEndpoint(string resource)
    {
        if (string.IsNullOrEmpty(resource))
            return false;

        // Normalize the resource path
        var normalizedResource = resource.TrimStart('/').ToLowerInvariant();

        // Check for /me endpoints
        return normalizedResource.StartsWith("me/") ||
               normalizedResource == "me" ||
               normalizedResource.Contains("/me/") ||
               normalizedResource.EndsWith("/me");
    }

    /// <summary>
    /// Validates that the request is compatible with the current authentication type
    /// </summary>
    /// <param name="resource">The resource path</param>
    /// <param name="method">The HTTP method</param>
    /// <returns>True if the request should proceed, false if it should be blocked</returns>
    private bool ValidateRequestCompatibility(string resource, string method)
    {
        // Check if this is a /me endpoint
        if (IsMeEndpoint(resource))
        {
            // Check if we're using client credentials (no user context)
            if (IsUsingClientCredentials())
            {
                _logger.LogWarning("🚫 BLOCKED: {Method} request to '{Resource}' - /me endpoints require user authentication context\n💡 Client Credentials authentication provides application-level access, not user-level access\n💡 Use Password Grant or Authorization Code Grant for user-specific operations\n💡 Alternative: Use specific user ID endpoints instead of /me (e.g., /user/{{userId}} instead of /me)", method, resource);
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Sets the API key type and updates the base URL accordingly
    /// </summary>
    /// <param name="apiKeyType">The API key type</param>
    public void SetApiKeyType(ApiKeyType apiKeyType)
    {
        ApiKeyType = apiKeyType;

        // Update the base URL based on API key type
        var baseUrl = apiKeyType == ApiKeyType.Private
            ? "https://partner.api.dailymotion.com/rest"
            : "https://api.dailymotion.com";

        // Create a new RestClient with the updated base URL
        var clientOptions = new RestClientOptions(baseUrl)
        {
            Timeout = Options.Timeout,
            ThrowOnDeserializationError = true,
            ThrowOnAnyError = false
        };

        // Dispose the old client and create a new one
        RestClient.Dispose();
        var newClient = new RestClient(clientOptions);

        // Copy the default headers from the old client
        newClient.AddDefaultHeader("User-Agent", Options.UserAgent);
        newClient.AddDefaultHeader("Accept", "application/json");
        newClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");

        // Replace the client reference
        RestClient = newClient;

        _logger.LogDebug("Updated base URL to {BaseUrl} for {ApiKeyType} API key", baseUrl, apiKeyType);
    }

    /// <summary>
    /// Executes a GET request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional query parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> GetAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate request compatibility with current authentication type
            if (!ValidateRequestCompatibility(resource, "GET"))
            {
                // Return a mock response indicating the request was blocked
                return new RestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = "{\"error\":{\"code\":403,\"message\":\"Request blocked: /me endpoints require user authentication context. Use Password Grant or Authorization Code Grant instead of Client Credentials.\",\"type\":\"authentication_incompatible\"}}",
                    IsSuccessStatusCode = false
                };
            }

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            // Merge client default global params first, then per-call overrides
            var mergedParams = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());

            // If Options has a DefaultGlobalApiParameters (optional), merge it:
            if (Options.DefaultGlobalApiParameters != null)
            {
                foreach (var kv in Options.DefaultGlobalApiParameters.ToDictionary())
                {
                    if (!mergedParams.ContainsKey(kv.Key))
                        mergedParams[kv.Key] = kv.Value;
                }
            }

            // Per-call overrides should take precedence
            if (globalParams != null)
            {
                foreach (var kv in globalParams.ToDictionary())
                {
                    mergedParams[kv.Key] = kv.Value;
                }
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.QueryString);
                }
            }

            _logger.LogDebug("Making GET request to {Resource}", resource);
            _logger.LogDebug("Request parameters: {Parameters}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None");
            _logger.LogDebug("Authorization header present: {HasAuth}", !string.IsNullOrEmpty(AccessToken));

            var response = await RestClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("GET request completed with status {StatusCode}", response.StatusCode);
            _logger.LogDebug("Response content length: {ContentLength}", response.Content?.Length ?? 0);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("GET request failed with status {StatusCode}: {Content}\nRequest Details:\n  - Endpoint: {Endpoint}\n  - Method: GET\n  - Parameters: {Parameters}\n  - Authorization: {AuthStatus}\n  - User-Agent: {UserAgent}\n  - Response Headers: {Headers}", response.StatusCode, response.Content, $"{RestClient.Options.BaseUrl}{cleanResource}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None", !string.IsNullOrEmpty(AccessToken) ? $"Bearer {AccessToken}..." : "None", Options.UserAgent, response.Headers != null ? string.Join(", ", response.Headers.Select(h => $"{h.Name}={h.Value}")) : "None");
            }
            else
            {
                _logger.LogDebug("GET request successful. Response preview: {Preview}",
                    response.Content?.Length > 200 ? response.Content[..200] + "..." : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making GET request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Executes a GET request using the public API base URL (https://api.dailymotion.com)
    /// regardless of the current API key type
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional query parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> GetPublicAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a temporary client with the public API base URL
            var publicClientOptions = new RestClientOptions("https://api.dailymotion.com")
            {
                Timeout = Options.Timeout,
                ThrowOnDeserializationError = true,
                ThrowOnAnyError = false
            };

            using var publicClient = new RestClient(publicClientOptions);

            // Set the same default headers as the main client
            publicClient.AddDefaultHeader("User-Agent", Options.UserAgent);
            publicClient.AddDefaultHeader("Accept", "application/json");
            publicClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            // Merge client default global params first, then per-call overrides
            var mergedParams = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());

            // If Options has a DefaultGlobalApiParameters (optional), merge it:
            if (Options.DefaultGlobalApiParameters != null)
            {
                foreach (var kv in Options.DefaultGlobalApiParameters.ToDictionary())
                {
                    if (!mergedParams.ContainsKey(kv.Key))
                        mergedParams[kv.Key] = kv.Value;
                }
            }

            // Per-call overrides should take precedence
            if (globalParams != null)
            {
                foreach (var kv in globalParams.ToDictionary())
                {
                    mergedParams[kv.Key] = kv.Value;
                }
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.QueryString);
                }
            }

            _logger.LogDebug("Making GET request to public API: {Resource}", resource);
            _logger.LogDebug("Request parameters: {Parameters}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None");
            _logger.LogDebug("Authorization header present: {HasAuth}", !string.IsNullOrEmpty(AccessToken));

            var response = await publicClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("GET request to public API completed with status {StatusCode}", response.StatusCode);
            _logger.LogDebug("Response content length: {ContentLength}", response.Content?.Length ?? 0);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "GET request to public API failed with status {StatusCode}: {Content}\n" +
                    "Request Details:\n" +
                    "  - Endpoint: https://api.dailymotion.com/{Resource}\n" +
                    "  - Method: GET\n" +
                    "  - Parameters: {Parameters}\n" +
                    "  - Authorization: {AuthStatus}\n" +
                    "  - User-Agent: {UserAgent}\n" +
                    "  - Response Headers: {Headers}",
                    response.StatusCode,
                    response.Content,
                    cleanResource,
                    parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None",
                    !string.IsNullOrEmpty(AccessToken) ? $"Bearer {AccessToken}..." : "None",
                    Options.UserAgent,
                    response.Headers != null ? string.Join(", ", response.Headers.Select(h => $"{h.Name}={h.Value}")) : "None"
                );
            }
            else
            {
                _logger.LogDebug("GET request to public API successful. Response preview: {Preview}",
                    response.Content?.Length > 200 ? response.Content[..200] + "..." : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making GET request to public API: {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Executes a POST request using the public API base URL (https://api.dailymotion.com)
    /// regardless of the current API key type
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional form parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> PostPublicAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a temporary client with the public API base URL
            var publicClientOptions = new RestClientOptions("https://api.dailymotion.com")
            {
                Timeout = Options.Timeout,
                ThrowOnDeserializationError = true,
                ThrowOnAnyError = false
            };

            using var publicClient = new RestClient(publicClientOptions);

            // Set the same default headers as the main client
            publicClient.AddDefaultHeader("User-Agent", Options.UserAgent);
            publicClient.AddDefaultHeader("Accept", "application/json");
            publicClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource, Method.Post);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            // Merge client default global params first, then per-call overrides
            var mergedParams = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());

            // If Options has a DefaultGlobalApiParameters (optional), merge it:
            if (Options.DefaultGlobalApiParameters != null)
            {
                foreach (var kv in Options.DefaultGlobalApiParameters.ToDictionary())
                {
                    if (!mergedParams.ContainsKey(kv.Key))
                        mergedParams[kv.Key] = kv.Value;
                }
            }

            // Per-call overrides should take precedence
            if (globalParams != null)
            {
                foreach (var kv in globalParams.ToDictionary())
                {
                    mergedParams[kv.Key] = kv.Value;
                }
            }

            // Add all parameters to request
            foreach (var param in mergedParams)
            {
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
                }
            }

            _logger.LogDebug("Making POST request to public API: {Resource}", resource);
            _logger.LogDebug("Request parameters: {Parameters}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None");
            _logger.LogDebug("Authorization header present: {HasAuth}", !string.IsNullOrEmpty(AccessToken));

            var response = await publicClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("POST request to public API completed with status {StatusCode}", response.StatusCode);
            _logger.LogDebug("Response content length: {ContentLength}", response.Content?.Length ?? 0);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("POST request to public API failed with status {StatusCode}: {Content}\nRequest Details:\n  - Endpoint: https://api.dailymotion.com/{Resource}\n  - Method: POST\n  - Parameters: {Parameters}\n  - Authorization: {AuthStatus}\n  - User-Agent: {UserAgent}\n  - Response Headers: {Headers}", response.StatusCode, response.Content, cleanResource, parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None", !string.IsNullOrEmpty(AccessToken) ? $"Bearer {AccessToken}..." : "None", Options.UserAgent, response.Headers != null ? string.Join(", ", response.Headers.Select(h => $"{h.Name}={h.Value}")) : "None");
            }
            else
            {
                _logger.LogDebug("POST request to public API successful. Response preview: {Preview}",
                    response.Content?.Length > 200 ? response.Content[..200] + "..." : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making POST request to public API: {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Executes a POST request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="parameters">Optional form parameters</param>
    /// <param name="globalParams">The global parameters.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> PostAsync(string resource, Dictionary<string, string>? parameters = null, GlobalApiParameters? globalParams = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate request compatibility with current authentication type
            if (!ValidateRequestCompatibility(resource, "POST"))
            {
                // Return a mock response indicating the request was blocked
                return new RestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = "{\"error\":{\"code\":403,\"message\":\"Request blocked: /me endpoints require user authentication context. Use Password Grant or Authorization Code Grant instead of Client Credentials.\",\"type\":\"authentication_incompatible\"}}",
                    IsSuccessStatusCode = false
                };
            }

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource, Method.Post);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            // Merge client default global params first, then per-call overrides
            var mergedParams = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());

            // If Options has a DefaultGlobalApiParameters (optional), merge it:
            if (Options.DefaultGlobalApiParameters != null)
            {
                foreach (var kv in Options.DefaultGlobalApiParameters.ToDictionary())
                {
                    if (!mergedParams.ContainsKey(kv.Key))
                        mergedParams[kv.Key] = kv.Value;
                }
            }

            // Per-call overrides should take precedence
            if (globalParams != null)
            {
                foreach (var kv in globalParams.ToDictionary())
                {
                    mergedParams[kv.Key] = kv.Value;
                }
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
                }
            }

            _logger.LogDebug("Making POST request to {Resource}", resource);
            _logger.LogDebug("Request parameters: {Parameters}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None");
            _logger.LogDebug("Authorization header present: {HasAuth}", !string.IsNullOrEmpty(AccessToken));

            var response = await RestClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("POST request completed with status {StatusCode}", response.StatusCode);
            _logger.LogDebug("Response content length: {ContentLength}", response.Content?.Length ?? 0);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("POST request failed with status {StatusCode}: {Content}\nRequest Details:\n  - Endpoint: {Endpoint}\n  - Method: POST\n  - Parameters: {Parameters}\n  - Authorization: {AuthStatus}\n  - User-Agent: {UserAgent}\n  - Response Headers: {Headers}", response.StatusCode, response.Content, $"{RestClient.Options.BaseUrl}{cleanResource}", parameters != null ? string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")) : "None", !string.IsNullOrEmpty(AccessToken) ? $"Bearer {AccessToken}..." : "None", Options.UserAgent, response.Headers != null ? string.Join(", ", response.Headers.Select(h => $"{h.Name}={h.Value}")) : "None");
            }
            else
            {
                _logger.LogDebug("POST request successful. Response preview: {Preview}",
                    response.Content?.Length > 200 ? response.Content[..200] + "..." : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making POST request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Executes a DELETE request
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> DeleteAsync(string resource, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate request compatibility with current authentication type
            if (!ValidateRequestCompatibility(resource, "DELETE"))
            {
                // Return a mock response indicating the request was blocked
                return new RestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = "{\"error\":{\"code\":403,\"message\":\"Request blocked: /me endpoints require user authentication context. Use Password Grant or Authorization Code Grant instead of Client Credentials.\",\"type\":\"authentication_incompatible\"}}",
                    IsSuccessStatusCode = false
                };
            }

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource, Method.Delete);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            _logger.LogDebug("Making DELETE request to {Resource}", resource);
            _logger.LogDebug("Authorization header present: {HasAuth}", !string.IsNullOrEmpty(AccessToken));

            var response = await RestClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("DELETE request completed with status {StatusCode}", response.StatusCode);
            _logger.LogDebug("Response content length: {ContentLength}", response.Content?.Length ?? 0);

            // For DELETE operations, empty response is often success
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("DELETE request failed with status {StatusCode}: {Content}\nRequest Details:\n  - Endpoint: {Endpoint}\n  - Method: DELETE\n  - Authorization: {AuthStatus}\n  - User-Agent: {UserAgent}\n  - Response Headers: {Headers}", response.StatusCode, response.Content, $"{RestClient.Options.BaseUrl}{cleanResource}", !string.IsNullOrEmpty(AccessToken) ? $"Bearer {AccessToken}..." : "None", Options.UserAgent, response.Headers != null ? string.Join(", ", response.Headers.Select(h => $"{h.Name}={h.Value}")) : "None");
            }
            else
            {
                _logger.LogDebug("DELETE request successful. Response: {Content}",
                    string.IsNullOrEmpty(response.Content) ? "Empty (success)" : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making DELETE request to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Executes a file upload request with multipart form data
    /// </summary>
    /// <param name="resource">The API resource path</param>
    /// <param name="fileStream">File stream to upload</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="parameters">Optional additional parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RestResponse</returns>
    public async Task<RestResponse> UploadFileAsync(string resource, Stream fileStream, string fileName, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate request compatibility with current authentication type
            if (!ValidateRequestCompatibility(resource, "POST (File Upload)"))
            {
                // Return a mock response indicating the request was blocked
                return new RestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = "{\"error\":{\"code\":403,\"message\":\"Request blocked: /me endpoints require user authentication context. Use Password Grant or Authorization Code Grant instead of Client Credentials.\",\"type\":\"authentication_incompatible\"}}",
                    IsSuccessStatusCode = false
                };
            }

            // Remove leading slash to avoid double slash in URL
            var cleanResource = resource.StartsWith("/") ? resource.Substring(1) : resource;
            var request = new RestRequest(cleanResource, Method.Post);

            // Add Authorization header if we have an access token
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            // Add the file as multipart form data
            request.AddFile("file", () => fileStream, fileName);

            // Add additional parameters if provided
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.RequestBody);
                }
            }

            _logger.LogDebug("Making file upload request to {Resource} for file {FileName}", resource, fileName);
            var response = await RestClient.ExecuteAsync(request, cancellationToken);
            _logger.LogDebug("File upload request completed with status {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("File upload failed with status {StatusCode}: {Content}", response.StatusCode, response.Content);
            }
            else
            {
                _logger.LogDebug("File upload successful. Response preview: {Preview}",
                    response.Content?.Length > 200 ? response.Content[..200] + "..." : response.Content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to {Resource}", resource);
            throw;
        }
    }

    /// <summary>
    /// Disposes the HTTP client
    /// </summary>
    public void Dispose()
    {
        RestClient.Dispose();
    }
}