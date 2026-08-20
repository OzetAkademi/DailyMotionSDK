namespace DailymotionSDK.Models;

/// <summary>
/// Enum VideoSort
/// </summary>
public enum VideoSort
{
    /// <summary>
    /// The created at
    /// </summary>
    CreatedAt
}

/// <summary>
/// Class VideoSortExtensions.
/// </summary>
public static class VideoSortExtensions
{
    /// <summary>
    /// Converts to apisortstring.
    /// </summary>
    /// <param name="sort">The sort.</param>
    /// <returns>System.String.</returns>
    public static string ToApiSortString(this VideoSort sort)
    {
        return sort switch
        {
            VideoSort.CreatedAt => "created_at",
            _ => throw new ArgumentException($"Unknown VideoSort value: {sort}", nameof(sort))
        };
    }
}