using System.ComponentModel.DataAnnotations;

namespace DailymotionSDK.Models;

/// <summary>
/// Parameters for video creation using the Dailymotion API
/// All properties are nullable to allow selective parameter inclusion
/// Based on the official Dailymotion API documentation for video creation
/// </summary>
public class VideoCreationParameters
{
    // Required parameters
    /// <summary>
    /// URL pointing to the video source file. You don't need to host the file, you can use the GET /file/upload API resource to create a temporary URL to a file of your own.
    /// </summary>
    [Required]
    public string? Url { get; set; }

    /// <summary>
    /// Title of the video.
    /// </summary>
    [Required]
    public string? Title { get; set; }

    // Basic video information
    /// <summary>
    /// Comprehensive description of this video. Maximum length is set to 3000 (5000 for partners).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Channel of this video. You can retrieve sub-fields of this channel object using the dot-notation (e.g.: channel.id).
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    /// List of tags attached to this video.
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// True if this video is private.
    /// </summary>
    public bool? Private { get; set; }

    /// <summary>
    /// True if this video is published (may still be waiting for encoding, see the status field for more information).
    /// </summary>
    public bool? Published { get; set; }

    /// <summary>
    /// True if this video is "Created for Kids" (intends to target an audience under the age of 16).
    /// </summary>
    public bool? IsCreatedForKids { get; set; }

    // Video mode and streaming
    /// <summary>
    /// Stream mode.
    /// </summary>
    public string? Mode { get; set; }

    // Language and location
    /// <summary>
    /// Language of this video. This value is declarative and corresponds to the user-declared spoken language of the video. Allowed values are ISO-639-3 alpha-2 and alpha-3 language codes.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Country of this video (declarative, may be null). Allowed values are ISO 3166-1 alpha-2 country codes.
    /// </summary>
    public string? Country { get; set; }

    // Advertising and monetization
    /// <summary>
    /// Returns the custom target value for given video. This value is sent to Liverail as an LR_DM_ADPARAM param. This can be used for targeting in liverail.
    /// </summary>
    public string? AdvertisingCustomTarget { get; set; }

    /// <summary>
    /// True if the owner blocked instream ads on this video.
    /// </summary>
    public bool? AdvertisingInstreamBlocked { get; set; }

    // AI and automation
    /// <summary>
    /// Whether AI chapter generation is required for the video or not.
    /// </summary>
    public bool? AiChapterGenerationRequired { get; set; }

    /// <summary>
    /// Whether the video stream has been altered by AI.
    /// </summary>
    public bool? StreamAlteredWithAi { get; set; }

    // Embedding and sharing
    /// <summary>
    /// True if this video can be embedded outside of Dailymotion.
    /// </summary>
    public bool? AllowEmbed { get; set; }

    /// <summary>
    /// True if this video can be added to playlists.
    /// </summary>
    public bool? AllowedInPlaylists { get; set; }

    // Content provider
    /// <summary>
    /// Content provider identifier.
    /// </summary>
    public string? ContentProviderId { get; set; }

    // Custom classification
    /// <summary>
    /// List of customizable values (maximum of 3 values).
    /// </summary>
    public string[]? CustomClassification { get; set; }

    // Scheduling and availability
    /// <summary>
    /// Date and time after which this video will be made publicly available. Beware: if the video was originally defined as private, setting this value will automatically make it public after the publish_date.
    /// </summary>
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// Date and time after which this video will be made private. Beware: if the video was originally defined as private, setting this value will automatically make it public between its publish_date and expiry_date.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// By default, videos reaching their expiry_date are still available to anyone who knows how to access their private URL. Set this to false to disable this behavior.
    /// </summary>
    public bool? ExpiryDateAvailability { get; set; }

    /// <summary>
    /// By default, videos are deleted (after a grace period) when their expiry_date is reached. Set this to false to disable this behavior.
    /// </summary>
    public bool? ExpiryDateDeletion { get; set; }

    /// <summary>
    /// Keep this video private when its publication_date is reached.
    /// </summary>
    public bool? PublishDateKeepPrivate { get; set; }

    // Content flags
    /// <summary>
    /// True if this video is explicit. Warning: It's not possible to remove this flag once set.
    /// </summary>
    public bool? Explicit { get; set; }

    // Geoblocking
    /// <summary>
    /// List of countries where this video is or isn't accessible. A list of country codes (ISO 3166-1 alpha-2) starting with the deny or allow (default) keyword to define if this is a block or an allowlist.
    /// </summary>
    public string[]? Geoblocking { get; set; }

    // Geolocation
    /// <summary>
    /// Geolocalization for this video. Result is an array with the longitude and latitude using point notation. Longitude range is from -180.0 (West) to 180.0 (East). Latitude range is from -90.0 (South) to 90.0 (North).
    /// </summary>
    public double[]? Geoloc { get; set; }

    // Hashtags
    /// <summary>
    /// List of hashtags attached to this video.
    /// </summary>
    public string[]? Hashtags { get; set; }

    // Live streaming
    /// <summary>
    /// End date and time of this live stream.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Launches a given number of ad breaks for this live stream.
    /// </summary>
    public int? LiveAdBreakLaunch { get; set; }

    /// <summary>
    /// True if this live stream is automatically recorded.
    /// </summary>
    public bool? LiveAutoRecord { get; set; }

    /// <summary>
    /// The live backup video, it is get/set as an xid, but is stored as an integer ID in DB.
    /// </summary>
    public string? LiveBackupVideo { get; set; }

    /// <summary>
    /// Start date and time of this live stream.
    /// </summary>
    public DateTime? StartTime { get; set; }

    // Password protection
    /// <summary>
    /// If a video is protected by a password, this field contains the password (deprecated, as it now only returns NULL). When setting a value on this field, the video visibility changes to "password protected". Setting it to NULL removes the password protection: the visibility is changed to "public".
    /// </summary>
    public string? Password { get; set; }

    // Player settings
    /// <summary>
    /// A unique video picked by the owner, displayed when video's playback ends. You can retrieve sub-fields of this video object using the dot-notation (e.g.: player_next_video.id).
    /// </summary>
    public string? PlayerNextVideo { get; set; }

    // Recording status
    /// <summary>
    /// Current state of the recording process of this video.
    /// starting: Recording video is going to start
    /// started: Recording video is in progress
    /// stopping: Recording video is going to stop
    /// stopped: Recording video is stopped
    /// </summary>
    public string? RecordStatus { get; set; }

    // Soundtrack
    /// <summary>
    /// The International Standard Recording Code of the soundtrack associated to this video.
    /// </summary>
    public string? SoundtrackIsrc { get; set; }

    /// <summary>
    /// Soundtrack popularity.
    /// </summary>
    public int? SoundtrackPopularity { get; set; }

    // Thumbnail
    /// <summary>
    /// URL of this video's raw thumbnail (full size respecting ratio). Some users have the permission to change this value by providing an URL to a custom thumbnail. If you don't have the permission, the thumbnail won't be updated. Note: for live streams, the thumbnail is automatically generated every 5 mn by default; it is not possible anymore to manually refresh the preview. Maximum allowed file size is 10MB.
    /// </summary>
    public string? ThumbnailUrl { get; set; }
    /// <summary>
    /// Gets or sets the fields.
    /// </summary>
    /// <value>The fields.</value>
    public VideoFields[]? Fields { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is protected by a password.
    /// </summary>
    public bool? PasswordProtected { get; set; }

    /// <summary>
    /// Audience meter URL to be used for this video. null if the audience shouldn’t be taken into account.
    /// </summary>
    public string? AudienceUrl { get; set; }
}
