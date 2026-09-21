using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class VideoStreamUrl.
    /// </summary>
    public class VideoStreamUrl
    {
        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        [JsonPropertyName("protocol")]
        public string? Protocol { get; set; }
        /// <summary>
        /// Gets or sets the stream URL.
        /// </summary>
        /// <value>The stream URL.</value>
        [JsonPropertyName("stream_url")]
        public string? StreamUrl { get; set; }
    }
}