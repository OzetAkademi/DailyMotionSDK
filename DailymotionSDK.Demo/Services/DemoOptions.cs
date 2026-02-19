namespace DailymotionSDK.Demo.Services;

/// <summary>
/// Configuration options for the demo application
/// </summary>
public class DemoOptions
{


    /// <summary>
    /// Redirect URI for authorization code flow
    /// </summary>
    public string AuthorizationCodeRedirectUri { get; set; } = "https://your-app.com/callback";

    /// <summary>
    /// Whether to enable interactive authorization code flow
    /// </summary>
    public bool EnableInteractiveAuth { get; set; } = false;

    /// <summary>
    /// Path to first test video file
    /// </summary>
    public string TestVideoPath1 { get; set; } = "test1.mp4";

    /// <summary>
    /// Test video file path 2
    /// </summary>
    public string TestVideoPath2 { get; set; } = "test2.mp4";

    /// <summary>
    /// Test player ID for player operations
    /// </summary>
    public string TestPlayerId { get; set; } = "YOUR_PLAYER_ID_HERE";

    /// <summary>
    /// Whether to cleanup test resources after tests
    /// </summary>
    public bool CleanupAfterTests { get; set; } = true;

    /// <summary>
    /// Milliseconds to wait between operations (to avoid rate limiting)
    /// </summary>
    public int WaitBetweenOperations { get; set; } = 2000;
}
