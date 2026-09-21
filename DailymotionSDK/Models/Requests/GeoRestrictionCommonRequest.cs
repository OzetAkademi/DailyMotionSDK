using DailymotionSDK.Models.Enums;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class GeoRestriction. 
    /// Embedded object for video geo restriction fields.
    /// </summary>
    public class GeoRestrictionCommonRequest
    {
        /// <summary>
        /// Whether to allow or deny the listed countries.
        /// </summary>
        /// <value>The mode.</value>
        [JsonPropertyName("mode")]
        public GeoRestrictionMode? Mode { get; set; }

        /// <summary>
        /// GetsISO 3166-1 alpha-2 country codes to allow or deny. 
        /// Required when a geo_restriction object is provided (use[] for none). 
        /// Each item must match pattern: ^[A-Z]{2}$.
        /// </summary>
        /// <value>The countries.</value>
        [JsonPropertyName("countries")]
        public List<string>? Countries { get; set; }
    }
}