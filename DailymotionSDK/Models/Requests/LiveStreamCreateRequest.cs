using DailymotionSDK.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class LiveStreamCreateRequest.
    /// </summary>
    public class LiveStreamCreateRequest
    {
        /// <summary>
        /// Parent resource ID.
        /// </summary>
        /// <value>The profile identifier.</value>
        [JsonIgnore]
        [Required(ErrorMessage = "ProfileId is required.")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// Video title shown in the player and catalog. 
        /// Must be at least 1 characters. 
        /// Must be at most 255 characters. Must match pattern: (?:\S+\s*)+.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")]
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        /// <summary>
        /// Short or long description; HTML allowed where configured. 
        /// Leading and trailing whitespace is trimmed. Must be at most 3000 characters. 
        /// Must match pattern: (?:\S+\s*)+. Accepts HTML content.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// ISO 639-1 language code (e.g. en, fr). 
        /// Must match pattern: ^[a-zA-Z]{2}$.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Language { get; set; }

        /// <summary>
        /// Content category used for discovery and reporting. Required on create.
        /// </summary>
        /// <value>The category.</value>
        [JsonPropertyName("category")]
        [Required(ErrorMessage = "Category is required.")]
        public Category? Category { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2 country code (e.g. US, FR). 
        /// Must match pattern: ^[a-zA-Z]{2}$.
        /// </summary>
        /// <value>The country.</value>
        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Country { get; set; }

        /// <summary>
        /// Who can watch the video (public, private, or password).
        /// </summary>
        /// <value>The visibility.</value>
        [JsonPropertyName("visibility")]
        [Required(ErrorMessage = "Visibility is required.")]
        public string? Visibility { get; set; }

        /// <summary>
        /// Whether the video is made for kids (COPPA). 
        /// Required on create. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_for_kids")]
        [Required(ErrorMessage = "IsForKids is required.")]
        public bool? IsForKids { get; set; } = false;

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
        /// Whether the video is exclusive to Dailymotion. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is exclusive content] contains no value, <c>true</c> if [is exclusive content]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_exclusive_content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsExclusiveContent { get; set; }

        /// <summary>
        /// Whether the video content was altered with AI. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [is ai altered] contains no value, <c>true</c> if [is ai altered]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_ai_altered")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsAiAltered { get; set; }

        /// <summary>
        /// Whether the video discloses a paid partnership. 
        /// Strict type checking (no coercion).
        /// </summary>
        /// <value><c>null</c> if [has paid partnership] contains no value, <c>true</c> if [has paid partnership]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("has_paid_partnership")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HasPaidPartnership { get; set; }

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
        /// ISO 8601 timestamp when the stream started (or is scheduled to start).
        /// </summary>
        /// <value>The start at.</value>
        [JsonPropertyName("start_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? StartAt { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when the stream ended (or is scheduled to end).
        /// </summary>
        /// <value>The end at.</value>
        [JsonPropertyName("end_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? EndAt { get; set; }

        /// <summary>
        /// Recording configuration and status.
        /// </summary>
        /// <value>The recording.</value>
        [JsonPropertyName("recording")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LiveStreamRecordingCommonRequest? Recording { get; set; }

        /// <summary>
        /// GetsGeo-restriction settings. 
        /// Optional at creation; when provided, both.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GeoRestrictionCommonRequest? GeoRestriction { get; set; }

        /// <summary>
        /// Embedding settings for the live player.
        /// </summary>
        /// <value>The embedding.</value>
        [JsonPropertyName("embedding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EmbeddingCommonRequest? Embedding { get; set; }
    }
}
