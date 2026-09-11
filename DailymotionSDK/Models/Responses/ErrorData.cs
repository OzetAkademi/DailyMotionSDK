using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Responses;

/// <summary>
/// Class ErrorData.
/// </summary>
public class ErrorData
{
    /// <summary>
    /// Error code or identifier for the error
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of the error
    /// Provides additional context about what went wrong
    /// </summary>
    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;
}