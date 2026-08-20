using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class Source.
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Gets or sets the file URL.
        /// </summary>
        /// <value>The file URL.</value>
        [JsonPropertyName("file_url")]
        public string? FileUrl { get; set; }
    }
}