using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Echo response model
/// https://developers.dailymotion.com/api/platform-api/reference/#echo-response
/// </summary>
public class EchoResponse
{
    /// <summary>
    /// The echoed message
    /// </summary>
    [JsonProperty("message")]
    public string? Data { get; set; }
}
