using System.ComponentModel;
using System.Reflection;

namespace DailymotionSDK.Models.Enums;

/// <summary>
/// Enum VideoFields
/// </summary>
public enum VideoFields
{
    /// <summary>
    /// The available formats
    /// </summary>
    [Description("available_formats")]
    AvailableFormats,

    /// <summary>
    /// The category
    /// </summary>
    [Description("category")]
    Category,

    /// <summary>
    /// The country
    /// </summary>
    [Description("country")]
    Country,

    /// <summary>
    /// The created at
    /// </summary>
    [Description("created_at")]
    CreatedAt,

    /// <summary>
    /// The description
    /// </summary>
    [Description("description")]
    Description,

    /// <summary>
    /// The embedding
    /// </summary>
    [Description("embedding")]
    Embedding,

    /// <summary>
    /// The enable ai chapter generation
    /// </summary>
    [Description("enable_ai_chapter_generation")]
    EnableAiChapterGeneration,

    /// <summary>
    /// The engagement message
    /// </summary>
    [Description("engagement_message")]
    EngagementMessage,

    /// <summary>
    /// The first frame
    /// </summary>
    [Description("first_frame")]
    FirstFrame,

    /// <summary>
    /// The geo restriction
    /// </summary>
    [Description("geo_restriction")]
    GeoRestriction,

    /// <summary>
    /// The hashtags
    /// </summary>
    [Description("hashtags")]
    Hashtags,

    /// <summary>
    /// The is ai altered
    /// </summary>
    [Description("is_ai_altered")]
    IsAiAltered,

    /// <summary>
    /// The is explicit
    /// </summary>
    [Description("is_explicit")]
    IsExplicit,

    /// <summary>
    /// The is for kids
    /// </summary>
    [Description("is_for_kids")]
    IsForKids,

    /// <summary>
    /// The is published
    /// </summary>
    [Description("is_published")]
    IsPublished,

    /// <summary>
    /// The language
    /// </summary>
    [Description("language")]
    Language,

    /// <summary>
    /// The preview
    /// </summary>
    [Description("preview")]
    Preview,

    /// <summary>
    /// The processing
    /// </summary>
    [Description("processing")]
    Processing,

    /// <summary>
    /// The profile
    /// </summary>
    [Description("profile")]
    Profile,

    /// <summary>
    /// The published at
    /// </summary>
    [Description("published_at")]
    PublishedAt,

    /// <summary>
    /// The source
    /// </summary>
    [Description("source")]
    Source,

    /// <summary>
    /// The tags
    /// </summary>
    [Description("tags")]
    Tags,

    /// <summary>
    /// The thumbnail
    /// </summary>
    [Description("thumbnail")]
    Thumbnail,

    /// <summary>
    /// The title
    /// </summary>
    [Description("title")]
    Title,

    /// <summary>
    /// The updated at
    /// </summary>
    [Description("updated_at")]
    UpdatedAt,

    /// <summary>
    /// The uploaded at
    /// </summary>
    [Description("uploaded_at")]
    UploadedAt,

    /// <summary>
    /// The video identifier
    /// </summary>
    [Description("video_id")]
    VideoId,

    /// <summary>
    /// The video URL
    /// </summary>
    [Description("video_url")]
    VideoUrl,

    /// <summary>
    /// The visibility
    /// </summary>
    [Description("visibility")]
    Visibility,

    /// <summary>
    /// The embedding embed HTML
    /// </summary>
    [Description("embedding.embed_html")]
    EmbeddingEmbedHtml,

    /// <summary>
    /// The embedding embed URL
    /// </summary>
    [Description("embedding.embed_url")]
    EmbeddingEmbedUrl,

    /// <summary>
    /// The first frame240 URL
    /// </summary>
    [Description("first_frame.240.url")]
    FirstFrame240Url,

    /// <summary>
    /// The first frame480 URL
    /// </summary>
    [Description("first_frame.480.url")]
    FirstFrame480Url,

    /// <summary>
    /// The first frame720 URL
    /// </summary>
    [Description("first_frame.720.url")]
    FirstFrame720Url,

    /// <summary>
    /// The first frame1080 URL
    /// </summary>
    [Description("first_frame.1080.url")]
    FirstFrame1080Url,

    /// <summary>
    /// The geo restriction countries
    /// </summary>
    [Description("geo_restriction.countries")]
    GeoRestrictionCountries,

    /// <summary>
    /// The geo restriction mode
    /// </summary>
    [Description("geo_restriction.mode")]
    GeoRestrictionMode,

    /// <summary>
    /// The preview240 URL
    /// </summary>
    [Description("preview.240.url")]
    Preview240Url,

    /// <summary>
    /// The preview480 URL
    /// </summary>
    [Description("preview.480.url")]
    Preview480Url,

    /// <summary>
    /// The preview720 URL
    /// </summary>
    [Description("preview.720.url")]
    Preview720Url,

    /// <summary>
    /// The preview1080 URL
    /// </summary>
    [Description("preview.1080.url")]
    Preview1080Url,

    /// <summary>
    /// The processing progress
    /// </summary>
    [Description("processing.progress")]
    ProcessingProgress,

    /// <summary>
    /// The processing status
    /// </summary>
    [Description("processing.status")]
    ProcessingStatus,

    /// <summary>
    /// The profile identifier
    /// </summary>
    [Description("profile.id")]
    ProfileId,

    /// <summary>
    /// The profile display name
    /// </summary>
    [Description("profile.display_name")]
    ProfileDisplayName,

    /// <summary>
    /// The profile avatar60 URL
    /// </summary>
    [Description("profile.avatar_60_url")]
    ProfileAvatar60Url,

    /// <summary>
    /// The profile avatar360 URL
    /// </summary>
    [Description("profile.avatar_360_url")]
    ProfileAvatar360Url,

    /// <summary>
    /// The source type
    /// </summary>
    [Description("source.type")]
    SourceType,

    /// <summary>
    /// The source URL
    /// </summary>
    [Description("source.url")]
    SourceUrl,

    /// <summary>
    /// The thumbnail240 URL
    /// </summary>
    [Description("thumbnail.240.url")]
    Thumbnail240Url,

    /// <summary>
    /// The thumbnail480 URL
    /// </summary>
    [Description("thumbnail.480.url")]
    Thumbnail480Url,

    /// <summary>
    /// The thumbnail720 URL
    /// </summary>
    [Description("thumbnail.720.url")]
    Thumbnail720Url,

    /// <summary>
    /// The thumbnail1080 URL
    /// </summary>
    [Description("thumbnail.1080.url")]
    Thumbnail1080Url,
}

public static class VideoFieldsExtensions
{
    /// <summary>
    /// Gets the API field name for a VideoFields enum value
    /// </summary>
    /// <param name="field">The VideoFields enum value</param>
    /// <returns>The API field name</returns>
    public static string GetApiFieldName(this VideoFields field)
    {
        var attribute = field.GetType()
            .GetField(field.ToString())?
            .GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? field.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Converts an array of VideoFields to an array of API field names
    /// </summary>
    /// <param name="fields">Array of VideoFields</param>
    /// <returns>Array of API field names</returns>
    public static string[] ToApiFieldNames(this VideoFields[] fields)
    {
        return [.. fields.Select(f => f.GetApiFieldName())];
    }
}