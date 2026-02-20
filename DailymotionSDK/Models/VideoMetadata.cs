using System.Text.Json;

namespace DailymotionSDK.Models;

/// <summary>
/// Unified video metadata that combines VideoFields enum with actual data
/// This eliminates duplication and ensures consistency between field definitions and data structure
/// https://developers.dailymotion.com/reference/video-fields
/// </summary>
public class VideoMetadata
{
    /// <summary>
    /// The field data
    /// </summary>
    private readonly Dictionary<VideoFields, object?> _fieldData = [];

    /// <summary>
    /// Gets a value for a specific video field
    /// </summary>
    /// <typeparam name="T">The expected type of the value</typeparam>
    /// <param name="field">The video field to retrieve</param>
    /// <returns>The value if present, otherwise default(T)</returns>
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
    /// Sets a value for a specific video field
    /// </summary>
    /// <param name="field">The video field to set</param>
    /// <param name="value">The value to set</param>
    public void SetValue(VideoFields field, object? value)
    {
        _fieldData[field] = value;
    }

    /// <summary>
    /// Checks if a specific field has a value
    /// </summary>
    /// <param name="field">The video field to check</param>
    /// <returns>True if the field has a value</returns>
    public bool HasValue(VideoFields field)
    {
        return _fieldData.ContainsKey(field) && _fieldData[field] != null;
    }

    /// <summary>
    /// Gets all available fields
    /// </summary>
    /// <returns>Collection of fields that have values</returns>
    public IEnumerable<VideoFields> GetAvailableFields()
    {
        return _fieldData.Keys.Where(field => _fieldData[field] != null);
    }

    /// <summary>
    /// Gets the raw field data dictionary
    /// </summary>
    /// <value>The field data.</value>
    public IReadOnlyDictionary<VideoFields, object?> FieldData => _fieldData.AsReadOnly();

    // Convenience properties for commonly used fields
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string? Id => GetValue<string>(VideoFields.Id);
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
    /// Gets the duration.
    /// </summary>
    /// <value>The duration.</value>
    public int? Duration => GetValue<int?>(VideoFields.Duration);
    /// <summary>
    /// Gets the thumbnail URL.
    /// </summary>
    /// <value>The thumbnail URL.</value>
    public string? ThumbnailUrl => GetValue<string>(VideoFields.ThumbnailUrl);
    /// <summary>
    /// Gets the URL.
    /// </summary>
    /// <value>The URL.</value>
    public string? Url => GetValue<string>(VideoFields.Url);
    /// <summary>
    /// Gets the created time.
    /// </summary>
    /// <value>The created time.</value>
    public long? CreatedTime => GetValue<long?>(VideoFields.CreatedTime);

    /// <summary>
    /// Gets the updated time.
    /// </summary>
    /// <value>The updated time.</value>
    public long? UpdatedTime => GetValue<long?>(VideoFields.UpdatedTime);
    /// <summary>
    /// Gets the time, in milliseconds since the Unix epoch, when the video was uploaded.
    /// </summary>
    /// <value>The uploaded time.</value>
    public long? UploadedTime => GetValue<long?>(VideoFields.UploadedTime);
    /// <summary>
    /// Gets the views total.
    /// </summary>
    /// <value>The views total.</value>
    public int? ViewsTotal => GetValue<int?>(VideoFields.ViewsTotal);
    /// <summary>
    /// Gets a value indicating whether this <see cref="VideoMetadata" /> is published.
    /// </summary>
    /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
    public bool? Published => GetValue<bool?>(VideoFields.Published);

    /// <summary>
    /// Gets the private identifier.
    /// </summary>
    /// <value>The private identifier.</value>
    public string? PrivateId => GetValue<string>(VideoFields.PrivateId);
    // Additional properties used by demo
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name => GetValue<string>(VideoFields.Name);
    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <value>The status.</value>
    public string? Status => GetValue<string>(VideoFields.Status);
    /// <summary>
    /// Gets a value indicating whether this instance is private.
    /// </summary>
    /// <value><c>true</c> if this instance is private; otherwise, <c>false</c>.</value>
    public bool? IsPrivate => GetValue<bool?>(VideoFields.Private);
    /// <summary>
    /// Gets the likes total.
    /// </summary>
    /// <value>The likes total.</value>
    public int? LikesTotal => GetValue<int?>(VideoFields.LikesTotal);
    /// <summary>
    /// Gets the width.
    /// </summary>
    /// <value>The width.</value>
    public int? Width => GetValue<int?>(VideoFields.Width);
    /// <summary>
    /// Gets the height.
    /// </summary>
    /// <value>The height.</value>
    public int? Height => GetValue<int?>(VideoFields.Height);
    /// <summary>
    /// Gets a value indicating whether [allow embed].
    /// </summary>
    /// <value><c>true</c> if [allow embed]; otherwise, <c>false</c>.</value>
    public bool? AllowEmbed => GetValue<bool?>(VideoFields.AllowEmbed);
    /// <summary>
    /// Gets the geoblocking.
    /// </summary>
    /// <value>The geoblocking.</value>
    public string? Geoblocking => GetValue<string>(VideoFields.Geoblocking);
    /// <summary>
    /// Gets the encoding progress.
    /// </summary>
    /// <value>The encoding progress.</value>
    public int? EncodingProgress => GetValue<int?>(VideoFields.EncodingProgress);
    /// <summary>
    /// Gets the live publish URL.
    /// </summary>
    /// <value>The live publish URL.</value>
    public string? LivePublishUrl => GetValue<string>(VideoFields.LivePublishUrl);
    /// <summary>
    /// Gets the live publish SRT URL.
    /// </summary>
    /// <value>The live publish SRT URL.</value>
    public string? LivePublishSrtUrl => GetValue<string>(VideoFields.LivePublishSrtUrl);

    /// <summary>
    /// Gets the custom advertising target value for this video.
    /// </summary>
    /// <value>A custom targeting string used for advertising systems (e.g., Liverail),
    /// or <c>null</c> if no custom target is defined.</value>
    public string? AdvertisingCustomTarget => GetValue<string>(VideoFields.AdvertisingCustomTarget);


    /// <summary>
    /// Gets a value indicating whether instream ads are blocked for this video.
    /// </summary>
    /// <value><c>true</c> if instream ads are blocked; otherwise <c>false</c>.
    /// Returns <c>null</c> if the value is not provided.</value>
    public bool? AdvertisingInstreamBlocked => GetValue<bool?>(VideoFields.AdvertisingInstreamBlocked);


    /// <summary>
    /// Gets a value indicating whether AI chapter generation is required.
    /// </summary>
    /// <value><c>true</c> if AI chapter generation is required; <c>false</c> if not;
    /// <c>null</c> if unspecified.</value>
    public bool? AiChapterGenerationRequired => GetValue<bool?>(VideoFields.AiChapterGenerationRequired);


    /// <summary>
    /// Gets the list of language codes requested for AI-generated subtitles.
    /// </summary>
    /// <value>An array of language codes (ISO standards) or <c>null</c>
    /// if no AI subtitle languages were requested.</value>
    public string[]? AiSubtitleLanguages => GetValue<string[]>(VideoFields.AiSubtitleLanguages);


    /// <summary>
    /// Gets a value indicating whether this video can be added to playlists.
    /// </summary>
    /// <value><c>true</c> if the video may be added to playlists; <c>false</c> otherwise.
    /// <c>null</c> if not supplied.</value>
    public bool? AllowedInPlaylists => GetValue<bool?>(VideoFields.AllowedInPlaylists);


    /// <summary>
    /// Gets the aspect ratio of this video.
    /// </summary>
    /// <value>A floating point aspect ratio value (e.g., 1.7777 for 16:9),
    /// or <c>null</c> if unknown.</value>
    public float? AspectRatio => GetValue<float?>(VideoFields.AspectRatio);

    /// <summary>
    /// Gets the current live audience count.
    /// </summary>
    /// <value>The current audience number, or <c>null</c> if not applicable.</value>
    public int? Audience => GetValue<int?>(VideoFields.Audience);

    /// <summary>
    /// Gets the total live audience since stream creation.
    /// </summary>
    /// <value>Total count of audience views for the stream, or <c>null</c> if not used.</value>
    public int? AudienceTotal => GetValue<int?>(VideoFields.AudienceTotal);

    /// <summary>
    /// Gets the audience meter URL for this video.
    /// </summary>
    /// <value>A URL string for audience metering, or <c>null</c> if not provided.</value>
    public string? AudienceUrl => GetValue<string>(VideoFields.AudienceUrl);

    /// <summary>
    /// Gets the list of available stream formats for this video.
    /// </summary>
    /// <value>Array of format identifiers, or <c>null</c> if not supplied.</value>
    public string[]? AvailableFormats => GetValue<string[]>(VideoFields.AvailableFormats);

    /// <summary>
    /// Gets the channel/category of this video.
    /// </summary>
    /// <value>The channel name or identifier, or <c>null</c> if undefined.</value>
    public string? Channel => GetValue<string>(VideoFields.Channel);

    /// <summary>
    /// Gets the checksum/hash of the video file.
    /// </summary>
    /// <value>A string hash value or <c>null</c> if not available.</value>
    public string? Checksum => GetValue<string>(VideoFields.Checksum);

    /// <summary>
    /// Gets the list of countries where this video is blocked by a claim rule.
    /// </summary>
    /// <value>Array of ISO 3166-1 alpha-2 country codes, or <c>null</c>.</value>
    public string[]? ClaimRuleBlockedCountries => GetValue<string[]>(VideoFields.ClaimRuleBlockedCountries);

    /// <summary>
    /// Gets the list of countries where this video is monetized by the claimer.
    /// </summary>
    /// <value>Array of ISO country codes, or <c>null</c>.</value>
    public string[]? ClaimRuleMonetizedCountries =>
        GetValue<string[]>(VideoFields.ClaimRuleMonetizedCountries);

    /// <summary>
    /// Gets the list of countries tracked by the claimer.
    /// </summary>
    /// <value>Array of ISO country codes, or <c>null</c>.</value>
    public string[]? ClaimRuleTrackedCountries =>
        GetValue<string[]>(VideoFields.ClaimRuleTrackedCountries);

    /// <summary>
    /// Gets the content provider name.
    /// </summary>
    /// <value>The provider name or <c>null</c>.</value>
    public string? ContentProvider => GetValue<string>(VideoFields.ContentProvider);

    /// <summary>
    /// Gets the content provider identifier.
    /// </summary>
    /// <value>The provider ID or <c>null</c>.</value>
    public string? ContentProviderId => GetValue<string>(VideoFields.ContentProviderId);

    /// <summary>
    /// Gets the declarative country of the video.
    /// </summary>
    /// <value>ISO 3166-1 alpha-2 country code, or <c>null</c>.</value>
    public string? Country => GetValue<string>(VideoFields.Country);

    /// <summary>
    /// Gets the custom classification values (max 3 values).
    /// </summary>
    /// <value>A list of custom classification strings, or <c>null</c>.</value>
    public string[]? CustomClassification => GetValue<string[]>(VideoFields.CustomClassification);

    /// <summary>
    /// Gets the HTML embed code for this video.
    /// </summary>
    /// <value>A string containing HTML embed markup, or <c>null</c>.</value>
    public string? EmbedHtml => GetValue<string>(VideoFields.EmbedHtml);

    /// <summary>
    /// Gets the embeddable URL for this video.
    /// </summary>
    /// <value>A URL string or <c>null</c>.</value>
    public string? EmbedUrl => GetValue<string>(VideoFields.EmbedUrl);

    /// <summary>
    /// Gets the live stream end time.
    /// </summary>
    /// <value><c>long</c> or <c>null</c>.</value>
    public long? EndTime => GetValue<long?>(VideoFields.EndTime);

    /// <summary>
    /// Gets the date after which this video will become private.
    /// </summary>
    /// <value><c>long</c> or <c>null</c>.</value>
    public long? ExpiryDate => GetValue<long?>(VideoFields.ExpiryDate);

    /// <summary>
    /// Gets whether expired videos remain accessible via private URL.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? ExpiryDateAvailability =>
        GetValue<bool?>(VideoFields.ExpiryDateAvailability);

    /// <summary>
    /// Gets whether videos are deleted when expiry is reached.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? ExpiryDateDeletion =>
        GetValue<bool?>(VideoFields.ExpiryDateDeletion);

    /// <summary>
    /// Gets whether this video is marked as explicit.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? Explicit => GetValue<bool?>(VideoFields.Explicit);

    /// <summary>
    /// Gets the URL of the 60px filmstrip sprite.
    /// </summary>
    /// <value>A URL string or <c>null</c>.</value>
    public string? Filmstrip60Url => GetValue<string>(VideoFields.Filmstrip60Url);

    /// <summary>
    /// Gets the list of hashtags attached to this video.
    /// </summary>
    /// <value>Array of hashtags, or <c>null</c>.</value>
    public string[]? Hashtags => GetValue<string[]>(VideoFields.Hashtags);

    /// <summary>
    /// Gets whether the video was declared as “Created for Kids”.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? IsCreatedForKids => GetValue<bool?>(VideoFields.IsCreatedForKids);

    /// <summary>
    /// Gets the declared language of the video.
    /// </summary>
    /// <value>ISO language code or <c>null</c>.</value>
    public string? Language => GetValue<string>(VideoFields.Language);

    /// <summary>
    /// Gets the date/time when the video was liked by the user.
    /// </summary>
    /// <value><c>long</c> or <c>null</c>.</value>
    public long? LikedAt => GetValue<long?>(VideoFields.LikedAt);

    /// <summary>
    /// Gets the live ingests list.
    /// </summary>
    /// <value>Array of ingest URLs, or <c>null</c>.</value>
    public string[]? LiveIngests => GetValue<string[]>(VideoFields.LiveIngests);

    /// <summary>
    /// Gets the external view logging URLs.
    /// </summary>
    /// <value>Array of URLs, or <c>null</c>.</value>
    public string[]? LogExternalViewUrls => GetValue<string[]>(VideoFields.LogExternalViewUrls);

    /// <summary>
    /// Gets the one-time view logging URL.
    /// </summary>
    /// <value>A URL or <c>null</c>.</value>
    public string? LogViewUrl => GetValue<string>(VideoFields.LogViewUrl);

    /// <summary>
    /// Gets the list of view logging URLs.
    /// </summary>
    /// <value>Array of URLs, or <c>null</c>.</value>
    public string[]? LogViewUrls => GetValue<string[]>(VideoFields.LogViewUrls);

    /// <summary>
    /// Gets the media type of this content.
    /// </summary>
    /// <value>A string such as "video", "live", etc., or <c>null</c>.</value>
    public string? MediaType => GetValue<string>(VideoFields.MediaType);

    /// <summary>
    /// Gets the stream mode for this video.
    /// </summary>
    /// <value>A string describing the stream mode, or <c>null</c>.</value>
    public string? Mode => GetValue<string>(VideoFields.Mode);

    /// <summary>
    /// Gets a value indicating whether the live stream is currently on-air.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? OnAir => GetValue<bool?>(VideoFields.OnAir);

    /// <summary>
    /// Gets the owner identifier of this video.
    /// </summary>
    /// <value>A string owner ID, or <c>null</c>.</value>
    public string? Owner => GetValue<string>(VideoFields.Owner);

    /// <summary>
    /// Gets whether the video belongs to a partner.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? Partner => GetValue<bool?>(VideoFields.Partner);

    /// <summary>
    /// Gets whether the video is password-protected.
    /// </summary>
    /// <value><c>true</c>, <c>false</c>, or <c>null</c>.</value>
    public bool? PasswordProtected => GetValue<bool?>(VideoFields.PasswordProtected);

    /// <summary>
    /// Gets the next video selected by the owner.
    /// </summary>
    /// <value>A video ID string or <c>null</c>.</value>
    public string? PlayerNextVideo => GetValue<string>(VideoFields.PlayerNextVideo);

    /// <summary>
    /// Gets the list of next videos selected by the owner.
    /// </summary>
    /// <value>Array of video IDs, or <c>null</c>.</value>
    public string[]? PlayerNextVideos => GetValue<string[]>(VideoFields.PlayerNextVideos);

    /// <summary>
    /// Gets the 240p preview video URL.
    /// </summary>
    /// <value>The preview240p URL.</value>
    public string? Preview240pUrl => GetValue<string>(VideoFields.Preview240pUrl);

    /// <summary>
    /// Gets the 360p preview video URL.
    /// </summary>
    /// <value>The preview360p URL.</value>
    public string? Preview360pUrl => GetValue<string>(VideoFields.Preview360pUrl);

    /// <summary>
    /// Gets the 480p preview video URL.
    /// </summary>
    /// <value>The preview480p URL.</value>
    public string? Preview480pUrl => GetValue<string>(VideoFields.Preview480pUrl);

    /// <summary>
    /// Gets the publication date of this video.
    /// </summary>
    /// <value>The publish date.</value>
    public long? PublishDate => GetValue<long?>(VideoFields.PublishDate);

    /// <summary>
    /// When this video status field is set to processing, this parameter indicates a number between 0 and 100 corresponding to
    /// the percentage of progress from the status waiting to ready.
    /// Unlike encoding_progress that can reach 100 well before the switch from processing to ready, this value will not.
    /// Min length: 1 / Max length: 150
    /// </summary>
    /// <value>The publishing progress.</value>
    public long? PublishingProgress => GetValue<long?>(VideoFields.PublishingProgress);

    /// <summary>
    /// Gets whether the video remains private after its publish date.
    /// </summary>
    /// <value><c>null</c> if [publish date keep private] contains no value, <c>true</c> if [publish date keep private]; otherwise, <c>false</c>.</value>
    public bool? PublishDateKeepPrivate =>
        GetValue<bool?>(VideoFields.PublishDateKeepPrivate);

    /// <summary>
    /// Gets the video recording start time.
    /// </summary>
    /// <value>The record start time.</value>
    public long? RecordStartTime => GetValue<long?>(VideoFields.RecordStartTime);

    /// <summary>
    /// Gets the video recording end time.
    /// </summary>
    /// <value>The record end time.</value>
    public long? RecordEndTime => GetValue<long?>(VideoFields.RecordEndTime);

    /// <summary>
    /// Gets the recording status.
    /// </summary>
    /// <value>The record status.</value>
    public string? RecordStatus => GetValue<string>(VideoFields.RecordStatus);

    /// <summary>
    /// Gets the recurrence pattern of the video.
    /// </summary>
    /// <value>The recurrence.</value>
    public string? Recurrence => GetValue<string>(VideoFields.Recurrence);

    /// <summary>
    /// Gets the seeker image resource URL.
    /// </summary>
    /// <value>The seeker URL.</value>
    public string? SeekerUrl => GetValue<string>(VideoFields.SeekerUrl);

    /// <summary>
    /// Gets the soundtrack ISRC code.
    /// </summary>
    /// <value>The soundtrack isrc.</value>
    public string? SoundtrackIsrc => GetValue<string>(VideoFields.SoundtrackIsrc);

    /// <summary>
    /// Gets the soundtrack popularity score.
    /// </summary>
    /// <value>The soundtrack popularity.</value>
    public int? SoundtrackPopularity => GetValue<int?>(VideoFields.SoundtrackPopularity);

    /// <summary>
    /// Gets the 320px sprite sheet URL.
    /// </summary>
    /// <value>The sprite320x URL.</value>
    public string? Sprite320xUrl => GetValue<string>(VideoFields.Sprite320xUrl);

    /// <summary>
    /// Gets the sprite sheet URL.
    /// </summary>
    /// <value>The sprite URL.</value>
    public string? SpriteUrl => GetValue<string>(VideoFields.SpriteUrl);

    /// <summary>
    /// Gets the studio indicator.
    /// </summary>
    /// <value><c>null</c> if [studio] contains no value, <c>true</c> if [studio]; otherwise, <c>false</c>.</value>
    public bool? Studio => GetValue<bool?>(VideoFields.Studio);

    /// <summary>
    /// Gets the tags attached to the video.
    /// </summary>
    /// <value>The tags.</value>
    public string[]? Tags => GetValue<string[]>(VideoFields.Tags);

    /// <summary>
    /// Gets the tiny URL of the video.
    /// </summary>
    /// <value>The tiny URL.</value>
    public string? TinyUrl => GetValue<string>(VideoFields.TinyUrl);

    // =======================================================
    // FIRST FRAME URLS
    // =======================================================

    /// <summary>
    /// Gets the URL of this video's first frame (60px height).
    /// </summary>
    /// <value>The first frame60 URL.</value>
    public string? FirstFrame60Url => GetValue<string>(VideoFields.FirstFrame60Url);

    /// <summary>
    /// Gets the URL of this video's first frame (120px height).
    /// </summary>
    /// <value>The first frame120 URL.</value>
    public string? FirstFrame120Url => GetValue<string>(VideoFields.FirstFrame120Url);

    /// <summary>
    /// Gets the URL of this video's first frame (180px height).
    /// </summary>
    /// <value>The first frame180 URL.</value>
    public string? FirstFrame180Url => GetValue<string>(VideoFields.FirstFrame180Url);

    /// <summary>
    /// Gets the URL of this video's first frame (240px height).
    /// </summary>
    /// <value>The first frame240 URL.</value>
    public string? FirstFrame240Url => GetValue<string>(VideoFields.FirstFrame240Url);

    /// <summary>
    /// Gets the URL of this video's first frame (360px height).
    /// </summary>
    /// <value>The first frame360 URL.</value>
    public string? FirstFrame360Url => GetValue<string>(VideoFields.FirstFrame360Url);

    /// <summary>
    /// Gets the URL of this video's first frame (480px height).
    /// </summary>
    /// <value>The first frame480 URL.</value>
    public string? FirstFrame480Url => GetValue<string>(VideoFields.FirstFrame480Url);

    /// <summary>
    /// Gets the URL of this video's first frame (720px height).
    /// </summary>
    /// <value>The first frame720 URL.</value>
    public string? FirstFrame720Url => GetValue<string>(VideoFields.FirstFrame720Url);

    /// <summary>
    /// Gets the URL of this video's first frame (1080px height).
    /// </summary>
    /// <value>The first frame1080 URL.</value>
    public string? FirstFrame1080Url => GetValue<string>(VideoFields.FirstFrame1080Url);

    // =======================================================
    // GEOLOCATION
    // =======================================================

    /// <summary>
    /// Gets the geolocation of this video.
    /// Returns an array of two values: [longitude, latitude].
    /// </summary>
    /// <value>The geoloc.</value>
    public float[]? Geoloc => GetValue<float[]>(VideoFields.Geoloc);


    // =======================================================
    // GENERAL FIELDS
    // =======================================================

    /// <summary>
    /// Gets the graph item type of this object (typically "video").
    /// </summary>
    /// <value>The type of the item.</value>
    public string? ItemType => GetValue<string>(VideoFields.ItemType);

    /// <summary>
    /// Gets the date and time when this live stream started airing.
    /// </summary>
    /// <value>The start time.</value>
    public long? StartTime => GetValue<long?>(VideoFields.StartTime);

    /// <summary>
    /// Gets a value indicating whether this video has been verified.
    /// </summary>
    /// <value>The verified.</value>
    public bool? Verified => GetValue<bool?>(VideoFields.Verified);

    // =======================================================
    // PASSWORD
    // =======================================================

    /// <summary>
    /// Gets the password protecting the video (deprecated; may return null).
    /// </summary>
    /// <value>The password.</value>
    public string? Password => GetValue<string>(VideoFields.Password);

    // =======================================================
    // LIVE STREAM — AD BREAKS
    // =======================================================

    /// <summary>
    /// Gets the end time of an ad break for this live stream.
    /// </summary>
    /// <value>The live ad break end time.</value>
    public long? LiveAdBreakEndTime => GetValue<long?>(VideoFields.LiveAdBreakEndTime);

    /// <summary>
    /// Gets a value indicating whether an ad break launch is requested.
    /// </summary>
    /// <value>The live ad break launch.</value>
    public bool? LiveAdBreakLaunch => GetValue<bool?>(VideoFields.LiveAdBreakLaunch);

    /// <summary>
    /// Gets the number of seconds remaining in the current ad break.
    /// </summary>
    /// <value>The live ad break remaining.</value>
    public int? LiveAdBreakRemaining => GetValue<int?>(VideoFields.LiveAdBreakRemaining);

    // =======================================================
    // LIVE STREAM — CORE METRICS
    // =======================================================

    /// <summary>
    /// Gets the date and time when this live stream started airing.
    /// </summary>
    /// <value>The live airing time.</value>
    public long? LiveAiringTime => GetValue<long?>(VideoFields.LiveAiringTime);

    /// <summary>
    /// Gets the audio bitrate of the live stream.
    /// </summary>
    /// <value>The live audio bitrate.</value>
    public int? LiveAudioBitrate => GetValue<int?>(VideoFields.LiveAudioBitrate);

    /// <summary>
    /// Gets a value indicating whether this live stream is automatically recorded.
    /// </summary>
    /// <value>The live automatic record.</value>
    public bool? LiveAutoRecord => GetValue<bool?>(VideoFields.LiveAutoRecord);

    /// <summary>
    /// Gets the backup video ID for this live stream.
    /// </summary>
    /// <value>The live backup video.</value>
    public string? LiveBackupVideo => GetValue<string>(VideoFields.LiveBackupVideo);

    // =======================================================
    // STREAM FLAGS
    // =======================================================

    /// <summary>
    /// Gets a value indicating whether this video stream was altered using AI.
    /// </summary>
    /// <value>The stream altered with ai.</value>
    public bool? StreamAlteredWithAi => GetValue<bool?>(VideoFields.StreamAlteredWithAi);

    // =======================================================
    // STREAM AUDIO
    // =======================================================

    /// <summary>
    /// Gets the URL of the audio stream for this video.
    /// </summary>
    /// <value>The stream audio URL.</value>
    public string? StreamAudioUrl => GetValue<string>(VideoFields.StreamAudioUrl);

    // =======================================================
    // STREAM — H.264 FORMATS
    // =======================================================

    /// <summary>
    /// Gets the URL of the 1080p H.264 stream.
    /// </summary>
    /// <value>The stream H264 HD1080 URL.</value>
    public string? StreamH264Hd1080Url => GetValue<string>(VideoFields.StreamH264Hd1080Url);

    /// <summary>
    /// Gets the URL of the 720p H.264 HD stream.
    /// </summary>
    /// <value>The stream H264 hd URL.</value>
    public string? StreamH264HdUrl => GetValue<string>(VideoFields.StreamH264HdUrl);

    /// <summary>
    /// Gets the URL of the 480p H.264 HQ stream.
    /// </summary>
    /// <value>The stream H264 hq URL.</value>
    public string? StreamH264HqUrl => GetValue<string>(VideoFields.StreamH264HqUrl);

    /// <summary>
    /// Gets the URL of the 144p/240p H.264 low-quality stream.
    /// </summary>
    /// <value>The stream H264 l1 URL.</value>
    public string? StreamH264L1Url => GetValue<string>(VideoFields.StreamH264L1Url);

    /// <summary>
    /// Gets the URL of the 360p H.264 stream.
    /// </summary>
    /// <value>The stream H264 l2 URL.</value>
    public string? StreamH264L2Url => GetValue<string>(VideoFields.StreamH264L2Url);

    /// <summary>
    /// Gets the URL of the low definition H.264 stream.
    /// </summary>
    /// <value>The stream H264 ld URL.</value>
    public string? StreamH264LdUrl => GetValue<string>(VideoFields.StreamH264LdUrl);

    /// <summary>
    /// Gets the URL of the 1440p H.264 stream.
    /// </summary>
    /// <value>The stream H264 QHD URL.</value>
    public string? StreamH264QhdUrl => GetValue<string>(VideoFields.StreamH264QhdUrl);

    /// <summary>
    /// Gets the URL of the 2160p H.264 stream.
    /// </summary>
    /// <value>The stream H264 uhd URL.</value>
    public string? StreamH264UhdUrl => GetValue<string>(VideoFields.StreamH264UhdUrl);

    /// <summary>
    /// Gets the URL of the generic H.264 stream.
    /// </summary>
    /// <value>The stream H264 URL.</value>
    public string? StreamH264Url => GetValue<string>(VideoFields.StreamH264Url);

    // =======================================================
    // STREAM — HLS + LIVE STREAMS
    // =======================================================

    /// <summary>
    /// Gets the URL of the HLS stream for this video.
    /// </summary>
    /// <value>The stream HLS URL.</value>
    public string? StreamHlsUrl => GetValue<string>(VideoFields.StreamHlsUrl);

    /// <summary>
    /// Gets the URL of the live HLS stream for this video.
    /// </summary>
    /// <value>The stream live HLS URL.</value>
    public string? StreamLiveHlsUrl => GetValue<string>(VideoFields.StreamLiveHlsUrl);

    /// <summary>
    /// Gets the URL of the live RTMP stream for this video.
    /// </summary>
    /// <value>The stream live RTMP URL.</value>
    public string? StreamLiveRtmpUrl => GetValue<string>(VideoFields.StreamLiveRtmpUrl);

    /// <summary>
    /// Gets the URL of the live SmoothStreaming stream.
    /// </summary>
    /// <value>The stream live smooth URL.</value>
    public string? StreamLiveSmoothUrl => GetValue<string>(VideoFields.StreamLiveSmoothUrl);

    // =======================================================
    // STREAM — SOURCE
    // =======================================================

    /// <summary>
    /// Gets the URL of the source stream for this video.
    /// </summary>
    /// <value>The stream source URL.</value>
    public string? StreamSourceUrl => GetValue<string>(VideoFields.StreamSourceUrl);

    // =======================================================
    // VIEW STATS
    // =======================================================

    /// <summary>
    /// Gets the number of views this video received in the last day.
    /// </summary>
    /// <value>The views last day.</value>
    public int? ViewsLastDay => GetValue<int?>(VideoFields.ViewsLastDay);

    /// <summary>
    /// Gets the number of views this video received in the last hour.
    /// </summary>
    /// <value>The views last hour.</value>
    public int? ViewsLastHour => GetValue<int?>(VideoFields.ViewsLastHour);

    /// <summary>
    /// Gets the number of views this video received in the last month.
    /// </summary>
    /// <value>The views last month.</value>
    public int? ViewsLastMonth => GetValue<int?>(VideoFields.ViewsLastMonth);

    /// <summary>
    /// Gets the number of views this video received in the last week.
    /// </summary>
    /// <value>The views last week.</value>
    public int? ViewsLastWeek => GetValue<int?>(VideoFields.ViewsLastWeek);

    /// <summary>
    /// Normalizes the json element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>System.Nullable&lt;System.Object&gt;.</returns>
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
    /// <returns>Dictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;.</returns>
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