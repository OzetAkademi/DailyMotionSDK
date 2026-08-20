using System.Text.Json.Serialization;

namespace DailymotionSDK.Models
{
    /// <summary>
    /// Class Video.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Gets or sets the video identifier.
        /// </summary>
        /// <value>The video identifier.</value>
        [JsonPropertyName("video_id")]
        public string? VideoId { get; set; }

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
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

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
        /// Gets or sets a value indicating whether this instance is explicit.
        /// </summary>
        /// <value><c>null</c> if [is explicit] contains no value, <c>true</c> if [is explicit]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_explicit")]
        public bool? IsExplicit { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        [JsonPropertyName("profile")]
        public VideoProfile? Profile { get; set; }

        /// <summary>
        /// Gets or sets the video URL.
        /// </summary>
        /// <value>The video URL.</value>
        [JsonPropertyName("video_url")]
        public string? VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at.</value>
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the uploaded at.
        /// </summary>
        /// <value>The uploaded at.</value>
        [JsonPropertyName("uploaded_at")]
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Gets or sets the published at.
        /// </summary>
        /// <value>The published at.</value>
        [JsonPropertyName("published_at")]
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the engagement message.
        /// </summary>
        /// <value>The engagement message.</value>
        [JsonPropertyName("engagement_message")]
        public string? EngagementMessage { get; set; }

        /// <summary>
        /// Gets or sets the hashtags.
        /// </summary>
        /// <value>The hashtags.</value>
        [JsonPropertyName("hashtags")]
        public List<string>? Hashtags { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        /// <value><c>null</c> if [is published] contains no value, <c>true</c> if [is published]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_published")]
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the processing.
        /// </summary>
        /// <value>The processing.</value>
        [JsonPropertyName("processing")]
        public Processing? Processing { get; set; }

        /// <summary>
        /// Gets or sets the available formats.
        /// </summary>
        /// <value>The available formats.</value>
        [JsonPropertyName("available_formats")]
        public List<AvailableFormat>? AvailableFormats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ai altered.
        /// </summary>
        /// <value><c>null</c> if [is ai altered] contains no value, <c>true</c> if [is ai altered]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("is_ai_altered")]
        public bool? IsAiAltered { get; set; }

        /// <summary>
        /// Gets or sets the ai subtitles.
        /// </summary>
        /// <value>The ai subtitles.</value>
        [JsonPropertyName("ai_subtitles")]
        public List<string>? AiSubtitles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable ai chapter generation].
        /// </summary>
        /// <value><c>null</c> if [enable ai chapter generation] contains no value, <c>true</c> if [enable ai chapter generation]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_ai_chapter_generation")]
        public bool? EnableAiChapterGeneration { get; set; }

        /// <summary>
        /// Gets or sets the embedding.
        /// </summary>
        /// <value>The embedding.</value>
        [JsonPropertyName("embedding")]
        public Embedding? Embedding { get; set; }

        /// <summary>
        /// Gets or sets the geo restriction.
        /// </summary>
        /// <value>The geo restriction.</value>
        [JsonPropertyName("geo_restriction")]
        public GeoRestrictionItem? GeoRestriction { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [JsonPropertyName("source")]
        public SourceItem? Source { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        [JsonPropertyName("thumbnail")]
        public Thumbnail? Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the first frame.
        /// </summary>
        /// <value>The first frame.</value>
        [JsonPropertyName("first_frame")]
        public FirstFrame? FirstFrame { get; set; }

        /// <summary>
        /// Gets or sets the preview.
        /// </summary>
        /// <value>The preview.</value>
        [JsonPropertyName("preview")]
        public Preview? Preview { get; set; }
    }

    /// <summary>
    /// Class VideoProfile.
    /// </summary>
    public class VideoProfile
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
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    /// <summary>
    /// Class Processing.
    /// </summary>
    public class Processing
    {
        /// <summary>
        /// Gets or sets the encoding status.
        /// </summary>
        /// <value>The encoding status.</value>
        [JsonPropertyName("encoding_status")]
        public string? EncodingStatus { get; set; }

        /// <summary>
        /// Gets or sets the encoding progress.
        /// </summary>
        /// <value>The encoding progress.</value>
        [JsonPropertyName("encoding_progress")]
        public int? EncodingProgress { get; set; }

        /// <summary>
        /// Gets or sets the publishing progress.
        /// </summary>
        /// <value>The publishing progress.</value>
        [JsonPropertyName("publishing_progress")]
        public int? PublishingProgress { get; set; }
    }

    /// <summary>
    /// Class AvailableFormat.
    /// </summary>
    public class AvailableFormat
    {
        /// <summary>
        /// Gets or sets the format identifier.
        /// </summary>
        /// <value>The format identifier.</value>
        [JsonPropertyName("format_id")]
        public string? FormatId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        [JsonPropertyName("quality")]
        public string? Quality { get; set; }

        /// <summary>
        /// Gets or sets the codecs.
        /// </summary>
        /// <value>The codecs.</value>
        [JsonPropertyName("codecs")]
        public List<string>? Codecs { get; set; }

        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        /// <value>The sample rate.</value>
        [JsonPropertyName("sample_rate")]
        public int? SampleRate { get; set; }

        /// <summary>
        /// Gets or sets the frame rate.
        /// </summary>
        /// <value>The frame rate.</value>
        [JsonPropertyName("frame_rate")]
        public double? FrameRate { get; set; }

        /// <summary>
        /// Gets or sets the bitrate.
        /// </summary>
        /// <value>The bitrate.</value>
        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the type of the sub.
        /// </summary>
        /// <value>The type of the sub.</value>
        [JsonPropertyName("sub_type")]
        public string? SubType { get; set; }
    }

    /// <summary>
    /// Class Embedding.
    /// </summary>
    public class Embedding
    {
        /// <summary>
        /// Gets or sets a value indicating whether [enable embed].
        /// </summary>
        /// <value><c>null</c> if [enable embed] contains no value, <c>true</c> if [enable embed]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("enable_embed")]
        public bool? EnableEmbed { get; set; }

        /// <summary>
        /// Gets or sets the embed URL.
        /// </summary>
        /// <value>The embed URL.</value>
        [JsonPropertyName("embed_url")]
        public string? EmbedUrl { get; set; }
    }

    /// <summary>
    /// Class GeoRestrictionItem.
    /// </summary>
    public class GeoRestrictionItem
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
    /// Class SourceItem.
    /// </summary>
    public class SourceItem
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        [JsonPropertyName("height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the checksum.
        /// </summary>
        /// <value>The checksum.</value>
        [JsonPropertyName("checksum")]
        public string? Checksum { get; set; }
    }

    /// <summary>
    /// Class Thumbnail.
    /// </summary>
    public class Thumbnail
    {
        /// <summary>
        /// Gets or sets the H1080 URL.
        /// </summary>
        /// <value>The H1080 URL.</value>
        [JsonPropertyName("h1080_url")]
        public string? H1080Url { get; set; }
    }

    /// <summary>
    /// Class FirstFrame.
    /// </summary>
    public class FirstFrame
    {
        /// <summary>
        /// Gets or sets the H1080 URL.
        /// </summary>
        /// <value>The H1080 URL.</value>
        [JsonPropertyName("h1080_url")]
        public string? H1080Url { get; set; }
    }

    /// <summary>
    /// Class Preview.
    /// </summary>
    public class Preview
    {
        /// <summary>
        /// Gets or sets the P480 URL.
        /// </summary>
        /// <value>The P480 URL.</value>
        [JsonPropertyName("p480_url")]
        public string? P480Url { get; set; }
    }
}