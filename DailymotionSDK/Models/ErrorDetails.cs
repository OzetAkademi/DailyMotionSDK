using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Error details containing comprehensive error information
/// Provides structured error data with code, message, and type information
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Numeric error code identifying the specific error
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// Human-readable error message describing what went wrong
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error type or category (e.g., "validation_error", "authentication_error")
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
}
