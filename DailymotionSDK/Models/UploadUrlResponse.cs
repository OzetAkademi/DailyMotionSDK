using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Upload URL response model
/// https://developers.dailymotion.com/api/platform-api/reference/#file-upload
/// </summary>
public class UploadUrlResponse
{
    /// <summary>
    /// The upload URL to use for file upload
    /// </summary>
    [JsonPropertyName("upload_url")]
    public string? UploadUrl { get; set; }

    /// <summary>
    /// The progress URL to monitor upload progress
    /// </summary>
    [JsonPropertyName("progress_url")]
    public string? ProgressUrl { get; set; }

    /// <summary>
    /// Gets whether a progress URL is available for monitoring upload progress
    /// </summary>
    public bool HasProgressUrl => !string.IsNullOrEmpty(ProgressUrl);
}
