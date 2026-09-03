using DailymotionSDK.Configuration;
using DailymotionSDK.Interfaces;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Internal;

/// <summary>
/// Class ClientManager.
/// Implements the <see cref="System.IDisposable" />
/// </summary>
/// <param name="options">The options.</param>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="authService">The authentication service.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <seealso cref="System.IDisposable" />
internal class ClientManager(DailymotionOptions options, IDailymotionHttpClient httpClient, IDailymotionAuthService authService, ILoggerFactory loggerFactory) : IDisposable
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ClientManager> _logger = loggerFactory.CreateLogger<ClientManager>();

    /// <summary>
    /// Me client
    /// </summary>
    private MeClient? _meClient;

    /// <summary>
    /// The videos client
    /// </summary>
    private VideosClient? _videosClient;

    /// <summary>
    /// The live stream client
    /// </summary>
    private LiveStreamClient? _liveStreamClient;

    /// <summary>
    /// The file client
    /// </summary>
    private UploadClient? _uploadClient;

    /// <summary>
    /// Gets me.
    /// </summary>
    /// <value>Me.</value>
    public IMe Me => _meClient ??= new(httpClient, loggerFactory.CreateLogger<MeClient>());

    /// <summary>
    /// Gets the videos.
    /// </summary>
    /// <value>The videos.</value>
    public IVideos Videos => _videosClient ??= new(Me, httpClient, loggerFactory.CreateLogger<VideosClient>());

    /// <summary>
    /// Gets the file.
    /// </summary>
    /// <value>The file.</value>
    public IUpload Upload => _uploadClient ??= new(httpClient, loggerFactory.CreateLogger<UploadClient>());

    /// <summary>
    /// Gets the live stream.
    /// </summary>
    /// <value>The live stream.</value>
    public ILiveStream LiveStream => _liveStreamClient ??= new(httpClient, loggerFactory.CreateLogger<LiveStreamClient>());

    /// <summary>
    /// Creates the user client.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>DailymotionSDK.Interfaces.IUser.</returns>
    public IUser CreateUserClient(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        return new UserClient(userId, httpClient, loggerFactory.CreateLogger<UserClient>());
    }

    public IProfile CreateProfileClient(string profileId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(profileId);
        return new ProfileClient(profileId, httpClient, loggerFactory.CreateLogger<ProfileClient>());
    }

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public void Dispose()
    {
        _logger.LogDebug("Disposing ClientManager and all managed clients");

        _videosClient = null;
        _uploadClient = null;
        _meClient = null;

        GC.SuppressFinalize(this);
    }
}