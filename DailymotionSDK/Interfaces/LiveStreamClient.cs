using DailymotionSDK.Helper;
using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Interfaces
{
    /// <summary>
    /// Class LiveStreamClient.
    /// Implements the <see cref="DailymotionSDK.Interfaces.ILiveStream" />
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <seealso cref="DailymotionSDK.Interfaces.ILiveStream" />
    public class LiveStreamClient(IDailymotionHttpClient httpClient, ILogger<LiveStreamClient> logger) : ILiveStream
    {
        /// <summary>
        /// Creates the live stream.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable{Livestream}.</returns>
        public async Task<Livestream?> CreateLiveStream(string profileId, LiveStreamCreationParameters parameters)
        {
            ArgumentException.ThrowIfNullOrEmpty(profileId);
            ArgumentException.ThrowIfNullOrEmpty(parameters.Title);
            ArgumentException.ThrowIfNullOrEmpty(parameters.Description);
            ArgumentException.ThrowIfNullOrEmpty(parameters.Category);
            ArgumentException.ThrowIfNullOrEmpty(parameters.Visibility);

            var response = await httpClient.PostJsonAsync($"profiles/{profileId}/livestreams", parameters);

            if (!response.IsSuccessStatusCode)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("Failed to create live stream: {Error}\nResponse Status: {StatusCode}, Content: {Content}\nRequest Parameters: {Parameters}",
                        response.ErrorMessage,
                        response.StatusCode,
                        response.Content,
                        JsonHandler.Serialize(parameters));
                }
                return null;
            }

            return JsonHandler.Deserialize<Livestream>(response.Content!);
        }

        /// <summary>
        /// Ends the live stream.
        /// </summary>
        /// <param name="livestreamId">The livestream identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<bool> EndLiveStream(string livestreamId)
        {
            ArgumentException.ThrowIfNullOrEmpty(livestreamId);

            var response = await httpClient.PatchAsync($"livestreams/{livestreamId}",
                new() {
                    { "end_at", DateTime.Now.ToUniversalTime().ToString("s") + "Z" }
                });

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Gets the live streams.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>System.Nullable{LiveStreamList}.</returns>
        public async Task<LiveStreamList?> GetLiveStreams(string profileId)
        {
            ArgumentException.ThrowIfNullOrEmpty(profileId);

            var response = await httpClient.GetAsync($"profiles/{profileId}/livestreams?fields=livestream_id,title,livestream_url,created_at,description,ingest,recording,updated_at,start_at,status");

            if (!response.IsSuccessStatusCode)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("Failed to get live streams: {Error}\nResponse Status: {StatusCode}, Content: {Content}",
                        response.ErrorMessage,
                        response.StatusCode,
                        response.Content);
                }
                return null;
            }

            return JsonHandler.Deserialize<LiveStreamList>(response.Content!);
        }
    }
}