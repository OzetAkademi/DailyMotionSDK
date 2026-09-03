using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces
{
    /// <summary>
    /// Interface ILiveStream
    /// </summary>
    public interface ILiveStream
    {
        /// <summary>
        /// Creates the live stream.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task{System.Nullable{Livestream}}.</returns>
        Task<Livestream?> CreateLiveStream(string profileId, LiveStreamCreationParameters parameters);

        /// <summary>
        /// Ends the live stream.
        /// </summary>
        /// <param name="livestreamId">The livestream identifier.</param>
        /// <returns>Task{System.Boolean}.</returns>
        Task<bool> EndLiveStream(string livestreamId);

        /// <summary>
        /// Gets the live streams.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>Task{System.Nullable{LiveStreamList}}.</returns>
        Task<LiveStreamList?> GetLiveStreams(string profileId);
    }
}