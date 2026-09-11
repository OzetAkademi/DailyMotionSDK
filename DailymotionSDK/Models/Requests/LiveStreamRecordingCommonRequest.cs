using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class LiveStreamRecordingCommonRequest.
    /// Recording configuration and status.
    /// </summary>
    public class LiveStreamRecordingCommonRequest
    {
        /// <summary>
        /// Enable DVR auto-recording. 
        /// Writable. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [automatic record] contains no value, <c>true</c> if [automatic record]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("auto_record")]
        public bool? AutoRecord { get; set; } = true;
    }
}