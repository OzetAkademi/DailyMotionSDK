using System.Text.Json;
using System.Text.Json.Serialization;
using DailymotionSDK.Models;
using DailymotionSDK.Exceptions;
using DailymotionSDK.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DailymotionSDK.Services;

/// <summary>
/// Authentication service for DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#auth
/// </summary>
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
    /// The refresh token
    /// </summary>
    private string? _refreshToken;

    /// <summary>
    /// The token expiry
    /// </summary>
    private DateTime _tokenExpiry;

    /// <summary>
    /// The current API key type used for authentication
    /// </summary>
    private ApiKeyType _currentApiKeyType;

    /// <summary>
    /// Gets the current access token
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken => _accessToken;

    /// <summary>
    /// Gets the current refresh token
    /// </summary>
    /// <value>The refresh token.</value>
    public string? RefreshToken => _refreshToken;

    /// <summary>
    /// Gets whether the current token is expired
    /// </summary>
    /// <value><c>true</c> if this instance is token expired; otherwise, <c>false</c>.</value>
    public bool IsTokenExpired => DateTime.UtcNow >= _tokenExpiry;

    /// <summary>
    /// Gets the current API key type used for authentication
    /// </summary>
    /// <value>The current API key type.</value>
    public ApiKeyType CurrentApiKeyType => _currentApiKeyType;

    /// <summary>
    /// Initializes a new instance of the DailymotionAuthService
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger instance</param>
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
    /// Authenticates using client credentials grant type
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#client-credentials
    /// </summary>
    public async Task<TokenResponse> AuthenticateWithClientCredentialsAsync(string apiKey, string apiSecret, ApiKeyType apiKeyType, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
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

            var oauthEndpoint = apiKeyType == ApiKeyType.Private
                ? "https://partner.api.dailymotion.com/oauth/v1/token"
                : "https://api.dailymotion.com/oauth/token";

            _logger.LogInformation("Making OAuth request to: {OAuthEndpoint} for {ApiKeyType} API key", oauthEndpoint, apiKeyType);

            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

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
            _refreshToken = tokenResponse.RefreshToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            _currentApiKeyType = apiKeyType;

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
                _httpClient.SetApiKeyType(apiKeyType);
            }

            _logger.LogInformation("Successfully authenticated with client credentials using {ApiKeyType} API key", apiKeyType);
            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error authenticating with client credentials");
            throw;
        }
    }

    /// <summary>
    /// Authenticates with username and password using OAuth 2.0 password grant (for Public API keys only)
    /// </summary>
    public async Task<TokenResponse> AuthenticateWithPasswordAsync(string username, string password, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        try
        {
            _logger.LogInformation("Authenticating with password for user {Username}", username);

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["username"] = username,
                ["password"] = password,
                ["client_id"] = _httpClient.Options.PublicApiKey,
                ["client_secret"] = _httpClient.Options.PublicApiSecret
            };

            if (scopes != null && scopes.Length > 0)
            {
                parameters["scope"] = string.Join(" ", scopes.Select(s => s.ToApiScopeString()));
            }

            var oauthEndpoint = "https://api.dailymotion.com/oauth/token";
            _logger.LogInformation("Making OAuth request to: {OAuthEndpoint}", oauthEndpoint);

            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

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
                _logger.LogError("Failed to authenticate with password: {Error}", response.ErrorMessage);
                var errorResponse = JsonSerializer.Deserialize<ErrorData>(response.Content!, _jsonOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    throw new DailymotionException(errorResponse.ErrorDescription, (int)response.StatusCode);
                }
                throw new DailymotionException("Authentication failed", (int)response.StatusCode);
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content!, _jsonOptions)
                ?? throw new DailymotionException("Failed to deserialize token response", (int)response.StatusCode);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
            }

            _logger.LogInformation("Successfully authenticated user {Username}", username);
            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error authenticating with password for user {Username}", username);
            throw;
        }
    }

    /// <summary>
    /// Authenticates with username and password using OAuth 2.0 password grant with specific API keys
    /// </summary>
    public async Task<TokenResponse> AuthenticateWithPasswordAsync(string username, string password, string apiKey, string apiSecret, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API Key cannot be null or empty", nameof(apiKey));
        if (string.IsNullOrWhiteSpace(apiSecret))
            throw new ArgumentException("API Secret cannot be null or empty", nameof(apiSecret));

        try
        {
            _logger.LogInformation("Authenticating with password for user {Username} using provided API keys", username);

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["username"] = username,
                ["password"] = password,
                ["client_id"] = apiKey,
                ["client_secret"] = apiSecret
            };

            if (scopes != null && scopes.Length > 0)
            {
                parameters["scope"] = string.Join(" ", scopes.Select(s => s.ToApiScopeString()));
            }

            var oauthEndpoint = "https://api.dailymotion.com/oauth/token";
            _logger.LogInformation("Making OAuth request to: {OAuthEndpoint}", oauthEndpoint);

            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

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
                _logger.LogError("Failed to authenticate with password: {Error}", response.ErrorMessage);
                var errorResponse = JsonSerializer.Deserialize<ErrorData>(response.Content!, _jsonOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    throw new DailymotionException(errorResponse.ErrorDescription, (int)response.StatusCode);
                }
                throw new DailymotionException("Authentication failed", (int)response.StatusCode);
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content!, _jsonOptions)
                ?? throw new DailymotionException("Failed to deserialize token response", (int)response.StatusCode);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            _currentApiKeyType = ApiKeyType.Public;

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
                _httpClient.SetApiKeyType(ApiKeyType.Public);
            }

            _logger.LogInformation("Successfully authenticated user {Username} with provided API keys", username);
            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error authenticating with password for user {Username}", username);
            throw;
        }
    }

    /// <summary>
    /// Exchanges authorization code for access token (for Public API keys only)
    /// </summary>
    public async Task<TokenResponse> ExchangeCodeForTokenAsync(string authorizationCode, string redirectUri, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(authorizationCode))
            throw new ArgumentException("Authorization code cannot be null or empty", nameof(authorizationCode));
        if (string.IsNullOrWhiteSpace(redirectUri))
            throw new ArgumentException("Redirect URI cannot be null or empty", nameof(redirectUri));

        try
        {
            _logger.LogDebug("Exchanging authorization code for token");

            var (clientId, clientSecret) = _currentApiKeyType == ApiKeyType.Private
                ? (_httpClient.Options.PrivateApiKey, _httpClient.Options.PrivateApiSecret)
                : (_httpClient.Options.PublicApiKey, _httpClient.Options.PublicApiSecret);

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = authorizationCode,
                ["redirect_uri"] = redirectUri,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            var oauthEndpoint = _currentApiKeyType == ApiKeyType.Private
                ? "https://partner.api.dailymotion.com/oauth/v1/token"
                : "https://api.dailymotion.com/oauth/token";
            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
            }

            var response = await oauthClient.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to exchange code for token: {Error}", response.ErrorMessage);
                var errorResponse = JsonSerializer.Deserialize<ErrorData>(response.Content!, _jsonOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    throw new DailymotionException(errorResponse.ErrorDescription, (int)response.StatusCode);
                }
                throw new DailymotionException("Token exchange failed", (int)response.StatusCode);
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content!, _jsonOptions)
                ?? throw new DailymotionException("Failed to deserialize token response", (int)response.StatusCode);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
            }

            _logger.LogInformation("Successfully exchanged authorization code for token");
            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error exchanging authorization code for token");
            throw;
        }
    }

    /// <summary>
    /// Refreshes the access token
    /// </summary>
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

        try
        {
            _logger.LogDebug("Refreshing access token");

            var (clientId, clientSecret) = _currentApiKeyType == ApiKeyType.Private
                ? (_httpClient.Options.PrivateApiKey, _httpClient.Options.PrivateApiSecret)
                : (_httpClient.Options.PublicApiKey, _httpClient.Options.PublicApiSecret);

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            var oauthEndpoint = _currentApiKeyType == ApiKeyType.Private
                ? "https://partner.api.dailymotion.com/oauth/v1/token"
                : "https://api.dailymotion.com/oauth/token";
            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
            }

            var response = await oauthClient.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to refresh token: {Error}", response.ErrorMessage);
                var errorResponse = JsonSerializer.Deserialize<ErrorData>(response.Content!, _jsonOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    throw new DailymotionException(errorResponse.ErrorDescription, (int)response.StatusCode);
                }
                throw new DailymotionException("Token refresh failed", (int)response.StatusCode);
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content!, _jsonOptions)
                ?? throw new DailymotionException("Failed to deserialize token response", (int)response.StatusCode);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken ?? refreshToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.SetAccessToken(_accessToken);
            }

            _logger.LogInformation("Successfully refreshed access token");
            return tokenResponse;
        }
        catch (Exception ex) when (ex is not DailymotionException)
        {
            _logger.LogError(ex, "Error refreshing access token");
            throw;
        }
    }

    /// <summary>
    /// Revokes the access token
    /// </summary>
    public async Task<bool> RevokeTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));

        try
        {
            _logger.LogDebug("Revoking access token");

            var (clientId, clientSecret) = _currentApiKeyType == ApiKeyType.Private
                ? (_httpClient.Options.PrivateApiKey, _httpClient.Options.PrivateApiSecret)
                : (_httpClient.Options.PublicApiKey, _httpClient.Options.PublicApiSecret);

            var parameters = new Dictionary<string, string>
            {
                ["token"] = accessToken,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            var oauthEndpoint = _currentApiKeyType == ApiKeyType.Private
                ? "https://partner.api.dailymotion.com/oauth/v1/revoke"
                : "https://api.dailymotion.com/oauth/revoke";
            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);
            }

            var response = await oauthClient.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to revoke token: {Error}", response.ErrorMessage);
                return false;
            }

            if (accessToken == _accessToken)
            {
                _accessToken = null;
                _refreshToken = null;
                _tokenExpiry = DateTime.MinValue;
                _httpClient.ClearAccessToken();
            }

            _logger.LogInformation("Successfully revoked access token");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking access token");
            throw;
        }
    }

    /// <summary>
    /// Validates the current access token
    /// </summary>
    public async Task<TokenValidationResponse?> ValidateTokenAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            _logger.LogWarning("No access token available for validation");
            return null;
        }

        try
        {
            _logger.LogDebug("Validating access token");

            var oauthEndpoint = _currentApiKeyType == ApiKeyType.Private
                ? "https://partner.api.dailymotion.com/oauth/v1/token/info"
                : "https://api.dailymotion.com/oauth/token/info";
            var oauthClientOptions = new RestClientOptions(oauthEndpoint);
            using var oauthClient = new RestClient(oauthClientOptions);
            oauthClient.AddDefaultHeader("User-Agent", _httpClient.Options.UserAgent);
            oauthClient.AddDefaultHeader("Accept", "application/json");
            oauthClient.AddDefaultHeader("Authorization", $"Bearer {_accessToken}");

            var request = new RestRequest("", Method.Get);
            var response = await oauthClient.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to validate token: {Error}", response.ErrorMessage);
                return null;
            }

            var validationResponse = JsonSerializer.Deserialize<TokenValidationResponse>(response.Content!, _jsonOptions);
            _logger.LogInformation("Token validation completed");
            return validationResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating access token");
            throw;
        }
    }

    /// <summary>
    /// Clears all stored tokens
    /// </summary>
    public void ClearTokens()
    {
        _accessToken = null;
        _refreshToken = null;
        _tokenExpiry = DateTime.MinValue;
        _currentApiKeyType = ApiKeyType.Public;
        _httpClient.ClearAccessToken();
        _logger.LogDebug("Cleared all stored tokens");
    }
}