# DailyMotion SDK Demo Application

This demo application tests all functionality of the DailyMotion SDK in a real environment. It demonstrates how to use every available function and provides a comprehensive test suite for the SDK.

## Features

The demo application tests the following SDK functionality:

### ✅ Basic API Endpoints
- **Echo** - Tests API connectivity
- **Languages** - Retrieves available languages
- **Locale** - Detects user locale

### ✅ Authentication
- Password-based authentication
- Token validation
- Token refresh (if applicable)
- Logout functionality

### ✅ Video Operations
- Get trending videos
- Search videos
- Get specific video details
- Video upload (with authentication)
- Video management

### ✅ User Operations
- Search users
- Get user details
- Get user videos
- Get user playlists

### ✅ Playlist Operations
- Get playlist details
- Create playlists (with authentication)
- Add/remove videos from playlists
- Delete playlists

### ✅ Channel Operations
- Get videos from different channels (Music, Sport, News, etc.)
- Channel-specific content retrieval

### ✅ Search Operations
- Video search with various terms
- User search
- Advanced search filters

### ✅ File Operations
- File upload functionality
- File management

### ✅ Player Operations
- Player creation and management
- Player configuration

### ✅ Subtitle Operations
- Subtitle upload and management
- Subtitle retrieval

## Configuration

### 1. Application Settings

Create or update `appsettings.json` with your DailyMotion API credentials:

```json
{
  "DailymotionOptions": {
    "ApiBaseUrl": "https://api.dailymotion.com",
    "OAuthBaseUrl": "https://www.dailymotion.com/oauth",
    "ApiKeyType": "Public",
    "PasswordAuthUsername": "YOUR_USERNAME_HERE",
    "PasswordAuthPassword": "YOUR_PASSWORD_HERE",
    "PrivateApiKey": "YOUR_PRIVATE_API_KEY_HERE",
    "PrivateApiSecret": "YOUR_PRIVATE_API_SECRET_HERE",
    "PublicApiKey": "YOUR_PUBLIC_API_KEY_HERE",
    "PublicApiSecret": "YOUR_PUBLIC_API_SECRET_HERE",
    "RedirectUri": "https://localhost:8080/callback",
    "Timeout": "00:02:00",
    "MaxRetries": 3,
    "UserAgent": "DailymotionSDK-Demo/1.0",
    "EnableLogging": true
  },
  "Demo": {
    "EnableInteractiveAuth": false,
    "TestVideoPath1": "test1.mp4",
    "TestVideoPath2": "test2.mp4",
    "TestPlayerId": "YOUR_PLAYER_ID_HERE",
    "CleanupAfterTests": true,
    "WaitBetweenOperations": 2000
  }
}
```

### 2. User Secrets (Recommended)

For security, store sensitive information in user secrets:

```bash
# Password Grant Authentication
dotnet user-secrets set "DailymotionOptions:PasswordAuthUsername" "your-username"
dotnet user-secrets set "DailymotionOptions:PasswordAuthPassword" "your-password"

# Client Credentials with Private API Keys
dotnet user-secrets set "DailymotionOptions:PrivateApiKey" "your-private-api-key"
dotnet user-secrets set "DailymotionOptions:PrivateApiSecret" "your-private-api-secret"

# Client Credentials with Public API Keys (also used for Password Grant and Authorization Code)
dotnet user-secrets set "DailymotionOptions:PublicApiKey" "your-public-api-key"
dotnet user-secrets set "DailymotionOptions:PublicApiSecret" "your-public-api-secret"
```

### 3. Test Video Files

Place test video files named `test1.mp4` and `test2.mp4` in the `DailymotionSDK.Demo` folder, or update the `TestVideoPath1` and `TestVideoPath2` configuration to point to your test videos.

## Getting DailyMotion API Credentials

1. Go to [DailyMotion Developer Portal](https://developers.dailymotion.com/)
2. Create a new application
3. Get your Client ID and Client Secret
4. Configure the redirect URI in your application settings

## Running the Demo

### Prerequisites

- .NET 10.0 or later
- DailyMotion API credentials
- Test video file (optional, for upload tests)

### Steps

1. **Clone and build the project:**
   ```bash
   git clone <repository-url>
   cd DailyMotionSdk/DailymotionSDK.Demo
   dotnet build
   ```

2. **Configure credentials** (see Configuration section above)

3. **Run the demo:**
   ```bash
   dotnet run
   ```

### Expected Output

The demo will output detailed logs showing:

- ✅ Successful operations with results
- ⚠️ Warnings for operations that require authentication or specific setup
- 🔍 Detailed information about API responses
- 🧹 Cleanup operations removing test data

## Demo Workflow

The demo follows this comprehensive workflow:

### 1. Basic API Testing
- Tests Echo endpoint to verify connectivity
- Retrieves available languages
- Detects current locale

### 2. Authentication Flow
- Authenticates with username/password
- Validates the received token
- Tests token refresh (if applicable)

### 3. Video Operations
- Gets trending videos
- Searches for videos
- Retrieves specific video details
- Uploads test video (if authenticated)

### 4. User Operations
- Searches for users
- Gets user details
- Retrieves user's videos and playlists

### 5. Playlist Management
- Creates a test playlist
- Adds videos to the playlist
- Modifies playlist contents
- Removes videos from playlist
- Deletes the test playlist

### 6. Channel Exploration
- Tests different channel categories
- Retrieves channel-specific content

### 7. Search Functionality
- Tests various search terms
- Demonstrates search filters and sorting

### 8. File Management
- Uploads test files
- Manages uploaded content

### 9. Player Operations
- Tests player creation and management
- Configures player settings

### 10. Subtitle Operations
- Tests subtitle upload and management
- Retrieves subtitle information

### 11. Cleanup
- Removes all test data created during the demo
- Logs out from the API
- Ensures no test artifacts remain

## Troubleshooting

### Common Issues

1. **Authentication Failed**
   - Verify your username and password
   - Check that your API credentials are correct
   - Ensure your DailyMotion account has API access

2. **File Upload Failed**
   - Verify the test video file exists
   - Check file format is supported by DailyMotion
   - Ensure you have upload permissions

3. **Rate Limiting**
   - The demo includes delays between operations
   - Increase `WaitBetweenOperations` if you encounter rate limits

4. **Missing Permissions**
   - Some operations require specific API permissions
   - Check your DailyMotion application settings

### Logs

The demo provides detailed logging at different levels:
- **Information**: General operation status
- **Debug**: Detailed API request/response information
- **Warning**: Non-critical issues or skipped operations
- **Error**: Critical failures

## Security Notes

- Never commit API credentials to version control
- Use user secrets or environment variables for sensitive data
- The demo automatically cleans up test data by default
- Review the cleanup process to ensure no important data is removed

## Extending the Demo

To add new functionality to the demo:

1. Add new test methods to `DemoService.cs`
2. Update the `RunDemoAsync` method to call your new tests
3. Add any required configuration to `DemoOptions.cs`
4. Update this README with documentation for the new features

## Support

If you encounter issues with the demo:

1. Check the logs for detailed error information
2. Verify your API credentials and permissions
3. Review the DailyMotion API documentation
4. Check the SDK source code for implementation details

The demo is designed to be comprehensive and educational, showing both successful operations and graceful handling of errors or missing permissions.
