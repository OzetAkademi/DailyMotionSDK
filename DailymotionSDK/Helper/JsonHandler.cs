using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DailymotionSDK.Helper;

/// <summary>
/// Class JsonHandler.
/// </summary>
public static class JsonHandler
{
    /// <summary>
    /// The json serializer options
    /// </summary>
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        /*To escape unicode char encoding. Serhat Deligöz 03.12.2025 */
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true, // Optional: for pretty printing
    };

    /// <summary>
    /// The case insensitive json serializer options
    /// </summary>
    public static readonly JsonSerializerOptions CaseInsensitiveJsonSerializerOptions = new(JsonSerializerOptions)
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Serializes the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>System.String.</returns>
    public static string Serialize(object? data)
    {
        try
        {
            return JsonSerializer.Serialize(data, JsonSerializerOptions);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Serializes to byte.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] SerializeToByte(object? data)
    {
        return Encoding.UTF8.GetBytes(Serialize(data));
    }

    /// <summary>
    /// Deserializes the specified data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">The data.</param>
    /// <returns>System.Nullable{T}.</returns>
    public static T? Deserialize<T>(string? data)
    {
        if (data is null)
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions) ?? default!;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Deserializes the specified data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">The data.</param>
    /// <param name="options">The options.</param>
    /// <returns>System.Nullable{T}.</returns>
    public static T? Deserialize<T>(string? data, JsonSerializerOptions options)
    {
        if (data is null)
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(data, options) ?? default!;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Deserializes the specified data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">The data.</param>
    /// <returns>System.Nullable{T}.</returns>
    public static T? Deserialize<T>(byte[]? data)
    {
        if (data is null)
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data), JsonSerializerOptions) ?? default!;
        }
        catch
        {
            return default;
        }
    }
}