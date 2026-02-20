using DailymotionSDK.Configuration;
using DailymotionSDK.Interfaces;
using DailymotionSDK.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register DailyMotion SDK services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DailyMotion SDK services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="options">Configuration options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, DailymotionOptions options)
    {
        return services.AddDailymotionSDK(options, ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Adds DailyMotion SDK services to the service collection with specified lifetime
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="options">Configuration options</param>
    /// <param name="lifetime">Service lifetime for DailymotionHandler and IDailymotionAuthService</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, DailymotionOptions options, ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(options);

        // Register configuration
        services.AddSingleton(options);

        // Register HTTP client
        services.AddSingleton<IDailymotionHttpClient, DailymotionHttpClient>();

        // Register authentication service with specified lifetime
        // When using singleton, auth service must also be singleton to avoid lifetime mismatch
        if (lifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton<IDailymotionAuthService, DailymotionAuthService>();
        }
        else
        {
            services.AddScoped<IDailymotionAuthService, DailymotionAuthService>();
        }

        // Register API clients (for backward compatibility)
        services.AddScoped<IVideos, VideosClient>();
        services.AddScoped<IChannels, ChannelsClient>();
        services.AddScoped<IGeneral, GeneralClient>();
        services.AddScoped<IEcho, EchoClient>();
        services.AddScoped<IFile, FileClient>();
        services.AddScoped<ILanguages, LanguagesClient>();
        services.AddScoped<ILocale, LocaleClient>();
        services.AddScoped<ILogout, LogoutClient>();
        services.AddScoped<IPlayer, PlayerClient>();
        services.AddScoped<ISubtitles, SubtitlesClient>();
        services.AddScoped<IMine, MineClient>();
        // Register playlist services
        services.AddScoped<IPlaylists>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<IDailymotionHttpClient>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return new PlaylistsClient(httpClient, loggerFactory.CreateLogger<PlaylistsClient>(), loggerFactory);
        });

        // Note: UserClient and PlaylistClient are not registered directly as they require
        // string parameters (userId/playlistId). They should be created via factory methods:
        // - DailymotionHandler.GetUser(userId)
        // - DailymotionHandler.GetPlaylist(playlistId)
        // - PlaylistsClient.GetPlaylist(playlistId)

        // Register main SDK client using factory with specified lifetime
        var handlerDescriptor = ServiceDescriptor.Describe(
            typeof(DailymotionHandler),
            serviceProvider =>
            {
                var httpClient = serviceProvider.GetRequiredService<IDailymotionHttpClient>();
                var authService = serviceProvider.GetRequiredService<IDailymotionAuthService>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new DailymotionHandler(options, httpClient, authService, loggerFactory);
            },
            lifetime);

        services.Add(handlerDescriptor);

        return services;
    }

    /// <summary>
    /// Adds DailyMotion SDK services to the service collection with default configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, Action<DailymotionOptions> configureOptions)
    {
        return services.AddDailymotionSDK(configureOptions, ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Adds DailyMotion SDK services to the service collection with default configuration and specified lifetime
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure options</param>
    /// <param name="lifetime">Service lifetime for DailymotionHandler and IDailymotionAuthService</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, Action<DailymotionOptions> configureOptions, ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        var options = new DailymotionOptions();
        configureOptions(options);

        return services.AddDailymotionSDK(options, lifetime);
    }

    /// <summary>
    /// Adds DailyMotion SDK services to the service collection as singleton (for use with workers and background services)
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="options">Configuration options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDKAsSingleton(this IServiceCollection services, DailymotionOptions options)
    {
        return services.AddDailymotionSDK(options, ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Adds DailyMotion SDK services to the service collection as singleton with default configuration (for use with workers and background services)
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDailymotionSDKAsSingleton(this IServiceCollection services, Action<DailymotionOptions> configureOptions)
    {
        return services.AddDailymotionSDK(configureOptions, ServiceLifetime.Singleton);
    }
}
