# DailyMotion SDK Demo - Usage Guide

## Quick Start

### 1. Basic Demo (No Authentication Required)

The demo can run basic tests without any configuration:

```bash
cd DailymotionSDK.Demo
dotnet run
```

This will test:
- ✅ Video operations (trending videos, video details)
- ✅ User operations (user search, user details)
- ✅ Channel operations (different channel categories)
- ✅ Search operations (video search)

### 2. Full Demo (With Authentication)

For complete testing including upload, playlist management, etc.:

1. **Get DailyMotion API Credentials:**
   - Visit [DailyMotion Developer Portal](https://developers.dailymotion.com/)
   - Create a new application
   - Get your Client ID and Client Secret

2. **Configure Credentials:**
   ```bash
   # Using the setup script (recommended)
   ..\setup-user-secrets.ps1
   
   # Or manually using user secrets
   
   # SDK Configuration (for Password Grant and Authorization Code):
   # Note: PublicApiKey and PublicApiSecret are used for Password Grant and Authorization Code flows
   
   # For Password flow authentication (user credentials)
   dotnet user-secrets set "DailymotionOptions:PasswordAuthUsername" "your-username"
   dotnet user-secrets set "DailymotionOptions:PasswordAuthPassword" "your-password"
   
   # For client_credentials flow authentication (Private API Keys)
   dotnet user-secrets set "DailymotionOptions:PrivateApiKey" "your-private-api-key"
   dotnet user-secrets set "DailymotionOptions:PrivateApiSecret" "your-private-api-secret"
   
   # For client_credentials flow authentication (Public API Keys)
   dotnet user-secrets set "DailymotionOptions:PublicApiKey" "your-public-api-key"
   dotnet user-secrets set "DailymotionOptions:PublicApiSecret" "your-public-api-secret"
   
   # For Authorization Code Grant (redirect URI)
   dotnet user-secrets set "DailymotionOptions:RedirectUri" "https://your-app.com/callback"
   ```

   Or copy `appsettings.example.json` to `appsettings.json` and update the values.

3. **Add Test Video (Optional):**
   ```bash
   # Place any MP4 file as test1.mp4 and test2.mp4 in the demo directory
   # Or create simple test videos:
   ffmpeg -f lavfi -i testsrc=duration=10:size=320x240:rate=30 -pix_fmt yuv420p test1.mp4
   ffmpeg -f lavfi -i testsrc=duration=15:size=640x480:rate=30 -pix_fmt yuv420p test2.mp4
   ```

4. **Run Full Demo:**
   ```bash
   dotnet run
   ```

## Authentication Methods

The SDK supports three OAuth 2.0 authentication methods:

### SDK Configuration vs Authentication Credentials

**SDK Configuration** (`DailymotionOptions`):
- **Password Grant**: Uses `PasswordAuthUsername` and `PasswordAuthPassword` (user credentials) + `PublicApiKey`/`PublicApiSecret` (for application identification)
- **Client Credentials (Private)**: Uses `PrivateApiKey` and `PrivateApiSecret` (Private API Key/Secret)
- **Client Credentials (Public)**: Uses `PublicApiKey` and `PublicApiSecret` (Public API Key/Secret)
- **Authorization Code**: Uses `PublicApiKey`/`PublicApiSecret` (Public API Key/Secret) + `RedirectUri`
- All authentication credentials are now centralized in `DailymotionOptions`

### 1. Client Credentials Grant (Private API Keys)
**Use case:** Server-to-server applications, background jobs
```bash
# Configure with Private API Key and Secret
dotnet user-secrets set "DailymotionOptions:PrivateApiKey" "your-private-api-key"
dotnet user-secrets set "DailymotionOptions:PrivateApiSecret" "your-private-api-secret"
```

**Available Scopes:**
- `manage_videos` - Create/edit/delete videos
- `manage_playlists` - Create/edit/delete playlists
- `manage_players` - Create/modify/delete players
- `manage_podcasts` - Create/edit/delete podcasts
- `manage_subscriptions` - Subscribe to channels
- `manage_subtitles` - Create/edit/delete subtitles

### 2. Password Grant (Public API Keys)
**Use case:** Trusted applications, mobile apps
```bash
# Configure with username and password
dotnet user-secrets set "DailymotionOptions:PasswordAuthUsername" "your-username"
dotnet user-secrets set "DailymotionOptions:PasswordAuthPassword" "your-password"
```

**Available Scopes:**
- `email` - Access to user's email
- `userinfo` - Read/write access to personal information
- `manage_videos` - Create/edit/delete videos
- `manage_likes` - Manage liked videos
- `manage_playlists` - Create/edit/delete playlists
- `manage_players` - Create/modify/delete players
- `manage_podcasts` - Create/edit/delete podcasts
- `manage_subscriptions` - Subscribe to channels
- `manage_subtitles` - Create/edit/delete subtitles

### 3. Authorization Code Grant (Public API Keys)
**Use case:** Web applications, user-facing apps
```bash
# Configure redirect URI
dotnet user-secrets set "DailymotionOptions:RedirectUri" "https://your-app.com/callback"
```

**Flow:**
1. User is redirected to DailyMotion authorization URL
2. User authorizes the application
3. DailyMotion redirects back with authorization code
4. Application exchanges code for access token

## Demo Output Explained

### ✅ Successful Operations
- **Green checkmarks** indicate successful API calls
- **Detailed logs** show API responses and data retrieved
- **Resource counts** show how many items were found

### ⚠️ Warnings (Expected)
- **Authentication required** - Some operations need valid credentials
- **File not found** - Upload tests need sample video files
- **API limitations** - Some endpoints may have restrictions
- **No credentials provided** - Authentication tests are skipped

### ❌ Errors (Investigate)
- **Network issues** - Check internet connection
- **Invalid credentials** - Verify API keys and login details
- **API changes** - DailyMotion API might have changed
- **Invalid scopes** - Check scope format and permissions

## Real-World Testing Workflow

The demo follows this comprehensive workflow:

```
1. Basic API Connectivity
   ├── Echo endpoint test
   ├── Languages retrieval
   └── Locale detection

2. Authentication Flow
   ├── Client Credentials (Private API Keys)
   ├── Password Grant (Public API Keys)
   ├── Authorization Code Grant (Public API Keys)
   ├── Token validation
   └── Token refresh

3. Video Management
   ├── Get trending videos
   ├── Search videos
   ├── Get video details
   └── Upload video (with auth)

4. User Operations
   ├── Search users
   ├── Get user details
   └── Get user content

5. Playlist Management
   ├── Create playlist
   ├── Add videos
   ├── Modify playlist
   └── Delete playlist

6. Content Discovery
   ├── Channel browsing
   ├── Search functionality
   └── Content filtering

7. File Operations
   ├── File upload
   └── File management

8. Advanced Features
   ├── Player management
   └── Subtitle operations

9. Cleanup
   ├── Remove test data
   └── Logout
```

## Configuration Options

### DailymotionOptions
```json
{
  "ApiBaseUrl": "https://api.dailymotion.com",
  "OAuthBaseUrl": "https://www.dailymotion.com/oauth",
  "ApiKeyType": "Public",
  "PasswordAuthUsername": "your-username",
  "PasswordAuthPassword": "your-password",
  "PrivateApiKey": "your-private-api-key",
  "PrivateApiSecret": "your-private-api-secret",
  "PublicApiKey": "your-public-api-key",
  "PublicApiSecret": "your-public-api-secret",
  "RedirectUri": "https://localhost:8080/callback",
  "Timeout": "00:02:00",
  "MaxRetries": 3,
  "UserAgent": "DailymotionSDK-Demo/1.0",
  "EnableLogging": true
}
```

### Demo Options
```json
{
  "EnableInteractiveAuth": false,
  "TestVideoPath1": "test1.mp4",
  "TestVideoPath2": "test2.mp4",
  "TestPlayerId": "YOUR_PLAYER_ID_HERE",
  "CleanupAfterTests": true,
  "WaitBetweenOperations": 2000
}
```

## Troubleshooting

### Common Issues

1. **"Echo request failed"**
   - This is often expected as the echo endpoint might not be available
   - The demo continues with other tests

2. **"No test credentials provided"**
   - Authentication tests are skipped without credentials
   - Configure credentials for full testing

3. **"Test video file not found"**
   - File upload tests are skipped without sample videos
   - Add test1.mp4 and test2.mp4 files to test uploads

4. **Rate limiting errors**
   - Increase `WaitBetweenOperations` value
   - The demo includes built-in delays to avoid rate limits

5. **"Invalid scope" errors**
   - Check that you're using the correct scope names from the [official documentation](https://developers.dailymotion.com/guides/api-scopes/)
   - Ensure your API key has the required permissions

6. **"Client credentials authentication failed"**
   - Verify you're using Private API Key and Secret (not Public)
   - Check that the API key supports the requested scopes

### Debug Mode

Enable detailed logging by setting:
```json
{
  "Logging": {
    "LogLevel": {
      "DailymotionSDK": "Debug"
    }
  }
}
```

## Production Usage

This demo shows how to use the SDK in production:

```csharp
// 1. Configure services with SDK credentials
services.AddDailymotionSDK(options => {
    options.PublicApiKey = "your-public-api-key";        // For Password Grant and Authorization Code
    options.PublicApiSecret = "your-public-api-secret";  // For Password Grant and Authorization Code
    options.PrivateApiKey = "your-private-api-key";      // For Client Credentials (Private)
    options.PrivateApiSecret = "your-private-api-secret"; // For Client Credentials (Private)
});

// 2. Inject and use
public class VideoService
{
    private readonly DailymotionSDK _sdk;
    
    public VideoService(DailymotionSDK sdk)
    {
        _sdk = sdk;
    }
    
    // Client Credentials Authentication (uses Private API Key/Secret)
    public async Task<VideoListResponse> GetTrendingAsync()
    {
        var privateApiKey = "your-private-api-key";      // Get from configuration
        var privateApiSecret = "your-private-api-secret"; // Get from configuration
        
        await _sdk.AuthenticateWithClientCredentialsAsync(privateApiKey, privateApiSecret, new[] { OAuthScope.ManageVideos });
        return await _sdk.GetTrendingVideosAsync(limit: 10);
    }
    
    // Password Grant Authentication (uses Public API Key/Secret from SDK config + username/password)
    public async Task<VideoListResponse> GetUserVideosAsync(string username, string password)
    {
        await _sdk.AuthenticateWithPasswordAsync(username, password, new[] { OAuthScope.ManageVideos });
        return await _sdk.GetTrendingVideosAsync(limit: 10);
    }
    
    // Authorization Code Grant (uses Public API Key/Secret from SDK config + authorization code)
    public async Task<VideoListResponse> GetUserVideosWithCodeAsync(string authCode, string redirectUri)
    {
        await _sdk.ExchangeCodeForTokenAsync(authCode, redirectUri);
        return await _sdk.GetTrendingVideosAsync(limit: 10);
    }
}
```

## Testing Your Own Implementation

Use this demo as a reference for:
- ✅ Proper error handling
- ✅ Authentication flows (all three methods)
- ✅ Resource cleanup
- ✅ Rate limit handling
- ✅ Logging and monitoring
- ✅ Configuration management
- ✅ Scope validation

## Support

If you encounter issues:
1. Check the logs for detailed error information
2. Verify your API credentials and key types
3. Review the [DailyMotion API documentation](https://developers.dailymotion.com/)
4. Check network connectivity
5. Ensure your account has the required permissions
6. Verify scope names match the [official documentation](https://developers.dailymotion.com/guides/api-scopes/)
