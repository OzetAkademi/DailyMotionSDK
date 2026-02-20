namespace DailymotionSDK.Models;

/// <summary>
/// OAuth scopes for DailyMotion API
/// https://developers.dailymotion.com/guides/api-scopes/
/// </summary>
public enum OAuthScope
{
    /// <summary>
    /// Provides read access. Can be used with public keys.
    /// </summary>
    Read,

    /// <summary>
    /// Provides write access. Can be used with public keys.
    /// </summary>
    Write,

    /// <summary>
    /// Provides delete access. Can be used with public keys.
    /// </summary>
    Delete,

    /// <summary>
    /// Provides access to the email of the main user of the Dailymotion channel. Can be used with public keys.
    /// </summary>
    Email,

    /// <summary>
    /// Provides read/write access to some personal user information like address and birthday. Can be used with public keys.
    /// </summary>
    UserInfo,

    /// <summary>
    /// Provides access to user feed. Can be used with public keys.
    /// </summary>
    Feed,

    /// <summary>
    /// Allows to create/edit/delete uploaded videos on behalf of the user. Can be used with both public and private API keys.
    /// </summary>
    ManageVideos,

    /// <summary>
    /// Allows to upload videos on behalf of the user. Can be used with private API keys.
    /// </summary>
    UploadVideos,

    /// <summary>
    /// Allows to read videos on behalf of the user. Can be used with private API keys.
    /// </summary>
    ReadVideos,

    /// <summary>
    /// Allows to edit videos on behalf of the user. Can be used with private API keys.
    /// </summary>
    EditVideos,

    /// <summary>
    /// Allows to delete videos on behalf of the user. Can be used with private API keys.
    /// </summary>
    DeleteVideos,

    /// <summary>
    /// Allows to manage comments on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageComments,

    /// <summary>
    /// Allows to create/edit/delete playlists on behalf of the user. Can be used with both public and private API keys.
    /// </summary>
    ManagePlaylists,

    /// <summary>
    /// Allows to manage tiles on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageTiles,

    /// <summary>
    /// Allows to subscribe to channels on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageSubscriptions,

    /// <summary>
    /// Allows to manage friends on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageFriends,

    /// <summary>
    /// Allows to manage favorites on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageFavorites,

    /// <summary>
    /// Allows to add/remove videos from the list liked video ("likes") on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageLikes,

    /// <summary>
    /// Allows to manage groups on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageGroups,

    /// <summary>
    /// Allows to manage records on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageRecords,

    /// <summary>
    /// Allows to create/edit/delete subtitles on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageSubtitles,

    /// <summary>
    /// Allows to manage features on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageFeatures,

    /// <summary>
    /// Allows to manage history on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageHistory,

    /// <summary>
    /// Provides access to IFTTT integration. Can be used with public keys.
    /// </summary>
    Ifttt,

    /// <summary>
    /// Allows to read insights data. Can be used with public keys.
    /// </summary>
    ReadInsights,

    /// <summary>
    /// Allows to manage claim rules on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageClaimRules,

    /// <summary>
    /// Allows to delegate account management. Can be used with public keys.
    /// </summary>
    DelegateAccountManagement,

    /// <summary>
    /// Allows to manage analytics on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageAnalytics,

    /// <summary>
    /// Allows to manage player on behalf of the user. Can be used with public keys.
    /// </summary>
    ManagePlayer,

    /// <summary>
    /// Allows create/modify/delete Players on behalf of user. Can be used with both public and private API keys.
    /// </summary>
    ManagePlayers,

    /// <summary>
    /// Allows to manage user settings on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageUserSettings,

    /// <summary>
    /// Allows to manage collections on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageCollections,

    /// <summary>
    /// Allows to manage app connections on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageAppConnections,

    /// <summary>
    /// Allows to manage applications on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageApplications,

    /// <summary>
    /// Allows to manage domains on behalf of the user. Can be used with public keys.
    /// </summary>
    ManageDomains,

    /// <summary>
    /// Allows to create/edit/delete podcasts on behalf of the user. Can be used with both public and private API keys.
    /// </summary>
    ManagePodcasts
}

/// <summary>
/// Extension methods for OAuthScope enum
/// </summary>
public static class OAuthScopeExtensions
{
    /// <summary>
    /// Array of OAuth scopes that can be used with public keys
    /// </summary>
    public static readonly OAuthScope[] PublicKeyScopes = new[]
    {
        OAuthScope.Read,
        OAuthScope.Write,
        OAuthScope.Delete,
        OAuthScope.Email,
        OAuthScope.UserInfo,
        OAuthScope.Feed,
        OAuthScope.ManageVideos,
        OAuthScope.ManageComments,
        OAuthScope.ManagePlaylists,
        OAuthScope.ManageTiles,
        OAuthScope.ManageSubscriptions,
        OAuthScope.ManageFriends,
        OAuthScope.ManageFavorites,
        OAuthScope.ManageLikes,
        OAuthScope.ManageGroups,
        OAuthScope.ManageRecords,
        OAuthScope.ManageSubtitles,
        OAuthScope.ManageFeatures,
        OAuthScope.ManageHistory,
        OAuthScope.Ifttt,
        OAuthScope.ReadInsights,
        OAuthScope.ManageClaimRules,
        OAuthScope.DelegateAccountManagement,
        OAuthScope.ManageAnalytics,
        OAuthScope.ManagePlayer,
        OAuthScope.ManagePlayers,
        OAuthScope.ManageUserSettings,
        OAuthScope.ManageCollections,
        OAuthScope.ManageAppConnections,
        OAuthScope.ManageApplications,
        OAuthScope.ManageDomains,
        OAuthScope.ManagePodcasts
    };

    /// <summary>
    /// Array of OAuth scopes that can be used with private API keys
    /// </summary>
    public static readonly OAuthScope[] PrivateKeyScopes = new[]
    {
        OAuthScope.ManageVideos,
        OAuthScope.ManagePlaylists,
        OAuthScope.ManagePodcasts,
        OAuthScope.UploadVideos,
        OAuthScope.ReadVideos,
        OAuthScope.EditVideos,
        OAuthScope.DeleteVideos,
        OAuthScope.ManagePlayers
    };

    /// <summary>
    /// Converts the OAuthScope enum value to the correct API scope string format
    /// </summary>
    /// <param name="scope">The OAuth scope enum value</param>
    /// <returns>The API scope string in the correct format</returns>
    public static string ToApiScopeString(this OAuthScope scope)
    {
        return scope switch
        {
            OAuthScope.Read => "read",
            OAuthScope.Write => "write",
            OAuthScope.Delete => "delete",
            OAuthScope.Email => "email",
            OAuthScope.UserInfo => "userinfo",
            OAuthScope.Feed => "feed",
            OAuthScope.ManageVideos => "manage_videos",
            OAuthScope.UploadVideos => "upload_videos",
            OAuthScope.ReadVideos => "read_videos",
            OAuthScope.EditVideos => "edit_videos",
            OAuthScope.DeleteVideos => "delete_videos",
            OAuthScope.ManageComments => "manage_comments",
            OAuthScope.ManagePlaylists => "manage_playlists",
            OAuthScope.ManageTiles => "manage_tiles",
            OAuthScope.ManageSubscriptions => "manage_subscriptions",
            OAuthScope.ManageFriends => "manage_friends",
            OAuthScope.ManageFavorites => "manage_favorites",
            OAuthScope.ManageLikes => "manage_likes",
            OAuthScope.ManageGroups => "manage_groups",
            OAuthScope.ManageRecords => "manage_records",
            OAuthScope.ManageSubtitles => "manage_subtitles",
            OAuthScope.ManageFeatures => "manage_features",
            OAuthScope.ManageHistory => "manage_history",
            OAuthScope.Ifttt => "ifttt",
            OAuthScope.ReadInsights => "read_insights",
            OAuthScope.ManageClaimRules => "manage_claim_rules",
            OAuthScope.DelegateAccountManagement => "delegate_account_management",
            OAuthScope.ManageAnalytics => "manage_analytics",
            OAuthScope.ManagePlayer => "manage_player",
            OAuthScope.ManagePlayers => "manage_players",
            OAuthScope.ManageUserSettings => "manage_user_settings",
            OAuthScope.ManageCollections => "manage_collections",
            OAuthScope.ManageAppConnections => "manage_app_connections",
            OAuthScope.ManageApplications => "manage_applications",
            OAuthScope.ManageDomains => "manage_domains",
            OAuthScope.ManagePodcasts => "manage_podcasts",
            _ => throw new ArgumentException($"Unknown OAuth scope: {scope}", nameof(scope))
        };
    }
}
