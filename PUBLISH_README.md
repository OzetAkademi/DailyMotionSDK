# DailyMotion SDK Publishing Guide

This guide explains how to publish the DailyMotion SDK as NuGet packages and GitHub releases.

## Auto-Increment Version System

The project uses an auto-increment version system that generates versions at build time:
- **Format**: `2.YYMM.DD.HHMM`
- **Example**: `2.2501.15.1430` (Year 2025, January 15, 14:30)

The version is automatically calculated on each build, so you don't need to manually update version numbers.

---

## NuGet Publishing

Builds, packs, and pushes packages to a NuGet feed.

### PowerShell (Windows)
```powershell
# Use all defaults (publishes to nuget.org)
.\publish-nuget.ps1

# Custom feed and API key
.\publish-nuget.ps1 -NuGetFeedUrl "https://pkgs.dev.azure.com/org/_packaging/feed/nuget/v3/index.json" -ApiKey "your-key"

# Override version
.\publish-nuget.ps1 -Version "2.1.0"

# Run tests before publishing
.\publish-nuget.ps1 -SkipTest:$false

# Pack only, skip pushing to feed
.\publish-nuget.ps1 -SkipPublish
```

### Shell (Linux/macOS)
```bash
# Use all defaults
./publish-nuget.sh

# Custom feed and API key
./publish-nuget.sh --nuget-feed "https://pkgs.dev.azure.com/org/_packaging/feed/nuget/v3/index.json" --api-key "your-key"

# Override version
./publish-nuget.sh --version "2.1.0"

# Run tests before publishing
./publish-nuget.sh --skip-test false

# Pack only, skip pushing to feed
./publish-nuget.sh --skip-publish
```

### Batch (Windows)
```batch
REM Use all defaults
publish-nuget.bat

REM Custom feed and API key
publish-nuget.bat --nuget-feed "https://pkgs.dev.azure.com/..." --api-key "your-key"

REM Pack only, skip pushing to feed
publish-nuget.bat --skip-publish
```

### Parameters

| PowerShell | Shell / Batch | Default | Description |
|---|---|---|---|
| `-NuGetFeedUrl` | `--nuget-feed` | nuget.org | Target NuGet feed URL |
| `-ApiKey` | `--api-key` | `x` | API key for the feed |
| `-Version` | `--version` | auto | Override the build version |
| `-SkipBuild` | `--skip-build` | `false` | Skip the build step |
| `-SkipTest` | `--skip-test` | `true` | Skip running tests |
| `-SkipPublish` | `--skip-publish` | `false` | Pack only, do not push |
| `-SkipCleanup` | `--skip-cleanup` | `false` | Keep the `nupkgs/` directory |

---

## GitHub Release

Delegates build and pack to `publish-nuget.*` (with `-SkipPublish -SkipCleanup`), then creates a tagged GitHub release with the packages attached as assets.

**Prerequisite:** [GitHub CLI](https://cli.github.com/) must be installed and authenticated:
```bash
gh auth login
```

### PowerShell (Windows)
```powershell
# Simple release
.\release-github.ps1

# Draft pre-release with custom notes
.\release-github.ps1 -Draft -PreRelease -ReleaseNotes "Beta build for testing"

# Run tests before releasing
.\release-github.ps1 -SkipTest:$false

# Override version
.\release-github.ps1 -Version "2.1.0"
```

### Shell (Linux/macOS)
```bash
# Simple release
./release-github.sh

# Draft pre-release with custom notes
./release-github.sh --draft --prerelease --release-notes "Beta build for testing"

# Run tests before releasing
./release-github.sh --skip-test false

# Override version
./release-github.sh --version "2.1.0"
```

### Parameters

| PowerShell | Shell | Default | Description |
|---|---|---|---|
| `-Version` | `--version` | auto | Override the build version |
| `-ReleaseNotes` | `--release-notes` | `"Release <version>"` | Release body text |
| `-SkipBuild` | `--skip-build` | `false` | Skip the build step |
| `-SkipTest` | `--skip-test` | `true` | Skip running tests |
| `-Draft` | `--draft` | `false` | Create as draft release |
| `-PreRelease` | `--prerelease` | `false` | Mark as pre-release |

---

## Combined Workflow: NuGet + GitHub Release

To publish to NuGet **and** create a GitHub release in one go, run both scripts sequentially:

```powershell
.\publish-nuget.ps1 -ApiKey "your-key"
.\release-github.ps1
```

```bash
./publish-nuget.sh --api-key "your-key"
./release-github.sh
```

---

## Prerequisites

- .NET 10 SDK
- Access to the target NuGet feed with a valid API key (for NuGet publishing)
- [GitHub CLI](https://cli.github.com/) authenticated via `gh auth login` (for GitHub releases)

## Notes

- Tests are skipped by default to speed up the process; use `-SkipTest:$false` / `--skip-test false` to enable them
- The auto-increment version ensures unique version numbers for every build
- `release-github.*` internally calls `publish-nuget.*` with `--skip-publish --skip-cleanup`, so the same build artifacts are reused — no double build
