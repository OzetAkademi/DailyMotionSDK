using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class VideoCreationParameters.
/// </summary>
public class VideoCreationParameters
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    [JsonPropertyName("source")]
    public Source? Source { get; set; }

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
    public string? Visibility { get; set; } = "private";

    /// <summary>
    /// Gets or sets a value indicating whether this instance is for kids.
    /// </summary>
    /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
    [JsonPropertyName("is_for_kids")]
    public bool? IsForKids { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is ai altered.
    /// </summary>
    /// <value><c>null</c> if [is ai altered] contains no value, <c>true</c> if [is ai altered]; otherwise, <c>false</c>.</value>
    [JsonPropertyName("is_ai_altered")]
    public bool? IsAiAltered { get; set; } = false;
}
