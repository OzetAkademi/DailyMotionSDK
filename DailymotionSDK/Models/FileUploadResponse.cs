using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// File upload response model
/// https://developers.dailymotion.com/api/platform-api/reference/#file-upload-response
/// </summary>
public class FileUploadResponse
{
    /// <summary>
    /// The uploaded file URL
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The file ID (extracted from URL if not provided directly)
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }


    /// <summary>
    /// Audio codec used in the file
    /// </summary>
    [JsonProperty("acodec")]
    public string? AudioCodec { get; set; }

    /// <summary>
    /// Bitrate of the file
    /// </summary>
    [JsonProperty("bitrate")]
    public string? Bitrate { get; set; }

    /// <summary>
    /// Video dimensions (e.g., "1280x720")
    /// </summary>
    [JsonProperty("dimension")]
    public string? Dimension { get; set; }

    /// <summary>
    /// Duration of the video in milliseconds
    /// </summary>
    [JsonProperty("duration")]
    public string? Duration { get; set; }

    /// <summary>
    /// File format (e.g., "MPEG-4")
    /// </summary>
    [JsonProperty("format")]
    public string? Format { get; set; }

    /// <summary>
    /// File hash
    /// </summary>
    [JsonProperty("hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// File name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// File seal (integrity check)
    /// </summary>
    [JsonProperty("seal")]
    public string? Seal { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    [JsonProperty("size")]
    public string? Size { get; set; }

    /// <summary>
    /// Whether the file is streamable
    /// </summary>
    [JsonProperty("streamable")]
    public string? Streamable { get; set; }

    /// <summary>
    /// Video codec used in the file
    /// </summary>
    [JsonProperty("vcodec")]
    public string? VideoCodec { get; set; }

    /// <summary>
    /// Gets the file ID, extracting it from the URL if not provided directly
    /// </summary>
    public string? GetFileId()
    {
        if (!string.IsNullOrEmpty(Id))
            return Id;

        if (string.IsNullOrEmpty(Url))
            return null;

        // Extract file ID from URL like: https://upload-01.dc3.dailymotion.com/files/2e9b0be012e8a2f855aa88b7209878f5.mp4#...
        try
        {
            var uri = new Uri(Url);
            var pathSegments = uri.AbsolutePath.Split('/');
            if (pathSegments.Length > 0)
            {
                var fileName = pathSegments[pathSegments.Length - 1];
                // Remove the .mp4 extension to get the file ID
                if (fileName.EndsWith(".mp4"))
                {
                    return fileName.Substring(0, fileName.Length - 4);
                }
                return fileName;
            }
        }
        catch
        {
            // If URL parsing fails, return null
        }

        return null;
    }


    /// <summary>
    /// Gets whether the file is streamable (convenience property)
    /// </summary>
    public bool IsStreamable => Streamable?.ToLowerInvariant() == "yes";

    /// <summary>
    /// Gets the file size as a long integer (convenience property)
    /// </summary>
    public long? FileSizeBytes => long.TryParse(Size, out var size) ? size : null;

    /// <summary>
    /// Gets the duration in seconds (convenience property)
    /// </summary>
    public double? DurationSeconds => long.TryParse(Duration, out var duration) ? duration / 1000.0 : null;
}
