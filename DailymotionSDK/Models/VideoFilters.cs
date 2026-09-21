namespace DailymotionSDK.Models;

/// <summary>
/// Class VideoFilters.
/// </summary>
public class VideoFilters
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    /// <value>The page.</value>
    public int? Page { get; set; } = 1;
    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    /// <value>The size of the page.</value>
    public int? PageSize { get; set; } = 20;
    /// <summary>
    /// Gets or sets the visibility.
    /// </summary>
    /// <value>The visibility.</value>
    public string? Visibility { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [enable advertising].
    /// </summary>
    /// <value><c>null</c> if [enable advertising] contains no value, <c>true</c> if [enable advertising]; otherwise, <c>false</c>.</value>
    public bool? EnableAdvertising { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is explicit.
    /// </summary>
    /// <value><c>null</c> if [is explicit] contains no value, <c>true</c> if [is explicit]; otherwise, <c>false</c>.</value>
    public bool? IsExplicit { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is for kids.
    /// </summary>
    /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
    public bool? IsForKids { get; set; }
    /// <summary>
    /// Gets or sets the created after.
    /// </summary>
    /// <value>The created after.</value>
    public DateTime? CreatedAfter { get; set; }
    /// <summary>
    /// Gets or sets the created before.
    /// </summary>
    /// <value>The created before.</value>
    public DateTime? CreatedBefore { get; set; }
    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    /// <value>The tags.</value>
    public string? Tags { get; set; }
}