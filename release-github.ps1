# DailyMotion SDK GitHub Release Creator
# Delegates build and pack to publish-nuget.ps1, then creates a GitHub release with the output packages as assets

param(
    [string]$Version = "",
    [string]$ReleaseNotes = "",
    [switch]$SkipBuild = $false,
    [switch]$SkipTest = $false,
    [switch]$Draft = $false,
    [switch]$PreRelease = $false
)

# Set error action preference
$ErrorActionPreference = "Stop"

# Display header
Write-Host "=== DailyMotion SDK GitHub Release Creator ===" -ForegroundColor Magenta
Write-Host "Skip Build: $SkipBuild" -ForegroundColor Cyan
Write-Host "Skip Test: $SkipTest" -ForegroundColor Cyan
Write-Host "Draft: $Draft" -ForegroundColor Cyan
Write-Host "Pre-Release: $PreRelease" -ForegroundColor Cyan
Write-Host ""

# Check gh CLI is available
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Error "GitHub CLI (gh) is not installed or not in PATH. Install from https://cli.github.com/"
    exit 1
}

# Check gh authentication
Write-Host "Checking GitHub CLI authentication..." -ForegroundColor Yellow
gh auth status 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Error "GitHub CLI is not authenticated. Run 'gh auth login' first."
    exit 1
}

# Build argument hashtable to forward to publish-nuget.ps1 (hashtable splatting binds switches by name)
$publishArgs = @{
    SkipPublish = $true
    SkipCleanup = $true
    SkipBuild   = [bool]$SkipBuild
    SkipTest    = [bool]$SkipTest
}
if (![string]::IsNullOrEmpty($Version)) { $publishArgs['Version'] = $Version }

# Delegate build and pack to publish-nuget.ps1
Write-Host "Running publish-nuget.ps1 to build and pack..." -ForegroundColor Yellow
& "$PSScriptRoot\publish-nuget.ps1" @publishArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "publish-nuget.ps1 failed"
    exit 1
}

# Find packages produced by publish-nuget.ps1
$nupkgFiles = Get-ChildItem -Path "nupkgs" -Filter "*.nupkg" -ErrorAction SilentlyContinue

if ($null -eq $nupkgFiles -or $nupkgFiles.Count -eq 0) {
    Write-Error "No NuGet packages found in nupkgs directory"
    exit 1
}

# Extract version from first nupkg filename (e.g. DailymotionSDK.2.2601.19.1430.nupkg)
$releaseVersion = $nupkgFiles[0].BaseName -replace "^DailymotionSDK\.", ""
$tagName = "v$releaseVersion"

Write-Host ""
Write-Host "Detected version: $releaseVersion" -ForegroundColor Green
Write-Host "Tag: $tagName" -ForegroundColor Green
Write-Host "Found $($nupkgFiles.Count) package(s):" -ForegroundColor Green
foreach ($package in $nupkgFiles) {
    Write-Host "  - $($package.Name)" -ForegroundColor Cyan
}

# Build release notes
if ([string]::IsNullOrEmpty($ReleaseNotes)) {
    $ReleaseNotes = "Release $releaseVersion"
}

# Build gh release create arguments
$ghArgs = @(
    "release", "create", $tagName,
    "--title", "v$releaseVersion",
    "--notes", $ReleaseNotes
)

if ($Draft)      { $ghArgs += "--draft" }
if ($PreRelease) { $ghArgs += "--prerelease" }

# Add all nupkg files as release assets
foreach ($package in $nupkgFiles) {
    $ghArgs += $package.FullName
}

# Create GitHub release
Write-Host ""
Write-Host "Creating GitHub release $tagName..." -ForegroundColor Yellow
& gh @ghArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to create GitHub release"
    exit 1
}

Write-Host "OK GitHub release $tagName created successfully" -ForegroundColor Green

# Clean up
Write-Host "Cleaning up..." -ForegroundColor Yellow
Remove-Item "nupkgs" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "Done!" -ForegroundColor Green
