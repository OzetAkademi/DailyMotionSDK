using DailymotionSDK.Helper;
using DailymotionSDK.Models.Enums;
using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;
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
        /// Create a livestream on this profile. Required body fields are title, visibility, category, and is_for_kids.
        /// Returns 201 with the new resource. Requires live.manage scope.
        /// </summary>
        /// <param name="liveStreamCreateRequest">The live stream create request.</param>
        /// <returns>Task{System.Nullable{Livestream}}.</returns>
        public async Task<Livestream?> CreateLiveStream(LiveStreamCreateRequest liveStreamCreateRequest)
        {
            ArgumentException.ThrowIfNullOrEmpty(liveStreamCreateRequest.ProfileId);
            ArgumentException.ThrowIfNullOrEmpty(liveStreamCreateRequest.Title);
            ArgumentException.ThrowIfNullOrEmpty(liveStreamCreateRequest.Description);
            ArgumentException.ThrowIfNullOrEmpty(liveStreamCreateRequest.Visibility);

            var response = await httpClient.PostJsonAsync($"profiles/{liveStreamCreateRequest.ProfileId}/livestreams", liveStreamCreateRequest);

            if (!response.IsSuccessStatusCode)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("Failed to create live stream: {Error}\nResponse Status: {StatusCode}, Content: {Content}\nRequest Parameters: {Parameters}",
                        response.ErrorMessage,
                        response.StatusCode,
                        response.Content,
                        JsonHandler.Serialize(liveStreamCreateRequest));
                }
                return null;
            }

            return JsonHandler.Deserialize<Livestream>(response.Content);
        }

        /// <summary>
        /// Ends the live stream.
        /// </summary>
        /// <param name="liveStreamEndRequest">The live stream end request.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<bool> EndLiveStream(LiveStreamEndRequest liveStreamEndRequest)
        {
            ArgumentException.ThrowIfNullOrEmpty(liveStreamEndRequest.Id);

            var response = await httpClient.PatchAsync($"livestreams/{liveStreamEndRequest.Id}",
                new() {
                    { "end_at", DateTime.Now.ToUniversalTime().ToString("s") + "Z" }
                });

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Gets the live streams.
        /// </summary>
        /// <param name="liveStreamListRequest">The live stream list request.</param>
        /// <returns>System.Nullable{LiveStreamList}.</returns>
        public async Task<LiveStreamList?> GetLiveStreams(LiveStreamListRequest liveStreamListRequest)
        {
            ArgumentNullException.ThrowIfNull(liveStreamListRequest);
            ArgumentException.ThrowIfNullOrEmpty(liveStreamListRequest.ProfileId);

            Dictionary<string, string> parameters = new()
            {
                ["fields"] = string.Join(',', liveStreamListRequest.LiveStreamQueryParameters?.Fields?.ToApiFieldNames() ?? [])
            };

            if (liveStreamListRequest.LiveStreamQueryParameters is not null)
            {
                foreach (var (key, value) in ConvertLiveStreamFiltersToParameters(liveStreamListRequest.LiveStreamQueryParameters))
                {
                    parameters[key] = value;
                }
            }

            var response = await httpClient.GetAsync($"profiles/{liveStreamListRequest.ProfileId}/livestreams", parameters);

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

            return JsonHandler.Deserialize<LiveStreamList>(response.Content);
        }

        private static Dictionary<string, string> ConvertLiveStreamFiltersToParameters(LiveStreamQueryParameters filters)
        {
            var parameters = new Dictionary<string, string>();

            if (filters.Page.HasValue)
                parameters["page"] = filters.Page.Value.ToString();

            if (filters.PageSize.HasValue)
                parameters["page_size"] = filters.PageSize.Value.ToString();

            if (!string.IsNullOrEmpty(filters.Sort))
                parameters["sort"] = filters.Sort;

            if (!string.IsNullOrEmpty(filters.Status))
                parameters["status"] = filters.Status;

            if (filters.Visibility.HasValue)
                parameters["visibility"] = filters.Visibility.Value.ToString().ToLowerInvariant();

            if (filters.EnableAdvertising.HasValue)
                parameters["enable_advertising"] = filters.EnableAdvertising.Value.ToString().ToLowerInvariant();

            if (filters.IsExplicit.HasValue)
                parameters["is_explicit"] = filters.IsExplicit.Value.ToString().ToLowerInvariant();

            if (filters.IsForKids.HasValue)
                parameters["is_for_kids"] = filters.IsForKids.Value.ToString().ToLowerInvariant();

            if (filters.CreatedAfter.HasValue)
                parameters["created_after"] = ((DateTimeOffset)filters.CreatedAfter.Value).ToUnixTimeSeconds().ToString();

            if (filters.CreatedBefore.HasValue)
                parameters["created_before"] = ((DateTimeOffset)filters.CreatedBefore.Value).ToUnixTimeSeconds().ToString();

            if (!string.IsNullOrWhiteSpace(filters.Tags))
                parameters["tags"] = filters.Tags;

            return parameters;
        }
    }
}