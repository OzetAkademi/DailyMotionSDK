using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for locale operations
/// https://developers.dailymotion.com/api/platform-api/reference/#locale
/// </summary>
public interface ILocale
{
    /// <summary>
    /// Detects and retrieves locale information
    /// https://developers.dailymotion.com/api/platform-api/reference/#detecting-and-retrieving-locales
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Locale detection response</returns>
    Task<LocaleDetectionResponse> DetectLocaleAsync(CancellationToken cancellationToken = default);
}
