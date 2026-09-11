using DailymotionSDK.Models.Responses;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IMe
/// </summary>
public interface IMe
{
    /// <summary>
    /// Gets me asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{Me}}.</returns>
    Task<Me?> GetMeAsync(CancellationToken cancellationToken = default);
}