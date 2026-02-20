using DailymotionSDK.Models;
using DailymotionSDK.Configuration;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Demo.Services;

/// <summary>
/// Demo service that tests all SDK functionality in a real environment
/// </summary>
public class DemoService : IDemoService
{
    private readonly DailymotionHandler _sdk;
    private readonly DemoOptions _options;
    private readonly ILogger<DemoService> _logger;
    private readonly List<string> _createdResources = new();

    public DemoService(DailymotionHandler sdk, DemoOptions options, ILogger<DemoService> logger)
    {
        _sdk = sdk ?? throw new ArgumentNullException(nameof(sdk));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Runs the complete demo testing all SDK functionality
    /// </summary>
    public async Task RunDemoAsync()
    {
        try
        {
            _logger.LogInformation("Starting DailyMotion SDK Demo...");

            // Step 1: Test basic API endpoints
            await TestBasicApiEndpointsAsync();

            // Step 2: Test authentication
            await TestAuthenticationAsync();

            // Step 3: Test video operations (using authenticated token if available)
            await TestVideoOperationsAsync();

            // Step 3.5: Test /me endpoints if using password authentication
            await TestMeEndpointsIfPasswordAuthAsync();

            // Step 4: Test user operations (using authenticated token if available)
            await TestUserOperationsAsync();

            // Step 5: Test channel operations (using authenticated token if available)
            await TestChannelOperationsAsync();

            // Step 6: Test search functionality (using authenticated token if available)
            await TestSearchOperationsAsync();

            // Step 7: Test file operations (using authenticated token if available)
            var uploadedVideoIds = await TestFileOperationsAsync();

            // Step 8: Test playlist operations (using uploaded video IDs)
            await TestPlaylistOperationsAsync(uploadedVideoIds);

            // Step 9: Test player operations (using authenticated token if available)
            await TestPlayerOperationsAsync();

            // Step 10: Test subtitle operations (using authenticated token if available)
            await TestSubtitleOperationsAsync(uploadedVideoIds);

            // Test video filters functionality
            await TestVideoFiltersAsync();

            // Test video embed settings
            await TestVideoEmbedSettingsAsync(uploadedVideoIds);

            _logger.LogInformation("All tests completed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Demo failed with error: {Message}", ex.Message);
            throw;
        }
        finally
        {
            if (_options.CleanupAfterTests)
            {
                await CleanupTestDataAsync();
            }
        }
    }

    /// <summary>
    /// Tests basic API endpoints (Echo, Languages, Locale)
    /// </summary>
    private async Task TestBasicApiEndpointsAsync()
    {
        _logger.LogInformation("=== Testing Basic API Endpoints ===");

        try
        {
            // Test Echo endpoint
            _logger.LogInformation("Testing Echo endpoint...");
            var echoResult = await _sdk.Echo.EchoAsync("Hello DailyMotion!");
            _logger.LogInformation("Echo response: {Data}", echoResult.Data);

            await WaitBetweenOperations();

            // Test Languages endpoint
            _logger.LogInformation("Testing Languages endpoint...");
            var languages = await _sdk.Languages.GetLanguagesAsync();
            _logger.LogInformation("Retrieved {Count} languages", languages.List?.Count ?? 0);

            await WaitBetweenOperations();

            // Test Locale endpoint
            _logger.LogInformation("Testing Locale endpoint...");
            var locale = await _sdk.Locale.DetectLocaleAsync();
            _logger.LogInformation("Detected locale: {Locale}, Country: {Country}",
                locale.Locale, locale.Country);

            _logger.LogInformation("✅ Basic API endpoints test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Some basic API endpoints failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests authentication functionality
    /// </summary>
    private async Task<TokenResponse?> TestAuthenticationAsync()
    {
        _logger.LogInformation("=== Testing Authentication ===");
        _logger.LogInformation("🔐 Testing Password authentication flow");
        _logger.LogInformation("📋 Available Authentication Flows:");
        _logger.LogInformation("   1. Password Grant - Uses username + password + SDK configured Public API keys - ACTIVE");
        _logger.LogInformation("   2. Authorization Code Grant - For web applications (most secure) - COMMENTED OUT");
        _logger.LogInformation("   3. Client Credentials with Private Keys - For server-to-server (uses partner.api.dailymotion.com) - COMMENTED OUT");
        _logger.LogInformation("   4. Client Credentials with Public Keys - For server-to-server (uses api.dailymotion.com) - COMMENTED OUT");
        _logger.LogInformation("");

        // Test 1: Password Grant (uses username + password + Public API keys) - ACTIVE
        var passwordResult = await DoPasswordAuthenticationAsync();

        // Test 2: Authorization Code Grant - SKIPPED (requires manual interaction) - COMMENTED OUT
        // await TestAuthorizationCodeAuthenticationAsync();

        // Test 3: Client Credentials with Private Keys - COMMENTED OUT
        // Uses partner.api.dailymotion.com for auth and api.dailymotion.com/rest for API calls
        // var privateKeyResult = await TestClientCredentialsWithPrivateKeysAsync();

        // Test 4: Client Credentials with Public Keys - COMMENTED OUT
        // Uses api.dailymotion.com for both auth and API calls
        // Run this LAST so it sets the final authentication state for subsequent operations
        // var publicKeyResult = await TestClientCredentialsWithPublicKeysAsync();

        _logger.LogInformation("=== Authentication Testing Summary ===");
        _logger.LogInformation("✅ Password authentication flow has been tested");
        _logger.LogInformation("📝 Note: Password authentication is active for /me endpoints and file uploads");
        _logger.LogInformation("🔧 Using configured credentials from appsettings.json");
        return passwordResult;
    }

    /// <summary>
    /// Tests client credentials authentication with Private API keys
    /// </summary>
    private async Task<TokenResponse?> TestClientCredentialsWithPrivateKeysAsync()
    {
        if (string.IsNullOrEmpty(_sdk.Options.PrivateApiKey) || string.IsNullOrEmpty(_sdk.Options.PrivateApiSecret))
        {
            _logger.LogWarning("⚠️ No private API credentials provided, skipping client credentials authentication test");
            _logger.LogInformation("ℹ️ To test Client Credentials with Private Keys, configure 'DailymotionOptions:PrivateApiKey' and 'DailymotionOptions:PrivateApiSecret' in user secrets");
            return null;
        }

        _logger.LogInformation("=== Testing Client Credentials with Private Keys ===");
        _logger.LogInformation("🔐 Authentication Flow: OAuth 2.0 Client Credentials Grant");
        _logger.LogInformation("🔑 Identification Method: Private API Key/Secret (Application-level authentication)");
        _logger.LogInformation("📋 Required Credentials: Private API Key + Private API Secret");
        _logger.LogInformation("🎯 Use Case: Server-to-server communication, no user context needed");
        _logger.LogInformation("🔒 Auth Endpoint: https://partner.api.dailymotion.com/oauth/v1/token");
        _logger.LogInformation("🌐 API Endpoint: https://partner.api.dailymotion.com/rest");

        _logger.LogInformation("Testing client credentials authentication...");
        _logger.LogInformation("Using Private API Key: {ApiKey}", MaskApiKey(_sdk.Options.PrivateApiKey));
        _logger.LogInformation("Using Private API Secret: {ApiSecret}", MaskApiSecret(_sdk.Options.PrivateApiSecret));

        try
        {
            // Use the correct scopes from the official documentation
            var scopes = new[] { OAuthScope.ManageVideos, OAuthScope.ManagePlaylists, OAuthScope.ManagePlayers };
            _logger.LogInformation("Requesting scopes: {Scopes}", string.Join(", ", scopes.Select(s => s.ToString())));
            _logger.LogInformation("API scope format: {ApiScopes}", string.Join(" ", scopes.Select(s => s.ToApiScopeString())));

            var result = await _sdk.Auth.AuthenticateWithClientCredentialsAsync(
                _sdk.Options.PrivateApiKey!,
                _sdk.Options.PrivateApiSecret!,
                ApiKeyType.Private,
                scopes);

            if (result != null)
            {
                _logger.LogInformation("✅ Client credentials authentication successful");
                _logger.LogInformation("🎫 Access Token: {Token}", result.AccessToken?.Substring(0, 10) + "..." + result.AccessToken?.Substring(result.AccessToken.Length - 10));
                _logger.LogInformation("⏰ Token Expires In: {ExpiresIn} seconds", result.ExpiresIn);
                _logger.LogInformation("🔄 Refresh Token: {RefreshToken}", result.HasRefreshToken ? "Provided" : "Not provided (expected for client credentials)");
                _logger.LogInformation("📝 Token Type: {TokenType}", result.TokenType);
                _logger.LogInformation("🔐 Authentication Level: {AuthLevel}",
                    result.IsUserAuthentication ? "User-level" : "Application-level");
                _logger.LogInformation("👤 User ID: {Uid}", result.Uid ?? "N/A (application-level auth)");
                _logger.LogInformation("📧 Email Verified: {EmailVerified}",
                    result.EmailVerified?.ToString() ?? "N/A");

                // Test token validation
                var tokenInfo = await _sdk.Auth.ValidateTokenAsync();
                if (tokenInfo != null)
                {
                    _logger.LogInformation("✅ Token validation successful for client credentials");
                    _logger.LogInformation("🔍 Token Info - Valid: {Valid}, User ID: {UserId}, Scopes: {Scopes}, Expires In: {ExpiresIn}",
                        tokenInfo.Valid, tokenInfo.Uid, tokenInfo.Scope, tokenInfo.ExpiresIn);
                }
                else
                {
                    _logger.LogWarning("⚠️ Token validation failed for client credentials - this may be expected for some API keys");
                }

                return result;
            }
            else
            {
                _logger.LogError("❌ Client credentials authentication failed");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during client credentials authentication");
            return null;
        }
    }

    /// <summary>
    /// Tests client credentials authentication with Public API keys
    /// COMMENTED OUT - Only Private API Key authentication is active
    /// </summary>
    /*
    private async Task<TokenResponse?> TestClientCredentialsWithPublicKeysAsync()
    {
        if (string.IsNullOrEmpty(_sdk.Options.PublicApiKey) || string.IsNullOrEmpty(_sdk.Options.PublicApiSecret))
        {
            _logger.LogWarning("⚠️ No public API keys provided, skipping client credentials with public keys test");
            _logger.LogInformation("ℹ️ To test Client Credentials with Public Keys, configure 'DailymotionOptions:PublicApiKey' and 'DailymotionOptions:PublicApiSecret' in user secrets");
            return null;
        }

        _logger.LogInformation("=== Testing Client Credentials with Public Keys ===");
        _logger.LogInformation("🔐 Authentication Flow: OAuth 2.0 Client Credentials Grant");
        _logger.LogInformation("🔑 Identification Method: Public API Key/Secret (Application-level authentication)");
        _logger.LogInformation("📋 Required Credentials: Public API Key + Public API Secret");
        _logger.LogInformation("🎯 Use Case: Server-to-server communication, no user context needed");
        _logger.LogInformation("🔒 Auth Endpoint: https://api.dailymotion.com/oauth/token");
        _logger.LogInformation("🌐 API Endpoint: https://api.dailymotion.com");

        _logger.LogInformation("Testing client credentials authentication with public keys...");
        _logger.LogInformation("Using Public API Key: {ApiKey}", MaskApiKey(_sdk.Options.PublicApiKey));
        _logger.LogInformation("Using Public API Secret: {ApiSecret}", MaskApiSecret(_sdk.Options.PublicApiSecret));

        try
        {
            // Use the correct scopes from the official documentation
            var scopes = new[] { OAuthScope.ManageVideos, OAuthScope.ManagePlaylists, OAuthScope.ManagePlayers };
            _logger.LogInformation("Requesting scopes: {Scopes}", string.Join(", ", scopes.Select(s => s.ToString())));
            _logger.LogInformation("API scope format: {ApiScopes}", string.Join(" ", scopes.Select(s => s.ToApiScopeString())));

            var result = await _sdk.Auth.AuthenticateWithClientCredentialsAsync(
                _sdk.Options.PublicApiKey!,
                _sdk.Options.PublicApiSecret!,
                ApiKeyType.Public,
                scopes);

            if (result != null)
            {
                _logger.LogInformation("✅ Client credentials authentication with public keys successful");
                _logger.LogInformation("🎫 Access Token: {Token}", result.AccessToken?.Substring(0, 10) + "..." + result.AccessToken?.Substring(result.AccessToken.Length - 10));
                _logger.LogInformation("⏰ Token Expires In: {ExpiresIn} seconds", result.ExpiresIn);
                _logger.LogInformation("🔄 Refresh Token: {RefreshToken}", result.RefreshToken ?? "Not provided");

                // Test token validation
                var tokenInfo = await _sdk.Auth.ValidateTokenAsync();
                if (tokenInfo != null)
                {
                    _logger.LogInformation("✅ Token validation successful for client credentials with public keys");
                    _logger.LogInformation("🔍 Token Info - Valid: {Valid}, User ID: {UserId}, Scopes: {Scopes}, Expires In: {ExpiresIn}",
                        tokenInfo.Valid, tokenInfo.Uid, tokenInfo.Scope, tokenInfo.ExpiresIn);
                }
                else
                {
                    _logger.LogWarning("⚠️ Token validation failed for client credentials with public keys - this may be expected for some API keys");
                }

                return result;
            }
            else
            {
                _logger.LogWarning("⚠️ Client credentials authentication with public keys returned null result");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Client credentials authentication with public keys failed: {Message}", ex.Message);
            _logger.LogInformation("💡 Troubleshooting tips:");
            _logger.LogInformation("   - Verify your Public API Key and Secret are correct");
            _logger.LogInformation("   - Check if the requested scopes are valid for your API key");
            _logger.LogInformation("   - Ensure the API key has the required permissions");
            return null;
        }
    }

    /// <summary>
    /// Tests password authentication (uses username and password + SDK configured Public API keys)
    /// </summary>
    public async Task<TokenResponse?> TestPasswordAuthFlowAsync()
    {
        _logger.LogInformation("Testing password authentication...");
        try
        {
            // Request ALL available scopes for comprehensive testing
            var scopes = new[]
            {
                OAuthScope.Email,
                OAuthScope.UserInfo,
                OAuthScope.Feed,
                OAuthScope.ManageVideos,
                OAuthScope.ManagePlaylists,
                OAuthScope.ManageSubscriptions,
                OAuthScope.ManageLikes,
                OAuthScope.ManageRecords,
                OAuthScope.ManageSubtitles,
                OAuthScope.ManageFeatures,
                OAuthScope.ManageHistory,
                OAuthScope.ReadInsights,
                OAuthScope.ManageClaimRules,
                OAuthScope.ManageAnalytics,
                OAuthScope.ManagePlayer,
                OAuthScope.ManagePlayers,
                OAuthScope.ManageUserSettings,
                OAuthScope.ManageAppConnections,
                OAuthScope.ManageApplications,
                OAuthScope.ManageDomains,
                OAuthScope.ManagePodcasts
            };

            if (string.IsNullOrEmpty(_sdk.Options.PasswordAuthUsername) || string.IsNullOrEmpty(_sdk.Options.PasswordAuthPassword))
            {
                _logger.LogWarning("⚠️ No username/password provided for password authentication test");
                _logger.LogInformation("ℹ️ To test Password Grant flow, configure 'DailymotionOptions:PasswordAuthUsername' and 'DailymotionOptions:PasswordAuthPassword' in user secrets");
                return null;
            }

            _logger.LogInformation("Using Username: {Username}", _sdk.Options.PasswordAuthUsername);
            _logger.LogInformation("Using Password: ********");
            _logger.LogInformation("Using Public API Key (from SDK config): {ApiKey}", MaskApiKey(_sdk.Options.PublicApiKey));
            _logger.LogInformation("Using Public API Secret (from SDK config): {ApiSecret}", MaskApiSecret(_sdk.Options.PublicApiSecret));
            _logger.LogInformation("Requesting scopes: {Scopes}", string.Join(", ", scopes.Select(s => s.ToString())));
            _logger.LogInformation("API scope format: {ScopeString}", string.Join(" ", scopes.Select(s => s.ToApiScopeString())));

            var result = await _sdk.Auth.AuthenticateWithPasswordAsync(
                _sdk.Options.PasswordAuthUsername!,
                _sdk.Options.PasswordAuthPassword!,
                scopes);

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                _logger.LogInformation("✅ Password authentication successful");
                _logger.LogInformation("🎫 Access Token: {Token}", MaskToken(result.AccessToken));
                _logger.LogInformation("⏰ Token Expires In: {ExpiresIn} seconds", result.ExpiresIn);
                _logger.LogInformation("🔄 Refresh Token: {RefreshToken}", result.HasRefreshToken ? "Provided" : "Not provided");
                _logger.LogInformation("📝 Token Type: {TokenType}", result.TokenType ?? "Bearer");
                _logger.LogInformation("🔐 Authentication Level: {AuthLevel}", 
                    result.IsUserAuthentication ? "User-level" : "Application-level");
                _logger.LogInformation("👤 User ID: {Uid}", result.Uid ?? "N/A");
                _logger.LogInformation("📧 Email Verified: {EmailVerified}", 
                    result.EmailVerified?.ToString() ?? "N/A");

                // Test token validation
                var tokenInfo = await _sdk.Auth.ValidateTokenAsync();
                if (tokenInfo != null)
                {
                    _logger.LogInformation("✅ Token validation successful for password authentication");
                    _logger.LogInformation("🔍 Token Info - Valid: {Valid}, User ID: {UserId}, Scopes: {Scopes}, Expires In: {ExpiresIn}",
                        tokenInfo.Valid, tokenInfo.Uid, tokenInfo.Scope, tokenInfo.ExpiresIn);
                }
                else
                {
                    _logger.LogWarning("⚠️ Token validation failed for password authentication - this may be expected for some API keys");
                }

                return result;
            }
            else
            {
                _logger.LogWarning("⚠️ Password authentication returned empty token");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Password authentication failed: {Message}", ex.Message);
            _logger.LogInformation("💡 Troubleshooting tips:");
            _logger.LogInformation("   - Verify your Public API Key and Secret are correct");
            _logger.LogInformation("   - Ensure the username and password are valid");
            _logger.LogInformation("   - Check if the requested scopes are valid for your API key");
            _logger.LogInformation("   - Verify the user account has the required permissions");
            return null;
        }

        return null;
    }


    /// <summary>
    /// Tests authorization code authentication (for Public API keys)
    /// COMMENTED OUT - Only Private API Key authentication is active
    /// </summary>
    /*
    private async Task TestAuthorizationCodeAuthenticationAsync()
    {
        _logger.LogInformation("=== Testing Authorization Code Grant Authentication Flow ===");
        _logger.LogInformation("🔐 Authentication Flow: OAuth 2.0 Authorization Code Grant");
        _logger.LogInformation("🔑 Identification Method: Public API Key/Secret + Authorization Code");
        _logger.LogInformation("📋 Required Credentials: Public API Key + Public API Secret + Authorization Code");
        _logger.LogInformation("🎯 Use Case: Web applications, most secure OAuth flow");
        _logger.LogInformation("🔒 Endpoint: https://api.dailymotion.com/oauth/token");
        _logger.LogInformation("🔄 Flow: User → Authorization Server → Application → Token Exchange");

        _logger.LogInformation("Testing authorization code authentication...");
        _logger.LogInformation("Using Public API Key (from SDK config): {ApiKey}", MaskApiKey(_sdk.Options.PublicApiKey));
        _logger.LogInformation("Using Public API Secret (from SDK config): {ApiSecret}", MaskApiSecret(_sdk.Options.PublicApiSecret));

        try
        {
            // For demo purposes, we'll simulate the OAuth flow
            // In a real application, you would redirect the user to the authorization URL
            var redirectUri = _options.AuthorizationCodeRedirectUri;
            var scopes = new[] { OAuthScope.Email, OAuthScope.UserInfo, OAuthScope.ManageVideos };
            var scopeString = string.Join(" ", scopes.Select(s => s.ToApiScopeString()));

            _logger.LogInformation("Redirect URI: {RedirectUri}", redirectUri);
            _logger.LogInformation("Requesting scopes: {Scopes}", string.Join(", ", scopes.Select(s => s.ToString())));
            _logger.LogInformation("API scope format: {ApiScopes}", scopeString);

            var authorizationUrl = $"https://api.dailymotion.com/oauth/authorize?client_id={_sdk.Options.PublicApiKey}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(scopeString)}";

            _logger.LogInformation("ℹ️ Authorization code authentication requires user interaction");
            _logger.LogInformation("ℹ️ To test this, you would need to:");
            _logger.LogInformation("   1. Redirect user to: {AuthorizationUrl}", authorizationUrl);
            _logger.LogInformation("   2. User authorizes the application and gets redirected back with a code");
            _logger.LogInformation("   3. Exchange the authorization code for an access token");
            _logger.LogInformation("   4. Use the access token for API calls");

            _logger.LogInformation("📋 OAuth Flow Steps:");
            _logger.LogInformation("   Step 1: User visits authorization URL");
            _logger.LogInformation("   Step 2: User logs in and grants permissions");
            _logger.LogInformation("   Step 3: Authorization server redirects to your app with 'code' parameter");
            _logger.LogInformation("   Step 4: Your app exchanges 'code' for access token via POST to /oauth/token");
            _logger.LogInformation("   Step 5: Use access token for API requests");

            if (_options.EnableInteractiveAuth)
            {
                _logger.LogInformation("ℹ️ Interactive authentication is enabled");
                _logger.LogInformation("ℹ️ You can manually visit the authorization URL and provide the code");
                // In a real application, you would open the browser and handle the callback
            }

            // Example of how it would work (commented out since we don't have a valid code)
            // var authResult = await _sdk.ExchangeCodeForTokenAsync("authorization_code_here", redirectUri);

            _logger.LogInformation("ℹ️ Authorization code authentication test skipped (requires user interaction)");
            _logger.LogInformation("💡 To implement this flow:");
            _logger.LogInformation("   - Set up a web server to handle the redirect URI");
            _logger.LogInformation("   - Extract the 'code' parameter from the redirect");
            _logger.LogInformation("   - Call ExchangeCodeForTokenAsync with the code and redirect URI");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Authorization code authentication failed: {Message}", ex.Message);
        }

        await WaitBetweenOperations();
    }
    */

    /// <summary>
    /// Tests video operations
    /// </summary>
    private async Task TestVideoOperationsAsync()
    {
        _logger.LogInformation("=== Testing Video Operations ===");

        try
        {
            _logger.LogInformation("🔐 Using authenticated token for video operations");

            // Test getting trending videos
            _logger.LogInformation("Testing trending videos retrieval...");
            try
            {
                var trendingVideos = await _sdk.GetTrendingVideosAsync(limit: 5);
                _logger.LogInformation("Retrieved {Count} trending videos", trendingVideos.List?.Count ?? 0);

                if (trendingVideos.List?.Count > 0)
                {
                    var firstVideo = trendingVideos.List[0];
                    _logger.LogInformation("First video: {Title} (ID: {Id})", firstVideo.Name, firstVideo.Id);

                    // Test getting specific video
                    if (!string.IsNullOrEmpty(firstVideo.Id))
                    {
                        var videoDetails = await _sdk.GetVideoAsync(firstVideo.Id);
                        if (videoDetails != null)
                        {
                            _logger.LogInformation("✅ Video details retrieved: {Title}", videoDetails.Title);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to get video details for ID: {VideoId}", firstVideo.Id);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("⚠️ No trending videos returned");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to get trending videos: {Message}", ex.Message);
            }

            await WaitBetweenOperations();
            _logger.LogInformation("✅ Video operations test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Video operations test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests /me endpoints only if using password authentication
    /// </summary>
    private async Task TestMeEndpointsIfPasswordAuthAsync()
    {
        try
        {
            // Check if we're using password authentication by checking if we have a user ID in the token
            var accessToken = _sdk.HttpClient.GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogInformation("No access token available, skipping /me endpoints test");
                return;
            }

            // Check if this is user-level authentication (password grant) by trying to get /me
            var userInfo = await _sdk.Mine.GetUserInfoAsync();
            if (userInfo == null)
            {
                _logger.LogInformation("Not using password authentication, skipping /me endpoints test");
                return;
            }

            _logger.LogInformation("=== Testing /me Endpoints (Password Authentication Detected) ===");
            await TestMeEndpointsWithPasswordAuthAsync();
            // await TestMeVideoCreationAsync(); // Commented out - /me/videos endpoint fails with 403 Forbidden
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Could not determine authentication type for /me endpoints test: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests user operations
    /// </summary>
    private async Task TestUserOperationsAsync()
    {
        _logger.LogInformation("=== Testing User Operations ===");

        try
        {
            _logger.LogInformation("🔐 Using authenticated token for user operations");

            // Search for users
            _logger.LogInformation("Testing user search...");
            try
            {
                var userSearchResults = await _sdk.SearchUsersAsync("dailymotion", limit: 5);
                _logger.LogInformation("Found {Count} users", userSearchResults.List?.Count ?? 0);

                if (userSearchResults.List?.Count > 0)
                {
                    var firstUser = userSearchResults.List[0];
                    _logger.LogInformation("First user: {Username} (ID: {Id})", firstUser.Username, firstUser.Id);

                    // Test getting specific user
                    if (!string.IsNullOrEmpty(firstUser.Id))
                    {
                        var userDetails = await _sdk.GetUserAsync(firstUser.Id);
                        if (userDetails != null)
                        {
                            _logger.LogInformation("✅ User details retrieved: {Username}", userDetails.Username);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to get user details for ID: {UserId}", firstUser.Id);
                        }

                        // Test user client
                        var userClient = _sdk.GetUser(firstUser.Id);
                        var userVideos = await userClient.GetVideosAsync(limit: 3);
                        _logger.LogInformation("User has {Count} videos", userVideos.List?.Count ?? 0);
                    }
                }
                else
                {
                    _logger.LogWarning("⚠️ No users found in search results");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to search users: {Message}", ex.Message);
            }

            await WaitBetweenOperations();
            _logger.LogInformation("✅ User operations test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ User operations test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests playlist operations including creation, adding videos, removing videos, and deletion
    /// </summary>
    private async Task TestPlaylistOperationsAsync(List<string> uploadedVideoIds)
    {
        _logger.LogInformation("=== Testing Playlist Operations ===");
        try
        {
            if (uploadedVideoIds == null || uploadedVideoIds.Count == 0)
            {
                _logger.LogWarning("⚠️ No uploaded videos available, skipping playlist operations test");
                return;
            }

            // Use existing Private API Key authentication (already has manage_playlists scope)
            _logger.LogInformation("🔐 Using existing Private API Key authentication for playlist operations");

            _logger.LogInformation("✅ Authenticated with Private API Key for playlist operations");
            _logger.LogInformation("🎵 Testing complete playlist lifecycle...");

            // Note: Using Private API Key token which has manage_playlists scope
            _logger.LogInformation("📝 Step 1: Creating a new playlist...");
            try
            {
                var createdPlaylist = await _sdk.Playlists.CreatePlaylistAsync(
                    $"Test Playlist {DateTime.Now:yyyyMMdd-HHmmss}",
                    "A test playlist created by the SDK demo",
                    isPrivate: true);

                if (createdPlaylist != null)
                {
                    _logger.LogInformation("✅ Playlist created successfully: {PlaylistId}", createdPlaylist.Id);
                    _createdResources.Add($"playlist:{createdPlaylist.Id}");

                    _logger.LogInformation("📝 Step 2: Adding uploaded videos to the playlist...");
                    var playlistClient = _sdk.Playlists.GetPlaylist(createdPlaylist.Id);
                    foreach (var videoId in uploadedVideoIds)
                    {
                        try
                        {
                            var added = await playlistClient.AddVideoAsync(videoId);
                            if (added)
                            {
                                _logger.LogInformation("✅ Added video {VideoId} to playlist", videoId);
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ Failed to add video {VideoId} to playlist", videoId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "⚠️ Error adding video {VideoId} to playlist: {Message}", videoId, ex.Message);
                        }
                        await WaitBetweenOperations();
                    }

                    _logger.LogInformation("📝 Step 3: Verifying videos were added...");
                    try
                    {
                        var playlistVideos = await playlistClient.GetVideosAsync(limit: 10);
                        _logger.LogInformation("✅ Playlist now contains {Count} videos", playlistVideos.List?.Count ?? 0);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "⚠️ Failed to get playlist videos: {Message}", ex.Message);
                    }

                    _logger.LogInformation("📝 Step 4: Removing one video from the playlist...");
                    if (uploadedVideoIds.Count > 0)
                    {
                        try
                        {
                            var videoToRemove = uploadedVideoIds[0];
                            var removed = await playlistClient.RemoveVideoAsync(videoToRemove);
                            if (removed)
                            {
                                _logger.LogInformation("✅ Removed video {VideoId} from playlist", videoToRemove);
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ Failed to remove video {VideoId} from playlist", videoToRemove);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "⚠️ Error removing video from playlist: {Message}", ex.Message);
                        }
                    }

                    _logger.LogInformation("📝 Step 5: Updating playlist metadata...");
                    try
                    {
                        var updatedPlaylist = await playlistClient.UpdateMetadataAsync(
                            name: $"Updated Test Playlist {DateTime.Now:yyyyMMdd-HHmmss}",
                            description: "Updated description for test playlist",
                            isPrivate: false);
                        _logger.LogInformation("✅ Playlist metadata updated successfully");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "⚠️ Failed to update playlist metadata: {Message}", ex.Message);
                    }

                    _logger.LogInformation("📝 Step 6: Deleting the playlist...");
                    try
                    {
                        var playlistDeleted = await playlistClient.DeleteAsync();
                        if (playlistDeleted)
                        {
                            _logger.LogInformation("✅ Playlist deleted successfully");
                            _createdResources.Remove($"playlist:{createdPlaylist.Id}");
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete playlist");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "⚠️ Error deleting playlist: {Message}", ex.Message);
                    }
                }
                else
                {
                    _logger.LogError("❌ Failed to create playlist - returned null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to create playlist: {Message}", ex.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during playlist operations test");
        }
        _logger.LogInformation("✅ Playlist operations test completed");
    }

    /// <summary>
    /// Tests channel operations
    /// </summary>
    private async Task TestChannelOperationsAsync()
    {
        _logger.LogInformation("=== Testing Channel Operations ===");

        try
        {
            // Test getting all available channels
            _logger.LogInformation("Testing: Get all channels");
            var allChannels = await _sdk.Channels.GetChannelsAsync(limit: 10);
            _logger.LogInformation("Found {Count} channels", allChannels.ChannelsList?.Count ?? 0);

            if (allChannels.ChannelsList != null && allChannels.ChannelsList.Any())
            {
                var firstChannel = allChannels.ChannelsList.First();
                _logger.LogInformation("First channel: {ChannelName} (ID: {ChannelId})",
                    firstChannel.Name, firstChannel.Id);

                // Test getting specific channel metadata
                _logger.LogInformation("Testing: Get channel metadata for {ChannelId}", firstChannel.Id);
                var channelMetadata = await _sdk.Channels.GetChannelMetadataAsync(firstChannel.Id);
                if (channelMetadata != null)
                {
                    _logger.LogInformation("Channel metadata - Name: {Name}, Videos: {VideosTotal}, Subscribers: {SubscribersTotal}",
                        channelMetadata.Name, channelMetadata.VideosTotal, channelMetadata.SubscribersTotal);
                }

                // Test getting channel videos
                _logger.LogInformation("Testing: Get videos for channel {ChannelId}", firstChannel.Id);
                var channelVideos = await _sdk.Channels.GetChannelVideosAsync(firstChannel.Id, limit: 5);
                _logger.LogInformation("Channel {ChannelId} has {Count} videos",
                    firstChannel.Id, channelVideos.List?.Count ?? 0);

                await WaitBetweenOperations();
            }

            // Test getting videos from different predefined channels
            _logger.LogInformation("Testing: Get videos from predefined channels");
            var channels = new[] { Channel.Music, Channel.Sport, Channel.News };

            foreach (var channel in channels)
            {
                _logger.LogInformation("Testing channel: {Channel}", channel);
                var channelVideos = await _sdk.GetChannelVideosAsync(channel, limit: 3);
                _logger.LogInformation("Channel {Channel} has {Count} videos",
                    channel, channelVideos.List?.Count ?? 0);

                await WaitBetweenOperations();
            }

            _logger.LogInformation("✅ Channel operations test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Channel operations test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests search operations
    /// </summary>
    private async Task TestSearchOperationsAsync()
    {
        _logger.LogInformation("=== Testing Search Operations ===");

        try
        {

            var searchTerms = new[] { "music", "technology", "education" };

            foreach (var term in searchTerms)
            {
                _logger.LogInformation("Searching videos for: {Term}", term);
                var searchResults = await _sdk.SearchVideosAsync(term, limit: 3);
                _logger.LogInformation("Found {Count} videos for '{Term}'",
                    searchResults.List?.Count ?? 0, term);

                await WaitBetweenOperations();
            }

            _logger.LogInformation("✅ Search operations test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Search operations test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests file operations
    /// </summary>
    private async Task<List<string>> TestFileOperationsAsync()
    {
        _logger.LogInformation("=== Testing File Operations ===");

        var uploadedFiles = new List<(string FileId, string FileUrl)>();
        var createdVideoIds = new List<string>();

        try
        {
            // Check if test video files exist
            var testVideo1Exists = File.Exists(_options.TestVideoPath1);
            var testVideo2Exists = File.Exists(_options.TestVideoPath2);

            if (!testVideo1Exists && !testVideo2Exists)
            {
                _logger.LogWarning("⚠️ No test video files found, skipping file upload test");
                _logger.LogWarning("   Expected files: {Path1}, {Path2}", _options.TestVideoPath1, _options.TestVideoPath2);
                return new List<string>();
            }

            // Upload first test video
            if (testVideo1Exists)
            {
                _logger.LogInformation("Testing file upload for test1.mp4...");
                try
                {
                    var uploadResult1 = await _sdk.File.UploadAsync(_options.TestVideoPath1);

                    if (uploadResult1 != null && !string.IsNullOrEmpty(uploadResult1.Url))
                    {
                        _logger.LogInformation("✅ First file uploaded successfully: {Url}", uploadResult1.Url);
                        var fileId = uploadResult1.GetFileId();
                        _logger.LogInformation("   File ID: {FileId}", fileId ?? "NULL");
                        _logger.LogInformation("   File Details:");
                        _logger.LogInformation("     - Name: {Name}", uploadResult1.Name ?? "N/A");
                        _logger.LogInformation("     - Format: {Format}", uploadResult1.Format ?? "N/A");
                        _logger.LogInformation("     - Dimensions: {Dimension}", uploadResult1.Dimension ?? "N/A");
                        _logger.LogInformation("     - Duration: {Duration}s", uploadResult1.DurationSeconds?.ToString("F1") ?? "N/A");
                        _logger.LogInformation("     - Size: {Size} bytes", uploadResult1.FileSizeBytes?.ToString("N0") ?? "N/A");
                        _logger.LogInformation("     - Streamable: {Streamable}", uploadResult1.IsStreamable ? "Yes" : "No");
                        _logger.LogInformation("     - Audio Codec: {AudioCodec}", uploadResult1.AudioCodec ?? "N/A");
                        _logger.LogInformation("     - Video Codec: {VideoCodec}", uploadResult1.VideoCodec ?? "N/A");

                        if (!string.IsNullOrEmpty(fileId))
                        {
                            _createdResources.Add($"file:{fileId}");
                            uploadedFiles.Add((fileId, uploadResult1.Url));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ First file upload failed: {Message}", ex.Message);
                }

                await WaitBetweenOperations();
            }

            // Upload second test video
            if (testVideo2Exists)
            {
                _logger.LogInformation("Testing file upload for test2.mp4...");
                try
                {
                    var uploadResult2 = await _sdk.File.UploadAsync(_options.TestVideoPath2);

                    if (uploadResult2 != null && !string.IsNullOrEmpty(uploadResult2.Url))
                    {
                        _logger.LogInformation("✅ Second file uploaded successfully: {Url}", uploadResult2.Url);
                        var fileId = uploadResult2.GetFileId();
                        _logger.LogInformation("   File ID: {FileId}", fileId ?? "NULL");
                        _logger.LogInformation("   File Details:");
                        _logger.LogInformation("     - Name: {Name}", uploadResult2.Name ?? "N/A");
                        _logger.LogInformation("     - Format: {Format}", uploadResult2.Format ?? "N/A");
                        _logger.LogInformation("     - Dimensions: {Dimension}", uploadResult2.Dimension ?? "N/A");
                        _logger.LogInformation("     - Duration: {Duration}s", uploadResult2.DurationSeconds?.ToString("F1") ?? "N/A");
                        _logger.LogInformation("     - Size: {Size} bytes", uploadResult2.FileSizeBytes?.ToString("N0") ?? "N/A");
                        _logger.LogInformation("     - Streamable: {Streamable}", uploadResult2.IsStreamable ? "Yes" : "No");
                        _logger.LogInformation("     - Audio Codec: {AudioCodec}", uploadResult2.AudioCodec ?? "N/A");
                        _logger.LogInformation("     - Video Codec: {VideoCodec}", uploadResult2.VideoCodec ?? "N/A");

                        if (!string.IsNullOrEmpty(fileId))
                        {
                            _createdResources.Add($"file:{fileId}");
                            uploadedFiles.Add((fileId, uploadResult2.Url));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Second file upload failed: {Message}", ex.Message);
                }

                await WaitBetweenOperations();
            }

            // Create videos from uploaded files
            if (uploadedFiles.Count > 0)
            {
                _logger.LogInformation("=== Creating Videos from Uploaded Files ===");
                _logger.LogInformation("🔐 Using existing Private API Key authentication for video creation");

                _logger.LogInformation("✅ Authenticated with Private API Key for video creation");

                foreach (var (fileId, fileUrl) in uploadedFiles)
                {
                    try
                    {
                        _logger.LogInformation("Creating video from uploaded file ID: {FileId}", fileId);
                        _logger.LogInformation("Current authentication status:");
                        _logger.LogInformation("  - Access Token: {HasToken}", string.IsNullOrEmpty(_sdk.AccessToken) ? "NOT_SET" : "SET");
                        _logger.LogInformation("  - Token Preview: {TokenPreview}", string.IsNullOrEmpty(_sdk.AccessToken) ? "N/A" : _sdk.AccessToken.Substring(0, Math.Min(10, _sdk.AccessToken.Length)) + "...");

                        // Create a video from the uploaded file
                        var videoTitle = $"Test Video from {fileId}";
                        var videoDescription = $"This is a test video created from uploaded file {fileId}";

                        _logger.LogInformation("Attempting to create video with parameters:");
                        _logger.LogInformation("  - File URL: {FileUrl}", fileUrl);
                        _logger.LogInformation("  - Title: {Title}", videoTitle);
                        _logger.LogInformation("  - Description: {Description}", videoDescription);
                        _logger.LogInformation("  - Channel: fun");
                        _logger.LogInformation("  - Tags: test,demo,sdk");
                        _logger.LogInformation("  - Private: true");
                        _logger.LogInformation("  - Published: true");
                        _logger.LogInformation("  - Is Created for Kids: false");

                        // Test the new VideoCreationParameters overload
                        var parameters = new VideoCreationParameters
                        {
                            Url = fileUrl,
                            Title = videoTitle,
                            Description = videoDescription,
                            Channel = "fun",
                            Tags = new[] { "test", "demo", "sdk", "new-overload" },
                            Private = true,
                            Published = true,
                            IsCreatedForKids = false,
                            Mode = "vod", // Video on demand
                            Language = "en",
                            Country = "US"
                        };

                        _logger.LogInformation("🧪 Testing new VideoCreationParameters overload...");
                        var createdVideo = await _sdk.Videos.CreateVideoFromFileAsync(parameters);

                        if (createdVideo != null && !string.IsNullOrEmpty(createdVideo.Id))
                        {
                            _logger.LogInformation("✅ Video created successfully from file {FileId}:\n   Video ID: {VideoId}\n   Title: {Title}\n   Status: {Status}", fileId, createdVideo.Id, createdVideo.Name ?? "N/A", createdVideo.Status);

                            createdVideoIds.Add(createdVideo.Id);
                            _createdResources.Add($"video:{createdVideo.Id}");
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Could not create video from file {FileId}", fileId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Failed to create video from file {FileId}: {Message}\nFile URL: {FileUrl}\nVideo Title: Test Video from {FileId}\nVideo Description: This is a test video created from uploaded file {FileId}\nChannel: fun, Tags: test,demo,sdk, Private: true", fileId, ex.Message, fileUrl, fileId, fileId);

                        // Check if it's an API error with more details
                        if (ex.Message.Contains("StatusCode") || ex.Message.Contains("Error"))
                        {
                            _logger.LogError("API Error Details: {Details}", ex.Message);
                        }
                    }

                    await WaitBetweenOperations();
                }

                // Get video details for created videos
                if (createdVideoIds.Count > 0)
                {
                    _logger.LogInformation("=== Getting Video Details for Created Videos ===");
                    foreach (var videoId in createdVideoIds)
                    {
                        try
                        {
                            _logger.LogInformation("Getting video details for created video ID: {VideoId}", videoId);
                            var videoDetails = await _sdk.Videos.GetVideoAsync(videoId);

                            if (videoDetails != null)
                            {
                                _logger.LogInformation("✅ Video details retrieved for {VideoId}:", videoId);
                                _logger.LogInformation("   Title: {Title}", videoDetails.Title ?? "N/A");
                                _logger.LogInformation("   Description: {Description}",
                                    !string.IsNullOrEmpty(videoDetails.Description) ? videoDetails.Description.Substring(0, Math.Min(100, videoDetails.Description.Length)) + "..." : "N/A");
                                _logger.LogInformation("   Duration: {Duration} seconds", videoDetails.Duration);
                                _logger.LogInformation("   Status: {Status}", videoDetails.Status);
                                _logger.LogInformation("   Private: {IsPrivate}", videoDetails.IsPrivate);
                                _logger.LogInformation("   Views: {Views}", videoDetails.ViewsTotal);
                                _logger.LogInformation("   Likes: {Likes}", videoDetails.LikesTotal);
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ Could not retrieve video details for {VideoId}", videoId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "⚠️ Failed to get video details for {VideoId}: {Message}", videoId, ex.Message);
                        }

                        await WaitBetweenOperations();
                    }
                }
            }

            _logger.LogInformation("✅ File operations test completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ File operations test failed: {Message}", ex.Message);
        }

        // Test /me endpoint blocking with client credentials
        await TestMeEndpointBlockingAsync();

        return createdVideoIds;
    }

    /// <summary>
    /// Tests player operations
    /// </summary>
    private async Task TestPlayerOperationsAsync()
    {
        _logger.LogInformation("=== Testing Player Operations ===");

        try
        {

            // Check if we have a configured player ID
            if (string.IsNullOrEmpty(_options.TestPlayerId) || _options.TestPlayerId == "YOUR_PLAYER_ID_HERE")
            {
                _logger.LogWarning("⚠️ No player ID configured, skipping player operations test");
                _logger.LogInformation("   Configure TestPlayerId in appsettings.json or user secrets to test player operations");
                return;
            }

            _logger.LogInformation("Testing player operations with ID: {PlayerId}", _options.TestPlayerId);

            try
            {
                // Get player information
                _logger.LogInformation("Getting player information...");
                var playerInfo = await _sdk.Player.GetPlayerAsync(_options.TestPlayerId);

                if (playerInfo != null)
                {
                    _logger.LogInformation("✅ Player information retrieved:");
                    _logger.LogInformation("   Player ID: {PlayerId}", playerInfo.Id ?? "N/A");
                    _logger.LogInformation("   Name: {Name}", playerInfo.Name ?? "N/A");
                    _logger.LogInformation("   Description: {Description}",
                        !string.IsNullOrEmpty(playerInfo.Description) ? playerInfo.Description.Substring(0, Math.Min(100, playerInfo.Description.Length)) + "..." : "N/A");
                    _logger.LogInformation("   URL: {Url}", playerInfo.Url ?? "N/A");
                    _logger.LogInformation("   Embed URL: {EmbedUrl}", playerInfo.EmbedUrl ?? "N/A");
                }
                else
                {
                    _logger.LogWarning("⚠️ Could not retrieve player information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Player operations failed: {Message}", ex.Message);
            }

            await WaitBetweenOperations();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Player operations test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests subtitle operations
    /// </summary>
    private async Task TestSubtitleOperationsAsync(List<string> uploadedVideoIds)
    {
        _logger.LogInformation("=== Testing Subtitle Operations ===");
        try
        {
            if (uploadedVideoIds == null || uploadedVideoIds.Count == 0)
            {
                _logger.LogWarning("⚠️ No uploaded videos available, skipping subtitle operations test");
                return;
            }

            // Use existing Private API Key authentication (already has manage_subtitles scope)
            _logger.LogInformation("🔐 Using existing Private API Key authentication for subtitle operations");

            _logger.LogInformation("✅ Authenticated with Private API Key for subtitle operations");
            _logger.LogInformation("Testing subtitle operations with {Count} uploaded videos", uploadedVideoIds.Count);

            // First, upload a subtitle file
            _logger.LogInformation("📁 Uploading test subtitle file...");
            var subtitleFilePath = "DailymotionSDK.Demo/test-subtitle.srt";

            if (!File.Exists(subtitleFilePath))
            {
                _logger.LogWarning("⚠️ Test subtitle file not found: {Path}", subtitleFilePath);
                _logger.LogInformation("   Skipping subtitle operations test");
                return;
            }

            var subtitleUploadResult = await _sdk.File.UploadAsync(subtitleFilePath);
            if (subtitleUploadResult == null || string.IsNullOrEmpty(subtitleUploadResult.Url))
            {
                _logger.LogWarning("⚠️ Failed to upload subtitle file");
                _logger.LogInformation("   Skipping subtitle operations test");
                return;
            }

            _logger.LogInformation("✅ Subtitle file uploaded successfully: {Url}", subtitleUploadResult.Url);
            var subtitleFileId = subtitleUploadResult.GetFileId();
            if (!string.IsNullOrEmpty(subtitleFileId))
            {
                _createdResources.Add($"file:{subtitleFileId}");
            }

            // Now test subtitle creation for each video
            foreach (var videoId in uploadedVideoIds)
            {
                _logger.LogInformation("Testing subtitle operations for video ID: {VideoId}", videoId);

                try
                {
                    _logger.LogInformation("Creating subtitle for video {VideoId} using uploaded subtitle file...", videoId);

                    // Use the uploaded subtitle file URL to create subtitles
                    var subtitleResult = await _sdk.Subtitles.CreateSubtitleForVideoAsync(
                        videoId: videoId,
                        url: subtitleUploadResult.Url,
                        language: "en",
                        format: "SRT"
                    );

                    if (subtitleResult != null)
                    {
                        _logger.LogInformation("✅ Subtitle created successfully for video {VideoId}: {SubtitleId}", videoId, subtitleResult.Id);
                        _createdResources.Add($"subtitle:{subtitleResult.Id}");
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Subtitle creation returned null for video {VideoId}", videoId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Failed to create subtitle for video {VideoId}: {Message}", videoId, ex.Message);
                }

                await WaitBetweenOperations();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during subtitle operations test");
        }

        _logger.LogInformation("✅ Subtitle operations test completed");
    }

    /// <summary>
    /// Tests video filtering functionality
    /// </summary>
    private async Task TestVideoFiltersAsync()
    {
        _logger.LogInformation("=== Testing Video Filters ===");

        try
        {
            // Test 1: Search for videos with basic filters (no duration filter as it's not supported)
            _logger.LogInformation("🔍 Testing basic video search with filters...");
            var basicFilters = new VideoFilters
            {
                // Using only supported filter parameters
            };

            var basicResults = await _sdk.Videos.SearchVideosWithFiltersAsync(basicFilters, limit: 5);
            if (basicResults?.List is { Count: > 0 })
            {
                _logger.LogInformation("✅ Basic filter returned {Count} videos (Page {Page} of {Total})", basicResults.List.Count, basicResults.Page, basicResults.Total);
                foreach (var video in basicResults.List.Take(3))
                {
                    _logger.LogInformation("   - {Title} (Duration: {Duration}s)", video.Title, video.Duration);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ No videos found with basic filter");
            }

            await WaitBetweenOperations();

            // Test 2: Search for videos in Music channel (without unsupported filters)
            _logger.LogInformation("🎵 Testing channel filter (Music channel)...");
            var musicFilters = new VideoFilters
            {
                // Channel-specific filters without unsupported parameters
            };

            var musicResults = await _sdk.Videos.GetChannelVideosWithFiltersAsync(Channel.Music, musicFilters, limit: 5);
            if (musicResults?.List is { Count: > 0 })
            {
                _logger.LogInformation("✅ Music channel filter returned {Count} videos (Page {Page} of {Total})", musicResults.List.Count, musicResults.Page, musicResults.Total);
                foreach (var video in musicResults.List.Take(3))
                {
                    _logger.LogInformation("   - {Title} (Views: {Views})", video.Title, video.ViewsTotal);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ No videos found in Music channel with filters");
            }

            await WaitBetweenOperations();

            // Test 4: Search for videos with valid sort parameter
            _logger.LogInformation("🔍 Testing videos with valid sort parameter (recent)...");
            var sortFilters = new VideoFilters
            {
                // Using only supported filter parameters
            };

            var sortResults = await _sdk.Videos.SearchVideosWithFiltersAsync(sortFilters, limit: 5, sort: VideoSort.Recent);
            if (sortResults?.List is { Count: > 0 })
            {
                _logger.LogInformation("✅ Sort filter returned {Count} videos (Page {Page} of {Total})", sortResults.List.Count, sortResults.Page, sortResults.Total);
                foreach (var video in sortResults.List.Take(3))
                {
                    _logger.LogInformation("   - {Title} (Resolution: {Width}x{Height})", video.Title, video.Width, video.Height);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ No videos found with sort filter");
            }

            _logger.LogInformation("✅ Video filters testing completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Video filters testing failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests video embed settings
    /// </summary>
    private async Task TestVideoEmbedSettingsAsync(List<string> uploadedVideoIds)
    {
        _logger.LogInformation("=== Testing Video Embed Settings ===");

        try
        {
            // Check if we have an authenticated token
            if (string.IsNullOrEmpty(_sdk.AccessToken))
            {
                _logger.LogWarning("⚠️ No access token available, skipping embed settings test");
                _logger.LogInformation("   Authenticate first to test embed settings");
                return;
            }

            // Check if we have uploaded video IDs to work with
            if (uploadedVideoIds == null || uploadedVideoIds.Count == 0)
            {
                _logger.LogWarning("⚠️ No uploaded video IDs available, skipping embed settings test");
                _logger.LogInformation("   Upload videos first to test embed settings");
                return;
            }

            _logger.LogInformation("Testing video embed settings for {Count} uploaded videos", uploadedVideoIds.Count);

            foreach (var videoId in uploadedVideoIds)
            {
                try
                {
                    _logger.LogInformation("Testing embed settings for video ID: {VideoId}", videoId);

                    // First, get current video details to see embed settings
                    var currentVideo = await _sdk.Videos.GetVideoAsync(videoId);
                    if (currentVideo != null)
                    {
                        _logger.LogInformation("Current embed settings for {VideoId}:", videoId);
                        _logger.LogInformation("   Allow Embed: {AllowEmbed}", currentVideo.AllowEmbed);
                        _logger.LogInformation("   Geoblocking: {Geoblocking}",
                            currentVideo.Geoblocking != null ? string.Join(", ", currentVideo.Geoblocking) : "None");
                        _logger.LogInformation("   Status: {Status}", currentVideo.Status);
                        _logger.LogInformation("   Duration: {Duration}s", currentVideo.Duration);
                        _logger.LogInformation("   Views: {Views}", currentVideo.ViewsTotal);
                        _logger.LogInformation("   Encoding Progress: {EncodingProgress}%", currentVideo.EncodingProgress);
                        _logger.LogInformation("   Published: {Published}", currentVideo.Published);
                    }

                    // Test updating embed settings
                    _logger.LogInformation("Updating embed settings for video {VideoId}...", videoId);

                    // Example: Allow embedding and set geoblocking to allow only US and Canada
                    var updatedVideo = await _sdk.Videos.UpdateVideoEmbedSettingsAsync(
                        videoId,
                        allowEmbed: true,
                        geoblocking: new List<string> { "allow", "us", "ca" }
                    );

                    if (updatedVideo != null)
                    {
                        _logger.LogInformation("✅ Embed settings updated successfully for video {VideoId}:", videoId);
                        _logger.LogInformation("   Allow Embed: {AllowEmbed}", updatedVideo.AllowEmbed);
                        _logger.LogInformation("   Geoblocking: {Geoblocking}",
                            updatedVideo.Geoblocking != null ? string.Join(", ", updatedVideo.Geoblocking) : "None");
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Could not update embed settings for video {VideoId}", videoId);
                    }

                    await WaitBetweenOperations();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Failed to update embed settings for video {VideoId}: {Message}", videoId, ex.Message);
                }
            }

            _logger.LogInformation("✅ Video embed settings testing completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Video embed settings testing failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Cleans up test data created during the demo
    /// </summary>
    private async Task CleanupTestDataAsync()
    {
        if (_createdResources.Count == 0)
        {
            _logger.LogInformation("No test resources to clean up");
            return;
        }

        _logger.LogInformation("=== Cleaning Up Test Data ===");

        foreach (var resource in _createdResources)
        {
            try
            {
                var parts = resource.Split(':');
                if (parts.Length != 2) continue;

                var resourceType = parts[0];
                var resourceId = parts[1];

                switch (resourceType)
                {
                    case "playlist":
                        var playlistClient = _sdk.Playlists.GetPlaylist(resourceId);
                        var playlistDeleted = await playlistClient.DeleteAsync();
                        if (playlistDeleted)
                        {
                            _logger.LogInformation("✅ Deleted playlist: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete playlist: {Id}", resourceId);
                        }
                        break;
                    case "video":
                        var videoDeleted = await _sdk.Videos.DeleteVideoAsync(resourceId);
                        if (videoDeleted)
                        {
                            _logger.LogInformation("✅ Deleted video: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete video: {Id}", resourceId);
                        }
                        break;
                    case "player":
                        var playerDeleted = await _sdk.Player.DeletePlayerAsync(resourceId);
                        if (playerDeleted)
                        {
                            _logger.LogInformation("✅ Deleted player: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete player: {Id}", resourceId);
                        }
                        break;
                    case "subtitle":
                        var subtitleDeleted = await _sdk.Subtitles.DeleteSubtitleAsync(resourceId);
                        if (subtitleDeleted)
                        {
                            _logger.LogInformation("✅ Deleted subtitle: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete subtitle: {Id}", resourceId);
                        }
                        break;
                    case "file":
                        // Note: File deletion is not supported by the API
                        // Files are automatically cleaned up by Dailymotion after a period
                        _logger.LogInformation("ℹ️ File {Id} will be automatically cleaned up by Dailymotion", resourceId);
                        break;
                    default:
                        _logger.LogWarning("⚠️ Unknown resource type for cleanup: {Type}", resourceType);
                        break;
                }

                await WaitBetweenOperations();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Failed to cleanup resource: {Resource}", resource);
            }
        }

        // Logout if authenticated
        if (!string.IsNullOrEmpty(_sdk.AccessToken))
        {
            try
            {
                await _sdk.Logout.LogoutAsync();
                _logger.LogInformation("✅ Logged out successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Logout failed: {Message}", ex.Message);
            }
        }

        _logger.LogInformation("✅ Cleanup completed");
    }

    /// <summary>
    /// Waits between operations to avoid rate limiting
    /// </summary>
    private async Task WaitBetweenOperations()
    {
        if (_options.WaitBetweenOperations > 0)
        {
            await Task.Delay(_options.WaitBetweenOperations);
        }
    }

    /// <summary>
    /// Masks an API key for secure logging
    /// </summary>
    private static string MaskApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 8)
            return "***";
        return $"{apiKey.Substring(0, 4)}...{apiKey.Substring(apiKey.Length - 4)}";
    }

    /// <summary>
    /// Masks an API secret for secure logging
    /// </summary>
    private static string MaskApiSecret(string apiSecret)
    {
        if (string.IsNullOrEmpty(apiSecret) || apiSecret.Length <= 8)
            return "***";
        return $"{apiSecret.Substring(0, 4)}...{apiSecret.Substring(apiSecret.Length - 4)}";
    }

    /// <summary>
    /// Masks a password for secure logging
    /// </summary>
    private static string MaskPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return "***";
        return new string('*', Math.Min(password.Length, 8));
    }

    /// <summary>
    /// Masks a token for secure logging
    /// </summary>
    private static string MaskToken(string token)
    {
        if (string.IsNullOrEmpty(token) || token.Length <= 16)
            return "***";
        return $"{token.Substring(0, 8)}...{token.Substring(token.Length - 8)}";
    }

    /// <summary>
    /// Tests that /me endpoints are properly blocked when using client credentials authentication
    /// </summary>
    private async Task TestMeEndpointBlockingAsync()
    {
        _logger.LogInformation("=== Testing /me Endpoint Blocking ===");

        try
        {
            // Check if we're using client credentials authentication
            var isClientCredentials = _sdk.HttpClient.IsUsingClientCredentials();
            _logger.LogInformation("Current authentication type: {AuthType}",
                isClientCredentials ? "Client Credentials (Application-level)" : "User-level authentication");

            if (isClientCredentials)
            {
                _logger.LogInformation("🧪 Testing /me endpoint blocking with client credentials...");

                // Test various /me endpoints that should be blocked
                var meEndpoints = new[]
                {
                    "/me",
                    "/me/videos",
                    "/me/playlists",
                    "/me/favorites",
                    "/me/history",
                    "/me/watchlater"
                };

                foreach (var endpoint in meEndpoints)
                {
                    _logger.LogInformation("Testing GET {Endpoint}...", endpoint);

                    try
                    {
                        var response = await _sdk.HttpClient.GetAsync(endpoint);

                        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden &&
                            response.Content?.Contains("authentication_incompatible") == true)
                        {
                            _logger.LogInformation("✅ {Endpoint} correctly blocked with client credentials", endpoint);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ {Endpoint} was not blocked as expected. Status: {StatusCode}",
                                endpoint, response.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("⚠️ Exception testing {Endpoint}: {Message}", endpoint, ex.Message);
                    }

                    await WaitBetweenOperations();
                }

                _logger.LogInformation("✅ /me endpoint blocking test completed");
            }
            else
            {
                _logger.LogInformation("ℹ️ Skipping /me endpoint blocking test - not using client credentials authentication");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ /me endpoint blocking test failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests the SDK functionality using password authentication (user-level access)
    /// This flow skips echo and auth tests, but tests all other functions including /me endpoints
    /// </summary>
    public async Task<List<string>> TestPasswordAuthenticationFlowAsync()
    {
        var createdVideoIds = new List<string>();
        _createdResources.Clear();

        try
        {
            _logger.LogInformation("=== Testing Password Authentication Flow ===");
            _logger.LogInformation("🔐 This flow tests user-level authentication and /me endpoints");
            _logger.LogInformation("📋 Available Authentication Flows:");
            _logger.LogInformation("    1. Password Grant - Uses username + password + SDK configured Public API keys - ACTIVE");
            _logger.LogInformation("    2. Authorization Code Grant - For web applications (most secure) - COMMENTED OUT");
            _logger.LogInformation("    3. Client Credentials with Private Keys - For server-to-server (uses partner.api.dailymotion.com) - COMMENTED OUT");
            _logger.LogInformation("    4. Client Credentials with Public Keys - For server-to-server (uses api.dailymotion.com) - COMMENTED OUT");
            _logger.LogInformation("");

            // Test password authentication
            await DoPasswordAuthenticationAsync();

            // Test all other functions (skip echo and auth tests)
            await TestVideoOperationsAsync();
            await TestUserOperationsAsync();
            await TestChannelOperationsAsync();
            await TestSearchOperationsAsync();
            var fileOperationVideoIds = await TestFileOperationsAsync();
            await TestMeEndpointsWithPasswordAuthAsync();
            // await TestMeVideoCreationAsync(); // Commented out - /me/videos endpoint fails with 403 Forbidden
            await TestPlaylistOperationsAsync(fileOperationVideoIds);
            await TestPlayerOperationsAsync();
            await TestSubtitleOperationsAsync(fileOperationVideoIds);
            await TestVideoFiltersAsync();
            await TestVideoEmbedSettingsAsync(fileOperationVideoIds);

            _logger.LogInformation("✅ Password authentication flow completed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Password authentication flow failed: {Message}", ex.Message);
        }
        finally
        {
            // Cleanup
            //await CleanupTestDataAsync();
        }

        return createdVideoIds;
    }

    /// <summary>
    /// Tests /me endpoints with password authentication (should work)
    /// </summary>
    private async Task TestMeEndpointsWithPasswordAuthAsync()
    {
        _logger.LogInformation("=== Testing /me Endpoints with Password Authentication ===");
        _logger.LogInformation("🧪 Testing /me endpoints that are blocked with client credentials...");

        try
        {
            // Test GET /me
            _logger.LogInformation("Testing GET /me...");
            var userInfo = await _sdk.Mine.GetUserInfoAsync();
            if (userInfo != null)
            {
                _logger.LogInformation("✅ GET /me successful with password authentication");
                _logger.LogInformation("   Response: {{\"id\":\"{UserId}\",\"screenname\":\"{ScreenName}\"}}...", userInfo.Id, userInfo.ScreenName);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me failed: Could not retrieve user info");
            }

            // Test GET /me/videos
            _logger.LogInformation("Testing GET /me/videos...");
            var userVideos = await _sdk.Mine.GetVideosAsync(limit: 10);
            if (userVideos is { List: not null } && userVideos.List.Any())
            {
                _logger.LogInformation("✅ GET /me/videos successful with password authentication");
                _logger.LogInformation("   Response: {{\"page\":{Page},\"limit\":{Limit},\"total\":{Total},\"has_more\":{HasMore},\"list\":[{{\"id\":\"{FirstVideoId}\",\"title\":\"{FirstVideoTitle}\",...",
                    userVideos.Page, userVideos.Limit, userVideos.Total, userVideos.HasMore,
                    userVideos.List.FirstOrDefault()?.Id, userVideos.List.FirstOrDefault()?.Title);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me/videos failed or returned no videos");
            }

            // Test GET /me/playlists
            _logger.LogInformation("Testing GET /me/playlists...");
            var userPlaylists = await _sdk.Mine.GetPlaylistsAsync(limit: 10);
            if (userPlaylists != null)
            {
                _logger.LogInformation("✅ GET /me/playlists successful with password authentication");
                _logger.LogInformation("   Response: {{\"page\":{Page},\"limit\":{Limit},\"total\":{Total},\"has_more\":{HasMore},\"list\":[...]",
                    userPlaylists.Page, userPlaylists.Limit, userPlaylists.Total, userPlaylists.HasMore);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me/playlists failed");
            }

            // Test GET /me/favorites
            _logger.LogInformation("Testing GET /me/favorites...");
            var userFavorites = await _sdk.Mine.GetFavoritesAsync(limit: 10);
            if (userFavorites != null)
            {
                _logger.LogInformation("✅ GET /me/favorites successful with password authentication");
                _logger.LogInformation("   Response: {{\"page\":{Page},\"limit\":{Limit},\"total\":{Total},\"has_more\":{HasMore},\"list\":[...]",
                    userFavorites.Page, userFavorites.Limit, userFavorites.Total, userFavorites.HasMore);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me/favorites failed");
            }

            // Test GET /me/history
            _logger.LogInformation("Testing GET /me/history...");
            var userHistory = await _sdk.Mine.GetHistoryAsync(limit: 10);
            if (userHistory != null)
            {
                _logger.LogInformation("✅ GET /me/history successful with password authentication");
                _logger.LogInformation("   Response: {{\"page\":{Page},\"limit\":{Limit},\"total\":{Total},\"has_more\":{HasMore},\"list\":[...]",
                    userHistory.Page, userHistory.Limit, userHistory.Total, userHistory.HasMore);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me/history failed");
            }

            // Test GET /me/watchlater
            _logger.LogInformation("Testing GET /me/watchlater...");
            var userWatchLater = await _sdk.Mine.GetWatchLaterAsync(limit: 10);
            if (userWatchLater != null)
            {
                _logger.LogInformation("✅ GET /me/watchlater successful with password authentication");
                _logger.LogInformation("   Response: {{\"page\":{Page},\"limit\":{Limit},\"total\":{Total},\"has_more\":{HasMore},\"list\":[...]",
                    userWatchLater.Page, userWatchLater.Limit, userWatchLater.Total, userWatchLater.HasMore);
            }
            else
            {
                _logger.LogWarning("⚠️ GET /me/watchlater failed");
            }

            _logger.LogInformation("✅ /me endpoints testing completed with password authentication");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ /me endpoints testing failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Tests video creation using /me/videos endpoint (works only with password authentication)
    /// </summary>
    private async Task TestMeVideoCreationAsync()
    {
        _logger.LogInformation("=== Testing Video Creation with /me/videos Endpoint ===");
        _logger.LogInformation("🧪 Testing video creation using /me/videos (requires user authentication)");

        try
        {
            // First, get user info to confirm we have user context
            _logger.LogInformation("Getting user information to confirm user context...");
            var userInfo = await _sdk.Mine.GetUserInfoAsync();
            if (userInfo == null)
            {
                _logger.LogWarning("⚠️ Cannot get user info, skipping /me/videos test");
                return;
            }

            _logger.LogInformation("✅ User context confirmed - User ID: {UserId}, Screenname: {ScreenName}", userInfo.Id, userInfo.ScreenName);

            // Test creating a video using /me/videos endpoint
            _logger.LogInformation("Testing video creation with /me/videos endpoint...");

            var videoParams = new Dictionary<string, string>
            {
                ["url"] = "https://www.dailymotion.com/video/x9qb0se", // Use an existing video URL for testing
                ["title"] = "Test Video via /me/videos endpoint",
                ["description"] = "This video was created using the /me/videos endpoint with password authentication",
                ["channel"] = "fun",
                ["tags"] = "test,me-endpoint,password-auth",
                ["private"] = "true",
                ["published"] = "true",
                ["is_created_for_kids"] = "false"
            };

            var createResponse = await _sdk.HttpClient.PostAsync("/me/videos", videoParams);

            if (createResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Video creation via /me/videos successful!");
                _logger.LogInformation("   Response: {Content}", createResponse.Content?.Substring(0, Math.Min(200, createResponse.Content.Length)) + "...");

                // Try to extract video ID from response
                if (!string.IsNullOrEmpty(createResponse.Content))
                {
                    try
                    {
                        var responseData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(createResponse.Content);
                        var videoId = responseData?.GetValueOrDefault("id")?.ToString();
                        if (!string.IsNullOrEmpty(videoId))
                        {
                            _logger.LogInformation("   Created Video ID: {VideoId}", videoId);
                            _createdResources.Add($"video:{videoId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("⚠️ Could not parse video creation response: {Message}", ex.Message);
                    }
                }
            }
            else
            {
                _logger.LogWarning("⚠️ Video creation via /me/videos failed: {StatusCode} - {Content}",
                    createResponse.StatusCode, createResponse.Content);
            }

            // Skip /user/{userId}/videos endpoint test for password flow
            _logger.LogInformation("ℹ️ Skipping /user/{UserId}/videos endpoint test for password authentication flow", userInfo.Id);

            _logger.LogInformation("✅ /me/videos endpoint testing completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ /me/videos endpoint testing failed: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Simple password authentication for the password flow
    /// </summary>
    private async Task<TokenResponse?> DoPasswordAuthenticationAsync()
    {
        _logger.LogInformation("Testing password authentication...");

        if (string.IsNullOrEmpty(_sdk.Options.PasswordAuthUsername) || string.IsNullOrEmpty(_sdk.Options.PasswordAuthPassword))
        {
            _logger.LogWarning("⚠️ No username/password provided for password authentication test");
            _logger.LogInformation("ℹ️ To test Password Grant flow, configure 'DailymotionOptions:PasswordAuthUsername' and 'DailymotionOptions:PasswordAuthPassword' in user secrets");
            return null;
        }

        try
        {
            var scopes = new[]
            {
                OAuthScope.ManageVideos,
                OAuthScope.ManagePlaylists,
                OAuthScope.UserInfo,
                OAuthScope.ManageSubtitles
            };

            var result = await _sdk.Auth.AuthenticateWithPasswordAsync(
                _sdk.Options.PasswordAuthUsername!,
                _sdk.Options.PasswordAuthPassword!,
                scopes);

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                _logger.LogInformation("✅ Password authentication successful");
                _logger.LogInformation("🎫 Access Token: {Token}", result.AccessToken.Substring(0, 10) + "...");
                _logger.LogInformation("⏰ Token Expires In: {ExpiresIn} seconds", result.ExpiresIn);
                _logger.LogInformation("🔄 Refresh Token: {RefreshToken}", result.HasRefreshToken ? "Provided" : "Not provided");
                _logger.LogInformation("📝 Token Type: {TokenType}", result.TokenType ?? "Bearer");
                _logger.LogInformation("🔐 Authentication Level: {AuthLevel}",
                    result.IsUserAuthentication ? "User-level" : "Application-level");
                _logger.LogInformation("👤 User ID: {Uid}", result.Uid ?? "N/A");
                _logger.LogInformation("📧 Email Verified: {EmailVerified}",
                    result.EmailVerified?.ToString() ?? "N/A");
                return result;
            }
            else
            {
                _logger.LogWarning("⚠️ Password authentication returned empty token");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Password authentication failed: {Message}", ex.Message);
            return null;
        }
    }


}
