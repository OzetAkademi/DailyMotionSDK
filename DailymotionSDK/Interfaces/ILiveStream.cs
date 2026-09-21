using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;

namespace DailymotionSDK.Interfaces
{
    /// <summary>
    /// Interface ILiveStream
    /// </summary>
    public interface ILiveStream
    {
        /// <summary>
        /// Create a livestream on this profile. Required body fields are title, visibility, category, and is_for_kids. 
        /// Returns 201 with the new resource. Requires live.manage scope.
        /// </summary>
        /// <param name="liveStreamCreateRequest">The live stream create request.</param>
        /// <returns>Task{System.Nullable{Livestream}}.</returns>
        Task<Livestream?> CreateLiveStream(LiveStreamCreateRequest liveStreamCreateRequest);

        /// <summary>
        /// Ends the live stream.
        /// </summary>
        /// <param name="liveStreamEndRequest">The live stream end request.</param>
        /// <returns>Task{System.Boolean}.</returns>
        Task<bool> EndLiveStream(LiveStreamEndRequest liveStreamEndRequest);

        /// <summary>
        /// Gets the live streams.
        /// </summary>
        /// <param name="liveStreamListRequest">The live stream list request.</param>
        /// <returns>Task{System.Nullable{LiveStreamList}}.</returns>
        Task<LiveStreamList?> GetLiveStreams(LiveStreamListRequest liveStreamListRequest);
    }
}