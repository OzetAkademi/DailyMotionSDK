using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class LiveStreamEndRequest.
    /// </summary>
    public class LiveStreamEndRequest
    {
        /// <summary>
        /// The video id of the live stream.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonIgnore]
        [Required(ErrorMessage = "Id is required.")]
        public string? Id { get; set; }
    }
}