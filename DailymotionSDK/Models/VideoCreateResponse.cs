using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class VideoCreateResponse.
    /// </summary>
    public class VideoCreateResponse
    {
        /// <summary>
        /// Gets or sets the video identifier.
        /// </summary>
        /// <value>The video identifier.</value>
        [JsonPropertyName("video_id")]
        public string? VideoId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}