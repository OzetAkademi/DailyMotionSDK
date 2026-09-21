using DailymotionSDK.Models.Enums;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class VideoListRequest.
    /// </summary>
    public class VideoListRequest
    {
        /// <summary>
        /// Gets or sets the profile identifier. 
        /// Parent resource ID 
        /// </summary>
        /// <value>The profile identifier.</value>
        public string? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the video query parameters.
        /// </summary>
        /// <value>The video query parameters.</value>
        public VideoQueryParameters? VideoQueryParameters { get; set; }
    }

    /// <summary>
    /// Class VideoQueryParameters.
    /// </summary>
    public class VideoQueryParameters
    {
        /// <summary>
        /// integer ≥ 1 Defaults to 1
        /// </summary>
        /// <value>The page.</value>
        public int? Page { get; set; } = 1;

        /// <summary>
        /// integer 1 to 100 Defaults to 20
        /// </summary>
        /// <value>The size of the page.</value>
        public int? PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the fields. 
        /// Comma-separated field names to include.
        /// </summary>
        /// <value>The fields.</value>
        public VideoFields[]? Fields { get; set; }

        /// <summary>
        /// Comma-separated field names. 
        /// Prefix with - for descending order (e.g. rating,-created_at). 
        /// Sortable fields: created_at
        /// </summary>
        /// <value>The sort.</value>
        public string? Sort { get; set; } = "created_at";

        /// <summary>
        /// Filter by visibility (public, private, password).
        /// </summary>
        /// <value>The visibility.</value>
        public Visibility? Visibility { get; set; }

        /// <summary>
        /// Filter by advertising enabled status.
        /// </summary>
        /// <value><c>null</c> if [enable advertising] contains no value, <c>true</c> if [enable advertising]; otherwise, <c>false</c>.</value>
        public bool? EnableAdvertising { get; set; }

        /// <summary>
        /// Filter by explicit content flag.
        /// </summary>
        /// <value><c>null</c> if [is explicity] contains no value, <c>true</c> if [is explicity]; otherwise, <c>false</c>.</value>
        public bool? IsExplicit { get; set; }

        /// <summary>
        /// Filter by is_for_kids flag.
        /// </summary>
        /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
        public bool? IsForKids { get; set; }

        /// <summary>
        /// Filter videos created after this ISO 8601 date.
        /// </summary>
        /// <value>The created after.</value>
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Filter videos created before this ISO 8601 date.
        /// </summary>
        /// <value>The created before.</value>
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Filter by tags (comma-separated).
        /// </summary>
        /// <value>The tags.</value>
        public string? Tags { get; set; }
    }
}