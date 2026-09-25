using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Enums
{
    /// <summary>
    /// Enum GeoRestrictionMode
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GeoRestrictionMode
    {
        /// <summary>
        /// The allow
        /// </summary>
        [JsonStringEnumMemberName("allow")] 
        Allow,
        /// <summary>
        /// The deny
        /// </summary>
        [JsonStringEnumMemberName("deny")] 
        Deny
    }
}