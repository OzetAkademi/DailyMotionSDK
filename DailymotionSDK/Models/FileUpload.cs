using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class FileUpload.
/// </summary>
public class FileUpload
{
    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the audio codec.
    /// </summary>
    /// <value>The audio codec.</value>
    [JsonPropertyName("acodec")]
    public string? AudioCodec { get; set; }

    /// <summary>
    /// Gets or sets the bitrate.
    /// </summary>
    /// <value>The bitrate.</value>
    [JsonPropertyName("bitrate")]
    public string? Bitrate { get; set; }

    /// <summary>
    /// Gets or sets the dimension.
    /// </summary>
    /// <value>The dimension.</value>
    [JsonPropertyName("dimension")]
    public string? Dimension { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    /// <value>The duration.</value>
    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    /// <value>The format.</value>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the hash.
    /// </summary>
    /// <value>The hash.</value>
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the seal.
    /// </summary>
    /// <value>The seal.</value>
    [JsonPropertyName("seal")]
    public string? Seal { get; set; }

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    /// <value>The size.</value>
    [JsonPropertyName("size")]
    public string? Size { get; set; }

    /// <summary>
    /// Gets or sets the streamable.
    /// </summary>
    /// <value>The streamable.</value>
    [JsonPropertyName("streamable")]
    public string? Streamable { get; set; }

    /// <summary>
    /// Gets or sets the video codec.
    /// </summary>
    /// <value>The video codec.</value>
    [JsonPropertyName("vcodec")]
    public string? VideoCodec { get; set; }

    /// <summary>
    /// Gets the file identifier.
    /// </summary>
    /// <returns>System.Nullable{System.String}.</returns>
    public string? GetFileId()
    {
        if (string.IsNullOrWhiteSpace(Url))
            return null;

        if (Uri.TryCreate(Url, UriKind.Absolute, out var uri))
        {
            var fileName = Path.GetFileNameWithoutExtension(uri.AbsolutePath);

            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is streamable.
    /// </summary>
    /// <value><c>true</c> if this instance is streamable; otherwise, <c>false</c>.</value>
    public bool IsStreamable => string.Equals(Streamable, "yes", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the file size bytes.
    /// </summary>
    /// <value>The file size bytes.</value>
    public long? FileSizeBytes => long.TryParse(Size, out var size) ? size : null;

    /// <summary>
    /// Gets the duration seconds.
    /// </summary>
    /// <value>The duration seconds.</value>
    public double? DurationSeconds => long.TryParse(Duration, out var duration) ? duration / 1000.0 : null;
}