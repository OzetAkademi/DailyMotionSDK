# DailyMotion SDK Demo - User Secrets Test Script
# This script tests if user secrets are properly configured

Write-Host "=== DailyMotion SDK Demo - User Secrets Test ===" -ForegroundColor Green
Write-Host ""

# Check if we're in the right directory
if (-not (Test-Path "DailymotionSDK.Demo\DailymotionSDK.Demo.csproj")) {
    Write-Host "Error: Please run this script from the root directory of the DailyMotion SDK project" -ForegroundColor Red
    exit 1
}

# Navigate to the demo project directory
Set-Location "DailymotionSDK.Demo"

Write-Host "Testing user secrets configuration..." -ForegroundColor Yellow
Write-Host ""

# List current user secrets
Write-Host "Current user secrets:" -ForegroundColor Cyan
dotnet user-secrets list

Write-Host ""
Write-Host "Testing configuration loading..." -ForegroundColor Yellow

# Check for required secrets
$secrets = dotnet user-secrets list --json | ConvertFrom-Json

Write-Host ""
Write-Host "Configuration Status:" -ForegroundColor Cyan

# Check SDK configuration
if ($secrets."DailymotionOptions:ClientId" -and $secrets."DailymotionOptions:ClientSecret") {
    Write-Host "✅ SDK Configuration: Client ID and Secret configured" -ForegroundColor Green
} else {
    Write-Host "❌ SDK Configuration: Missing Client ID or Secret" -ForegroundColor Red
}

# Check authentication methods
$authMethods = @()

# Client Credentials uses separate Private API Key/Secret
if ($secrets."Demo:ClientCredentialsApiKey" -and $secrets."Demo:ClientCredentialsApiSecret") {
    Write-Host "✅ Client Credentials: Private API Key and Secret configured" -ForegroundColor Green
    $authMethods += "Client Credentials"
} else {
    Write-Host "⚠️ Client Credentials: Not configured (optional)" -ForegroundColor Yellow
}

if ($secrets."Demo:TestUsername" -and $secrets."Demo:TestPassword") {
    Write-Host "✅ Password Grant: Username and Password configured" -ForegroundColor Green
    $authMethods += "Password Grant"
} else {
    Write-Host "⚠️ Password Grant: Not configured (optional)" -ForegroundColor Yellow
}

if ($secrets."Demo:AuthorizationCodeRedirectUri") {
    Write-Host "✅ Authorization Code: Redirect URI configured" -ForegroundColor Green
    $authMethods += "Authorization Code"
} else {
    Write-Host "⚠️ Authorization Code: Redirect URI not configured (optional)" -ForegroundColor Yellow
}

Write-Host ""
if ($authMethods.Count -gt 0) {
    Write-Host "Available Authentication Methods:" -ForegroundColor Green
    $authMethods | ForEach-Object { Write-Host "  • $_" -ForegroundColor White }
} else {
    Write-Host "⚠️ No authentication methods configured" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "To set up user secrets, run:" -ForegroundColor Green
Write-Host "  ..\setup-user-secrets.ps1" -ForegroundColor White
Write-Host ""

# Return to original directory
Set-Location ".."
