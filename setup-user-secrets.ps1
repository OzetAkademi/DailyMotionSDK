# DailyMotion SDK Demo - User Secrets Setup Script
# This script helps you set up user secrets for the demo application

Write-Host "=== DailyMotion SDK Demo - User Secrets Setup ===" -ForegroundColor Green
Write-Host ""

# Check if we're in the right directory
if (-not (Test-Path "DailymotionSDK.Demo\DailymotionSDK.Demo.csproj")) {
    Write-Host "Error: Please run this script from the root directory of the DailyMotion SDK project" -ForegroundColor Red
    exit 1
}

# Navigate to the demo project directory
Set-Location "DailymotionSDK.Demo"

Write-Host "Setting up user secrets for DailyMotion SDK Demo..." -ForegroundColor Yellow
Write-Host ""

# Prompt for credentials
Write-Host "Please enter your DailyMotion API credentials:" -ForegroundColor Cyan
Write-Host ""
Write-Host "SDK Configuration (for Password Grant and Authorization Code):" -ForegroundColor Yellow
$clientId = Read-Host "Public API Key (for SDK configuration)"
$clientSecret = Read-Host "Public API Secret (for SDK configuration)" -AsSecureString

Write-Host ""
Write-Host "For Client Credentials Authentication (Private API Keys):" -ForegroundColor Cyan
$privateApiKey = Read-Host "Private API Key (for client credentials)"
$privateApiSecret = Read-Host "Private API Secret (for client credentials)" -AsSecureString

Write-Host ""
Write-Host ""
Write-Host "For Password Grant Authentication:" -ForegroundColor Cyan
$username = Read-Host "Username (for testing)"
$password = Read-Host "Password (for testing)" -AsSecureString

Write-Host ""
Write-Host "For Authorization Code Grant:" -ForegroundColor Cyan
$redirectUri = Read-Host "Redirect URI (default: https://localhost:8080/callback)"

Write-Host ""
Write-Host "For Player Operations:" -ForegroundColor Cyan
$playerId = Read-Host "Player ID (for testing player operations)"

# Convert secure strings to plain text for user secrets
$clientSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($clientSecret))
$privateApiSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($privateApiSecret))
$passwordPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))

# Clear the secure strings from memory
$clientSecret.Dispose()
$privateApiSecret.Dispose()
$password.Dispose()

Write-Host ""
Write-Host "Setting user secrets..." -ForegroundColor Yellow

# Set the user secrets
dotnet user-secrets set "DailymotionOptions:ClientId" $clientId
dotnet user-secrets set "DailymotionOptions:ClientSecret" $clientSecretPlain

if ($privateApiKey -and $privateApiSecretPlain) {
    dotnet user-secrets set "Demo:ClientCredentialsApiKey" $privateApiKey
    dotnet user-secrets set "Demo:ClientCredentialsApiSecret" $privateApiSecretPlain
}

if ($username -and $passwordPlain) {
    dotnet user-secrets set "Demo:TestUsername" $username
    dotnet user-secrets set "Demo:TestPassword" $passwordPlain
}

if ($redirectUri) {
    dotnet user-secrets set "Demo:AuthorizationCodeRedirectUri" $redirectUri
}

if ($playerId) {
    dotnet user-secrets set "Demo:TestPlayerId" $playerId
}

Write-Host ""
Write-Host "User secrets have been set successfully!" -ForegroundColor Green
Write-Host ""

# Show current user secrets (without sensitive data)
Write-Host "Current user secrets configuration:" -ForegroundColor Cyan
dotnet user-secrets list

Write-Host ""
Write-Host "Authentication Methods Available:" -ForegroundColor Green
Write-Host "  1. Client Credentials (Private API Keys) - for server-to-server applications" -ForegroundColor White
Write-Host "  2. Password Grant (Public API Keys) - for trusted applications" -ForegroundColor White
Write-Host "  3. Authorization Code Grant (Public API Keys) - for web applications" -ForegroundColor White
Write-Host ""

Write-Host "You can now run the demo with:" -ForegroundColor Green
Write-Host "  dotnet run" -ForegroundColor White
Write-Host ""

Write-Host "Note: User secrets are stored locally and are not committed to source control." -ForegroundColor Yellow
Write-Host ""

# Return to original directory
Set-Location ".."
