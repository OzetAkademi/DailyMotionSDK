using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Class MeClient.
/// Implements the <see cref="DailymotionSDK.Interfaces.IMe" />
/// </summary>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Interfaces.IMe" />
public class MeClient(IDailymotionHttpClient httpClient, ILogger<MeClient> logger) : IMe
{
    /// <summary>
    /// The json options
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Gets me asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Me&gt; representing the asynchronous operation.</returns>
    public async Task<Me?> GetMeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Getting user info");

            var response = await httpClient.GetAsync("/me?fields=profiles,user_id,username", null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get user info: {StatusCode} - {Content}", response.StatusCode, response.Content);
                return null;
            }

            return JsonSerializer.Deserialize<Me>(response.Content!, _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user info");
            throw;
        }
    }
}