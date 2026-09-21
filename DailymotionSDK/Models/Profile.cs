using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class Profile.
    /// </summary>
    public class Profile
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

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can change name.
        /// </summary>
        /// <value><c>null</c> if [can change name] contains no value, <c>true</c> if [can change name]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("can_change_name")]
        public bool? CanChangeName { get; set; }

        /// <summary>
        /// Gets or sets the social links.
        /// </summary>
        /// <value>The social links.</value>
        [JsonPropertyName("social_links")]
        public SocialLinks? SocialLinks { get; set; }

        /// <summary>
        /// Gets or sets the content defaults.
        /// </summary>
        /// <value>The content defaults.</value>
        [JsonPropertyName("content_defaults")]
        public ContentDefaults? ContentDefaults { get; set; }

        /// <summary>
        /// Gets or sets the webhook.
        /// </summary>
        /// <value>The webhook.</value>
        [JsonPropertyName("webhook")]
        public Webhook? Webhook { get; set; }
    }

    /// <summary>
    /// Class SocialLinks.
    /// </summary>
    public class SocialLinks
    {
        /// <summary>
        /// Gets or sets the website URL.
        /// </summary>
        /// <value>The website URL.</value>
        [JsonPropertyName("website_url")]
        public string? WebsiteUrl { get; set; }
    }

    /// <summary>
    /// Class ContentDefaults.
    /// </summary>
    public class ContentDefaults
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is for kids.
        /// </summary>
        /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_for_kids")]
        public bool? IsForKids { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the ai subtitles.
        /// </summary>
        /// <value>The ai subtitles.</value>
        [JsonPropertyName("ai_subtitles")]
        public List<string>? AiSubtitles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comments].
        /// </summary>
        /// <value><c>null</c> if [enable comments] contains no value, <c>true</c> if [enable comments]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_comments")]
        public bool? EnableComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable ai chapter generation].
        /// </summary>
        /// <value><c>null</c> if [enable ai chapter generation] contains no value, <c>true</c> if [enable ai chapter generation]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_ai_chapter_generation")]
        public bool? EnableAiChapterGeneration { get; set; }

        /// <summary>
        /// Gets or sets the geo restriction.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        public GeoRestriction? GeoRestriction { get; set; }
    }

    /// <summary>
    /// Class GeoRestriction.
    /// </summary>
    public class GeoRestriction
    {
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        /// <value>The countries.</value>
        [JsonPropertyName("countries")]
        public List<string>? Countries { get; set; }
    }

    /// <summary>
    /// Class Webhook.
    /// </summary>
    public class Webhook
    {
        // Ready for future properties; currently an empty object in the JSON payload.
    }
}