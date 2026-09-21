using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class Source. 
    /// Source media metadata and optional ingest URL for create/replace.
    /// </summary>
    public class SourceCommonRequest
    {
        /// <summary>
        /// HTTPS URL of the media file to ingest when creating or replacing a video. 
        /// URL scheme must be one of: http, https.       
        /// </summary>
        /// <value>The file URL.</value>
        [JsonPropertyName("file_url")]
        public string? FileUrl { get; set; }
    }
}