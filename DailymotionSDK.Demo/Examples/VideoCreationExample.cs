using DailymotionSDK.Models;

namespace DailymotionSDK.Demo.Examples;

/// <summary>
/// Example demonstrating the new VideoCreationParameters overload
/// </summary>
public class VideoCreationExample
{
    /// <summary>
    /// Example of creating a video with basic parameters using the original method
    /// </summary>
    public static async Task<VideoMetadata?> CreateVideoWithBasicParametersAsync(DailymotionHandler sdk, string fileUrl)
    {
        return await sdk.Videos.CreateVideoFromFileAsync(
            fileUrl: fileUrl,
            title: "My Basic Video",
            description: "A simple video created with basic parameters",
            channel: "fun",
            tags: new[] { "example", "basic", "demo" },
            isPrivate: false,
            published: true,
            isCreatedForKids: false
        );
    }

    /// <summary>
    /// Example of creating a video with comprehensive parameters using the new overload
    /// </summary>
    public static async Task<VideoMetadata?> CreateVideoWithComprehensiveParametersAsync(DailymotionHandler sdk, string fileUrl)
    {
        var parameters = new VideoCreationParameters
        {
            // Required fields
            Url = fileUrl,
            Title = "My Comprehensive Video",

            // Basic video properties
            Description = "A comprehensive video created with many supported parameters from the Dailymotion API",
            Channel = "news",
            Tags = new[] { "comprehensive", "example", "demo", "advanced" },
            Language = "en",
            Country = "US",

            // Privacy and visibility settings
            Private = false,
            Published = true,
            IsCreatedForKids = false,

            // Video mode
            Mode = "vod", // Video on demand

            // Advertising and monetization
            AdvertisingCustomTarget = "demo_target_123",
            AdvertisingInstreamBlocked = false,

            // AI and automation
            AiChapterGenerationRequired = false,
            StreamAlteredWithAi = false,

            // Embedding and sharing
            AllowEmbed = true,
            AllowedInPlaylists = true,

            // Content provider
            ContentProviderId = "DEMO_PROVIDER_001",

            // Custom classification
            CustomClassification = new[] { "demo", "test", "sdk" },

            // Content flags
            Explicit = false,

            // Geoblocking (allow in US, Canada, UK)
            Geoblocking = new[] { "allow", "US", "CA", "GB" },

            // Geolocation (New York City)
            Geoloc = new[] { -74.0060, 40.7128 },

            // Hashtags
            Hashtags = new[] { "#demo", "#sdk", "#dailymotion" },

            // Live streaming settings (for live videos)
            LiveAutoRecord = true,

            // Player settings
            PlayerNextVideo = null, // No next video

            // Soundtrack
            SoundtrackIsrc = "US-UMG-21-00001",
            SoundtrackPopularity = 85
        };

        return await sdk.Videos.CreateVideoFromFileAsync(parameters);
    }

    /// <summary>
    /// Example of creating a video with minimal custom parameters using the new overload
    /// </summary>
    public static async Task<VideoMetadata?> CreateVideoWithMinimalCustomParametersAsync(DailymotionHandler sdk, string fileUrl)
    {
        var parameters = new VideoCreationParameters
        {
            // Required fields only
            Url = fileUrl,
            Title = "My Minimal Custom Video",

            // Just a few optional fields
            Description = "A video created with minimal custom parameters",
            Channel = "fun",
            Mode = "vod",
            Language = "en"
        };

        return await sdk.Videos.CreateVideoFromFileAsync(parameters);
    }
}
