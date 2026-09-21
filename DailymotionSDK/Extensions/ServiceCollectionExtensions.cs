using DailymotionSDK.Configuration;
using DailymotionSDK.Internal;
using DailymotionSDK.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DailymotionSDK.Extensions;

/// <summary>
/// Class ServiceCollectionExtensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the dailymotion SDK.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configurationSection">The configuration section.</param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection.</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);

        services.Configure<DailymotionOptions>(configurationSection);
        return RegisterCoreServices(services);
    }

    /// <summary>
    /// Adds the dailymotion SDK.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection.</returns>
    public static IServiceCollection AddDailymotionSDK(this IServiceCollection services, Action<DailymotionOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.Configure(configureOptions);
        return RegisterCoreServices(services);
    }

    /// <summary>
    /// Registers the core services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection.</returns>
    private static IServiceCollection RegisterCoreServices(IServiceCollection services)
    {
        // Extract the evaluated options from the DI container so internal classes can use it without IOptions<T> boilerplate
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<DailymotionOptions>>().Value);

        // Core Infrastructure
        services.AddSingleton<IDailymotionHttpClient, DailymotionHttpClient>();
        services.AddSingleton<IDailymotionAuthService, DailymotionAuthService>();

        // Register the main SDK Facade / Handler
        services.AddSingleton<DailymotionHandler>();

        // Optional: Register individual clients for developers who prefer direct injection over using the Handler
        services.AddTransient(sp => sp.GetRequiredService<DailymotionHandler>().Videos);
        services.AddTransient(sp => sp.GetRequiredService<DailymotionHandler>().File);
        services.AddTransient(sp => sp.GetRequiredService<ClientManager>().Me);

        return services;
    }
}