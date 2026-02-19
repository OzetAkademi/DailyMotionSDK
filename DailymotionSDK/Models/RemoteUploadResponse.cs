namespace DailymotionSDK.Models;

/// <summary>
/// Remote upload response containing details about a video uploaded from a remote URL
/// Returned when a remote video has been successfully uploaded and processed
/// </summary>
public class RemoteUploadResponse
{
    /// <summary>
    /// Unique identifier of the remotely uploaded video
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Title of the remotely uploaded video
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Owner/creator of the remotely uploaded video
    /// </summary>
    public string? Owner { get; set; }

    /// <summary>
    /// Channel where the video was uploaded
    /// </summary>
    public string? Channel { get; set; }
}
