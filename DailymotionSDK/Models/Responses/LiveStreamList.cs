using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Responses
{
    /// <summary>
    /// Class LiveStreamList.
    /// </summary>
    public class LiveStreamList
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonPropertyName("data")]
        public List<Livestream>? Data { get; set; }

        /// <summary>
        /// Gets or sets the pagination.
        /// </summary>
        /// <value>The pagination.</value>
        [JsonPropertyName("pagination")]
        public Pagination? Pagination { get; set; }
    }
}