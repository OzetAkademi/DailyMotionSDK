using DailymotionSDK.Models;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface for player operations
/// https://developers.dailymotion.com/api/platform-api/reference/#player
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Gets player information
    /// https://developers.dailymotion.com/api/platform-api/reference/#player
    /// </summary>
    /// <param name="playerId">Player ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Player metadata</returns>
    Task<PlayerMetadata?> GetPlayerAsync(string playerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new player
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-players
    /// </summary>
    /// <param name="playerData">Player data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created player metadata</returns>
    Task<PlayerMetadata?> CreatePlayerAsync(Dictionary<string, string> playerData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a player
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-players
    /// </summary>
    /// <param name="playerId">Player ID</param>
    /// <param name="playerData">Player data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated player metadata</returns>
    Task<PlayerMetadata?> UpdatePlayerAsync(string playerId, Dictionary<string, string> playerData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a player
    /// https://developers.dailymotion.com/api/platform-api/reference/#manipulating-players
    /// </summary>
    /// <param name="playerId">Player ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeletePlayerAsync(string playerId, CancellationToken cancellationToken = default);
}
