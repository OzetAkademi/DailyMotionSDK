using System.Text.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Class VideoMetadata.
/// </summary>
public class VideoMetadata
{
    /// <summary>
    /// The field data
    /// </summary>
    private readonly Dictionary<VideoFields, object?> _fieldData = [];

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field">The field.</param>
    /// <returns>System.Nullable{T}.</returns>
    public T? GetValue<T>(VideoFields field)
    {
        if (_fieldData.TryGetValue(field, out var value) && value != null)
        {
            try
            {
                // Handle nullable types
                var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                return (T)Convert.ChangeType(value, targetType);
            }
            catch
            {
                return default;
            }
        }

        return default;
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="value">The value.</param>
    public void SetValue(VideoFields field, object? value)
    {
        _fieldData[field] = value;
    }

    /// <summary>
    /// Determines whether the specified field has value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns><c>true</c> if the specified field has value; otherwise, <c>false</c>.</returns>
    public bool HasValue(VideoFields field)
    {
        return _fieldData.ContainsKey(field) && _fieldData[field] != null;
    }

    /// <summary>
    /// Gets the available fields.
    /// </summary>
    /// <returns>IEnumerable{VideoFields}.</returns>
    public IEnumerable<VideoFields> GetAvailableFields()
    {
        return _fieldData.Keys.Where(field => _fieldData[field] != null);
    }

    /// <summary>
    /// Gets the field data.
    /// </summary>
    /// <value>The field data.</value>
    public IReadOnlyDictionary<VideoFields, object?> FieldData => _fieldData.AsReadOnly();

    // Convenience properties for commonly used fields
    /// <summary>
    /// Gets the video identifier.
    /// </summary>
    /// <value>The video identifier.</value>
    public string? VideoId => GetValue<string>(VideoFields.VideoId);
    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>The title.</value>
    public string? Title => GetValue<string>(VideoFields.Title);
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description => GetValue<string>(VideoFields.Description);
    /// <summary>
    /// Gets the visibility.
    /// </summary>
    /// <value>The visibility.</value>
    public string? Visibility => GetValue<string>(VideoFields.Visibility);

    /// <summary>
    /// Gets the available formats.
    /// </summary>
    /// <value>The available formats.</value>
    public string[]? AvailableFormats => GetValue<string[]>(VideoFields.AvailableFormats);

    /// <summary>
    /// Gets the country.
    /// </summary>
    /// <value>The country.</value>
    public string? Country => GetValue<string>(VideoFields.Country);

    /// <summary>
    /// Gets the hashtags.
    /// </summary>
    /// <value>The hashtags.</value>
    public string[]? Hashtags => GetValue<string[]>(VideoFields.Hashtags);

    /// <summary>
    /// Gets a value indicating whether this instance is for kids.
    /// </summary>
    /// <value><c>null</c> if [is for kids] contains no value, <c>true</c> if [is for kids]; otherwise, <c>false</c>.</value>
    public bool? IsForKids => GetValue<bool?>(VideoFields.IsForKids);

    /// <summary>
    /// Gets the language.
    /// </summary>
    /// <value>The language.</value>
    public string? Language => GetValue<string>(VideoFields.Language);

    /// <summary>
    /// Gets the tags.
    /// </summary>
    /// <value>The tags.</value>
    public string[]? Tags => GetValue<string[]>(VideoFields.Tags);

    /// <summary>
    /// Gets the first frame240 URL.
    /// </summary>
    /// <value>The first frame240 URL.</value>
    public string? FirstFrame240Url => GetValue<string>(VideoFields.FirstFrame240Url);

    /// <summary>
    /// Gets the first frame480 URL.
    /// </summary>
    /// <value>The first frame480 URL.</value>
    public string? FirstFrame480Url => GetValue<string>(VideoFields.FirstFrame480Url);

    /// <summary>
    /// Gets the first frame720 URL.
    /// </summary>
    /// <value>The first frame720 URL.</value>
    public string? FirstFrame720Url => GetValue<string>(VideoFields.FirstFrame720Url);

    /// <summary>
    /// Gets the first frame1080 URL.
    /// </summary>
    /// <value>The first frame1080 URL.</value>
    public string? FirstFrame1080Url => GetValue<string>(VideoFields.FirstFrame1080Url);

    /// <summary>
    /// Normalizes the json element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>System.Nullable{System.Object}.</returns>
    private static object? NormalizeJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => null,
            JsonValueKind.String => element.GetString(),
            // System.Text.Json doesn't separate int/float at the ValueKind level, so we try int64 first
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => NormalizeArray(element),
            JsonValueKind.Object => NormalizeJsonObject(element),
            _ => element.ToString()
        };
    }

    /// <summary>
    /// Normalizes the json dictionary.
    /// </summary>
    /// <param name="raw">The raw.</param>
    /// <returns>Dictionary{System.String, System.Nullable{System.Object}}.</returns>
    private static Dictionary<string, object?> NormalizeJsonDictionary(Dictionary<string, object?> raw)
    {
        var result = new Dictionary<string, object?>();

        foreach (var kvp in raw)
        {
            // Check if the object is actually a boxed JsonElement
            if (kvp.Value is JsonElement element)
            {
                result[kvp.Key] = NormalizeJsonElement(element);
            }
            else
            {
                // If it's null or already a primitive type, just pass it through
                result[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a VideoMetadata from JSON response
    /// </summary>
    /// <param name="jsonResponse">The JSON response from the API</param>
    /// <param name="requestedFields">The fields that were requested (optional)</param>
    /// <returns>A populated VideoMetadata instance</returns>
    public static VideoMetadata FromJson(string jsonResponse, VideoFields[]? requestedFields = null)
    {
        var result = new VideoMetadata();

        try
        {
            // Parse the JSON string into a lightweight document
            using var doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            // Ensure the root is a JSON object (equivalent to Dictionary)
            if (root.ValueKind != JsonValueKind.Object)
                return result;

            // Normalize the entire JSON object into a Dictionary<string, object?>
            var jsonData = NormalizeJsonObject(root);

            IEnumerable<VideoFields> fieldsToProcess =
                requestedFields is { Length: > 0 }
                    ? requestedFields
                    : Enum.GetValues<VideoFields>();

            foreach (var field in fieldsToProcess)
            {
                var apiFieldName = field.GetApiFieldName();

                if (jsonData.TryGetValue(apiFieldName, out var normalized))
                    result.SetValue(field, normalized);
            }
        }
        catch
        {
            // fail silently & return empty metadata
        }

        return result;
    }

    /// <summary>
    /// Normalizes the array.
    /// </summary>
    /// <param name="jarr">The jarr.</param>
    /// <returns>System.Object.</returns>
    private static object NormalizeArray(JsonElement jarr)
    {
        if (jarr.GetArrayLength() == 0)
            return Array.Empty<string>();

        // Determine element type by first element
        var first = jarr[0];

        return first.ValueKind switch
        {
            JsonValueKind.String =>
                jarr.EnumerateArray().Select(x => x.GetString()!).ToArray(),

            // If it's a number, check if it fits in a long, otherwise fallback to float
            JsonValueKind.Number when first.TryGetInt64(out _) =>
                jarr.EnumerateArray().Select(x => x.GetInt64()).ToArray(),

            JsonValueKind.Number =>
                jarr.EnumerateArray().Select(x => x.GetSingle()).ToArray(),

            // fallback to string[]
            _ => jarr.EnumerateArray().Select(x => x.ToString()).ToArray()
        };
    }

    /// <summary>
    /// Normalizes the j object.
    /// </summary>
    /// <param name="jobj">The jobj.</param>
    /// <returns>object.</returns>
    private static Dictionary<string, object?> NormalizeJsonObject(JsonElement jobj)
    {
        var dict = new Dictionary<string, object?>();

        foreach (var prop in jobj.EnumerateObject())
        {
            dict[prop.Name] = NormalizeJsonElement(prop.Value);
        }

        return dict;
    }

    /// <summary>
    /// Converts to a dictionary with API field names as keys
    /// </summary>
    /// <returns>Dictionary with API field names and values</returns>
    public Dictionary<string, object?> ToApiDictionary()
    {
        return _fieldData.ToDictionary(
            kvp => kvp.Key.GetApiFieldName(),
            kvp => kvp.Value
        );
    }

    /// <summary>
    /// Gets a subset of fields as a new VideoMetadata instance
    /// </summary>
    /// <param name="fields">The fields to include</param>
    /// <returns>A new instance with only the specified fields</returns>
    public VideoMetadata GetSubset(VideoFields[] fields)
    {
        var result = new VideoMetadata();
        foreach (var field in fields)
        {
            if (HasValue(field))
            {
                result.SetValue(field, GetValue<object>(field));
            }
        }
        return result;
    }

    /// <summary>
    /// Gets a value for a specific video field using the VideoFields enum
    /// This method provides backward compatibility and type safety
    /// </summary>
    /// <typeparam name="T">The expected type of the value</typeparam>
    /// <param name="field">The video field to retrieve</param>
    /// <returns>The value if present, otherwise default(T)</returns>
    public T? GetFieldValue<T>(VideoFields field)
    {
        return GetValue<T>(field);
    }

    /// <summary>
    /// Checks if a specific field has a value
    /// </summary>
    /// <param name="field">The video field to check</param>
    /// <returns>True if the field has a value</returns>
    public bool HasFieldValue(VideoFields field)
    {
        return HasValue(field);
    }
}