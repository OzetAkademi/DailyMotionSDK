namespace DailymotionSDK.Models;

/// <summary>
/// Video sort options for DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#video
///
/// recent, visited, visited-hour, visited-today, visited-week, visited-month, relevance, random, trending, old, live-audience, least-visited, live-airing-time, id-asc
/// </summary>
public enum VideoSort
{
    /// <summary>
    /// Sort by most recent videos
    /// </summary>
    Recent,
    
    /// <summary>
    /// Sort by visited videos
    /// </summary>
    Visited,
    
    /// <summary>
    /// Sort by visited in the last hour
    /// </summary>
    VisitedHour,
    
    /// <summary>
    /// Sort by visited today
    /// </summary>
    VisitedToday,
    
    /// <summary>
    /// Sort by visited this week
    /// </summary>
    VisitedWeek,
    
    /// <summary>
    /// Sort by visited this month
    /// </summary>
    VisitedMonth,
    
    /// <summary>
    /// Sort by relevance to search query
    /// </summary>
    Relevance,
    
    /// <summary>
    /// Sort randomly
    /// </summary>
    Random,
    
    /// <summary>
    /// Sort by trending videos
    /// </summary>
    Trending,
    
    /// <summary>
    /// Sort by oldest videos first
    /// </summary>
    Old,
    
    /// <summary>
    /// Sort by live audience
    /// </summary>
    LiveAudience,
    
    /// <summary>
    /// Sort by least visited videos
    /// </summary>
    LeastVisited,
    
    /// <summary>
    /// Sort by live airing time
    /// </summary>
    LiveAiringTime,
    
    /// <summary>
    /// Sort by ID ascending
    /// </summary>
    IdAsc
}

/// <summary>
/// Extension methods for VideoSort enum
/// </summary>
public static class VideoSortExtensions
{
    /// <summary>
    /// Converts the VideoSort enum value to the correct API sort string format
    /// </summary>
    /// <param name="sort">The VideoSort enum value</param>
    /// <returns>The API sort string in the correct format</returns>
    public static string ToApiSortString(this VideoSort sort)
    {
        return sort switch
        {
            VideoSort.Recent => "recent",
            VideoSort.Visited => "visited",
            VideoSort.VisitedHour => "visited-hour",
            VideoSort.VisitedToday => "visited-today",
            VideoSort.VisitedWeek => "visited-week",
            VideoSort.VisitedMonth => "visited-month",
            VideoSort.Relevance => "relevance",
            VideoSort.Random => "random",
            VideoSort.Trending => "trending",
            VideoSort.Old => "old",
            VideoSort.LiveAudience => "live-audience",
            VideoSort.LeastVisited => "least-visited",
            VideoSort.LiveAiringTime => "live-airing-time",
            VideoSort.IdAsc => "id-asc",
            _ => throw new ArgumentException($"Unknown VideoSort value: {sort}", nameof(sort))
        };
    }
}
