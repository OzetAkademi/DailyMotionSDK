using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class UploadSession.
/// </summary>
public class UploadSession
{
    /// <summary>
    /// Gets or sets the upload URL.
    /// </summary>
    /// <value>The upload URL.</value>
    [JsonPropertyName("upload_url")]
    public string? UploadUrl { get; set; }

    /// <summary>
    /// Gets or sets the progress URL.
    /// </summary>
    /// <value>The progress URL.</value>
    [JsonPropertyName("progress_url")]
    public string? ProgressUrl { get; set; }
}