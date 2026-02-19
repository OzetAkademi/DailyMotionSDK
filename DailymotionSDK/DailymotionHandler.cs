using DailymotionSDK.Configuration;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using DailymotionSDK.Interfaces;
using DailymotionSDK.Internal;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK;

/// <summary>
/// Main DailyMotion SDK client
/// Provides a unified interface for all DailyMotion Platform API operations
/// https://developers.dailymotion.com/api/platform-api/reference/
/// </summary>
public class DailymotionHandler : IDisposable
{
    /// <summary>
    /// The authentication service
    /// </summary>
    private readonly IDailymotionAuthService _authService;
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<DailymotionHandler>? _logger;
    /// <summary>
    /// The options
    /// </summary>
    private readonly DailymotionOptions _options;
    /// <summary>
    /// The internal client manager
    /// </summary>
    private readonly ClientManager _clientManager;

    /// <summary>
    /// Initializes a new instance of the DailyMotion SDK
    /// </summary>
    /// <param name="options">Configuration options</param>
    /// <param name="httpClient">HTTP client instance</param>
    /// <param name="authService">Authentication service instance</param>
    /// <param name="loggerFactory">Logger factory instance</param>
    /// <exception cref="System.ArgumentNullException">options</exception>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    /// <exception cref="System.ArgumentNullException">authService</exception>
    /// <exception cref="System.ArgumentNullException">loggerFactory</exception>
    public DailymotionHandler(
        DailymotionOptions options,
        IDailymotionHttpClient httpClient,
        IDailymotionAuthService authService,
        ILoggerFactory loggerFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = loggerFactory?.CreateLogger<DailymotionHandler>();
        
        _clientManager = new ClientManager(_options, _httpClient, _authService, loggerFactory!);

        _logger?.LogInformation("DailyMotion SDK initialized");
    }


    /// <summary>
    /// Gets the authentication service
    /// </summary>
    /// <value>The authentication.</value>
    public IDailymotionAuthService Auth => _authService;

    /// <summary>
    /// Gets the HTTP client
    /// </summary>
    /// <value>The HTTP client.</value>
    public IDailymotionHttpClient HttpClient => _httpClient;

    /// <summary>
    /// Gets the configuration options
    /// </summary>
    /// <value>The options.</value>
    public DailymotionOptions Options => _options;

    /// <summary>
    /// Gets the videos client for video-related operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <value>The videos.</value>
    public IVideos Videos => _clientManager.Videos;

    /// <summary>
    /// Gets a user client for a specific user ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User client for the specified user</returns>
    /// <exception cref="System.ArgumentException">User ID cannot be null or empty - userId</exception>
    public IUser GetUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

        return _clientManager.CreateUserClient(userId);
    }

    /// <summary>
    /// Gets the channels client for channel-related operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#channel
    /// </summary>
    /// <value>The channels.</value>
    public IChannels Channels => _clientManager.Channels;

    /// <summary>
    /// Gets the general client for general API operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#general
    /// </summary>
    /// <value>The general.</value>
    public IGeneral General => _clientManager.General;

    /// <summary>
    /// Gets a playlist client for a specific playlist ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns>Playlist client for the specified playlist</returns>
    /// <exception cref="System.ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public IPlaylist GetPlaylist(string playlistId)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
            throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

        return _clientManager.CreatePlaylistClient(playlistId);
    }

    /// <summary>
    /// Gets the echo client for testing API connectivity
    /// https://developers.dailymotion.com/api/platform-api/reference/#echo
    /// </summary>
    /// <value>The echo.</value>
    public IEcho Echo => _clientManager.Echo;

    /// <summary>
    /// Gets the file client for file upload operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#file
    /// </summary>
    /// <value>The file.</value>
    public IFile File => _clientManager.File;

    /// <summary>
    /// Gets the languages client for language operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#languages
    /// </summary>
    /// <value>The languages.</value>
    public ILanguages Languages => _clientManager.Languages;

    /// <summary>
    /// Gets the locale client for locale operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#locale
    /// </summary>
    /// <value>The locale.</value>
    public ILocale Locale => _clientManager.Locale;

    /// <summary>
    /// Gets the logout client for logout operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#logout
    /// </summary>
    /// <value>The logout.</value>
    public ILogout Logout => _clientManager.Logout;

    /// <summary>
    /// Gets the player client for player operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#player
    /// </summary>
    /// <value>The player.</value>
    public IPlayer Player => _clientManager.Player;

    /// <summary>
    /// Gets the subtitles client for subtitle operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#subtitles
    /// </summary>
    /// <value>The subtitles.</value>
    public ISubtitles Subtitles => _clientManager.Subtitles;

    /// <summary>
    /// Gets the mine client for current user operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#user
    /// </summary>
    /// <value>The mine.</value>
    public IMine Mine => _clientManager.Mine;

    /// <summary>
    /// Gets the playlists client for playlist management operations
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <value>The playlists.</value>
    public IPlaylists Playlists => _clientManager.Playlists;

    /// <summary>
    /// Authenticates using client credentials grant type
    /// Uses the appropriate API keys from SDK configuration based on ApiKeyType
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#client-credentials
    /// </summary>
    /// <param name="scopes">Required scopes for the access token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    public async Task<TokenResponse> AuthenticateWithClientCredentialsAsync(OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Authenticating with client credentials using SDK configuration");
        
        var (apiKey, apiSecret) = _options.ApiKeyType == ApiKeyType.Private 
            ? (_options.PrivateApiKey, _options.PrivateApiSecret)
            : (_options.PublicApiKey, _options.PublicApiSecret);
            
        return await _authService.AuthenticateWithClientCredentialsAsync(apiKey, apiSecret, _options.ApiKeyType, scopes, cancellationToken);
    }

    /// <summary>
    /// Authenticates using client credentials grant type
    /// Uses explicitly provided API key and secret
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#client-credentials
    /// </summary>
    /// <param name="apiKey">API Key for client credentials</param>
    /// <param name="apiSecret">API Secret for client credentials</param>
    /// <param name="apiKeyType">Type of API key (Public or Private) - determines which endpoints to use</param>
    /// <param name="scopes">Required scopes for the access token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    public async Task<TokenResponse> AuthenticateWithClientCredentialsAsync(string apiKey, string apiSecret, ApiKeyType apiKeyType, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Authenticating with client credentials using provided credentials");
        return await _authService.AuthenticateWithClientCredentialsAsync(apiKey, apiSecret, apiKeyType, scopes, cancellationToken);
    }

    /// <summary>
    /// Authenticates with username and password (for Public API keys only)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="scopes">Optional scopes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    /// <exception cref="System.ArgumentException">Username cannot be null or empty - username</exception>
    /// <exception cref="System.ArgumentException">Password cannot be null or empty - password</exception>
    public async Task<TokenResponse> AuthenticateWithPasswordAsync(string username, string password, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        _logger?.LogInformation("Authenticating with username and password");
        return await _authService.AuthenticateWithPasswordAsync(username, password, scopes, cancellationToken);
    }

    /// <summary>
    /// Authenticates with username and password using specific API keys (for Public API keys only)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="apiKey">Public API Key</param>
    /// <param name="apiSecret">Public API Secret</param>
    /// <param name="scopes">Optional scopes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    /// <exception cref="System.ArgumentException">Username cannot be null or empty - username</exception>
    /// <exception cref="System.ArgumentException">Password cannot be null or empty - password</exception>
    /// <exception cref="System.ArgumentException">API Key cannot be null or empty - apiKey</exception>
    /// <exception cref="System.ArgumentException">API Secret cannot be null or empty - apiSecret</exception>
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

        _logger?.LogInformation("Authenticating with username and password using provided API keys");
        return await _authService.AuthenticateWithPasswordAsync(username, password, apiKey, apiSecret, scopes, cancellationToken);
    }

    /// <summary>
    /// Authenticates with username and password (legacy method - use AuthenticateWithPasswordAsync instead)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="scopes">Optional scopes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    [Obsolete("Use AuthenticateWithPasswordAsync instead")]
    public async Task<TokenResponse> AuthenticateAsync(string username, string password, OAuthScope[]? scopes = null, CancellationToken cancellationToken = default)
    {
        return await AuthenticateWithPasswordAsync(username, password, scopes, cancellationToken);
    }

    /// <summary>
    /// Exchanges authorization code for access token (for Public API keys only)
    /// https://developers.dailymotion.com/guides/platform-api-authentication/#authorization-code
    /// </summary>
    /// <param name="authorizationCode">Authorization code</param>
    /// <param name="redirectUri">Redirect URI</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token exchange result</returns>
    /// <exception cref="System.ArgumentException">Authorization code cannot be null or empty - authorizationCode</exception>
    /// <exception cref="System.ArgumentException">Redirect URI cannot be null or empty - redirectUri</exception>
    public async Task<TokenResponse> ExchangeCodeForTokenAsync(string authorizationCode, string redirectUri, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(authorizationCode))
            throw new ArgumentException("Authorization code cannot be null or empty", nameof(authorizationCode));
        if (string.IsNullOrWhiteSpace(redirectUri))
            throw new ArgumentException("Redirect URI cannot be null or empty", nameof(redirectUri));

        _logger?.LogInformation("Exchanging authorization code for token");
        return await _authService.ExchangeCodeForTokenAsync(authorizationCode, redirectUri, cancellationToken);
    }

    /// <summary>
    /// Refreshes the access token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token refresh result</returns>
    /// <exception cref="System.ArgumentException">Refresh token cannot be null or empty - refreshToken</exception>
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

        _logger?.LogInformation("Refreshing access token");
        return await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
    }

    /// <summary>
    /// Validates the current access token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token validation result</returns>
    public async Task<TokenValidationResponse?> ValidateTokenAsync(CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Validating access token");
        return await _authService.ValidateTokenAsync(cancellationToken);
    }

    /// <summary>
    /// Revokes the current access token
    /// https://developers.dailymotion.com/api/platform-api/reference/#auth
    /// </summary>
    /// <param name="accessToken">Access token to revoke</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    /// <exception cref="System.ArgumentException">Access token cannot be null or empty - accessToken</exception>
    public async Task<bool> RevokeTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));

        _logger?.LogInformation("Revoking access token");
        return await _authService.RevokeTokenAsync(accessToken, cancellationToken);
    }

    /// <summary>
    /// Gets whether the current token is expired
    /// </summary>
    /// <value><c>true</c> if this instance is token expired; otherwise, <c>false</c>.</value>
    public bool IsTokenExpired => _authService.IsTokenExpired;

    /// <summary>
    /// Gets the current access token
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken => _authService.AccessToken;

    /// <summary>
    /// Gets the current refresh token
    /// </summary>
    /// <value>The refresh token.</value>
    public string? RefreshToken => _authService.RefreshToken;

    /// <summary>
    /// Gets a specific video by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="videoId">Video ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video metadata</returns>
    /// <exception cref="System.ArgumentException">Video ID cannot be null or empty - videoId</exception>
    public async Task<VideoMetadata?> GetVideoAsync(string videoId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(videoId))
            throw new ArgumentException("Video ID cannot be null or empty", nameof(videoId));

        _logger?.LogDebug("Getting video: {VideoId}", videoId);
        return await Videos.GetVideoAsync(videoId, null, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a specific user by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User metadata</returns>
    /// <exception cref="System.ArgumentException">User ID cannot be null or empty - userId</exception>
    public async Task<UserMetadata?> GetUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

        _logger?.LogDebug("Getting user: {UserId}", userId);
        var userClient = GetUser(userId);
        return await userClient.GetMetadataAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a specific playlist by ID
    /// https://developers.dailymotion.com/api/platform-api/reference/#playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist metadata</returns>
    /// <exception cref="System.ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public async Task<PlaylistMetadata?> GetPlaylistAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
            throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

        _logger?.LogDebug("Getting playlist: {PlaylistId}", playlistId);
        var playlistClient = GetPlaylist(playlistId);
        return await playlistClient.GetMetadataAsync(cancellationToken);
    }

    /// <summary>
    /// Searches for videos
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="System.ArgumentException">Search query cannot be null or empty - query</exception>
    /// <exception cref="System.ArgumentException">Limit must be between 1 and 100 - limit</exception>
    /// <exception cref="System.ArgumentException">Page must be greater than 0 - page</exception>
    public async Task<VideoListResponse> SearchVideosAsync(string query, int limit = 20, int page = 1, VideoSort sort = VideoSort.Relevance, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be null or empty", nameof(query));
        if (limit <= 0 || limit > 100)
            throw new ArgumentException("Limit must be between 1 and 100", nameof(limit));
        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        _logger?.LogDebug("Searching videos: {Query}, Limit: {Limit}, Page: {Page}", query, limit, page);
        return await General.SearchVideosAsync(query, limit, page, sort, cancellationToken);
    }

    /// <summary>
    /// Searches for users
    /// https://developers.dailymotion.com/api/platform-api/reference/#user
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    /// <exception cref="System.ArgumentException">Search query cannot be null or empty - query</exception>
    /// <exception cref="System.ArgumentException">Limit must be between 1 and 100 - limit</exception>
    /// <exception cref="System.ArgumentException">Page must be greater than 0 - page</exception>
    public async Task<UserListResponse> SearchUsersAsync(string query, int limit = 20, int page = 1, UserSort sort = UserSort.Relevance, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be null or empty", nameof(query));
        if (limit <= 0 || limit > 100)
            throw new ArgumentException("Limit must be between 1 and 100", nameof(limit));
        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        _logger?.LogDebug("Searching users: {Query}, Limit: {Limit}, Page: {Page}", query, limit, page);
        return await General.SearchUsersAsync(query, limit, page, sort, cancellationToken);
    }

    /// <summary>
    /// Gets trending videos
    /// https://developers.dailymotion.com/api/platform-api/reference/#video
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="System.ArgumentException">Limit must be between 1 and 100 - limit</exception>
    /// <exception cref="System.ArgumentException">Page must be greater than 0 - page</exception>
    public async Task<VideoListResponse> GetTrendingVideosAsync(int limit = 20, int page = 1, CancellationToken cancellationToken = default)
    {
        if (limit <= 0 || limit > 100)
            throw new ArgumentException("Limit must be between 1 and 100", nameof(limit));
        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        _logger?.LogDebug("Getting trending videos, Limit: {Limit}, Page: {Page}", limit, page);
        return await General.GetFeaturedVideosAsync(limit, page, cancellationToken);
    }

    /// <summary>
    /// Gets videos from a specific channel
    /// https://developers.dailymotion.com/api/platform-api/reference/#channel
    /// </summary>
    /// <param name="channel">Channel</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    /// <exception cref="System.ArgumentException">Limit must be between 1 and 100 - limit</exception>
    /// <exception cref="System.ArgumentException">Page must be greater than 0 - page</exception>
    public async Task<VideoListResponse> GetChannelVideosAsync(Channel channel, int limit = 20, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default)
    {
        if (limit <= 0 || limit > 100)
            throw new ArgumentException("Limit must be between 1 and 100", nameof(limit));
        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        _logger?.LogDebug("Getting channel videos: {Channel}, Limit: {Limit}, Page: {Page}", channel, limit, page);
        return await Channels.GetChannelVideosAsync(channel.ToString().ToLowerInvariant(), limit, page, sort, cancellationToken);
    }

    /// <summary>
    /// Disposes the SDK and releases resources
    /// </summary>
    public void Dispose()
    {
        _logger?.LogInformation("Disposing DailyMotion SDK");

        // Dispose the client manager which will dispose all managed clients
        _clientManager?.Dispose();

        // Dispose of any disposable services if needed
        if (_httpClient is IDisposable disposableHttpClient)
        {
            disposableHttpClient.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
