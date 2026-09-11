using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Enums
{
    /// <summary>
    /// Enum Category
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// The animals
        /// </summary>
        [JsonStringEnumMemberName("animals")] Animals,
        /// <summary>
        /// The automatic
        /// </summary>
        [JsonStringEnumMemberName("auto")] Auto,
        /// <summary>
        /// The creation
        /// </summary>
        [JsonStringEnumMemberName("creation")] Creation,
        /// <summary>
        /// The fun
        /// </summary>
        [JsonStringEnumMemberName("fun")] Fun,
        /// <summary>
        /// The kids
        /// </summary>
        [JsonStringEnumMemberName("kids")] Kids,
        /// <summary>
        /// The lifestyle
        /// </summary>
        [JsonStringEnumMemberName("lifestyle")] Lifestyle,
        /// <summary>
        /// The music
        /// </summary>
        [JsonStringEnumMemberName("music")] Music,
        /// <summary>
        /// The news
        /// </summary>
        [JsonStringEnumMemberName("news")] News,
        /// <summary>
        /// The people
        /// </summary>
        [JsonStringEnumMemberName("people")] People,
        /// <summary>
        /// The school
        /// </summary>
        [JsonStringEnumMemberName("school")] School,
        /// <summary>
        /// The sport
        /// </summary>
        [JsonStringEnumMemberName("sport")] Sport,
        /// <summary>
        /// The tech
        /// </summary>
        [JsonStringEnumMemberName("tech")] Tech,
        /// <summary>
        /// The travel
        /// </summary>
        [JsonStringEnumMemberName("travel")] Travel,
        /// <summary>
        /// The tv
        /// </summary>
        [JsonStringEnumMemberName("tv")] TV,
        /// <summary>
        /// The video games
        /// </summary>
        [JsonStringEnumMemberName("videogames")] VideoGames,
        /// <summary>
        /// The webcam
        /// </summary>
        [JsonStringEnumMemberName("webcam")] Webcam
    }
}