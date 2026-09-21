using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IUser
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the user asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{System.Nullable{User}}.</returns>
    Task<User?> GetUserAsync(CancellationToken cancellationToken = default);
}