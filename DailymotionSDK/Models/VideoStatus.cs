namespace DailymotionSDK.Models;

/// <summary>
/// Video status enumeration for DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#video
/// </summary>
public enum VideoStatus
{
    /// <summary>
    /// Video is waiting to be processed
    /// </summary>
    Waiting,
    
    /// <summary>
    /// Video is currently being processed
    /// </summary>
    Processing,
    
    /// <summary>
    /// Video processing is complete and ready
    /// </summary>
    Ready,
    
    /// <summary>
    /// Video has been published
    /// </summary>
    Published,
    
    /// <summary>
    /// Video has been rejected
    /// </summary>
    Rejected,
    
    /// <summary>
    /// Video has been deleted
    /// </summary>
    Deleted,
    
    /// <summary>
    /// Video encoding encountered an error
    /// </summary>
    EncodingError
}
