namespace DailymotionSDK.Models;

/// <summary>
/// Video filters for DailyMotion API queries
/// https://developers.dailymotion.com/api/platform-api/reference/#video-filters
/// </summary>
public class VideoFilters
{
    /// <summary>
    /// Limit the result set to 360 videos.
    /// [FLAG] - Added to 'flags' parameter as '360_degree'
    /// </summary>
    public bool? ThreeSixtyDegree { get; set; }

    /// <summary>
    /// Limit the result set to advertising_instream_blocked
    /// [FLAG] - Added to 'flags' parameter as 'advertising_instream_blocked'
    /// </summary>
    public bool? AdvertisingInstreamBlocked { get; set; }

    /// <summary>
    /// Limit the results to videos which are allowed in playlists.
    /// [FLAG] - Added to 'flags' parameter as 'allowed_in_playlists'
    /// </summary>
    public bool? AllowedInPlaylists { get; set; }

    /// <summary>
    /// Limit the result set to available videos.
    /// [FLAG] - Added to 'flags' parameter as 'availability'
    /// </summary>
    public bool? Availability { get; set; }

    /// <summary>
    /// Limit the result set to this channel.
    /// [PARAMETER] - Added as 'channel' query parameter
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    /// Limit the result set to this country (declarative).
    /// [PARAMETER] - Added as 'country' query parameter
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Limit the result set to videos created after this date and time.
    /// [PARAMETER] - Added as 'created_after' query parameter (Unix timestamp)
    /// </summary>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// Limit the result set to videos created before this date and time.
    /// [PARAMETER] - Added as 'created_before' query parameter (Unix timestamp)
    /// </summary>
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// List of channels ids to exclude from the result set.
    /// [PARAMETER] - Added as 'exclude_channel_ids' query parameter (comma-separated)
    /// </summary>
    public string[]? ExcludeChannelIds { get; set; }

    /// <summary>
    /// List of video ids to exclude from the result set.
    /// [PARAMETER] - Added as 'exclude_ids' query parameter (comma-separated)
    /// </summary>
    public string[]? ExcludeIds { get; set; }

    /// <summary>
    /// Limit the result set to the provided explicit value for videos.
    /// [PARAMETER] - Added as 'explicit' query parameter (true/false)
    /// </summary>
    public bool? Explicit { get; set; }

    /// <summary>
    /// Limit the result set to exportable (i.e: no-export flag not set) videos.
    /// [FLAG] - Added to 'flags' parameter as 'exportable'
    /// </summary>
    public bool? Exportable { get; set; }

    /// <summary>
    /// Limit the result set to featured videos.
    /// [FLAG] - Added to 'flags' parameter as 'featured'
    /// </summary>
    public bool? Featured { get; set; }

    /// <summary>
    /// List of simple boolean flags available to reduce the result set.
    /// [FLAG] - Added to 'flags' parameter (comma-separated)
    /// Allowed values: featured, hd, ugc, ugc_partner, live, no_live, live_onair, live_offair, live_upcoming, no_live_recording, has_game, premium, no_premium, in_history, 360_degree, availability, is_created_for_kids, explicit, private, unpublished, exportable, allowed_in_playlists, advertising_instream_blocked, partner, verified, password_protected
    /// </summary>
    public string[]? Flags { get; set; }

    /// <summary>
    /// Limit the result set to videos related to a video-game.
    /// [FLAG] - Added to 'flags' parameter as 'has_game'
    /// </summary>
    public bool? HasGame { get; set; }

    /// <summary>
    /// Limit the result set to high definition videos (vertical resolution greater than or equal to 720p).
    /// [FLAG] - Added to 'flags' parameter as 'hd'
    /// </summary>
    public bool? Hd { get; set; }

    /// <summary>
    /// Limit the result set to this list of video identifiers (works only with xids).
    /// [PARAMETER] - Added as 'ids' query parameter (comma-separated)
    /// </summary>
    public string[]? Ids { get; set; }

    /// <summary>
    /// Limit the result set to videos in your watch history.
    /// [FLAG] - Added to 'flags' parameter as 'in_history'
    /// </summary>
    public bool? InHistory { get; set; }

    /// <summary>
    /// Limit the result set to the provided is_created_for_kids value for videos.
    /// [PARAMETER] - Added as 'is_created_for_kids' query parameter (true/false)
    /// </summary>
    public bool? IsCreatedForKids { get; set; }

    /// <summary>
    /// Limit the result set to this list of languages. Language is declarative and corresponds to the user-declared spoken language of the video. If you wish to retrieve content curated for a specific locale, use the localization global parameter instead.
    /// [PARAMETER] - Added as 'languages' query parameter (comma-separated)
    /// </summary>
    public string[]? Languages { get; set; }

    /// <summary>
    /// Limit the result set to this video list. Warning: Can not be combined with a search.
    /// [PARAMETER] - Added as 'list' query parameter
    /// Allowed values: what-to-watch, recommended
    /// </summary>
    public string? List { get; set; }

    /// <summary>
    /// Limit the result set to live streaming videos.
    /// [FLAG] - Added to 'flags' parameter as 'live'
    /// </summary>
    public bool? Live { get; set; }

    /// <summary>
    /// Limit the result set to off-air live streaming videos.
    /// [FLAG] - Added to 'flags' parameter as 'live_offair'
    /// </summary>
    public bool? LiveOffair { get; set; }

    /// <summary>
    /// Limit the result set to on-air live streaming videos.
    /// [FLAG] - Added to 'flags' parameter as 'live_onair'
    /// </summary>
    public bool? LiveOnair { get; set; }

    /// <summary>
    /// Limit the result set to upcoming live streaming videos.
    /// [FLAG] - Added to 'flags' parameter as 'live_upcoming'
    /// </summary>
    public bool? LiveUpcoming { get; set; }

    /// <summary>
    /// Limit the results to videos with a duration longer than or equal to the specified number of minutes.
    /// [PARAMETER] - Added as 'longer_than' query parameter
    /// </summary>
    public int? LongerThan { get; set; }

    /// <summary>
    /// Limit the result set to videos of this mode.
    /// [PARAMETER] - Added as 'mode' query parameter
    /// Allowed values: vod, live
    /// </summary>
    public string? Mode { get; set; }

    /// <summary>
    /// Limit the result set to non-live streaming videos.
    /// [FLAG] - Added to 'flags' parameter as 'no_live'
    /// </summary>
    public bool? NoLive { get; set; }

    /// <summary>
    /// Limit the result set to live recording videos.
    /// [FLAG] - Added to 'flags' parameter as 'no_live_recording'
    /// </summary>
    public bool? NoLiveRecording { get; set; }

    /// <summary>
    /// Limit the result set to free video content.
    /// [FLAG] - Added to 'flags' parameter as 'no_premium'
    /// </summary>
    public bool? NoPremium { get; set; }

    /// <summary>
    /// Limit the result set by excluding this genre.
    /// [PARAMETER] - Added as 'nogenre' query parameter
    /// </summary>
    public string? NoGenre { get; set; }

    /// <summary>
    /// Limit the result set to this list of user identifiers or logins.
    /// [PARAMETER] - Added as 'owners' query parameter (comma-separated)
    /// </summary>
    public string[]? Owners { get; set; }

    /// <summary>
    /// Limit the result set to partner videos.
    /// [FLAG] - Added to 'flags' parameter as 'partner'
    /// </summary>
    public bool? Partner { get; set; }

    /// <summary>
    /// Limit the result set to password protected partner videos.
    /// [PARAMETER] - Added as 'password_protected' query parameter (true/false)
    /// </summary>
    public bool? PasswordProtected { get; set; }

    /// <summary>
    /// Limit the result set to premium SVOD and TVOD video content.
    /// [FLAG] - Added to 'flags' parameter as 'premium'
    /// </summary>
    public bool? Premium { get; set; }

    /// <summary>
    /// Limit the result set to private videos.
    /// [PARAMETER] - Added as 'private' query parameter (true/false)
    /// </summary>
    public bool? Private { get; set; }

    /// <summary>
    /// Forces the recommendation result to use a specific algorithm.
    /// [PARAMETER] - Added as 'related_videos_algorithm' query parameter
    /// Allowed values: uploader-only, uploader-with-siblings, uploader-with-parent, uploader-with-children
    /// </summary>
    public string? RelatedVideosAlgorithm { get; set; }

    /// <summary>
    /// Limit the result set to this full text search.
    /// [PARAMETER] - Added as 'search' query parameter
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Limit the results to videos with a duration shorter than or equal to the specified number of minutes.
    /// [PARAMETER] - Added as 'shorter_than' query parameter
    /// </summary>
    public int? ShorterThan { get; set; }

    /// <summary>
    /// Change the default result set ordering.
    /// [PARAMETER] - Added as 'sort' query parameter
    /// Allowed values: recent, visited, visited-hour, visited-today, visited-week, visited-month, relevance, random, trending, old, live-audience, least-visited, live-airing-time, id-asc
    /// Note: the relevance filter can only be used in conjunction with the search filter.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Limit the result set to this full text search of video tags. By default perform 'AND' operation between terms. Use enclosing parenthesis '()' on the query to perform 'OR' operation.
    /// [PARAMETER] - Added as 'tags' query parameter (comma-separated)
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Limit the result set to videos created after this N last seconds.
    /// [PARAMETER] - Added as 'timeframe' query parameter
    /// </summary>
    public int? Timeframe { get; set; }

    /// <summary>
    /// Limit the result set to user generated video content (no partner content).
    /// [FLAG] - Added to 'flags' parameter as 'ugc'
    /// </summary>
    public bool? Ugc { get; set; }

    /// <summary>
    /// Limit the result set to user generated or partner video content.
    /// [FLAG] - Added to 'flags' parameter as 'ugc_partner'
    /// </summary>
    public bool? UgcPartner { get; set; }

    /// <summary>
    /// Limit the result set to unpublished videos.
    /// [PARAMETER] - Added as 'unpublished' query parameter (true/false)
    /// </summary>
    public bool? Unpublished { get; set; }

    /// <summary>
    /// Limit the result set to verified partner videos.
    /// [FLAG] - Added to 'flags' parameter as 'verified'
    /// </summary>
    public bool? Verified { get; set; }

    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    public int? Page { get; set; }

    /// <summary>
    /// Gets or sets the limit.
    /// </summary>
    /// <value>The limit.</value>
    public int? Limit { get; set; }
}