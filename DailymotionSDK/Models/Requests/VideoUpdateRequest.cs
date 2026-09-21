using DailymotionSDK.Models.Enums;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class VideoUpdateRequest.
    /// </summary>
    public class VideoUpdateRequest
    {
        /// <summary>
        /// Video Id.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonIgnore]
        public string? Id { get; set; }

        /// <summary>
        /// Video title shown in the player and catalog. 
        /// Must be at least 1 characters. 
        /// Must be at most 255 characters. 
        /// Must match pattern: (?:\S+\s*)+.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }

        /// <summary>
        /// Short or long description; HTML allowed where configured. 
        /// Leading and trailing whitespace is trimmed. Must be at most 3000 characters. 
        /// Must match pattern: (?:\S+\s*)+. Accepts HTML content.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        /// <summary>
        /// Content category used for discovery and reporting. Required on create.
        /// </summary>
        /// <value>The category.</value>
        [JsonPropertyName("category")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Category? Category { get; set; }

        /// <summary>
        /// Who can watch the video (public, private, or password).
        /// </summary>
        /// <value>The visibility.</value>
        [JsonPropertyName("visibility")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Visibility? Visibility { get; set; }

        /// <summary>
        /// Whether the video is made for kids (COPPA). 
        /// Required on create. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_for_kids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsForKids { get; set; }

        /// <summary>
        /// Whether the video is explicit. 
        /// Warning This flag cannot be removed once set. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is explicit] contains no value, <c>true</c> if [is explicit]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_explicit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsExplicit { get; set; }

        /// <summary>
        /// Whether the video may be used as a source for content stitching. 
        /// Stitching requires a public video: setting it on a private or password-protected video leaves it disabled. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [can be stitched] contains no value, <c>true</c> if [can be stitched]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("can_be_stitched")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CanBeStitched { get; set; }

        /// <summary>
        /// Whether comments are enabled on this video. 
        /// Comments cannot be enabled on a video made for kids: the request is refused upstream with 403. 
        /// Strict type checking(no coercion).
        /// </summary>
        /// <value><c>null</c> if [enable comments] contains no value, <c>true</c> if [enable comments]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_comments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableComments { get; set; }

        /// <summary>
        /// Whether the video is exclusive to Dailymotion. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is exclusive content] contains no value, <c>true</c> if [is exclusive content]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_exclusive_content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsExclusiveContent { get; set; }

        /// <summary>
        /// Whether the video discloses a paid partnership. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [has paid partnership] contains no value, <c>true</c> if [has paid partnership]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("has_paid_partnership")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HasPaidPartnership { get; set; }

        /// <summary>
        /// Password for the video. Writable. Must be at least 1 character. 
        /// Must be at most 32 characters. 
        /// Required when visibility is password. 
        /// Only allowed when visibility is password.
        /// </summary>
        /// <value>The password.</value>
        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Password { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when the video was published.
        /// </summary>
        /// <value>The published at.</value>
        [JsonPropertyName("published_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PublishedAt { get; set; }

        /// <summary>
        /// ISO 639-1 language code (e.g. en, fr). 
        /// Must match pattern: ^[a-zA-Z]{2}$.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Language { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2 country code (e.g. US, FR). 
        /// Must match pattern: ^[a-zA-Z]{2}$.
        /// </summary>
        /// <value>The country.</value>
        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Country { get; set; }

        /// <summary>
        /// Engagement message associated with the video. 
        /// Must be at most 500 characters. 
        /// Must match pattern: (?:[\S]+[\s]*)+.
        /// </summary>
        /// <value>The engagement message.</value>
        [JsonPropertyName("engagement_message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EngagementMessage { get; set; }

        /// <summary>
        /// Hashtags associated with the video.
        /// </summary>
        /// <value>The hash tags.</value>
        [JsonPropertyName("hash_tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? HashTags { get; set; }

        /// <summary>
        /// Tags associated with the video.
        /// </summary>
        /// <value>The tags.</value>
        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Tags { get; set; }

        /// <summary>
        /// Whether the video content was altered with AI. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is ai altered] contains no value, <c>true</c> if [is ai altered]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_ai_altered")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsAiAltered { get; set; }

        /// <summary>
        /// Languages available for AI-generated subtitles.
        /// </summary>
        /// <value>The ai subtitles.</value>
        [JsonPropertyName("ai_subtitles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? AiSubtitles { get; set; }

        /// <summary>
        /// Whether AI chapter generation is enabled. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [enable ai chapter generation] contains no value, <c>true</c> if [enable ai chapter generation]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_ai_chapter_generation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableAiChapterGeneration { get; set; }

        /// <summary>
        /// Embed settings for this video (URLs and HTML snippet).
        /// </summary>
        /// <value>The embedding.</value>
        [JsonPropertyName("embedding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EmbeddingCommonRequest? Embedding { get; set; }

        /// <summary>
        /// Embedded object for video geo restriction fields.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GeoRestrictionCommonRequest? GeoRestriction { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [JsonPropertyName("source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SourceCommonRequest? Source { get; set; }
    }
}