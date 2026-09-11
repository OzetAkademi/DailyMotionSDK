using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;

namespace DailymotionSDK.Services;

/// <summary>
/// Interface IDailymotionAuthService
/// </summary>
public interface IDailymotionAuthService
{
    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <value>The access token.</value>
    string? AccessToken { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is token expired.
    /// </summary>
    /// <value><c>true</c> if this instance is token expired; otherwise, <c>false</c>.</value>
    bool IsTokenExpired { get; }

    /// <summary>
    /// Authenticates the with private asynchronous.
    /// </summary>
    /// <param name="authRequest">The authentication request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task{TokenResponse}.</returns>
    Task<TokenResponse> AuthenticateWithPrivateAsync(AuthRequest authRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the tokens.
    /// </summary>
    void ClearTokens();
}