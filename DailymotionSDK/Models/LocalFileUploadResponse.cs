namespace DailymotionSDK.Models;

/// <summary>
/// Local file upload response containing details about an uploaded file
/// Returned when a local file has been successfully uploaded to the platform
/// </summary>
public class LocalFileUploadResponse
{
    /// <summary>
    /// Unique identifier of the uploaded file
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Audio codec used in the uploaded file
    /// </summary>
    public string? AudioCodec { get; set; }

    /// <summary>
    /// Bitrate of the uploaded file
    /// </summary>
    public string? Bitrate { get; set; }

    /// <summary>
    /// Video dimensions (width x height)
    /// </summary>
    public string? Dimension { get; set; }

    /// <summary>
    /// Duration of the uploaded file in seconds
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// File format of the uploaded file
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Hash of the uploaded file for integrity verification
    /// </summary>
    public string? Hash { get; set; }

    /// <summary>
    /// Name of the uploaded file
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Seal for file integrity verification
    /// </summary>
    public string? Seal { get; set; }

    /// <summary>
    /// Size of the uploaded file in bytes
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Whether the file is streamable
    /// </summary>
    public string? Streamable { get; set; }

    /// <summary>
    /// URL to access the uploaded file
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Video codec used in the uploaded file
    /// </summary>
    public string? VideoCodec { get; set; }
}
