namespace DailymotionSDK.Models;

/// <summary>
/// Enum OAuthScope
/// </summary>
public enum OAuthScope
{
    /// <summary>
    /// The manage account
    /// </summary>
    ManageAccount,
    /// <summary>
    /// The read account
    /// </summary>
    ReadAccount,
    /// <summary>
    /// The manage live
    /// </summary>
    ManageLive,
    /// <summary>
    /// The read live
    /// </summary>
    ReadLive,
    /// <summary>
    /// The manage organization
    /// </summary>
    ManageOrganization,
    /// <summary>
    /// The read organization
    /// </summary>
    ReadOrganization,
    /// <summary>
    /// The manage player
    /// </summary>
    ManagePlayer,
    /// <summary>
    /// The read player
    /// </summary>
    ReadPlayer,
    /// <summary>
    /// The manage playlist
    /// </summary>
    ManagePlaylist,
    /// <summary>
    /// The read playlist
    /// </summary>
    ReadPlaylist,
    /// <summary>
    /// The manage profile
    /// </summary>
    ManageProfile,
    /// <summary>
    /// The read profile
    /// </summary>
    ReadProfile,
    /// <summary>
    /// The manage video
    /// </summary>
    ManageVideo,
    /// <summary>
    /// The read video
    /// </summary>
    ReadVideo
}

/// <summary>
/// Class OAuthScopeExtensions.
/// </summary>
public static class OAuthScopeExtensions
{
    /// <summary>
    /// The private key scopes
    /// </summary>
    public static readonly OAuthScope[] PrivateKeyScopes =
    [
        OAuthScope.ManageAccount,
        OAuthScope.ManageLive,
        OAuthScope.ManageOrganization,
        OAuthScope.ManagePlayer,
        OAuthScope.ManagePlaylist,
        OAuthScope.ManageProfile,
        OAuthScope.ManageVideo
    ];

    /// <summary>
    /// Converts to apiscopestring.
    /// </summary>
    /// <param name="scope">The scope.</param>
    /// <returns>System.String.</returns>
    public static string ToApiScopeString(this OAuthScope scope)
    {
        return scope switch
        {
            OAuthScope.ManageAccount => "account.manage",
            OAuthScope.ReadAccount => "account.read",
            OAuthScope.ManageLive => "live.manage",
            OAuthScope.ReadLive => "live.read",
            OAuthScope.ManageOrganization => "organization.manage",
            OAuthScope.ReadOrganization => "organization.read",
            OAuthScope.ManagePlayer => "player.manage",
            OAuthScope.ReadPlayer => "player.read",
            OAuthScope.ManagePlaylist => "playlist.manage",
            OAuthScope.ReadPlaylist => "playlist.read",
            OAuthScope.ManageProfile => "profile.manage",
            OAuthScope.ReadProfile => "profile.read",
            OAuthScope.ManageVideo => "video.manage",
            OAuthScope.ReadVideo => "video.read",
            _ => throw new ArgumentException($"Unknown OAuth scope: {scope}", nameof(scope))
        };
    }
}
