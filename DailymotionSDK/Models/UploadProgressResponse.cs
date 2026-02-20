using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Upload progress response model
/// Used to monitor file upload progress via the progress URL
/// </summary>
public class UploadProgressResponse
{
    /// <summary>
    /// The current status of the upload
    /// Possible values: "uploading", "processing", "completed", "failed"
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// The upload progress percentage (0-100)
    /// </summary>
    [JsonPropertyName("progress")]
    public int? Progress { get; set; }

    /// <summary>
    /// Additional status message or error information
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// The file URL when upload is completed
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets whether the upload is completed
    /// </summary>
    public bool IsCompleted => Status?.ToLowerInvariant() == "completed";

    /// <summary>
    /// Gets whether the upload has failed
    /// </summary>
    public bool IsFailed => Status?.ToLowerInvariant() == "failed";

    /// <summary>
    /// Gets whether the upload is still in progress
    /// </summary>
    public bool IsInProgress => Status?.ToLowerInvariant() == "uploading" || Status?.ToLowerInvariant() == "processing";
}
