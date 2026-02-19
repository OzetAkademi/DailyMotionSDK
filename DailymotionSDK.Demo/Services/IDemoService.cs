namespace DailymotionSDK.Demo.Services;

/// <summary>
/// Interface for the demo service that tests all SDK functionality
/// </summary>
public interface IDemoService
{
    /// <summary>
    /// Runs the complete demo testing all SDK functionality
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task RunDemoAsync();

    /// <summary>
    /// Tests the SDK functionality using password authentication (user-level access)
    /// This flow skips echo and auth tests, but tests all other functions including /me endpoints
    /// </summary>
    /// <returns>List of created video IDs</returns>
    Task<List<string>> TestPasswordAuthenticationFlowAsync();

    /// <summary>
    /// Tests the SDK without dependency injection
    /// </summary>
    /// <returns>Task representing the async operation</returns>
}
