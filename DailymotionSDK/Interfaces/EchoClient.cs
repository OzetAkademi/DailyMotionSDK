using DailymotionSDK.Models;
using DailymotionSDK.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Client for echo operations
/// https://developers.dailymotion.com/api/platform-api/reference/#echo
/// </summary>
public class EchoClient : IEcho
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IDailymotionHttpClient _httpClient;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<EchoClient> _logger;
    /// <summary>
    /// The json options
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the EchoClient
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <exception cref="ArgumentNullException">httpClient</exception>
    /// <exception cref="ArgumentNullException">logger</exception>
    public EchoClient(IDailymotionHttpClient httpClient, ILogger<EchoClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true // Highly recommended for API deserialization
        };
    }

    /// <summary>
    /// Tests API connectivity by echoing back the provided data
    /// https://developers.dailymotion.com/api/platform-api/reference/#using-echo
    /// </summary>
    /// <param name="data">Data to echo back</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Echo response</returns>
    /// <exception cref="ArgumentException">Data cannot be null or empty - data</exception>
    /// <exception cref="Exception">Echo request failed: {response.ErrorMessage ?? response.StatusDescription ?? "Unknown error"}</exception>
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
                var result = JsonSerializer.Deserialize<EchoResponse>(response.Content, _jsonOptions);
                _logger.LogDebug("Echo response received successfully");
                return result ?? new();
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
