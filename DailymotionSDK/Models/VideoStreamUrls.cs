using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class VideoStreamUrls.
    /// </summary>
    public class VideoStreamUrls
    {
        /// <summary>
        /// Gets or sets the stream urls.
        /// </summary>
        /// <value>The stream urls.</value>
        [JsonPropertyName("stream_urls")]
        public List<VideoStreamUrl>? StreamUrls { get; set; }
    }
}