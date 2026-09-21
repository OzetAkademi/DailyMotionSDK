using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Responses
{
    /// <summary>
    /// Class MeProfiles.
    /// </summary>
    public class MeProfiles
    {
        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        /// <value>The profile identifier.</value>
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}