using DailymotionSDK.Configuration;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using DailymotionSDK.Interfaces;
using DailymotionSDK.Internal;
using Microsoft.Extensions.Logging;
using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;

namespace DailymotionSDK;

/// <summary>
/// Class DailymotionHandler.
/// Implements the <see cref="System.IDisposable" />
/// </summary>
/// <param name="options">The options.</param>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="authService">The authentication service.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <seealso cref="System.IDisposable" />
public class DailymotionHandler(DailymotionOptions options, IDailymotionHttpClient httpClient, IDailymotionAuthService authService, ILoggerFactory loggerFactory) : IDisposable
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<DailymotionHandler> _logger = loggerFactory.CreateLogger<DailymotionHandler>();

    /// <summary>
    /// The client manager
    /// </summary>
    private readonly ClientManager _clientManager = new(options, httpClient, authService, loggerFactory);

    /// <summary>
    /// Gets the authentication.
    /// </summary>
    /// <value>The authentication.</value>
    public IDailymotionAuthService Auth => authService;
    /// <summary>
    /// Gets the HTTP client.
    /// </summary>
    /// <value>The HTTP client.</value>
    public IDailymotionHttpClient HttpClient => httpClient;
    /// <summary>
    /// Gets the options.
    /// </summary>
    /// <value>The options.</value>
    public DailymotionOptions Options => options;

    /// <summary>
    /// Gets me.
    /// </summary>
    /// <value>Me.</value>
    public IMe Me => _clientManager.Me;

    /// <summary>
    /// Gets the videos.
    /// </summary>
    /// <value>The videos.</value>
    public IVideos Videos => _clientManager.Videos;
    /// <summary>
    /// Gets the file.
    /// </summary>
    /// <value>The file.</value>
    public IUpload File => _clientManager.Upload;

    /// <summary>
    /// Gets the live stream.
    /// </summary>
    /// <value>The live stream.</value>
    public ILiveStream LiveStream => _clientManager.LiveStream;

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken => authService.AccessToken;
    /// <summary>
    /// Gets the is token expired.
    /// </summary>
    /// <value>The is token expired.</value>
    public bool IsTokenExpired => authService.IsTokenExpired;

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>DailymotionSDK.Interfaces.IUser.</returns>
    public IUser GetUser(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        return _clientManager.CreateUserClient(userId);
    }

    /// <summary>
    /// Authenticate as an asynchronous operation.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <param name="apiSecret">The API secret.</param>
    /// <param name="scopes">The scopes.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TokenResponse&gt; representing the asynchronous operation.</returns>
    public async Task<TokenResponse> AuthenticateAsync(AuthRequest authRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Authenticating with client credentials");

        return await authService.AuthenticateWithPrivateAsync(authRequest, cancellationToken);
    }

    /// <summary>
    /// Get video as an asynchronous operation.
    /// </summary>
    /// <param name="videoGetRequest">The video get request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task{System.Nullable{Video}}. representing the asynchronous operation.</returns>
    public async Task<Video?> GetVideoAsync(VideoGetRequest videoGetRequest, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(videoGetRequest.Id);

        _logger.LogDebug("Getting video: {VideoId}", videoGetRequest.Id);
        return await Videos.GetVideoAsync(videoGetRequest, cancellationToken);

    }

    /// <summary>
    /// Get user as an asynchronous operation.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;DailymotionSDK.Models.User?&gt; representing the asynchronous operation.</returns>
    public async Task<User?> GetUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        _logger.LogDebug("Getting user: {UserId}", userId);
        return await GetUser(userId).GetUserAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public void Dispose()
    {
        _logger.LogInformation("Disposing Dailymotion SDK Handler");

        _clientManager.Dispose();

        if (httpClient is IDisposable disposableHttpClient)
        {
            disposableHttpClient.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}