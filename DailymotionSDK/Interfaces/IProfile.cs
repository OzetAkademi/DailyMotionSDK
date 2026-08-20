using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces
{
    /// <summary>
    /// Interface IProfile
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// Gets the profile asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task{System.Nullable{Profile}}.</returns>
        Task<Profile?> GetProfileAsync(CancellationToken cancellationToken = default);
    }
}