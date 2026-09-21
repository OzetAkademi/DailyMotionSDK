using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Enums
{
    /// <summary>
    /// Enum Visibility
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// The public
        /// </summary>
        [JsonStringEnumMemberName("public")] Public,
        /// <summary>
        /// The private
        /// </summary>
        [JsonStringEnumMemberName("private")] Private,
        /// <summary>
        /// The password
        /// </summary>
        [JsonStringEnumMemberName("password")] Password
    }
}