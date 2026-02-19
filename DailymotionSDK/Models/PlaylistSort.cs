namespace DailymotionSDK.Models;

/// <summary>
/// Playlist sort options for DailyMotion API
/// https://developers.dailymotion.com/api/platform-api/reference/#playlist
///
/// recent, relevance, alpha, most, least, alphaaz, alphaza, changed
/// </summary>
public enum PlaylistSort
{
    /// <summary>
    /// Sort by most recent playlists
    /// </summary>
    Recent,
    
    /// <summary>
    /// Sort by relevance to search query
    /// </summary>
    Relevance,
    
    /// <summary>
    /// Sort alphabetically
    /// </summary>
    Alpha,
    
    /// <summary>
    /// Sort by most
    /// </summary>
    Most,
    
    /// <summary>
    /// Sort by least
    /// </summary>
    Least,
    
    /// <summary>
    /// Sort alphabetically A-Z
    /// </summary>
    AlphaAz,
    
    /// <summary>
    /// Sort alphabetically Z-A
    /// </summary>
    AlphaZa,
    
    /// <summary>
    /// Sort by changed date
    /// </summary>
    Changed
}

/// <summary>
/// Extension methods for PlaylistSort enum
/// </summary>
public static class PlaylistSortExtensions
{
    /// <summary>
    /// Converts the PlaylistSort enum value to the correct API sort string format
    /// </summary>
    /// <param name="sort">The PlaylistSort enum value</param>
    /// <returns>The API sort string in the correct format</returns>
    public static string ToApiSortString(this PlaylistSort sort)
    {
        return sort switch
        {
            PlaylistSort.Recent => "recent",
            PlaylistSort.Relevance => "relevance",
            PlaylistSort.Alpha => "alpha",
            PlaylistSort.Most => "most",
            PlaylistSort.Least => "least",
            PlaylistSort.AlphaAz => "alphaaz",
            PlaylistSort.AlphaZa => "alphaza",
            PlaylistSort.Changed => "changed",
            _ => throw new ArgumentException($"Unknown PlaylistSort value: {sort}", nameof(sort))
        };
    }
}
