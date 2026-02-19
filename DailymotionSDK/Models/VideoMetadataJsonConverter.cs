using Newtonsoft.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Custom JSON converter for VideoMetadata that handles field selection
/// </summary>
public class VideoMetadataJsonConverter : JsonConverter<VideoMetadata>
{
    private readonly VideoFields[]? _requestedFields;

    /// <summary>
    /// Initializes a new instance of the VideoMetadataJsonConverter
    /// </summary>
    /// <param name="requestedFields">The fields that were requested from the API</param>
    public VideoMetadataJsonConverter(VideoFields[]? requestedFields = null)
    {
        _requestedFields = requestedFields;
    }

    public override void WriteJson(JsonWriter writer, VideoMetadata? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();
        
        foreach (var kvp in value.FieldData)
        {
            var apiFieldName = kvp.Key.GetApiFieldName();
            writer.WritePropertyName(apiFieldName);
            serializer.Serialize(writer, kvp.Value);
        }
        
        writer.WriteEndObject();
    }

    public override VideoMetadata? ReadJson(JsonReader reader, Type objectType, VideoMetadata? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var jsonObject = serializer.Deserialize<Dictionary<string, object?>>(reader);
        if (jsonObject == null)
            return new VideoMetadata();

        var result = new VideoMetadata();

        // If specific fields were requested, only populate those
        if (_requestedFields != null && _requestedFields.Length > 0)
        {
            foreach (var field in _requestedFields)
            {
                var apiFieldName = field.GetApiFieldName();
                if (jsonObject.TryGetValue(apiFieldName, out var value))
                {
                    result.SetValue(field, value);
                }
            }
        }
        else
        {
            // Populate all available fields
            foreach (var field in Enum.GetValues<VideoFields>())
            {
                var apiFieldName = field.GetApiFieldName();
                if (jsonObject.TryGetValue(apiFieldName, out var value))
                {
                    result.SetValue(field, value);
                }
            }
        }

        return result;
    }
}
