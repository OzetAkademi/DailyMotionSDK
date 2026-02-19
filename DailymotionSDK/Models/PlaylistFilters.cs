namespace DailymotionSDK.Models;

/// <summary>
/// Filters for playlist queries
/// https://developers.dailymotion.com/api/platform-api/reference/#playlist
/// </summary>
public class PlaylistFilters
{
    /// <summary>
    /// Limit the result set to this list of playlist identifiers (works only with xids)
    /// </summary>
    public string[]? Ids { get; set; }

    /// <summary>
    /// Limit the result set to playlists of this user
    /// </summary>
    public string? Owner { get; set; }

    /// <summary>
    /// Limit the result set to private playlists
    /// </summary>
    public bool? Private { get; set; }

    /// <summary>
    /// Limit the result set to this full text search
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Change the default result set ordering
    /// </summary>
    public PlaylistSort? Sort { get; set; }

    /// <summary>
    /// Limit the result set to verified playlists
    /// </summary>
    public bool? Verified { get; set; }

    /// <summary>
    /// Converts the filters to a dictionary for API requests
    /// </summary>
    /// <returns>Dictionary of filter parameters</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var parameters = new Dictionary<string, string>();

        if (Ids != null && Ids.Length > 0)
            parameters["ids"] = string.Join(",", Ids);

        if (!string.IsNullOrWhiteSpace(Owner))
            parameters["owner"] = Owner;

        if (Private.HasValue)
            parameters["private"] = Private.Value.ToString().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(Search))
            parameters["search"] = Search;

        if (Sort.HasValue)
            parameters["sort"] = Sort.Value.ToApiSortString();

        if (Verified.HasValue)
            parameters["verified"] = Verified.Value.ToString().ToLowerInvariant();

        return parameters;
    }
}
