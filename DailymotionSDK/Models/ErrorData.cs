using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Error data containing OAuth and API error information
/// Used for OAuth flow errors and general API error responses
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
