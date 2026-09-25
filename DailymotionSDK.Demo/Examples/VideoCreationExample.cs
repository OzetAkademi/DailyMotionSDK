using DailymotionSDK.Models;
using DailymotionSDK.Models.Enums;

namespace DailymotionSDK.Demo.Examples;

/// <summary>
/// Class VideoCreationExample.
/// </summary>
public class VideoCreationExample
{
    /// <summary>
    /// Create video with basic parameters as an asynchronous operation.
    /// </summary>
    /// <param name="sdk">The SDK.</param>
    /// <param name="profileId">The profile ID.</param>
    /// <param name="fileUrl">The file URL.</param>
    /// <returns>A Task&lt;VideoCreateResponse&gt; representing the asynchronous operation.</returns>
    public static async Task<VideoCreateResponse?> CreateVideoWithBasicParametersAsync(DailymotionHandler sdk, string profileId, string fileUrl)
    {
        return await sdk.Videos.CreateVideoFromFileAsync(
            profileId: profileId,
            fileUrl: fileUrl,
            title: "My Basic Video",
            description: "A simple video created with basic parameters",
            category: Category.School,
            tags: ["example", "basic", "demo"],
            isPrivate: false,
            published: true,
            isForKids: false
        );
    }
}