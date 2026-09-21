using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DailymotionSDK.Interfaces
{
    /// <summary>
    /// Class ProfileClient.
    /// Implements the <see cref="DailymotionSDK.Interfaces.IProfile" />
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <seealso cref="DailymotionSDK.Interfaces.IProfile" />
    public class ProfileClient(string profileId, IDailymotionHttpClient httpClient, ILogger<ProfileClient> logger) : IProfile
    {
        /// <summary>
        /// Gets the profile asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task&lt;Profile&gt; representing the asynchronous operation.</returns>
        public async Task<Profile?> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogDebug("Getting profile for {UserId}", profileId);

                var queryParams = "can_change_name,content_defaults,created_at,description,display_name,name,profile_id,social_links,webhook";

                var response = await httpClient.GetAsync($"/profiles/{profileId}?fields={queryParams}", null, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Failed to get profile: {StatusCode} - {Content}", response.StatusCode, response.Content);
                    return null;
                }
                return JsonSerializer.Deserialize<Profile>(response.Content ?? string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting profile for {UserId}", profileId);
                throw;
            }
        }
    }
}
