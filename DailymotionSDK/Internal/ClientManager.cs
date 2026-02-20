using DailymotionSDK.Configuration;
using DailymotionSDK.Interfaces;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Internal;

/// <summary>
/// Internal client manager that creates and manages API clients without using DI scopes
/// </summary>
internal class ClientManager : IDisposable
{
    /// <summary>
    /// The options
    /// </summary>
    private readonly DailymotionOptions _options;
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The authentication service
    /// </summary>
    private readonly IDailymotionAuthService _authService;
    /// <summary>
    /// The logger factory
    /// </summary>
    private readonly ILoggerFactory _loggerFactory;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ClientManager> _logger;

    // Cached clients
    /// <summary>
    /// The videos client
    /// </summary>
    private VideosClient? _videosClient;
    /// <summary>
    /// The channels client
    /// </summary>
    private ChannelsClient? _channelsClient;
    /// <summary>
    /// The general client
    /// </summary>
    private GeneralClient? _generalClient;
    /// <summary>
    /// The echo client
    /// </summary>
    private EchoClient? _echoClient;
    /// <summary>
    /// The file client
    /// </summary>
    private FileClient? _fileClient;
    /// <summary>
    /// The languages client
    /// </summary>
    private LanguagesClient? _languagesClient;
    /// <summary>
    /// The locale client
    /// </summary>
    private LocaleClient? _localeClient;
    /// <summary>
    /// The logout client
    /// </summary>
    private LogoutClient? _logoutClient;
    /// <summary>
    /// The player client
    /// </summary>
    private PlayerClient? _playerClient;
    /// <summary>
    /// The subtitles client
    /// </summary>
    private SubtitlesClient? _subtitlesClient;
    /// <summary>
    /// The mine client
    /// </summary>
    private MineClient? _mineClient;
    /// <summary>
    /// The playlists client
    /// </summary>
    private PlaylistsClient? _playlistsClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientManager"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="authService">The authentication service.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <exception cref="ArgumentNullException">options</exception>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">authService</exception>
    /// <exception cref="ArgumentNullException">loggerFactory</exception>
    public ClientManager(
        DailymotionOptions options,
        IDailymotionHttpClient httpClient,
        IDailymotionAuthService authService,
        ILoggerFactory loggerFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = _loggerFactory.CreateLogger<ClientManager>();
    }

    /// <summary>
    /// Gets the videos client
    /// </summary>
    /// <value>The videos.</value>
    public IVideos Videos
    {
        get
        {
            if (_videosClient == null)
            {
                _videosClient = new VideosClient(_httpClient, _loggerFactory.CreateLogger<VideosClient>());
            }
            return _videosClient;
        }
    }

    /// <summary>
    /// Gets the channels client
    /// </summary>
    /// <value>The channels.</value>
    public IChannels Channels
    {
        get
        {
            if (_channelsClient == null)
            {
                _channelsClient = new ChannelsClient(_httpClient, _loggerFactory.CreateLogger<ChannelsClient>());
            }
            return _channelsClient;
        }
    }

    /// <summary>
    /// Gets the general client
    /// </summary>
    /// <value>The general.</value>
    public IGeneral General
    {
        get
        {
            if (_generalClient == null)
            {
                _generalClient = new GeneralClient(_httpClient, _loggerFactory.CreateLogger<GeneralClient>());
            }
            return _generalClient;
        }
    }

    /// <summary>
    /// Gets the echo client
    /// </summary>
    /// <value>The echo.</value>
    public IEcho Echo
    {
        get
        {
            if (_echoClient == null)
            {
                _echoClient = new EchoClient(_httpClient, _loggerFactory.CreateLogger<EchoClient>());
            }
            return _echoClient;
        }
    }

    /// <summary>
    /// Gets the file client
    /// </summary>
    /// <value>The file.</value>
    public IFile File
    {
        get
        {
            if (_fileClient == null)
            {
                _fileClient = new FileClient(_httpClient, _loggerFactory.CreateLogger<FileClient>());
            }
            return _fileClient;
        }
    }

    /// <summary>
    /// Gets the languages client
    /// </summary>
    /// <value>The languages.</value>
    public ILanguages Languages
    {
        get
        {
            if (_languagesClient == null)
            {
                _languagesClient = new LanguagesClient(_httpClient, _loggerFactory.CreateLogger<LanguagesClient>());
            }
            return _languagesClient;
        }
    }

    /// <summary>
    /// Gets the locale client
    /// </summary>
    /// <value>The locale.</value>
    public ILocale Locale
    {
        get
        {
            if (_localeClient == null)
            {
                _localeClient = new LocaleClient(_httpClient, _loggerFactory.CreateLogger<LocaleClient>());
            }
            return _localeClient;
        }
    }

    /// <summary>
    /// Gets the logout client
    /// </summary>
    /// <value>The logout.</value>
    public ILogout Logout
    {
        get
        {
            if (_logoutClient == null)
            {
                _logoutClient = new LogoutClient(_httpClient, _loggerFactory.CreateLogger<LogoutClient>());
            }
            return _logoutClient;
        }
    }

    /// <summary>
    /// Gets the player client
    /// </summary>
    /// <value>The player.</value>
    public IPlayer Player
    {
        get
        {
            if (_playerClient == null)
            {
                _playerClient = new PlayerClient(_httpClient, _loggerFactory.CreateLogger<PlayerClient>());
            }
            return _playerClient;
        }
    }

    /// <summary>
    /// Gets the subtitles client
    /// </summary>
    /// <value>The subtitles.</value>
    public ISubtitles Subtitles
    {
        get
        {
            if (_subtitlesClient == null)
            {
                _subtitlesClient = new SubtitlesClient(_httpClient, _loggerFactory.CreateLogger<SubtitlesClient>());
            }
            return _subtitlesClient;
        }
    }

    /// <summary>
    /// Gets the mine client
    /// </summary>
    /// <value>The mine.</value>
    public IMine Mine
    {
        get
        {
            if (_mineClient == null)
            {
                _mineClient = new MineClient(_httpClient, _loggerFactory.CreateLogger<MineClient>());
            }
            return _mineClient;
        }
    }

    /// <summary>
    /// Gets the playlists client
    /// </summary>
    /// <value>The playlists.</value>
    public IPlaylists Playlists
    {
        get
        {
            if (_playlistsClient == null)
            {
                _playlistsClient = new PlaylistsClient(_httpClient, _loggerFactory.CreateLogger<PlaylistsClient>(), _loggerFactory);
            }
            return _playlistsClient;
        }
    }

    /// <summary>
    /// Creates a user client for a specific user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User client for the specified user</returns>
    /// <exception cref="ArgumentException">User ID cannot be null or empty - userId</exception>
    public IUser CreateUserClient(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

        return new UserClient(userId, _httpClient, _loggerFactory.CreateLogger<UserClient>());
    }

    /// <summary>
    /// Creates a playlist client for a specific playlist ID
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns>Playlist client for the specified playlist</returns>
    /// <exception cref="ArgumentException">Playlist ID cannot be null or empty - playlistId</exception>
    public IPlaylist CreatePlaylistClient(string playlistId)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
            throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

        return new PlaylistClient(playlistId, _httpClient, _loggerFactory.CreateLogger<PlaylistClient>());
    }

    /// <summary>
    /// Disposes all managed clients
    /// </summary>
    public void Dispose()
    {
        _logger.LogDebug("Disposing ClientManager and all managed clients");

        // Note: Client classes don't implement IDisposable, so we just clear references
        _videosClient = null;
        _channelsClient = null;
        _generalClient = null;
        _echoClient = null;
        _fileClient = null;
        _languagesClient = null;
        _localeClient = null;
        _logoutClient = null;
        _playerClient = null;
        _subtitlesClient = null;
        _mineClient = null;
        _playlistsClient = null;

        GC.SuppressFinalize(this);
    }
}
