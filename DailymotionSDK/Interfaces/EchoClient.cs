using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for echo operations
/// https://developers.dailymotion.com/api/platform-api/reference/#echo
/// </summary>
public class EchoClient : IEcho
{
    private readonly IDailymotionHttpClient _httpClient;
    private readonly ILogger<EchoClient> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    /// <summary>
    /// Initializes a new instance of the EchoClient
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    public EchoClient(IDailymotionHttpClient httpClient, ILogger<EchoClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    /// <summary>
    /// Tests API connectivity by echoing back the provided data
    /// https://developers.dailymotion.com/api/platform-api/reference/#using-echo
    /// </summary>
    /// <param name="data">Data to echo back</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Echo response</returns>
    public async Task<EchoResponse> EchoAsync(string data, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(data))
            throw new ArgumentException("Data cannot be null or empty", nameof(data));

        try
        {
            _logger.LogDebug("Echoing data: {Data}", data);

            var parameters = new Dictionary<string, string>
            {
                ["message"] = data
            };

            var response = await _httpClient.GetAsync("/echo", parameters, null, cancellationToken);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonConvert.DeserializeObject<EchoResponse>(response.Content, _jsonSettings);
                _logger.LogDebug("Echo response received successfully");
                return result ?? new EchoResponse();
            }

            _logger.LogWarning("Echo request failed with status: {StatusCode}", response.StatusCode);
            throw new Exception($"Echo request failed: {response.ErrorMessage ?? response.StatusDescription ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during echo operation");
            throw;
        }
    }
}
