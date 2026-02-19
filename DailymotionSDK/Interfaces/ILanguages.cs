using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for language operations
/// https://developers.dailymotion.com/api/platform-api/reference/#languages
/// </summary>
public interface ILanguages
{
    /// <summary>
    /// Retrieves available languages
    /// https://developers.dailymotion.com/api/platform-api/reference/#retrieving-languages
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Languages response</returns>
    Task<LanguagesResponse> GetLanguagesAsync(CancellationToken cancellationToken = default);
}
