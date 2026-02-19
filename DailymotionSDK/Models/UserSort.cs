namespace DailymotionSDK.Models;

/// <summary>
/// User sort options for DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#user
///
/// recent, relevance, popular, activity
/// </summary>
public enum UserSort
{
    /// <summary>
    /// Sort by most recent users
    /// </summary>
    Recent,
    
    /// <summary>
    /// Sort by relevance to search query
    /// </summary>
    Relevance,
    
    /// <summary>
    /// Sort by popularity
    /// </summary>
    Popular,
    
    /// <summary>
    /// Sort by activity
    /// </summary>
    Activity
}

/// <summary>
/// Extension methods for UserSort enum
/// </summary>
public static class UserSortExtensions
{
    /// <summary>
    /// Converts the UserSort enum value to the correct API sort string format
    /// </summary>
    /// <param name="sort">The UserSort enum value</param>
    /// <returns>The API sort string in the correct format</returns>
    public static string ToApiSortString(this UserSort sort)
    {
        return sort switch
        {
            UserSort.Recent => "recent",
            UserSort.Relevance => "relevance",
            UserSort.Popular => "popular",
            UserSort.Activity => "activity",
            _ => throw new ArgumentException($"Unknown UserSort value: {sort}", nameof(sort))
        };
    }
}
