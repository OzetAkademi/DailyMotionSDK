using DailymotionSDK.Models;
using System.ComponentModel.DataAnnotations;

namespace DailymotionSDK.Configuration;

/// <summary>
/// Type of API key for authentication
/// </summary>
public enum ApiKeyType
{
    /// <summary>
    /// Public API key - uses https://api.dailymotion.com/ endpoints
    /// </summary>
    Public,
    
    /// <summary>
    /// Private API key - uses https://partner.api.dailymotion.com/ for auth and https://partner.api.dailymotion.com/rest for API calls
    /// </summary>
    Private
}

/// <summary>
/// Configuration options for DailyMotion API client
/// </summary>
public class DailymotionOptions
{
    /// <summary>
    /// DailyMotion API base URL
    /// </summary>
    public const string DefaultApiBaseUrl = "https://api.dailymotion.com";
    
    /// <summary>
    /// DailyMotion OAuth base URL
    /// </summary>
    public const string DefaultOAuthBaseUrl = "https://www.dailymotion.com/oauth";
    
    /// <summary>
    /// API base URL (defaults to https://api.dailymotion.com)
    /// </summary>
    [Required]
    public string ApiBaseUrl { get; set; } = DefaultApiBaseUrl;
    
    /// <summary>
    /// OAuth base URL (defaults to https://www.dailymotion.com/oauth)
    /// </summary>
    [Required]
    public string OAuthBaseUrl { get; set; } = DefaultOAuthBaseUrl;

    /// <summary>
    /// Type of API key (Public or Private) - determines which endpoints to use
    /// </summary>
    public ApiKeyType ApiKeyType { get; set; } = ApiKeyType.Public;

    /// <summary>
    /// Username for password authentication
    /// </summary>
    public string PasswordAuthUsername { get; set; } = "";

    /// <summary>
    /// Password for password authentication
    /// </summary>
    public string PasswordAuthPassword { get; set; } = "";

    /// <summary>
    /// Private API Key for client credentials authentication
    /// </summary>
    public string PrivateApiKey { get; set; } = "";

    /// <summary>
    /// Private API Secret for client credentials authentication
    /// </summary>
    public string PrivateApiSecret { get; set; } = "";

    /// <summary>
    /// Public API Key for client credentials authentication
    /// </summary>
    public string PublicApiKey { get; set; } = "";

    /// <summary>
    /// Public API Secret for client credentials authentication
    /// </summary>
    public string PublicApiSecret { get; set; } = "";
    
    /// <summary>
    /// Redirect URI for OAuth flow
    /// </summary>
    public string? RedirectUri { get; set; }
    
    /// <summary>
    /// Default timeout for HTTP requests (defaults to 60 seconds)
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);
    
    /// <summary>
    /// Maximum number of retries for failed requests
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// User agent string for HTTP requests
    /// </summary>
    public string UserAgent { get; set; } = "DailymotionSDK/2.0.0";
    
    /// <summary>
    /// Whether to enable request/response logging
    /// </summary>
    public bool EnableLogging { get; set; } = false;

    /// <summary>
    /// Optional client-wide default global API parameters.
    /// Per-call GlobalApiParameters passed to HTTP methods override these.
    /// </summary>
    public GlobalApiParameters? DefaultGlobalApiParameters { get; set; }
} 