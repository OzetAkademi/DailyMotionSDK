using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Class UserClient.
/// Implements the <see cref="DailymotionSDK.Interfaces.IUser" />
/// </summary>
/// <param name="userId">The user identifier.</param>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Interfaces.IUser" />
public class UserClient(string userId, IDailymotionHttpClient httpClient, ILogger<UserClient> logger) : IUser
{
    /// <summary>
    /// Gets the user asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;User&gt; representing the asynchronous operation.</returns>
    public async Task<User?> GetUserAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Getting user metadata for {UserId}", userId);

            var response = await httpClient.GetAsync($"/users/{userId}", null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get user metadata: {StatusCode} - {Content}", response.StatusCode, response.Content);
                return null;
            }

            return JsonHandler.Deserialize<User>(response.Content!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user metadata for {UserId}", userId);
            throw;
        }
    }
}