namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for logout operations
/// https://developers.dailymotion.com/api/platform-api/reference/#logout
/// </summary>
public interface ILogout
{
    /// <summary>
    /// Logs out the current user
    /// https://developers.dailymotion.com/api/platform-api/reference/#logging-out
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if logout was successful</returns>
    Task<bool> LogoutAsync(CancellationToken cancellationToken = default);
}
