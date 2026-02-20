using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailymotionSDK.Models;

/// <summary>
/// Class VideoMetadataJsonConverter.
/// Implements the <see cref="System.Text.Json.Serialization.JsonConverter{DailymotionSDK.Models.VideoMetadata}" /></summary>
/// <seealso cref="System.Text.Json.Serialization.JsonConverter{DailymotionSDK.Models.VideoMetadata}" />
public class VideoMetadataJsonConverter(VideoFields[]? requestedFields = null) : JsonConverter<VideoMetadata>
{
    /// <summary>
    /// The requested fields
    /// </summary>
    private readonly VideoFields[]? _requestedFields = requestedFields;

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, VideoMetadata value, JsonSerializerOptions options)
    {
        // Note: System.Text.Json handles null checks automatically before calling Write(), 
        // so you do not need the explicit `if (value == null)` check here.

        writer.WriteStartObject();

        foreach (var kvp in value.FieldData)
        {
            var apiFieldName = kvp.Key.GetApiFieldName();
            writer.WritePropertyName(apiFieldName);

            // Pass the options down to maintain any global settings (like naming policies)
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Reads and converts the JSON to type <typeparamref name="T" />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override VideoMetadata? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        // Parse the current object from the reader into a fast, read-only JsonDocument
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var result = new VideoMetadata();

        if (root.ValueKind != JsonValueKind.Object)
            return result;

        var fieldsToProcess = _requestedFields is { Length: > 0 }
            ? _requestedFields
            : Enum.GetValues<VideoFields>();

        foreach (var field in fieldsToProcess)
        {
            var apiFieldName = field.GetApiFieldName();

            // TryGetProperty replaces the old Dictionary.TryGetValue
            if (root.TryGetProperty(apiFieldName, out var jsonProperty))
            {
                // Unwrap the JsonElement into a standard C# primitive (long, string, etc.)
                var unwrappedValue = NormalizeJsonElement(jsonProperty);
                result.SetValue(field, unwrappedValue);
            }
        }

        return result;
    }

    // -------------------------------------------------------------------
    // Internal Normalization Helpers
    // -------------------------------------------------------------------

    /// <summary>
    /// Normalizes the json element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>System.Object.</returns>
    private static object? NormalizeJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => null,
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => NormalizeArray(element),
            JsonValueKind.Object => NormalizeJsonObject(element),
            _ => element.ToString()
        };
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

        var first = jarr[0];

        return first.ValueKind switch
        {
            JsonValueKind.String =>
                jarr.EnumerateArray().Select(x => x.GetString()!).ToArray(),
            JsonValueKind.Number when first.TryGetInt64(out _) =>
                jarr.EnumerateArray().Select(x => x.GetInt64()).ToArray(),
            JsonValueKind.Number =>
                jarr.EnumerateArray().Select(x => x.GetSingle()).ToArray(),
            _ => jarr.EnumerateArray().Select(x => x.ToString()).ToArray()
        };
    }

    /// <summary>
    /// Normalizes the json object.
    /// </summary>
    /// <param name="jobj">The jobj.</param>
    /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.Object&gt;.</returns>
    private static Dictionary<string, object?> NormalizeJsonObject(JsonElement jobj)
    {
        var dict = new Dictionary<string, object?>();

        foreach (var prop in jobj.EnumerateObject())
        {
            dict[prop.Name] = NormalizeJsonElement(prop.Value);
        }

        return dict;
    }
}