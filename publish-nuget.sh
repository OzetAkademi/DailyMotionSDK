#!/bin/bash
# DailyMotion SDK NuGet Package Publisher (Shell Version)
# This script builds and publishes the DailyMotion SDK to your internal NuGet feed

# Default parameters
NUGET_FEED_URL="https://api.nuget.org/v3/index.json"
API_KEY="x"
CONFIGURATION="Release"
VERSION=""
SKIP_BUILD=false
SKIP_TEST=true
SKIP_PUBLISH=false
SKIP_CLEANUP=false
FORCE=false

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --nuget-feed)
            NUGET_FEED_URL="$2"
            shift 2
            ;;
        --api-key)
            API_KEY="$2"
            shift 2
            ;;
        --configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        --version)
            VERSION="$2"
            shift 2
            ;;
        --skip-build)
            SKIP_BUILD=true
            shift
            ;;
        --skip-test)
            SKIP_TEST=true
            shift
            ;;
        --skip-publish)
            SKIP_PUBLISH=true
            shift
            ;;
        --skip-cleanup)
            SKIP_CLEANUP=true
            shift
            ;;
        --force)
            FORCE=true
            shift
            ;;
        --help)
            echo "DailyMotion SDK NuGet Package Publisher"
            echo ""
            echo "Usage: $0 [options]"
            echo ""
            echo "Options:"
            echo "  --nuget-feed URL     NuGet feed URL (default: $NUGET_FEED_URL)"
            echo "  --api-key KEY        API key for NuGet feed (default: $API_KEY)"
            echo "  --configuration CONFIG Build configuration (default: $CONFIGURATION)"
            echo "  --version VERSION    Version to set (default: auto-increment)"
            echo "  --skip-build         Skip building the project"
            echo "  --skip-test          Skip running tests (default: true)"
            echo "  --skip-publish       Skip pushing packages to NuGet feed"
            echo "  --skip-cleanup       Skip removing the nupkgs directory after completion"
            echo "  --force              Force publish even if validation fails"
            echo "  --help               Show this help message"
            echo ""
            exit 0
            ;;
        *)
            echo "Unknown parameter: $1"
            exit 1
            ;;
    esac
done

echo "DailyMotion SDK NuGet Package Publisher"
echo "========================================="
echo ""
echo "Using parameters:"
echo "  NuGet Feed URL: $NUGET_FEED_URL"
echo "  API Key: $API_KEY"
echo "  Configuration: $CONFIGURATION"
echo "  Skip Build: $SKIP_BUILD"
echo "  Skip Test: $SKIP_TEST"
echo ""

# Set version if provided
if [ -n "$VERSION" ]; then
    echo "Setting version to: $VERSION"
    sed -i.bak "s/<VersionPrefix>.*<\/VersionPrefix>/<VersionPrefix>$VERSION<\/VersionPrefix>/" DailymotionSDK/DailymotionSDK.csproj
    rm -f DailymotionSDK/DailymotionSDK.csproj.bak
fi

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean --configuration $CONFIGURATION
if [ $? -ne 0 ]; then
    echo "Failed to clean project"
    exit 1
fi

# Restore packages
echo "Restoring packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "Failed to restore packages"
    exit 1
fi

# Build project
if [ "$SKIP_BUILD" = false ]; then
    echo "Building project..."
    dotnet build --configuration $CONFIGURATION --no-restore
    if [ $? -ne 0 ]; then
        echo "Failed to build project"
        exit 1
    fi
fi

# Run tests (if any) - Skipped by default for faster publishing
if [ "$SKIP_TEST" = false ]; then
    echo "Running tests..."
    dotnet test --configuration $CONFIGURATION --no-build
    if [ $? -ne 0 ]; then
        echo "Tests failed"
        if [ "$FORCE" = false ]; then
            exit 1
        fi
    fi
else
    echo "Skipping tests for faster publishing..."
    echo "Use --skip-test=false to run tests if needed"
fi

# Pack the project for multiple architectures
echo "Creating NuGet packages for multiple architectures..."

# Define target runtimes
RUNTIMES=("win-x64" "win-x86" "win-arm64" "linux-x64" "linux-arm64" "osx-x64" "osx-arm64")

# Create packages for each runtime
for runtime in "${RUNTIMES[@]}"; do
    echo "Packing for $runtime..."
    
    # Build for specific runtime
    dotnet build "DailymotionSDK/DailymotionSDK.csproj" --configuration $CONFIGURATION --runtime $runtime --no-restore
    if [ $? -ne 0 ]; then
        echo "Warning: Failed to build for $runtime, skipping..."
        continue
    fi
    
    # Pack for specific runtime
    dotnet pack "DailymotionSDK/DailymotionSDK.csproj" --configuration $CONFIGURATION --runtime $runtime --no-build --output "nupkgs"
    if [ $? -ne 0 ]; then
        echo "Warning: Failed to pack for $runtime, skipping..."
        continue
    fi
done

# Also create a framework-dependent package (no specific runtime)
echo "Creating framework-dependent package..."
dotnet pack "DailymotionSDK/DailymotionSDK.csproj" --configuration $CONFIGURATION --no-build --output "nupkgs"
if [ $? -ne 0 ]; then
    echo "Failed to create framework-dependent NuGet package"
    exit 1
fi

# Find all created packages
echo ""
echo "Found packages:"
for package in nupkgs/*.nupkg; do
    if [ -f "$package" ]; then
        echo "  - $(basename "$package")"
    fi
done

# Publish all packages to NuGet feed
if [ "$SKIP_PUBLISH" = false ]; then
    echo ""
    echo "Publishing all packages to NuGet feed: $NUGET_FEED_URL"

    SUCCESS_COUNT=0
    FAILURE_COUNT=0

    for package in nupkgs/*.nupkg; do
        if [ -f "$package" ]; then
            echo "Publishing $(basename "$package")..."

            dotnet nuget push "$package" --source "$NUGET_FEED_URL" --api-key "$API_KEY"

            if [ $? -eq 0 ]; then
                echo "✓ Successfully published $(basename "$package")"
                SUCCESS_COUNT=$((SUCCESS_COUNT + 1))
            else
                echo "✗ Failed to publish $(basename "$package")"
                FAILURE_COUNT=$((FAILURE_COUNT + 1))
            fi
        fi
    done

    echo ""
    echo "Publishing completed!"
    echo "Successfully published: $SUCCESS_COUNT packages"
    if [ $FAILURE_COUNT -gt 0 ]; then
        echo "Failed to publish: $FAILURE_COUNT packages"
    fi
    echo "Feed: $NUGET_FEED_URL"
else
    echo ""
    echo "Skipping publish (use --skip-publish to enable publishing)"
fi

# Clean up
if [ "$SKIP_CLEANUP" = false ]; then
    echo ""
    echo "Cleaning up..."
    rm -rf nupkgs
else
    echo ""
    echo "Skipping cleanup, nupkgs directory retained."
fi

echo ""
echo "Done!"