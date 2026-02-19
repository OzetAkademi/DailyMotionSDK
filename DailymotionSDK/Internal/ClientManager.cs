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
    private readonly DailymotionOptions _options;
    private readonly IDailymotionHttpClient _httpClient;
    private readonly IDailymotionAuthService _authService;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<ClientManager> _logger;

    // Cached clients
    private VideosClient? _videosClient;
    private ChannelsClient? _channelsClient;
    private GeneralClient? _generalClient;
    private EchoClient? _echoClient;
    private FileClient? _fileClient;
    private LanguagesClient? _languagesClient;
    private LocaleClient? _localeClient;
    private LogoutClient? _logoutClient;
    private PlayerClient? _playerClient;
    private SubtitlesClient? _subtitlesClient;
    private MineClient? _mineClient;
    private PlaylistsClient? _playlistsClient;

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
