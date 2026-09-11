using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class Embedding. 
    /// Embed settings for this video (URLs and HTML snippet).
    /// </summary>
    public class EmbeddingCommonRequest
    {
        /// <summary>
        /// Whether embedding is enabled. 
        /// Writable. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>true</c> if [enable embed]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_embed")]
        public bool EnableEmbed { get; set; }
    }
}