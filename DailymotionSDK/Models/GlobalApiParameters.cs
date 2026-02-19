namespace DailymotionSDK.Models;

/// <summary>
/// Typed representation of Dailymotion global API parameters.
/// </summary>
public class GlobalApiParameters
{
    /// <summary>
    /// Gets or sets the ams country.
    /// </summary>
    /// <value>The ams country.</value>
    public string? AmsCountry { get; set; }
    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>The context.</value>
    public string? Context { get; set; }
    /// <summary>
    /// Gets or sets the device filter.
    /// "detect", "web", "mobile", "iptv"
    /// </summary>
    /// <value>The device filter.</value>
    public string? DeviceFilter { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [family filter].
    /// true|false
    /// </summary>
    /// <value><c>null</c> if [family filter] contains no value, <c>true</c> if [family filter]; otherwise, <c>false</c>.</value>
    public bool? FamilyFilter { get; set; }
    /// <summary>
    /// Gets or sets the localization.
    /// e.g. "en_US" or "detect"
    /// </summary>
    /// <value>The localization.</value>
    public string? Localization { get; set; }
    /// <summary>
    /// Gets or sets the thumbnail ratio.
    /// "original", "widescreen", "square"
    /// </summary>
    /// <value>The thumbnail ratio.</value>
    public string? ThumbnailRatio { get; set; }

    /// <summary>
    /// Converts to dictionary.
    /// </summary>
    /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(AmsCountry)) dict["ams_country"] = AmsCountry;
        if (!string.IsNullOrWhiteSpace(Context)) dict["context"] = Uri.EscapeDataString(Context);
        if (!string.IsNullOrWhiteSpace(DeviceFilter)) dict["device_filter"] = DeviceFilter;
        if (FamilyFilter.HasValue) dict["family_filter"] = FamilyFilter.Value.ToString().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(Localization)) dict["localization"] = Localization;
        if (!string.IsNullOrWhiteSpace(ThumbnailRatio)) dict["thumbnail_ratio"] = ThumbnailRatio;
        return dict;
    }

    /// <summary>
    /// Helper: build and set Context from a dictionary of key/value pairs.
    /// The method constructs an embedded query-string like "k1=v1&k2=v2".
    /// ToDictionary() will URL-encode the whole string when sending the request.
    /// </summary>
    /// <param name="contextValues">Key/value pairs to include in the context.</param>
    public void SetContextFromDictionary(IDictionary<string, string>? contextValues)
    {
        if (contextValues == null || contextValues.Count == 0)
        {
            Context = null;
            return;
        }

        // Build raw embedded query string (unencoded). ToDictionary() will encode it.
        Context = string.Join("&", contextValues.Select(kv => $"{kv.Key}={kv.Value}"));
    }

    /// <summary>
    /// Static helper: returns an already encoded context string from a dictionary.
    /// Use this when you need the encoded string directly (e.g. manual parameter assembly).
    /// </summary>
    /// <param name="contextValues">Key/value pairs.</param>
    /// <returns>URL-encoded context string.</returns>
    public static string BuildEncodedContext(IDictionary<string, string>? contextValues)
    {
        if (contextValues == null || contextValues.Count == 0) return string.Empty;
        var raw = string.Join("&", contextValues.Select(kv => $"{kv.Key}={kv.Value}"));
        return Uri.EscapeDataString(raw);
    }
}
