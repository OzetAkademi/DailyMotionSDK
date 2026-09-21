using DailymotionSDK.Models.Enums;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class VideoGetRequest.
    /// </summary>
    public class VideoGetRequest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public VideoFields[]? Fields { get; set; }
    }
}