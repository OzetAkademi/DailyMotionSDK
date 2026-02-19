#!/bin/bash
# DailyMotion SDK GitHub Release Creator
# Delegates build and pack to publish-nuget.sh, then creates a GitHub release with the output packages as assets

# Default parameters
VERSION=""
RELEASE_NOTES=""
SKIP_BUILD=false
SKIP_TEST=true
DRAFT=false
PRE_RELEASE=false

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --version)
            VERSION="$2"
            shift 2
            ;;
        --release-notes)
            RELEASE_NOTES="$2"
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
        --draft)
            DRAFT=true
            shift
            ;;
        --prerelease)
            PRE_RELEASE=true
            shift
            ;;
        --help)
            echo "DailyMotion SDK GitHub Release Creator"
            echo ""
            echo "Usage: $0 [options]"
            echo ""
            echo "Options:"
            echo "  --version VERSION    Version to set (default: auto-generated)"
            echo "  --release-notes TEXT Release notes (default: auto-generated)"
            echo "  --skip-build         Skip building the project"
            echo "  --skip-test          Skip running tests (default: true)"
            echo "  --draft              Create as draft release"
            echo "  --prerelease         Mark as pre-release"
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

echo "DailyMotion SDK GitHub Release Creator"
echo "========================================="
echo ""
echo "Using parameters:"
echo "  Skip Build: $SKIP_BUILD"
echo "  Skip Test: $SKIP_TEST"
echo "  Draft: $DRAFT"
echo "  Pre-Release: $PRE_RELEASE"
echo ""

# Check gh CLI is available
if ! command -v gh &> /dev/null; then
    echo "Error: GitHub CLI (gh) is not installed or not in PATH. Install from https://cli.github.com/"
    exit 1
fi

# Check gh authentication
echo "Checking GitHub CLI authentication..."
if ! gh auth status &> /dev/null; then
    echo "Error: GitHub CLI is not authenticated. Run 'gh auth login' first."
    exit 1
fi

# Build argument list to forward to publish-nuget.sh
PUBLISH_ARGS=("--skip-publish" "--skip-cleanup")
[ "$SKIP_BUILD" = true ] && PUBLISH_ARGS+=("--skip-build")
[ "$SKIP_TEST"  = true ] && PUBLISH_ARGS+=("--skip-test")
[ -n "$VERSION" ]        && PUBLISH_ARGS+=("--version" "$VERSION")

# Delegate build and pack to publish-nuget.sh
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
echo "Running publish-nuget.sh to build and pack..."
"$SCRIPT_DIR/publish-nuget.sh" "${PUBLISH_ARGS[@]}"
if [ $? -ne 0 ]; then
    echo "publish-nuget.sh failed"
    exit 1
fi

# Find packages produced by publish-nuget.sh
NUPKG_FILES=(nupkgs/*.nupkg)
if [ ! -f "${NUPKG_FILES[0]}" ]; then
    echo "No NuGet packages found in nupkgs directory"
    exit 1
fi

# Extract version from first nupkg filename (e.g. DailymotionSDK.2.2601.19.1430.nupkg)
FIRST_NUPKG=$(basename "${NUPKG_FILES[0]}" .nupkg)
RELEASE_VERSION="${FIRST_NUPKG#DailymotionSDK.}"
TAG_NAME="v$RELEASE_VERSION"

echo ""
echo "Detected version: $RELEASE_VERSION"
echo "Tag: $TAG_NAME"
echo "Found packages:"
for package in "${NUPKG_FILES[@]}"; do
    echo "  - $(basename "$package")"
done

# Build release notes
if [ -z "$RELEASE_NOTES" ]; then
    RELEASE_NOTES="Release $RELEASE_VERSION"
fi

# Build gh release create arguments
GH_ARGS=("release" "create" "$TAG_NAME" "--title" "v$RELEASE_VERSION" "--notes" "$RELEASE_NOTES")

[ "$DRAFT"       = true ] && GH_ARGS+=("--draft")
[ "$PRE_RELEASE" = true ] && GH_ARGS+=("--prerelease")

# Add all nupkg files as release assets
for package in "${NUPKG_FILES[@]}"; do
    GH_ARGS+=("$package")
done

# Create GitHub release
echo ""
echo "Creating GitHub release $TAG_NAME..."
gh "${GH_ARGS[@]}"

if [ $? -ne 0 ]; then
    echo "Failed to create GitHub release"
    exit 1
fi

echo "✓ GitHub release $TAG_NAME created successfully"

# Clean up
echo "Cleaning up..."
rm -rf nupkgs

echo ""
echo "Done!"
