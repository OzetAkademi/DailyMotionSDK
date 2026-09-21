using System.ComponentModel;
using System.Reflection;

namespace DailymotionSDK.Models.Enums
{
    public enum LiveStreamFields
    {
        /// <summary>
        /// The advertising
        /// </summary>
        [Description("advertising")]
        Advertising,

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
        /// The enable comments
        /// </summary>
        [Description("enable_comments")]
        EnableComments,

        /// <summary>
        /// The end at
        /// </summary>
        [Description("end_at")]
        EndAt,

        /// <summary>
        /// The geo restriction
        /// </summary>
        [Description("geo_restriction")]
        GeoRestriction,

        /// <summary>
        /// The has paid partnership
        /// </summary>
        [Description("has_paid_partnership")]
        HasPaidPartnership,

        /// <summary>
        /// The ingest
        /// </summary>
        [Description("ingest")]
        Ingest,

        /// <summary>
        /// The is ai altered
        /// </summary>
        [Description("is_ai_altered")]
        IsAiAltered,

        /// <summary>
        /// The is exclusive content
        /// </summary>
        [Description("is_exclusive_content")]
        IsExclusiveContent,

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
        /// The language
        /// </summary>
        [Description("language")]
        Language,

        /// <summary>
        /// The livestream identifier
        /// </summary>
        [Description("livestream_id")]
        LivestreamId,

        /// <summary>
        /// The livestream URL
        /// </summary>
        [Description("livestream_url")]
        LivestreamUrl,

        /// <summary>
        /// The profile
        /// </summary>
        [Description("profile")]
        Profile,

        /// <summary>
        /// The recording
        /// </summary>
        [Description("recording")]
        Recording,

        /// <summary>
        /// The start at
        /// </summary>
        [Description("start_at")]
        StartAt,

        /// <summary>
        /// The status
        /// </summary>
        [Description("status")]
        Status,

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
        /// The visibility
        /// </summary>
        [Description("visibility")]
        Visibility,
    }

    public static class LiveStreamFieldsExtensions
    {
        /// <summary>
        /// Gets the API field name for a LiveStreamFields enum value
        /// </summary>
        /// <param name="field">The LiveStreamFields enum value</param>
        /// <returns>The API field name</returns>
        public static string GetApiFieldName(this LiveStreamFields field)
        {
            var attribute = field.GetType()
                .GetField(field.ToString())?
                .GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? field.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Converts an array of LiveStreamFields to an array of API field names
        /// </summary>
        /// <param name="fields">Array of LiveStreamFields</param>
        /// <returns>Array of API field names</returns>
        public static string[] ToApiFieldNames(this LiveStreamFields[] fields)
        {
            return [.. fields.Select(f => f.GetApiFieldName())];
        }
    }
}