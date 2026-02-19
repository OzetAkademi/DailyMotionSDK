using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for echo operations
/// https://developers.dailymotion.com/api/platform-api/reference/#echo
/// </summary>
public interface IEcho
{
    /// <summary>
    /// Tests API connectivity by echoing back the provided data
    /// https://developers.dailymotion.com/api/platform-api/reference/#using-echo
    /// </summary>
    /// <param name="data">Data to echo back</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Echo response</returns>
    Task<EchoResponse> EchoAsync(string data, CancellationToken cancellationToken = default);
}
