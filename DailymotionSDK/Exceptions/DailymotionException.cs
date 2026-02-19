namespace DailymotionSDK.Exceptions;

/// <summary>
/// Exception thrown when DailyMotion API operations fail
/// </summary>
public class DailymotionException : Exception
{
    /// <summary>
    /// Gets the error code associated with this exception
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the DailymotionException class
    /// </summary>
    /// <param name="message">The error message</param>
    public DailymotionException(string message) : base(message)
    {
        ErrorCode = 0;
    }

    /// <summary>
    /// Initializes a new instance of the DailymotionException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="errorCode">The error code</param>
    public DailymotionException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the DailymotionException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public DailymotionException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = 0;
    }

    /// <summary>
    /// Initializes a new instance of the DailymotionException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="errorCode">The error code</param>
    /// <param name="innerException">The inner exception</param>
    public DailymotionException(string message, int errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
