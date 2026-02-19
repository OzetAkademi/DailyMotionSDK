namespace DailymotionSDK.Models;

/// <summary>
/// Privacy settings for videos in DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#video
/// </summary>
public enum Privacy
{
    /// <summary>
    /// Video is publicly accessible
    /// </summary>
    Public,
    
    /// <summary>
    /// Video is private and only accessible to the owner
    /// </summary>
    Private,
    
    /// <summary>
    /// Video is password protected
    /// </summary>
    Password
}
