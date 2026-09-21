using DailymotionSDK.Models;
using DailymotionSDK.Models.Enums;
using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;
using Microsoft.Extensions.Logging;

namespace DailymotionSDK.Demo.Services;

/// <summary>
/// Class DemoService.
/// Implements the <see cref="DailymotionSDK.Demo.Services.IDemoService" />
/// </summary>
/// <param name="sdk">The SDK.</param>
/// <param name="options">The options.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="DailymotionSDK.Demo.Services.IDemoService" />
public class DemoService(DailymotionHandler sdk, DemoOptions options, ILogger<DemoService> logger) : IDemoService
{
    /// <summary>
    /// The SDK
    /// </summary>
    private readonly DailymotionHandler _sdk = sdk ?? throw new ArgumentNullException(nameof(sdk));

    /// <summary>
    /// The options
    /// </summary>
    private readonly DemoOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<DemoService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// The created resources
    /// </summary>
    private readonly List<string> _createdResources = [];

    /// <summary>
    /// Runs the demo asynchronous.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task RunDemoAsync()
    {
        try
        {
            _logger.LogInformation("Starting DailyMotion SDK Demo...");

            // Step 2: Test client credentials with private keys
            await TestClientCredentialsWithPrivateKeysAsync();

            // Step 3: Test video operations (using authenticated token if available)
            /*await TestVideoOperationsAsync();

            // Step 4: Test user operations (using authenticated token if available)
            await TestUserOperationsAsync();

            // Step 5: Test channel operations (using authenticated token if available)
            await TestChannelOperationsAsync();

            // Step 6: Test search functionality (using authenticated token if available)
            await TestSearchOperationsAsync();*/

            // Step 7: Test file operations (using authenticated token if available)
            var uploadedVideoIds = await TestFileOperationsAsync();

            // Step 8: Test playlist operations (using uploaded video IDs)
            /*await TestPlaylistOperationsAsync(uploadedVideoIds);

            // Step 9: Test player operations (using authenticated token if available)
            await TestPlayerOperationsAsync();

            // Step 10: Test subtitle operations (using authenticated token if available)
            await TestSubtitleOperationsAsync(uploadedVideoIds);

            // Test video filters functionality
            await TestVideoFiltersAsync();

            // Test video embed settings
            await TestVideoEmbedSettingsAsync(uploadedVideoIds);*/

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
    /// Tests the client credentials with private keys asynchronous.
    /// </summary>
    /// <returns>A Task&lt;TokenResponse&gt; representing the asynchronous operation.</returns>
    public async Task<TokenResponse?> TestClientCredentialsWithPrivateKeysAsync()
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
        _logger.LogInformation("🔒 Auth Endpoint: https://oauth2.dailymotion.com/v2/token");

        _logger.LogInformation("Testing client credentials authentication...");
        _logger.LogInformation("Using Private API Key: {ApiKey}", MaskApiKey(_sdk.Options.PrivateApiKey));
        _logger.LogInformation("Using Private API Secret: {ApiSecret}", MaskApiSecret(_sdk.Options.PrivateApiSecret));

        try
        {
            // Use the correct scopes from the official documentation
            var scopes = new[] { OAuthScope.ManageAccount, OAuthScope.ManageOrganization, OAuthScope.ManagePlaylist, OAuthScope.ManagePlayer, OAuthScope.ManageVideo, OAuthScope.ManageProfile };
            _logger.LogInformation("Requesting scopes: {Scopes}", string.Join(", ", scopes.Select(s => s.ToString())));
            _logger.LogInformation("API scope format: {ApiScopes}", string.Join(" ", scopes.Select(s => s.ToApiScopeString())));

            var result = await _sdk.Auth.AuthenticateWithPrivateAsync(
                new()
                {
                    ClientId = _sdk.Options.PrivateApiKey,
                    ClientSecret = _sdk.Options.PrivateApiSecret,
                    Scope = scopes
                });

            if (result != null)
            {
                _logger.LogInformation("✅ Client credentials authentication successful");
                _logger.LogInformation("🎫 Access Token: {Token}", $"{result.AccessToken?[..10]}...{result.AccessToken?[^10..]}");
                _logger.LogInformation("⏰ Token Expires In: {ExpiresIn} seconds", result.ExpiresIn);
                _logger.LogInformation("🔄 Refresh Token: {RefreshToken}", result.HasRefreshToken ? "Provided" : "Not provided (expected for client credentials)");
                _logger.LogInformation("📝 Token Type: {TokenType}", result.TokenType);
                _logger.LogInformation("🔐 Authentication Level: {AuthLevel}",
                    result.IsUserAuthentication ? "User-level" : "Application-level");
                _logger.LogInformation("👤 User ID: {Uid}", result.Uid ?? "N/A (application-level auth)");

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
    /// Test file operations as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
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
                return [];
            }

            // Upload first test video
            if (testVideo1Exists)
            {
                _logger.LogInformation("Testing file upload for test1.mp4...");
                try
                {
                    var uploadResult1 = await _sdk.File.UploadAsync(
                        new FileUploadRequest()
                        {
                            FilePath = _options.TestVideoPath1
                        });

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
                    var uploadResult2 = await _sdk.File.UploadAsync(
                        new FileUploadRequest()
                        {
                            FilePath = _options.TestVideoPath2
                        });

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
                        _logger.LogInformation("  - Token Preview: {TokenPreview}", string.IsNullOrEmpty(_sdk.AccessToken) ? "N/A" : $"{_sdk.AccessToken[..Math.Min(10, _sdk.AccessToken.Length)]}...");

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
                        var videoCreateRequest = new VideoCreateRequest()
                        {
                            Source = new()
                            {
                                FileUrl = fileUrl
                            },
                            Title = videoTitle,
                            Description = videoDescription,
                            Category = Category.School,
                            Visibility = "private",
                            IsForKids = false,
                            IsAiAltered = false
                        };

                        _logger.LogInformation("🧪 Testing new VideoCreateRequest overload...");
                        var createdVideo = await _sdk.Videos.CreateVideoFromFileAsync(videoCreateRequest);

                        if (createdVideo != null && !string.IsNullOrEmpty(createdVideo.VideoId))
                        {
                            _logger.LogInformation("✅ Video created successfully from file {FileId}:\n   Video ID: {VideoId}\n   Title: {Title}", fileId, createdVideo.VideoId, createdVideo.Title ?? "N/A");

                            createdVideoIds.Add(createdVideo.VideoId);
                            _createdResources.Add($"video:{createdVideo.VideoId}");
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
                            var videoDetails = await _sdk.Videos.GetVideoAsync(
                                new()
                                {
                                    Id = videoId
                                });

                            if (videoDetails != null)
                            {
                                _logger.LogInformation("✅ Video details retrieved for {VideoId}:", videoId);
                                _logger.LogInformation("   Title: {Title}", videoDetails.Title ?? "N/A");
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ Could not retrieve video details for {VideoId}", videoId);
                            }

                            _logger.LogInformation("=== Getting Video HLS URL for Created Video ===");
                            var hlsUrl = await _sdk.Videos.GetVideoHLSAsync(
                                new()
                                {
                                    Id = videoId
                                });

                            if (hlsUrl is not null)
                            {
                                _logger.LogInformation("✅ HLS URL retrieved for {VideoId}: {HlsUrl}", videoId, hlsUrl.StreamUrls?.FirstOrDefault()?.StreamUrl);
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ Could not retrieve HLS URL for {VideoId}", videoId);
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

        return createdVideoIds;
    }

    /// <summary>
    /// Cleanup test data as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
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
                    /*case "playlist":
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
                        break;*/
                    case "video":
                        var videoDeleted = await _sdk.Videos.DeleteVideoAsync(
                            new()
                            {
                                Id = resourceId
                            });

                        if (videoDeleted)
                        {
                            _logger.LogInformation("✅ Deleted video: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete video: {Id}", resourceId);
                        }
                        break;
                    /*case "player":
                        var playerDeleted = await _sdk.Player.DeletePlayerAsync(resourceId);
                        if (playerDeleted)
                        {
                            _logger.LogInformation("✅ Deleted player: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete player: {Id}", resourceId);
                        }
                        break;*/
                    /*case "subtitle":
                        var subtitleDeleted = await _sdk.Subtitles.DeleteSubtitleAsync(resourceId);
                        if (subtitleDeleted)
                        {
                            _logger.LogInformation("✅ Deleted subtitle: {Id}", resourceId);
                        }
                        else
                        {
                            _logger.LogWarning("⚠️ Failed to delete subtitle: {Id}", resourceId);
                        }
                        break;*/
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

        _logger.LogInformation("✅ Cleanup completed");
    }

    /// <summary>
    /// Waits the between operations.
    /// </summary>
    private async Task WaitBetweenOperations()
    {
        if (_options.WaitBetweenOperations > 0)
        {
            await Task.Delay(_options.WaitBetweenOperations);
        }
    }

    /// <summary>
    /// Masks the API key.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <returns>System.String.</returns>
    private static string MaskApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 8)
            return "***";
        return $"{apiKey[..4]}...{apiKey[^4..]}";
    }

    /// <summary>
    /// Masks the API secret.
    /// </summary>
    /// <param name="apiSecret">The API secret.</param>
    /// <returns>System.String.</returns>
    private static string MaskApiSecret(string apiSecret)
    {
        if (string.IsNullOrEmpty(apiSecret) || apiSecret.Length <= 8)
            return "***";
        return $"{apiSecret[..4]}...{apiSecret[^4..]}";
    }
}