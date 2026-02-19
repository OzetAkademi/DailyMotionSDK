using System.ComponentModel;
using System.Reflection;

namespace DailymotionSDK.Models;

/// <summary>
/// Strongly typed enum for video fields that can be requested from the Dailymotion API
/// https://developers.dailymotion.com/reference/video-fields
/// </summary>
public enum VideoFields
{
    /// <summary>
    /// Returns the custom target value for given video. This value is sent to Liverail as an LR_DM_ADPARAM param. This can be used for targeting in liverail.
    /// </summary>
    [Description("advertising_custom_target")]
    AdvertisingCustomTarget,

    /// <summary>
    /// True if the owner blocked instream ads on this video.
    /// </summary>
    [Description("advertising_instream_blocked")]
    AdvertisingInstreamBlocked,

    /// <summary>
    /// Whether AI chapter generation is required for the video or not.
    /// </summary>
    [Description("ai_chapter_generation_required")]
    AiChapterGenerationRequired,

    /// <summary>
    /// True if this video can be embedded outside of Dailymotion.
    /// </summary>
    [Description("allow_embed")]
    AllowEmbed,

    /// <summary>
    /// True if this video can be added to playlists.
    /// </summary>
    [Description("allowed_in_playlists")]
    AllowedInPlaylists,

    /// <summary>
    /// Aspect ratio of this video (i.e.: 1.33333 for 4/3, 1.77777 for 16/9…).
    /// </summary>
    [Description("aspect_ratio")]
    AspectRatio,

    /// <summary>
    /// Current live stream audience. null if the audience shouldn't be taken into consideration.
    /// </summary>
    [Description("audience")]
    Audience,

    /// <summary>
    /// Total live stream audience since stream creation. null if the audience shouldn't be taken into account.
    /// </summary>
    [Description("audience_total")]
    AudienceTotal,

    /// <summary>
    /// Audience meter URL to be used for this video. null if the audience shouldn't be taken into account.
    /// </summary>
    [Description("audience_url")]
    AudienceUrl,

    /// <summary>
    /// List of available stream formats for this video.
    /// </summary>
    [Description("available_formats")]
    AvailableFormats,

    /// <summary>
    /// Channel of this video. You can retrieve sub-fields of this channel object using the dot-notation (e.g.: channel.id).
    /// </summary>
    [Description("channel")]
    Channel,

    /// <summary>
    /// Video file hash.
    /// </summary>
    [Description("checksum")]
    Checksum,

    /// <summary>
    /// List of countries where this video is blocked by the claimer. A list of country codes (ISO 3166-1 alpha-2) e.g.: ["FR", "US"] will block this video in France and US.
    /// </summary>
    [Description("claim_rule_blocked_countries")]
    ClaimRuleBlockedCountries,

    /// <summary>
    /// List of countries where this video is monetized by the claimer. A list of country codes (ISO 3166-1 alpha-2) e.g.: ["FR", "US"] will monetize this video in France and US.
    /// </summary>
    [Description("claim_rule_monetized_countries")]
    ClaimRuleMonetizedCountries,

    /// <summary>
    /// List of countries where this video is tracked by the claimer. A list of country codes (ISO 3166-1 alpha-2) e.g.: ["FR", "US"] will track this video in France and US but it won't be blocked nor monetized by the claimer.
    /// </summary>
    [Description("claim_rule_tracked_countries")]
    ClaimRuleTrackedCountries,

    /// <summary>
    /// Content provider name.
    /// </summary>
    [Description("content_provider")]
    ContentProvider,

    /// <summary>
    /// Content provider identifier.
    /// </summary>
    [Description("content_provider_id")]
    ContentProviderId,

    /// <summary>
    /// Country of this video (declarative, may be null). Allowed values are ISO 3166-1 alpha-2 country codes.
    /// </summary>
    [Description("country")]
    Country,

    /// <summary>
    /// List of customizable values (maximum of 3 values).
    /// </summary>
    [Description("custom_classification")]
    CustomClassification,

    /// <summary>
    /// Date and time when this video was uploaded.
    /// </summary>
    [Description("created_time")]
    CreatedTime,

    /// <summary>
    /// Comprehensive description of this video. Maximum length is set to 3000 (5000 for partners).
    /// </summary>
    [Description("description")]
    Description,

    /// <summary>
    /// Duration of this video in seconds.
    /// </summary>
    [Description("duration")]
    Duration,

    /// <summary>
    /// HTML code to embed this video. Deprecation notice: Former endpoint dailymotion.com/embed has been deprecated.
    /// </summary>
    [Description("embed_html")]
    EmbedHtml,

    /// <summary>
    /// URL to embed this video. Deprecation notice: Former endpoint dailymotion.com/embed has been deprecated.
    /// </summary>
    [Description("embed_url")]
    EmbedUrl,

    /// <summary>
    /// End date and time of this live stream.
    /// </summary>
    [Description("end_time")]
    EndTime,

    /// <summary>
    /// Date and time after which this video will be made private.
    /// </summary>
    [Description("expiry_date")]
    ExpiryDate,

    /// <summary>
    /// By default, videos reaching their expiry_date are still available to anyone who knows how to access their private URL. Set this to false to disable this behavior.
    /// </summary>
    [Description("expiry_date_availability")]
    ExpiryDateAvailability,

    /// <summary>
    /// By default, videos are deleted (after a grace period) when their expiry_date is reached. Set this to false to disable this behavior.
    /// </summary>
    [Description("expiry_date_deletion")]
    ExpiryDateDeletion,

    /// <summary>
    /// True if this video is explicit. Warning: It's not possible to remove this flag once set.
    /// </summary>
    [Description("explicit")]
    Explicit,

    /// <summary>
    /// URL of the filmstrip sprite of this video. 100 images arranged in a 10×10 grid. Not available for short videos.
    /// </summary>
    [Description("filmstrip_60_url")]
    Filmstrip60Url,

    /// <summary>
    /// URL of this video's first_frame image (60px height).
    /// </summary>
    [Description("first_frame_60_url")]
    FirstFrame60Url,

    /// <summary>
    /// URL of this video's first_frame image (120px height).
    /// </summary>
    [Description("first_frame_120_url")]
    FirstFrame120Url,

    /// <summary>
    /// URL of this video's first_frame image (180px height).
    /// </summary>
    [Description("first_frame_180_url")]
    FirstFrame180Url,

    /// <summary>
    /// URL of this video's first_frame image (240px height).
    /// </summary>
    [Description("first_frame_240_url")]
    FirstFrame240Url,

    /// <summary>
    /// URL of this video's first_frame image (360px height).
    /// </summary>
    [Description("first_frame_360_url")]
    FirstFrame360Url,

    /// <summary>
    /// URL of this video's first_frame image (480px height).
    /// </summary>
    [Description("first_frame_480_url")]
    FirstFrame480Url,

    /// <summary>
    /// URL of this video's first_frame image (720px height).
    /// </summary>
    [Description("first_frame_720_url")]
    FirstFrame720Url,

    /// <summary>
    /// URL of this video's first_frame image (1080px height).
    /// </summary>
    [Description("first_frame_1080_url")]
    FirstFrame1080Url,

    /// <summary>
    /// List of countries where this video is or isn't accessible. A list of country codes (ISO 3166-1 alpha-2) starting with the deny or allow (default) keyword to define if this is a block or an allowlist.
    /// </summary>
    [Description("geoblocking")]
    Geoblocking,

    /// <summary>
    /// Geolocalization for this video. Result is an array with the longitude and latitude using point notation. Longitude range is from -180.0 (West) to 180.0 (East). Latitude range is from -90.0 (South) to 90.0 (North).
    /// </summary>
    [Description("geoloc")]
    Geoloc,

    /// <summary>
    /// List of hashtags attached to this video.
    /// </summary>
    [Description("hashtags")]
    Hashtags,

    /// <summary>
    /// Height of this video from the source (px).
    /// </summary>
    [Description("height")]
    Height,

    /// <summary>
    /// Unique object identifier (unique among all videos).
    /// </summary>
    [Description("id")]
    Id,

    /// <summary>
    /// True if this video is "Created for Kids" (intends to target an audience under the age of 16).
    /// </summary>
    [Description("is_created_for_kids")]
    IsCreatedForKids,

    /// <summary>
    /// True if this video is private.
    /// </summary>
    [Description("private")]
    Private,

    /// <summary>
    /// Graph type of this object (hopefully video).
    /// </summary>
    [Description("item_type")]
    ItemType,

    /// <summary>
    /// Language of this video. This value is declarative and corresponds to the user-declared spoken language of the video. Allowed values are ISO-639-3 alpha-2 and alpha-3 language codes.
    /// </summary>
    [Description("language")]
    Language,

    /// <summary>
    /// Date and time when this video was liked by the user.
    /// </summary>
    [Description("liked_at")]
    LikedAt,

    /// <summary>
    /// Total amount of times this video has been liked.
    /// </summary>
    [Description("likes_total")]
    LikesTotal,

    /// <summary>
    /// End date and time of the ad break for this live stream.
    /// </summary>
    [Description("live_ad_break_end_time")]
    LiveAdBreakEndTime,

    /// <summary>
    /// Launches a given number of ad breaks for this live stream.
    /// </summary>
    [Description("live_ad_break_launch")]
    LiveAdBreakLaunch,

    /// <summary>
    /// Remaining time for the ad break for this live stream.
    /// </summary>
    [Description("live_ad_break_remaining")]
    LiveAdBreakRemaining,

    /// <summary>
    /// Date and time when this live stream started airing.
    /// </summary>
    [Description("live_airing_time")]
    LiveAiringTime,

    /// <summary>
    /// Audio bitrate of this live stream.
    /// </summary>
    [Description("live_audio_bitrate")]
    LiveAudioBitrate,

    /// <summary>
    /// True if this live stream is automatically recorded.
    /// </summary>
    [Description("live_auto_record")]
    LiveAutoRecord,

    /// <summary>
    /// The live backup video, it is get/set as an xid, but is stored as an integer ID in DB.
    /// </summary>
    [Description("live_backup_video")]
    LiveBackupVideo,

    /// <summary>
    /// List of live stream ingests for this video.
    /// </summary>
    [Description("live_ingests")]
    LiveIngests,

    /// <summary>
    /// URL to publish the live source stream on using SRT. The current logged-in user needs to own this video to retrieve this field.
    /// </summary>
    [Description("live_publish_srt_url")]
    LivePublishSrtUrl,

    /// <summary>
    /// URL to publish the live source stream on. The current logged in user need to own this video in order to retrieve this field.
    /// </summary>
    [Description("live_publish_url")]
    LivePublishUrl,

    /// <summary>
    /// A one time usage list of URLs to be called in order log a view on this video made on a third party site (i.e.: embed player).
    /// </summary>
    [Description("log_external_view_urls")]
    LogExternalViewUrls,

    /// <summary>
    /// A one time usage URL to log a view on this video. This URL expires after a short period of time.
    /// </summary>
    [Description("log_view_url")]
    LogViewUrl,

    /// <summary>
    /// A one time usage list of URLs that will be called in order to log video view events.
    /// </summary>
    [Description("log_view_urls")]
    LogViewUrls,

    /// <summary>
    /// Stream mode.
    /// </summary>
    [Description("mode")]
    Mode,

    /// <summary>
    /// True if this live stream is broadcasting and watchable in the player.
    /// </summary>
    [Description("onair")]
    OnAir,

    /// <summary>
    /// Owner of this video. You can retrieve sub-fields of this owner object using the dot-notation (e.g.: owner.id).
    /// </summary>
    [Description("owner")]
    Owner,

    /// <summary>
    /// True if the video is owned by a partner.
    /// </summary>
    [Description("partner")]
    Partner,

    /// <summary>
    /// If a video is protected by a password, this field contains the password (deprecated, as it now only returns NULL). When setting a value on this field, the video visibility changes to "password protected". Setting it to NULL removes the password protection: the visibility is changed to "public".
    /// </summary>
    [Description("password")]
    Password,

    /// <summary>
    /// True if this video is password-protected.
    /// </summary>
    [Description("password_protected")]
    PasswordProtected,

    /// <summary>
    /// A unique video picked by the owner, displayed when video's playback ends. You can retrieve sub-fields of this video object using the dot-notation (e.g.: player_next_video.id).
    /// </summary>
    [Description("player_next_video")]
    PlayerNextVideo,

    /// <summary>
    /// An array of video picked by the owner, displayed when video's playback ends.
    /// </summary>
    [Description("player_next_videos")]
    PlayerNextVideos,

    /// <summary>
    /// URL of this video's video preview (240p).
    /// </summary>
    [Description("preview_240p_url")]
    Preview240pUrl,

    /// <summary>
    /// URL of this video's video preview (360p).
    /// </summary>
    [Description("preview_360p_url")]
    Preview360pUrl,

    /// <summary>
    /// URL of this video's video preview (480p).
    /// </summary>
    [Description("preview_480p_url")]
    Preview480pUrl,

    /// <summary>
    /// The private video id. Null if the authenticated user is not the owner of this video.
    /// </summary>
    [Description("private_id")]
    PrivateId,

    /// <summary>
    /// Date and time after which this video will be made publicly available.
    /// </summary>
    [Description("publish_date")]
    PublishDate,

    /// <summary>
    /// Keep this video private when its publication_date is reached.
    /// </summary>
    [Description("publish_date_keep_private")]
    PublishDateKeepPrivate,

    /// <summary>
    /// True if this video is published (may still be waiting for encoding, see the status field for more information).
    /// </summary>
    [Description("published")]
    Published,

    /// <summary>
    /// When this video status field is set to processing, this parameter indicates a number between 0 and 100 corresponding to the percentage of publishing already completed.
    /// </summary>
    [Description("publishing_progress")]
    PublishingProgress,

    /// <summary>
    /// When this video status field is set to processing, this parameter indicates a number between 0 and 100 corresponding to the percentage of encoding already completed.
    /// When this value reaches 100, it’s possible for the owner to play his video. For other statuses this parameter returns -1.
    /// </summary>
    [Description("encoding_progress")]
    EncodingProgress,

    /// <summary>
    /// Date and time when the video record was stopped.
    /// </summary>
    [Description("record_end_time")]
    RecordEndTime,

    /// <summary>
    /// Date and time when the video record started.
    /// </summary>
    [Description("record_start_time")]
    RecordStartTime,

    /// <summary>
    /// Current state of the recording process of this video.
    /// </summary>
    [Description("record_status")]
    RecordStatus,

    /// <summary>
    /// Recurrence pattern for this video.
    /// </summary>
    [Description("recurrence")]
    Recurrence,

    /// <summary>
    /// URL of the image-based seeker resource of this video. internal resource format is proprietary. Not available for short videos.
    /// </summary>
    [Description("seeker_url")]
    SeekerUrl,

    /// <summary>
    /// ISRC code of the soundtrack of this video.
    /// </summary>
    [Description("soundtrack_isrc")]
    SoundtrackIsrc,

    /// <summary>
    /// Popularity of the soundtrack of this video.
    /// </summary>
    [Description("soundtrack_popularity")]
    SoundtrackPopularity,

    /// <summary>
    /// URL of the sprite of this video (320x180).
    /// </summary>
    [Description("sprite_320x_url")]
    Sprite320xUrl,

    /// <summary>
    /// URL of the sprite of this video. Not available for short videos.
    /// </summary>
    [Description("sprite_url")]
    SpriteUrl,

    /// <summary>
    /// Start date and time of this live stream.
    /// </summary>
    [Description("start_time")]
    StartTime,

    /// <summary>
    /// Current state of this video.
    /// </summary>
    [Description("status")]
    Status,

    /// <summary>
    /// True if this video stream has been altered with AI.
    /// </summary>
    [Description("stream_altered_with_ai")]
    StreamAlteredWithAi,

    /// <summary>
    /// URL of the audio stream of this video.
    /// </summary>
    [Description("stream_audio_url")]
    StreamAudioUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (1080p).
    /// </summary>
    [Description("stream_h264_hd1080_url")]
    StreamH264Hd1080Url,

    /// <summary>
    /// URL of the H.264 stream of this video (720p).
    /// </summary>
    [Description("stream_h264_hd_url")]
    StreamH264HdUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (480p).
    /// </summary>
    [Description("stream_h264_hq_url")]
    StreamH264HqUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (240p).
    /// </summary>
    [Description("stream_h264_l1_url")]
    StreamH264L1Url,

    /// <summary>
    /// URL of the H.264 stream of this video (360p).
    /// </summary>
    [Description("stream_h264_l2_url")]
    StreamH264L2Url,

    /// <summary>
    /// URL of the H.264 stream of this video (480p).
    /// </summary>
    [Description("stream_h264_ld_url")]
    StreamH264LdUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (1440p).
    /// </summary>
    [Description("stream_h264_qhd_url")]
    StreamH264QhdUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (2160p).
    /// </summary>
    [Description("stream_h264_uhd_url")]
    StreamH264UhdUrl,

    /// <summary>
    /// URL of the H.264 stream of this video (480p).
    /// </summary>
    [Description("stream_h264_url")]
    StreamH264Url,

    /// <summary>
    /// URL of the HLS stream of this video.
    /// Required roles: can-read-video-streams, can-read-my-video-streams
    /// </summary>
    [Description("stream_hls_url")]
    StreamHlsUrl,

    /// <summary>
    /// URL of the HLS stream of this live video.
    /// Required roles: can-read-video-streams, can-read-my-video-streams
    /// </summary>
    [Description("stream_live_hls_url")]
    StreamLiveHlsUrl,

    /// <summary>
    /// URL of the RTMP stream of this live video.
    /// </summary>
    [Description("stream_live_rtmp_url")]
    StreamLiveRtmpUrl,

    /// <summary>
    /// URL of the Smooth stream of this live video.
    /// </summary>
    [Description("stream_live_smooth_url")]
    StreamLiveSmoothUrl,

    /// <summary>
    /// URL of the source stream of this video.
    /// </summary>
    [Description("stream_source_url")]
    StreamSourceUrl,

    /// <summary>
    /// Studio of this video. You can retrieve sub-fields of this studio object using the dot-notation (e.g.: studio.id).
    /// </summary>
    [Description("studio")]
    Studio,

    /// <summary>
    /// List of tags attached to this video.
    /// </summary>
    [Description("tags")]
    Tags,

    /// <summary>
    /// URL of this video's thumbnail (60px height).
    /// </summary>
    [Description("thumbnail_60_url")]
    Thumbnail60Url,

    /// <summary>
    /// URL of this video's thumbnail (62px height).
    /// </summary>
    [Description("thumbnail_62_url")]
    Thumbnail62Url,

    /// <summary>
    /// URL of this video's thumbnail (120px height).
    /// </summary>
    [Description("thumbnail_120_url")]
    Thumbnail120Url,

    /// <summary>
    /// URL of this video's thumbnail (180px height).
    /// </summary>
    [Description("thumbnail_180_url")]
    Thumbnail180Url,

    /// <summary>
    /// URL of this video's thumbnail (240px height).
    /// </summary>
    [Description("thumbnail_240_url")]
    Thumbnail240Url,

    /// <summary>
    /// URL of this video's thumbnail (360px height).
    /// </summary>
    [Description("thumbnail_360_url")]
    Thumbnail360Url,

    /// <summary>
    /// URL of this video's thumbnail (480px height).
    /// </summary>
    [Description("thumbnail_480_url")]
    Thumbnail480Url,

    /// <summary>
    /// URL of this video's thumbnail (720px height).
    /// </summary>
    [Description("thumbnail_720_url")]
    Thumbnail720Url,

    /// <summary>
    /// URL of this video's thumbnail (1080px height).
    /// </summary>
    [Description("thumbnail_1080_url")]
    Thumbnail1080Url,

    /// <summary>
    /// URL of this video's thumbnail.
    /// </summary>
    [Description("thumbnail_url")]
    ThumbnailUrl,

    /// <summary>
    /// Short URL of this video.
    /// </summary>
    [Description("tiny_url")]
    TinyUrl,

    /// <summary>
    /// Title of this video. Maximum length is set to 100 (150 for partners).
    /// </summary>
    [Description("title")]
    Title,

    /// <summary>
    /// Date and time when this video was last updated.
    /// </summary>
    [Description("updated_time")]
    UpdatedTime,

    /// <summary>
    /// Date and time when this video was uploaded.
    /// </summary>
    [Description("uploaded_time")]
    UploadedTime,

    /// <summary>
    /// URL of this video.
    /// </summary>
    [Description("url")]
    Url,

    /// <summary>
    /// True if this video is verified.
    /// </summary>
    [Description("verified")]
    Verified,

    /// <summary>
    /// Number of views of this video in the last day.
    /// </summary>
    [Description("views_last_day")]
    ViewsLastDay,

    /// <summary>
    /// Number of views of this video in the last hour.
    /// </summary>
    [Description("views_last_hour")]
    ViewsLastHour,

    /// <summary>
    /// Number of views of this video in the last month.
    /// </summary>
    [Description("views_last_month")]
    ViewsLastMonth,

    /// <summary>
    /// Number of views of this video in the last week.
    /// </summary>
    [Description("views_last_week")]
    ViewsLastWeek,

    /// <summary>
    /// Total number of views of this video.
    /// </summary>
    [Description("views_total")]
    ViewsTotal,

    /// <summary>
    /// Width of this video from the source (px).
    /// </summary>
    [Description("width")]
    Width,

    /// <summary>
    /// Name of this video.
    /// </summary>
    [Description("name")]
    Name,

    /// <summary>
    /// List of language codes requested for AI-generated subtitles.
    /// </summary>
    [Description("ai_subtitle_languages")]
    AiSubtitleLanguages,

    /// <summary>
    /// Media type of this video.
    /// </summary>
    [Description("media_type")]
    MediaType
}

/// <summary>
/// Extension methods for VideoFields enum
/// </summary>
public static class VideoFieldsExtensions
{
    /// <summary>
    /// Gets the API field name for a VideoFields enum value
    /// </summary>
    /// <param name="field">The VideoFields enum value</param>
    /// <returns>The API field name</returns>
    public static string GetApiFieldName(this VideoFields field)
    {
        var attribute = field.GetType()
            .GetField(field.ToString())?
            .GetCustomAttribute<DescriptionAttribute>();
        
        return attribute?.Description ?? field.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Converts an array of VideoFields to an array of API field names
    /// </summary>
    /// <param name="fields">Array of VideoFields</param>
    /// <returns>Array of API field names</returns>
    public static string[] ToApiFieldNames(this VideoFields[] fields)
    {
        return fields.Select(f => f.GetApiFieldName()).ToArray();
    }

    /// <summary>
    /// Checks if a field requires special permissions (can-read-video-streams, can-read-my-video-streams)
    /// and cannot be used in list endpoints
    /// </summary>
    /// <param name="field">The VideoFields enum value</param>
    /// <returns>True if the field requires special permissions</returns>
    public static bool RequiresStreamPermissions(this VideoFields field)
    {
        // Fields that require can-read-video-streams or can-read-my-video-streams roles
        // These fields are not supported on list endpoints and must be fetched individually
        return field == VideoFields.StreamHlsUrl || 
               field == VideoFields.StreamLiveHlsUrl;
    }

    /// <summary>
    /// Filters out fields that require special permissions and cannot be used in list endpoints
    /// </summary>
    /// <param name="fields">Array of VideoFields</param>
    /// <returns>Array of VideoFields with restricted fields removed</returns>
    public static VideoFields[] FilterRestrictedFields(this VideoFields[] fields)
    {
        return fields.Where(f => !f.RequiresStreamPermissions()).ToArray();
    }

    /// <summary>
    /// Gets fields that require special permissions and were filtered out
    /// </summary>
    /// <param name="fields">Array of VideoFields</param>
    /// <returns>Array of VideoFields that require special permissions</returns>
    public static VideoFields[] GetRestrictedFields(this VideoFields[] fields)
    {
        return fields.Where(f => f.RequiresStreamPermissions()).ToArray();
    }
}
