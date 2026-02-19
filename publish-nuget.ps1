# DailyMotion SDK NuGet Package Publisher
# This script builds and publishes the DailyMotion SDK as NuGet packages for multiple architectures

param(
    [string]$Version = "",
    [string]$ApiKey = "x",
    [string]$NuGetFeedUrl = "https://api.nuget.org/v3/index.json",
    [switch]$SkipBuild = $false,
    [switch]$SkipTest = $false,
    [switch]$SkipPublish = $false,
    [switch]$SkipCleanup = $false
)

# Set error action preference
$ErrorActionPreference = "Stop"

# Display header
Write-Host "=== DailyMotion SDK NuGet Publisher ===" -ForegroundColor Magenta
Write-Host "Version: $Version" -ForegroundColor Cyan
Write-Host "Feed: $NuGetFeedUrl" -ForegroundColor Cyan
Write-Host "Skip Build: $SkipBuild" -ForegroundColor Cyan
Write-Host "Skip Test: $SkipTest" -ForegroundColor Cyan
Write-Host "Skip Publish: $SkipPublish" -ForegroundColor Cyan
Write-Host ""

# Validate API key if publishing
if (-not $SkipPublish -and [string]::IsNullOrEmpty($ApiKey)) {
    Write-Error "API key is required for publishing. Use -ApiKey parameter or set NUGET_API_KEY environment variable."
    exit 1
}

# Get API key from environment if not provided
if ([string]::IsNullOrEmpty($ApiKey)) {
    $ApiKey = $env:NUGET_API_KEY
}

# Set version if provided
if (![string]::IsNullOrEmpty($Version)) {
    Write-Host "Setting version to: $Version" -ForegroundColor Yellow
    dotnet build DailymotionSDK/DailymotionSDK.csproj -p:Version=$Version
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to set version"
        exit 1
    }
}

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean DailymotionSDK/DailymotionSDK.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to clean project"
    exit 1
}

# Build the project
if (-not $SkipBuild) {
    Write-Host "Building project..." -ForegroundColor Yellow
    dotnet build DailymotionSDK/DailymotionSDK.csproj -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed"
        exit 1
    }
}

# Run tests
if (-not $SkipTest) {
    Write-Host "Running tests..." -ForegroundColor Yellow
    dotnet test DailymotionSDK.Tests/DailymotionSDK.Tests.csproj
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Tests failed"
        exit 1
    }
}

# Define target runtimes
$runtimes = @(
    "win-x64",
    "win-x86", 
    "win-arm64",
    "linux-x64",
    "linux-arm64",
    "osx-x64",
    "osx-arm64"
)

# Build all runtimes first to avoid version increment issues
Write-Host "Building all runtimes first..." -ForegroundColor Yellow
foreach ($runtime in $runtimes) {
    Write-Host "Building for $runtime..." -ForegroundColor Cyan
    dotnet build DailymotionSDK/DailymotionSDK.csproj -c Release -r $runtime --no-restore
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Failed to build for $runtime, skipping..."
        continue
    }
}

# Clear and recreate nupkgs directory
Write-Host "Preparing nupkgs directory..." -ForegroundColor Yellow
if (Test-Path "nupkgs") {
    Remove-Item "nupkgs" -Recurse -Force -ErrorAction SilentlyContinue
}
New-Item -ItemType Directory -Force -Path "nupkgs" | Out-Null

# Now pack all packages in one go to ensure same version
Write-Host "Creating NuGet packages for all architectures..." -ForegroundColor Yellow
dotnet pack DailymotionSDK/DailymotionSDK.csproj -c Release -o "nupkgs" --no-build
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to create NuGet packages"
    exit 1
}

# List all generated packages
$nupkgFiles = Get-ChildItem -Path "nupkgs" -Filter "*.nupkg"

if ($nupkgFiles.Count -eq 0) {
    Write-Error "No NuGet packages found in nupkgs directory"
    exit 1
}

Write-Host "Found $($nupkgFiles.Count) packages:" -ForegroundColor Green
foreach ($package in $nupkgFiles) {
    Write-Host "  - $($package.Name)" -ForegroundColor Cyan
}

# Publish all packages to NuGet feed
if (-not $SkipPublish) {
    Write-Host "Publishing all packages to NuGet feed: $NuGetFeedUrl" -ForegroundColor Yellow
    
    $successCount = 0
    $failureCount = 0
    
    foreach ($package in $nupkgFiles) {
        Write-Host "Publishing $($package.Name)..." -ForegroundColor Cyan
        
        dotnet nuget push $package.FullName --source $NuGetFeedUrl --api-key $ApiKey
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Successfully published $($package.Name)" -ForegroundColor Green
            $successCount++
        } else {
            Write-Error "✗ Failed to publish $($package.Name)"
            $failureCount++
        }
    }
    
    # Summary
    Write-Host ""
    Write-Host "Publishing Summary:" -ForegroundColor Magenta
    Write-Host "  Successfully published: $successCount" -ForegroundColor Green
    if ($failureCount -gt 0) {
        Write-Host "  Failed to publish: $failureCount" -ForegroundColor Red
    }
} else {
    Write-Host 'Skipping publish (use -SkipPublish:$false to enable publishing)' -ForegroundColor Yellow
}

# Clean up
if (-not $SkipCleanup) {
    Write-Host "Cleaning up..." -ForegroundColor Yellow
    Remove-Item "nupkgs" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "Skipping cleanup, nupkgs directory retained." -ForegroundColor Yellow
}

Write-Host "Done!" -ForegroundColor Green