using DailymotionSDK.Models;
using System.ComponentModel.DataAnnotations;

namespace DailymotionSDK.Configuration;

/// <summary>
/// Class DailymotionOptions.
/// </summary>
public class DailymotionOptions
{
    /// <summary>
    /// The default API base URL
    /// </summary>
    public const string DefaultApiBaseUrl = "https://api.dailymotion.com/v2";

    /// <summary>
    /// The default o authentication base URL
    /// </summary>
    public const string DefaultOAuthBaseUrl = "https://oauth2.dailymotion.com/v2/token";

    /// <summary>
    /// Gets or sets the API base URL.
    /// </summary>
    /// <value>The API base URL.</value>
    [Required]
    public string ApiBaseUrl { get; set; } = DefaultApiBaseUrl;

    /// <summary>
    /// Gets or sets the o authentication base URL.
    /// </summary>
    /// <value>The o authentication base URL.</value>
    [Required]
    public string OAuthBaseUrl { get; set; } = DefaultOAuthBaseUrl;

    /// <summary>
    /// Gets or sets the password authentication username.
    /// </summary>
    /// <value>The password authentication username.</value>
    public string PasswordAuthUsername { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password authentication password.
    /// </summary>
    /// <value>The password authentication password.</value>
    public string PasswordAuthPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the private API key.
    /// </summary>
    /// <value>The private API key.</value>
    public string PrivateApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the private API secret.
    /// </summary>
    /// <value>The private API secret.</value>
    public string PrivateApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the public API key.
    /// </summary>
    /// <value>The public API key.</value>
    public string PublicApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the public API secret.
    /// </summary>
    /// <value>The public API secret.</value>
    public string PublicApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the redirect URI.
    /// </summary>
    /// <value>The redirect URI.</value>
    public string? RedirectUri { get; set; }

    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Gets or sets the maximum retries.
    /// </summary>
    /// <value>The maximum retries.</value>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the user agent.
    /// </summary>
    /// <value>The user agent.</value>
    public string UserAgent { get; set; } = "DailymotionSDK/2.0.0";

    /// <summary>
    /// Gets or sets a value indicating whether [enable logging].
    /// </summary>
    /// <value><c>true</c> if [enable logging]; otherwise, <c>false</c>.</value>
    public bool EnableLogging { get; set; } = false;
}