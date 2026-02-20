# DailyMotion SDK for .NET 10

A modern, secure, and comprehensive .NET 10 library for the DailyMotion API. This SDK provides a unified interface for all DailyMotion API operations with proper authentication, error handling, and modern development practices.

## Features

- ✅ **Modern .NET 10** - Built with the latest .NET 10 framework
- ✅ **RestSharp Integration** - Modern HTTP client with better performance and features
- ✅ **Dependency Injection** - Full DI support with Microsoft.Extensions.DependencyInjection
- ✅ **Comprehensive Authentication** - OAuth 2.0 flows with proper token management
- ✅ **Strongly Typed Models** - All API responses use concrete types with PascalCase naming
- ✅ **Input Validation** - Comprehensive parameter validation for all API calls
- ✅ **Structured Logging** - Built-in logging with Microsoft.Extensions.Logging
- ✅ **Security First** - No hardcoded credentials, configurable redirect URIs
- ✅ **Unit Tests** - Comprehensive test coverage with xUnit and Moq
- ✅ **XML Documentation** - Full API documentation with links to DailyMotion API docs
- ✅ **Single Entry Point** - Unified `DailymotionSDK` class for all operations

## Installation

### NuGet Package

```bash
dotnet add package DailymotionSDK
```

### Manual Installation

```bash
git clone https://github.com/OzetAkademi/DailyMotionSDK.git
cd DailymotionSDK
dotnet build
```

## Quick Start

### 1. Register Services

```csharp
using DailymotionSDK;
using DailymotionSDK.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

// Add logging
services.AddLogging(builder => builder.AddConsole());

// Add DailyMotion SDK with configuration
services.AddDailyMotionSDK(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.RedirectUri = "https://your-app.com/callback";
    options.EnableLogging = true;
});

var serviceProvider = services.BuildServiceProvider();
```

### 2. Use the SDK

```csharp
// Get the SDK instance
var sdk = serviceProvider.GetRequiredService<DailymotionSDK>();

// Authenticate with username/password
var authResult = await sdk.AuthenticateAsync("username", "password");

// Or authenticate with existing token
sdk.AuthenticateWithToken("your-access-token");

// Search for videos
var videos = await sdk.SearchVideosAsync("cats", limit: 10);

// Get trending videos
var trending = await sdk.GetTrendingVideosAsync(limit: 20);

// Get channel videos
var musicVideos = await sdk.GetChannelVideosAsync(Channel.Music, limit: 15);

// Get specific video
var video = await sdk.GetVideoAsync("video-id");

// Get specific user
var user = await sdk.GetUserAsync("user-id");
```

## API Examples

### Authentication

```csharp
// Password authentication
var authResult = await sdk.AuthenticateAsync("username", "password", new[]
{
    OAuthScope.Email,
    OAuthScope.UserInfo,
    OAuthScope.ManageVideos
});

// Get authorization URL for OAuth flow
var authUrl = sdk.GetAuthorizationUrl(
    OAuthResponseType.Code,
    new[] { OAuthScope.Email, OAuthScope.UserInfo }
);

// Exchange authorization code for token
var tokenResult = await sdk.ExchangeCodeForTokenAsync("authorization-code");

// Refresh token
var refreshResult = await sdk.RefreshTokenAsync("refresh-token");

// Validate token
var validation = await sdk.ValidateTokenAsync();

// Revoke token
var revoked = await sdk.RevokeTokenAsync();
```

### Videos

```csharp
// Search videos
var searchResults = await sdk.SearchVideosAsync(
    query: "nature documentaries",
    limit: 20,
    page: 1,
    sort: VideoSort.Trending
);

// Get trending videos
var trending = await sdk.GetTrendingVideosAsync(limit: 10);

// Get specific video
var video = await sdk.GetVideoAsync("x8c0v1");

// Get video direct links (requires authentication)
var directLinks = await sdk.Videos.GetVideoDirectLinksAsync("x8c0v1");
```

### Users

```csharp
// Search users
var users = await sdk.SearchUsersAsync(
    query: "music producer",
    limit: 15,
    sort: UserSort.Popular
);

// Get specific user
var user = await sdk.GetUserAsync("user123");

// Get user videos
var userVideos = await sdk.Users.GetUserVideosAsync("user123", limit: 20);

// Get user playlists
var userPlaylists = await sdk.Users.GetUserPlaylistsAsync("user123");
```

### Channels

```csharp
// Get channel videos
var musicVideos = await sdk.GetChannelVideosAsync(
    Channel.Music,
    limit: 25,
    sort: VideoSort.Recent
);

// Get channel metadata
var channelInfo = await sdk.Channels.GetChannelAsync(Channel.Sports);

// Get channel subscribers
var subscribers = await sdk.Channels.GetChannelSubscribersAsync(Channel.Tech);
```

### Current User Operations (Mine)

```csharp
// Get current user info
var myInfo = await sdk.Mine.GetUserInfoAsync();

// Get my videos
var myVideos = await sdk.Mine.GetMyVideosAsync(limit: 20);

// Get my playlists
var myPlaylists = await sdk.Mine.GetMyPlaylistsAsync();

// Get my followers
var followers = await sdk.Mine.GetMyFollowersAsync();

// Get my following
var following = await sdk.Mine.GetMyFollowingAsync();
```

### Playlists

```csharp
// Create a new playlist (requires authentication)
var newPlaylist = await sdk.Playlists.CreatePlaylistAsync(
    name: "My Favorites",
    description: "My favorite videos",
    isPrivate: false
);

// Get playlist by ID
var playlist = await sdk.Playlists.GetPlaylistAsync("playlist123");

// Get playlist videos
var playlistVideos = await sdk.Playlists.GetPlaylist("playlist123").GetVideosAsync(limit: 20);

// Add video to playlist
var playlistClient = sdk.Playlists.GetPlaylist("playlist123");
await playlistClient.AddVideoAsync("video123");

// Remove video from playlist
await playlistClient.RemoveVideoAsync("video123");

// Update playlist metadata
var updatedPlaylist = await playlistClient.UpdateMetadataAsync(
    name: "Updated Playlist Name",
    description: "Updated description",
    isPrivate: true
);

// Delete playlist
await playlistClient.DeleteAsync();

// Search playlists with filters
var filters = new PlaylistFilters
{
    Search = "favorites",
    Private = false,
    Sort = PlaylistSort.Recent
};
var searchResults = await sdk.Playlists.SearchPlaylistsAsync(filters, limit: 10);

// Get playlists with filters
var privateFilters = new PlaylistFilters
{
    Private = true,
    Owner = "user123"
};
var privatePlaylists = await sdk.Playlists.GetPlaylistsAsync(privateFilters, limit: 5);

// Get user's playlists (through Mine service)
var myPlaylists = await sdk.Mine.GetPlaylistsAsync(limit: 10);

// Search user's playlists (through Mine service)
var searchResults = await sdk.Mine.SearchPlaylistsAsync("favorites", limit: 5);
```

### Logout

```csharp
// Logout the current user (invalidates the access token)
var logoutSuccess = await sdk.Logout.LogoutAsync();

if (logoutSuccess)
{
    Console.WriteLine("Successfully logged out");
}
else
{
    Console.WriteLine("Logout failed");
}
```

### Likes and Favorites

```csharp
// Get my likes
var myLikes = await sdk.Likes.GetMyLikesAsync(limit: 20);

// Like a video
await sdk.Likes.LikeVideoAsync("video123");

// Unlike a video
await sdk.Likes.UnlikeVideoAsync("video123");

// Get my favorites
var myFavorites = await sdk.Favorites.GetMyFavoritesAsync(limit: 20);

// Add to favorites
await sdk.Favorites.AddToFavoritesAsync("video123");

// Remove from favorites
await sdk.Favorites.RemoveFromFavoritesAsync("video123");
```

## Configuration

### DailymotionOptions

```csharp
var options = new DailymotionOptions
{
    // Required for authentication
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    
    // Required for OAuth flows
    RedirectUri = "https://your-app.com/callback",
    
    // Optional settings
    ApiBaseUrl = "https://api.dailymotion.com",
    OAuthBaseUrl = "https://www.dailymotion.com/oauth",
    Timeout = TimeSpan.FromSeconds(60),
    MaxRetries = 3,
    UserAgent = "MyApp/1.0",
    EnableLogging = true
};
```

## Error Handling

The SDK uses custom exceptions for better error handling:

```csharp
try
{
    var video = await sdk.GetVideoAsync("invalid-id");
}
catch (DailymotionException ex)
{
    Console.WriteLine($"API Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation Error: {ex.Message}");
}
```

## Testing

Run the unit tests to verify the SDK is working correctly:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test DailymotionSDK.Tests/
```

## NuGet Publishing

### Using PowerShell Script (Windows)

```powershell
.\publish-nuget.ps1 -NuGetFeedUrl "https://your-nuget-feed.com/v3/index.json" -ApiKey "your-api-key" -Version "2.0.0"
```

### Using Shell Script (Linux/macOS)

```bash
./publish-nuget.sh --feed-url "https://your-nuget-feed.com/v3/index.json" --api-key "your-api-key" --version "2.0.0"
```

### Manual Publishing

```bash
# Build the project
dotnet build --configuration Release

# Create NuGet package
dotnet pack --configuration Release --output nupkgs

# Push to NuGet feed
dotnet nuget push nupkgs/DailymotionSDK.2.0.0.nupkg --source "your-feed-url" --api-key "your-api-key"
```

## Usage Examples

### Video Operations with Filters and Fields

The SDK provides comprehensive video filtering and field selection capabilities:

#### Basic Video Retrieval

```csharp
// Get all videos with basic fields
var videos = await sdk.Videos.GetVideosAsync();

// Get videos with specific fields
var videosWithFields = await sdk.Videos.GetVideosAsync(
    fields: new[] { VideoFields.Title, VideoFields.Duration, VideoFields.ViewsTotal }
);
```

#### Advanced Filtering

```csharp
// Search for HD videos with specific criteria
var filters = new VideoFilters
{
    Search = "football",
    Hd = true,
    Featured = true,
    Sort = "trending",
    Languages = new[] { "en", "fr" },
    CreatedAfter = DateTime.Now.AddDays(-7)
};

var hdVideos = await sdk.Videos.GetVideosAsync(filters);
```

#### Live Streaming Content

```csharp
// Get live streaming videos
var liveFilters = new VideoFilters
{
    Live = true,
    LiveOnair = true,
    Mode = "live"
};

var liveVideos = await sdk.Videos.GetVideosAsync(
    filters: liveFilters,
    fields: new[] { VideoFields.Title, VideoFields.LivePublishUrl, VideoFields.Audience }
);
```

#### User-Specific Content

```csharp
// Get videos from specific users
var userFilters = new VideoFilters
{
    Owners = new[] { "user1", "user2" },
    Private = false,
    Verified = true
};

var userVideos = await sdk.Videos.GetVideosAsync(
    filters: userFilters,
    fields: new[] { VideoFields.Title, VideoFields.Owner, VideoFields.Verified }
);
```

#### Content Classification

```csharp
// Get premium content with specific characteristics
var premiumFilters = new VideoFilters
{
    Premium = true,
    Partner = true,
    Explicit = false,
    IsCreatedForKids = false,
    Hd = true
};

var premiumVideos = await sdk.Videos.GetVideosAsync(
    filters: premiumFilters,
    fields: new[] { VideoFields.Title, VideoFields.Partner, VideoFields.Premium }
);
```

#### Duration and Temporal Filtering

```csharp
// Get short videos from the last day
var shortVideoFilters = new VideoFilters
{
    ShorterThan = 60, // Less than 1 minute
    Timeframe = 86400, // Last 24 hours
    Sort = "recent"
};

var shortVideos = await sdk.Videos.GetVideosAsync(
    filters: shortVideoFilters,
    fields: new[] { VideoFields.Title, VideoFields.Duration, VideoFields.CreatedTime }
);
```

#### Geographic and Language Filtering

```csharp
// Get videos from specific countries and languages
var geoFilters = new VideoFilters
{
    Country = "US",
    Languages = new[] { "en", "es" },
    Channel = "news"
};

var geoVideos = await sdk.Videos.GetVideosAsync(
    filters: geoFilters,
    fields: new[] { VideoFields.Title, VideoFields.Country, VideoFields.Language }
);
```

#### Advanced Flag Combinations

```csharp
// Use multiple flags for complex filtering
var advancedFilters = new VideoFilters
{
    Flags = new[] { "featured", "hd", "verified", "partner" },
    Search = "technology",
    Sort = "trending"
};

var advancedVideos = await sdk.Videos.GetVideosAsync(
    filters: advancedFilters,
    fields: new[] { VideoFields.Title, VideoFields.Tags, VideoFields.ViewsTotal }
);
```

#### Synchronous Operations

```csharp
// Synchronous version for simple operations
var videos = sdk.Videos.GetVideos(
    filters: new VideoFilters { Hd = true },
    fields: new[] { VideoFields.Title, VideoFields.ThumbnailUrl }
);
```

### Field Selection Examples

#### Basic Video Information
```csharp
var basicFields = new[] {
    VideoFields.Id,
    VideoFields.Title,
    VideoFields.Description,
    VideoFields.Duration,
    VideoFields.ThumbnailUrl
};
```

#### Analytics and Engagement
```csharp
var analyticsFields = new[] {
    VideoFields.ViewsTotal,
    VideoFields.ViewsLastDay,
    VideoFields.LikesTotal,
    VideoFields.CommentsTotal
};
```

#### Live Streaming Information
```csharp
var liveFields = new[] {
    VideoFields.LivePublishUrl,
    VideoFields.Audience,
    VideoFields.Onair,
    VideoFields.StartTime,
    VideoFields.EndTime
};
```

#### Media URLs
```csharp
var mediaFields = new[] {
    VideoFields.StreamH264Url,
    VideoFields.StreamHlsUrl,
    VideoFields.Thumbnail720Url,
    VideoFields.EmbedUrl
};
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Changelog

### Version 2.0.0
- **Complete Modernization**: Upgraded to .NET 10 with latest language features
- **RestSharp Integration**: Replaced custom HTTP client with RestSharp for better performance
- **Dependency Injection**: Full DI support with Microsoft.Extensions.DependencyInjection
- **New Model System**: Replaced Hungarian notation with PascalCase naming and concrete types
- **Single Entry Point**: Created unified `DailymotionSDK` class for all operations
- **Comprehensive Validation**: Added input validation for all API parameters
- **Security Improvements**: Removed hardcoded credentials and suspicious redirect URIs
- **Enhanced Documentation**: Added XML documentation with links to DailyMotion API docs
- **Unit Tests**: Added comprehensive test coverage with xUnit and Moq
- **Structured Logging**: Integrated Microsoft.Extensions.Logging for better observability
- **Updated Dependencies**: All NuGet packages updated to latest versions
- **Complete Playlist Management**: Added full playlist CRUD operations and video management
- **Logout Functionality**: Implemented proper session termination and token invalidation
- **Video Embed Settings**: Added support for configuring video embedding permissions
- **Advanced Video Filters**: Implemented comprehensive filtering and sorting options

### Version 1.x.x
- Initial release with basic DailyMotion API support

