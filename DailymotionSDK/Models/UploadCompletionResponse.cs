namespace DailymotionSDK.Models;

/// <summary>
/// Upload completion response containing details about a successfully uploaded video
/// Returned when a file upload has been completed and processed
/// </summary>
public class UploadCompletionResponse
{
    /// <summary>
    /// Unique identifier of the uploaded video
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Title of the uploaded video
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Owner/creator of the uploaded video
    /// </summary>
    public string? Owner { get; set; }

    /// <summary>
    /// Channel where the video was uploaded
    /// </summary>
    public string? Channel { get; set; }
}
