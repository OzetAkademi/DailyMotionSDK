using DailymotionSDK.Models;
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
            //var uploadedVideoIds = await TestFileOperationsAsync();

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
                _sdk.Options.PrivateApiKey!,
                _sdk.Options.PrivateApiSecret!,
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
    /// Test playlist operations as an asynchronous operation.
    /// </summary>
    /// <param name="uploadedVideoIds">The uploaded video ids.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
   /* private async Task TestPlaylistOperationsAsync(List<string> uploadedVideoIds)
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
    }*/

    /// <summary>
    /// Test search operations as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /*private async Task TestSearchOperationsAsync()
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
    }*/

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
                        var parameters = new VideoCreationParameters()
                        {
                            Source = new() { FileUrl = fileUrl },
                            Title = videoTitle,
                            Description = videoDescription,
                            Category = "school",
                            Visibility = "private",
                            IsForKids = false,
                            IsAiAltered = false
                        };

                        _logger.LogInformation("🧪 Testing new VideoCreationParameters overload...");
                        var createdVideo = await _sdk.Videos.CreateVideoFromFileAsync(parameters);

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
                            var videoDetails = await _sdk.Videos.GetVideoAsync(videoId);

                            if (videoDetails != null)
                            {
                                _logger.LogInformation("✅ Video details retrieved for {VideoId}:", videoId);
                                _logger.LogInformation("   Title: {Title}", videoDetails.Title ?? "N/A");
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

        return createdVideoIds;
    }

    /// <summary>
    /// Test player operations as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
   /* private async Task TestPlayerOperationsAsync()
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
    }*/

    /// <summary>
    /// Tests video filtering functionality
    /// </summary>
    /*private async Task TestVideoFiltersAsync()
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
                    _logger.LogInformation("   - {Title}", video.Title);
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
                    _logger.LogInformation("   - {Title}", video.Title);
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
                    _logger.LogInformation("   - {Title}", video.Title);
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
    }*/

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
    /// Tests video creation using /me/videos endpoint (works only with password authentication)
    /// </summary>
    /*private async Task TestMeVideoCreationAsync()
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
    }*/
}