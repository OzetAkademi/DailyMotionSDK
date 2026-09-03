using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class Livestream.
    /// </summary>
    public class Livestream
    {
        /// <summary>
        /// Gets or sets the livestream identifier.
        /// </summary>
        /// <value>The livestream identifier.</value>
        [JsonPropertyName("livestream_id")]
        public string? LivestreamId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the livestream URL.
        /// </summary>
        /// <value>The livestream URL.</value>
        [JsonPropertyName("livestream_url")]
        public string? LivestreamUrl { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is for kids.
        /// </summary>
        /// <value><c>true</c> if this instance is for kids; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_for_kids")]
        public bool IsForKids { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is explicit.
        /// </summary>
        /// <value><c>true</c> if this instance is explicit; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_explicit")]
        public bool IsExplicit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is exclusive content.
        /// </summary>
        /// <value><c>true</c> if this instance is exclusive content; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_exclusive_content")]
        public bool IsExclusiveContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ai altered.
        /// </summary>
        /// <value><c>true</c> if this instance is ai altered; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_ai_altered")]
        public bool IsAiAltered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has paid partnership.
        /// </summary>
        /// <value><c>true</c> if this instance has paid partnership; otherwise, <c>false</c>.</value>
        [JsonPropertyName("has_paid_partnership")]
        public bool HasPaidPartnership { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comments].
        /// </summary>
        /// <value><c>true</c> if [enable comments]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_comments")]
        public bool EnableComments { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        [JsonPropertyName("profile")]
        public LivestreamProfile? Profile { get; set; }

        /// <summary>
        /// Gets or sets the start at.
        /// </summary>
        /// <value>The start at.</value>
        [JsonPropertyName("start_at")]
        public DateTime? StartAt { get; set; }

        /// <summary>
        /// Gets or sets the end at.
        /// </summary>
        /// <value>The end at.</value>
        [JsonPropertyName("end_at")]
        public DateTime? EndAt { get; set; }

        /// <summary>
        /// Gets or sets the ingest.
        /// </summary>
        /// <value>The ingest.</value>
        [JsonPropertyName("ingest")]
        public Ingest? Ingest { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonPropertyName("status")]
        public LivestreamStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the recording.
        /// </summary>
        /// <value>The recording.</value>
        [JsonPropertyName("recording")]
        public Recording? Recording { get; set; }

        /// <summary>
        /// Gets or sets the geo restriction.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        public LiveStreamGeoRestriction? GeoRestriction { get; set; }

        /// <summary>
        /// Gets or sets the embedding.
        /// </summary>
        /// <value>The embedding.</value>
        [JsonPropertyName("embedding")]
        public LivestreamEmbedding? Embedding { get; set; }

        /// <summary>
        /// Gets or sets the advertising.
        /// </summary>
        /// <value>The advertising.</value>
        [JsonPropertyName("advertising")]
        public Advertising? Advertising { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        [JsonPropertyName("thumbnail")]
        public LivestreamThumbnail? Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at.</value>
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Class LivestreamProfile.
    /// </summary>
    public class LivestreamProfile
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
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can change name.
        /// </summary>
        /// <value><c>true</c> if this instance can change name; otherwise, <c>false</c>.</value>
        [JsonPropertyName("can_change_name")]
        public bool CanChangeName { get; set; }

        /// <summary>
        /// Gets or sets the social links.
        /// </summary>
        /// <value>The social links.</value>
        [JsonPropertyName("social_links")]
        public ExtendedSocialLinks? SocialLinks { get; set; }

        /// <summary>
        /// Gets or sets the content defaults.
        /// </summary>
        /// <value>The content defaults.</value>
        [JsonPropertyName("content_defaults")]
        public LivestreamContentDefaults? ContentDefaults { get; set; }

        /// <summary>
        /// Gets or sets the webhook.
        /// </summary>
        /// <value>The webhook.</value>
        [JsonPropertyName("webhook")]
        public WebhookConfig? Webhook { get; set; }
    }

    /// <summary>
    /// Class ExtendedSocialLinks.
    /// </summary>
    public class ExtendedSocialLinks
    {
        /// <summary>
        /// Gets or sets the twitter URL.
        /// </summary>
        /// <value>The twitter URL.</value>
        [JsonPropertyName("twitter_url")]
        public string? TwitterUrl { get; set; }

        /// <summary>
        /// Gets or sets the instagram URL.
        /// </summary>
        /// <value>The instagram URL.</value>
        [JsonPropertyName("instagram_url")]
        public string? InstagramUrl { get; set; }

        /// <summary>
        /// Gets or sets the facebook URL.
        /// </summary>
        /// <value>The facebook URL.</value>
        [JsonPropertyName("facebook_url")]
        public string? FacebookUrl { get; set; }

        /// <summary>
        /// Gets or sets the website URL.
        /// </summary>
        /// <value>The website URL.</value>
        [JsonPropertyName("website_url")]
        public string? WebsiteUrl { get; set; }
    }

    /// <summary>
    /// Class LivestreamContentDefaults.
    /// </summary>
    public class LivestreamContentDefaults
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
        /// <value><c>true</c> if this instance is for kids; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_for_kids")]
        public bool IsForKids { get; set; }

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
        public List<string> Tags { get; set; } = [];

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
        public List<string> AiSubtitles { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether [enable comments].
        /// </summary>
        /// <value><c>true</c> if [enable comments]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_comments")]
        public bool EnableComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable ai chapter generation].
        /// </summary>
        /// <value><c>true</c> if [enable ai chapter generation]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_ai_chapter_generation")]
        public bool EnableAiChapterGeneration { get; set; }

        /// <summary>
        /// Gets or sets the geo restriction.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        public LiveStreamGeoRestriction? GeoRestriction { get; set; }
    }

    /// <summary>
    /// Class WebhookConfig.
    /// </summary>
    public class WebhookConfig
    {
        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        [JsonPropertyName("callback_url")]
        public string? CallbackUrl { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        /// <value>The events.</value>
        [JsonPropertyName("events")]
        public List<string> Events { get; set; } = [];
    }

    /// <summary>
    /// Class Ingest.
    /// </summary>
    public class Ingest
    {
        /// <summary>
        /// Gets or sets the RTMP URL.
        /// </summary>
        /// <value>The RTMP URL.</value>
        [JsonPropertyName("rtmp_url")]
        public string? RtmpUrl { get; set; }

        /// <summary>
        /// Gets or sets the SRT URL.
        /// </summary>
        /// <value>The SRT URL.</value>
        [JsonPropertyName("srt_url")]
        public string? SrtUrl { get; set; }

        /// <summary>
        /// Gets or sets the available servers.
        /// </summary>
        /// <value>The available servers.</value>
        [JsonPropertyName("available_servers")]
        public Dictionary<string, string> AvailableServers { get; set; } = [];
    }

    /// <summary>
    /// Class LivestreamStatus.
    /// </summary>
    public class LivestreamStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether [on air].
        /// </summary>
        /// <value><c>true</c> if [on air]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("onair")]
        public bool OnAir { get; set; }

        /// <summary>
        /// Gets or sets the airing at.
        /// </summary>
        /// <value>The airing at.</value>
        [JsonPropertyName("airing_at")]
        public DateTime AiringAt { get; set; }

        /// <summary>
        /// Gets or sets the audio bitrate.
        /// </summary>
        /// <value>The audio bitrate.</value>
        [JsonPropertyName("audio_bitrate")]
        public int AudioBitrate { get; set; }
    }

    /// <summary>
    /// Class Recording.
    /// </summary>
    public class Recording
    {
        /// <summary>
        /// Gets or sets a value indicating whether [automatic record].
        /// </summary>
        /// <value><c>true</c> if [automatic record]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("auto_record")]
        public bool AutoRecord { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the start at.
        /// </summary>
        /// <value>The start at.</value>
        [JsonPropertyName("start_at")]
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Gets or sets the end at.
        /// </summary>
        /// <value>The end at.</value>
        [JsonPropertyName("end_at")]
        public DateTime EndAt { get; set; }
    }

    /// <summary>
    /// Class LiveStreamGeoRestriction.
    /// </summary>
    public class LiveStreamGeoRestriction
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
        public List<string> Countries { get; set; } = [];

    }

    /// <summary>
    /// Class LivestreamEmbedding.
    /// </summary>
    public class LivestreamEmbedding
    {
        /// <summary>
        /// Gets or sets a value indicating whether [enable embed].
        /// </summary>
        /// <value><c>true</c> if [enable embed]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_embed")]
        public bool EnableEmbed { get; set; }

        /// <summary>
        /// Gets or sets the embed URL.
        /// </summary>
        /// <value>The embed URL.</value>
        [JsonPropertyName("embed_url")]
        public string? EmbedUrl { get; set; }

        /// <summary>
        /// Gets or sets the embed HTML.
        /// </summary>
        /// <value>The embed HTML.</value>
        [JsonPropertyName("embed_html")]
        public string? EmbedHtml { get; set; }
    }

    /// <summary>
    /// Class Advertising.
    /// </summary>
    public class Advertising
    {
        /// <summary>
        /// Gets or sets the end at.
        /// </summary>
        /// <value>The end at.</value>
        [JsonPropertyName("end_at")]
        public DateTime EndAt { get; set; }

        /// <summary>
        /// Gets or sets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }
    }

    /// <summary>
    /// Class LivestreamThumbnail.
    /// </summary>
    public class LivestreamThumbnail
    {
        /// <summary>
        /// Gets or sets the H1080 URL.
        /// </summary>
        /// <value>The H1080 URL.</value>
        [JsonPropertyName("h1080_url")]
        public string? H1080Url { get; set; }

        /// <summary>
        /// Gets or sets the H720 URL.
        /// </summary>
        /// <value>The H720 URL.</value>
        [JsonPropertyName("h720_url")]
        public string? H720Url { get; set; }

        /// <summary>
        /// Gets or sets the H480 URL.
        /// </summary>
        /// <value>The H480 URL.</value>
        [JsonPropertyName("h480_url")]
        public string? H480Url { get; set; }

        /// <summary>
        /// Gets or sets the H240 URL.
        /// </summary>
        /// <value>The H240 URL.</value>
        [JsonPropertyName("h240_url")]
        public string? H240Url { get; set; }

        /// <summary>
        /// Gets or sets the default URL.
        /// </summary>
        /// <value>The default URL.</value>
        [JsonPropertyName("default_url")]
        public string? DefaultUrl { get; set; }
    }
}
