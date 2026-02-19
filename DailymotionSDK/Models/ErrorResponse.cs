using System.ComponentModel;

namespace DailymotionSDK.Models;

/// <summary>
/// Error response from DailyMotion API containing error details and metadata
/// Used when API requests fail or encounter errors
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Detailed error information including code, message, and type
    /// Hidden from designer and serialization for cleaner API
    /// </summary>
    [Browsable(false)]
    [Bindable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ErrorDetails? Error { get; set; }
    
    /// <summary>
    /// Convenience property to get the error message
    /// Returns the error message from the ErrorDetails object
    /// </summary>
    public string? ErrorMessage => Error?.Message;
    
    /// <summary>
    /// Convenience property to get the error code
    /// Returns the error code from the ErrorDetails object, or 0 if no error
    /// </summary>
    public int ErrorCode => Error?.Code ?? 0;
}
