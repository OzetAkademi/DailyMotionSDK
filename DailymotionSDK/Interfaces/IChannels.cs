using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for DailyMotion channel operations
/// https://developer.dailymotion.com/api#channel
/// </summary>
public interface IChannels
{
    /// <summary>
    /// Gets channel metadata
    /// https://developer.dailymotion.com/api#channel-fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel metadata</returns>
    Task<ChannelMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets channel subscribers
    /// https://developer.dailymotion.com/api#channel-subscribers
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User list response</returns>
    Task<UserListResponse> GetSubscribersAsync(int limit = 100, int page = 1, UserSort sort = UserSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets channel videos
    /// https://developer.dailymotion.com/api#channel-videos
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetVideosAsync(int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets channel videos by channel ID
    /// https://developer.dailymotion.com/api#channel-videos
    /// </summary>
    /// <param name="channelId">Channel ID</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> GetChannelVideosAsync(string channelId, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets channel playlists
    /// https://developer.dailymotion.com/api#channel-playlists
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Playlist list response</returns>
    Task<PlaylistListResponse> GetPlaylistsAsync(int limit = 100, int page = 1, PlaylistSort sort = PlaylistSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for videos in the channel
    /// https://developer.dailymotion.com/api#channel-videos
    /// </summary>
    /// <param name="keyword">Search keyword</param>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="sort">Sort order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Video list response</returns>
    Task<VideoListResponse> SearchVideosAsync(string keyword, int limit = 100, int page = 1, VideoSort sort = VideoSort.Recent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets channel metadata by channel ID
    /// https://developer.dailymotion.com/api#channel-fields
    /// </summary>
    /// <param name="channelId">Channel ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel metadata</returns>
    Task<ChannelMetadata?> GetChannelMetadataAsync(string channelId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all available channels
    /// https://developer.dailymotion.com/api#channel
    /// </summary>
    /// <param name="limit">Number of results to return</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Channel list response</returns>
    Task<ChannelListResponse> GetChannelsAsync(int limit = 100, int page = 1, CancellationToken cancellationToken = default);

}
