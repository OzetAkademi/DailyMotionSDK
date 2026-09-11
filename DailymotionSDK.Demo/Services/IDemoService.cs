using DailymotionSDK.Models.Responses;

namespace DailymotionSDK.Demo.Services;

/// <summary>
/// Interface IDemoService
/// </summary>
public interface IDemoService
{
    /// <summary>
    /// Runs the demo asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task RunDemoAsync();

    /// <summary>
    /// Tests the client credentials with private keys asynchronous.
    /// </summary>
    /// <returns>Task{System.Nullable{TokenResponse}}.</returns>
    Task<TokenResponse?> TestClientCredentialsWithPrivateKeysAsync();
}