using System.Text.Json;
using System.Text.Json.Serialization;
using DailymotionSDK.Models;
using DailymotionSDK.Exceptions;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// Class DailymotionAuthService.
/// Implements the <see cref="DailymotionSDK.Services.IDailymotionAuthService" />
/// </summary>
/// <seealso cref="DailymotionSDK.Services.IDailymotionAuthService" />
public class DailymotionAuthService : IDailymotionAuthService
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;

    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<DailymotionAuthService> _logger;

    /// <summary>
    /// The json options
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// The access token
    /// </summary>
    private string? _accessToken;

    /// <summary>
    /// The token expiry
    /// </summary>
    private DateTime _tokenExpiry;

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken => _accessToken;

    /// <summary>
    /// Gets a value indicating whether this instance is token expired.
    /// </summary>
    /// <value><c>true</c> if this instance is token expired; otherwise, <c>false</c>.</value>
    public bool IsTokenExpired => DateTime.UtcNow >= _tokenExpiry;

    /// <summary>
    /// Initializes a new instance of the <see cref="DailymotionAuthService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    /// <exception cref="System.ArgumentNullException">logger</exception>
    public DailymotionAuthService(IDailymotionHttpClient httpClient, ILogger<DailymotionAuthService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Replaces Newtonsoft's default case-insensitivity
        };
    }

    /// <summary>
    /// Authenticates the with client credentials asynchronous.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <param name="apiSecret">The API secret.</param>
    /// <param name="scopes">The scopes.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TokenResponse&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentException">API Key cannot be null or empty - apiKey</exception>
    /// <exception cref="System.ArgumentException">API Secret cannot be null or empty - apiSecret</exception>
    /// <exception cref="DailymotionSDK.Exceptions.DailymotionException"></exception>
    /// <exception cref="DailymotionSDK.Exceptions.DailymotionException">Client credentials authentication failed</exception>
    /// <exception cref="DailymotionSDK.Exceptions.DailymotionException">Failed to deserialize token response</exception>
    public async Task<TokenResponse> AuthenticateWithPrivateAsync(string apiKey, string apiSecret, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating with client credentials");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API Key cannot be null or empty", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(apiSecret))
                throw new ArgumentException("API Secret cannot be null or empty", nameof(apiSecret));

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = apiKey,
                ["client_secret"] = apiSecret
            };

            if (scopes != null && scopes.Length > 0)
            {
                parameters["scope"] = string.Join(" ", scopes.Select(s => s.ToApiScopeString()));
            }

            var oauthEndpoint = "https://oauth2.dailymotion.com/v2/token";

            _logger.LogInformation("Making OAuth request to: {OAuthEndpoint}", oauthEndpoint);

            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
            }

            var response = await oauthClient.ExecuteAsync(request, cancellationToken);
            _logger.LogInformation("OAuth response status: {StatusCode}, Content length: {ContentLength}", response.StatusCode, response.Content?.Length ?? 0);
            _logger.LogInformation("OAuth response content: {Content}", response.Content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to authenticate with client credentials: {Error}", response.ErrorMessage);
                var errorResponse = JsonSerializer.Deserialize<ErrorData>(response.Content!, _jsonOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    throw new DailymotionException(errorResponse.ErrorDescription, (int)response.StatusCode);
                }
                throw new DailymotionException("Client credentials authentication failed", (int)response.StatusCode);
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content!, _jsonOptions)
                ?? throw new DailymotionException("Failed to deserialize token response", (int)response.StatusCode);

            _accessToken = tokenResponse.AccessToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
            }

            _logger.LogInformation("Successfully authenticated with client credentials");

            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error authenticating with client credentials");
            throw;
        }
    }

    /// <summary>
    /// Clears the tokens.
    /// </summary>
    public void ClearTokens()
    {
        _accessToken = null;
        _tokenExpiry = DateTime.MinValue;
        _httpClient.ClearAccessToken();
        _logger.LogDebug("Cleared all stored tokens");
    }
}