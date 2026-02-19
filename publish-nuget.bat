@echo off
REM DailyMotion SDK NuGet Package Publisher (Batch Version)
REM This script builds and publishes the DailyMotion SDK to your internal NuGet feed

setlocal enabledelayedexpansion

REM Default parameters
set "NuGetFeedUrl=https://api.nuget.org/v3/index.json"
set "ApiKey=x"
set "Configuration=Release"
set "Version="
set "SkipBuild=false"
set "SkipTest=true"
set "SkipPublish=false"
set "SkipCleanup=false"
set "Force=false"

REM Parse command line arguments
:parse_args
if "%~1"=="" goto :args_done
if "%~1"=="--nuget-feed" (
    set "NuGetFeedUrl=%~2"
    shift
    shift
    goto :parse_args
)
if "%~1"=="--api-key" (
    set "ApiKey=%~2"
    shift
    shift
    goto :parse_args
)
if "%~1"=="--configuration" (
    set "Configuration=%~2"
    shift
    shift
    goto :parse_args
)
if "%~1"=="--version" (
    set "Version=%~2"
    shift
    shift
    goto :parse_args
)
if "%~1"=="--skip-build" (
    set "SkipBuild=true"
    shift
    goto :parse_args
)
if "%~1"=="--skip-test" (
    set "SkipTest=true"
    shift
    goto :parse_args
)
if "%~1"=="--skip-publish" (
    set "SkipPublish=true"
    shift
    goto :parse_args
)
if "%~1"=="--skip-cleanup" (
    set "SkipCleanup=true"
    shift
    goto :parse_args
)
if "%~1"=="--force" (
    set "Force=true"
    shift
    goto :parse_args
)
if "%~1"=="--help" (
    echo DailyMotion SDK NuGet Package Publisher
    echo.
    echo Usage: publish-nuget.bat [options]
    echo.
    echo Options:
    echo   --nuget-feed URL     NuGet feed URL (default: %NuGetFeedUrl%)
    echo   --api-key KEY        API key for NuGet feed (default: %ApiKey%)
    echo   --configuration CONFIG Build configuration (default: %Configuration%)
    echo   --version VERSION    Version to set (default: auto-increment)
    echo   --skip-build         Skip building the project
    echo   --skip-test          Skip running tests (default: true)
    echo   --skip-publish       Skip pushing packages to NuGet feed
    echo   --skip-cleanup       Skip removing the nupkgs directory after completion
    echo   --force              Force publish even if validation fails
    echo   --help               Show this help message
    echo.
    exit /b 0
)
echo Unknown parameter: %~1
exit /b 1

:args_done

echo DailyMotion SDK NuGet Package Publisher
echo =========================================
echo.
echo Using parameters:
echo   NuGet Feed URL: %NuGetFeedUrl%
echo   API Key: %ApiKey%
echo   Configuration: %Configuration%
echo   Skip Build: %SkipBuild%
echo   Skip Test: %SkipTest%
echo.

REM Set version if provided
if not "%Version%"=="" (
    echo Setting version to: %Version%
    powershell -Command "(Get-Content 'DailymotionSDK\DailymotionSDK.csproj') -replace '<VersionPrefix>.*?</VersionPrefix>', '<VersionPrefix>%Version%</VersionPrefix>' | Set-Content 'DailymotionSDK\DailymotionSDK.csproj'"
)

REM Clean previous builds
echo Cleaning previous builds...
dotnet clean --configuration %Configuration%
if %ERRORLEVEL% neq 0 (
    echo Failed to clean project
    exit /b 1
)

REM Restore packages
echo Restoring packages...
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo Failed to restore packages
    exit /b 1
)

REM Build project
if "%SkipBuild%"=="false" (
    echo Building project...
    dotnet build --configuration %Configuration% --no-restore
    if %ERRORLEVEL% neq 0 (
        echo Failed to build project
        exit /b 1
    )
)

REM Run tests (if any) - Skipped by default for faster publishing
if "%SkipTest%"=="false" (
    echo Running tests...
    dotnet test --configuration %Configuration% --no-build
    if %ERRORLEVEL% neq 0 (
        echo Tests failed
        if "%Force%"=="false" (
            exit /b 1
        )
    )
) else (
    echo Skipping tests for faster publishing...
    echo Use --skip-test:false to run tests if needed
)

REM Create nupkgs directory
if not exist "nupkgs" mkdir "nupkgs"

REM Define target runtimes
set "runtimes=win-x64 win-x86 win-arm64 linux-x64 linux-arm64 osx-x64 osx-arm64"

REM Create packages for each runtime
echo Creating NuGet packages for each architecture...
for %%r in (%runtimes%) do (
    echo Creating package for %%r...
    
    REM Pack for specific runtime
    dotnet pack "DailymotionSDK\DailymotionSDK.csproj" --configuration %Configuration% --runtime %%r --no-build --output "nupkgs"
    if %ERRORLEVEL% neq 0 (
        echo Warning: Failed to pack for %%r, skipping...
        continue
    )
)

REM Also create a framework-dependent package (no specific runtime)
echo Creating framework-dependent package...
dotnet pack "DailymotionSDK\DailymotionSDK.csproj" --configuration %Configuration% --no-build --output "nupkgs"
if %ERRORLEVEL% neq 0 (
    echo Failed to create framework-dependent NuGet package
    exit /b 1
)

REM Find all created packages
echo.
echo Found packages:
for %%f in (nupkgs\*.nupkg) do (
    echo   - %%~nxf
)

REM Publish all packages to NuGet feed
if "%SkipPublish%"=="false" (
    echo.
    echo Publishing all packages to NuGet feed: %NuGetFeedUrl%
    
    set "successCount=0"
    set "failureCount=0"
    
    for %%f in (nupkgs\*.nupkg) do (
        echo Publishing %%~nxf...
        
        dotnet nuget push "%%f" --source "%NuGetFeedUrl%" --api-key "%ApiKey%"
        
        if %ERRORLEVEL% equ 0 (
            echo ✓ Successfully published %%~nxf
            set /a successCount+=1
        ) else (
            echo ✗ Failed to publish %%~nxf
            set /a failureCount+=1
        )
    )
    
    echo.
    echo Publishing completed!
    echo Successfully published: %successCount% packages
    if %failureCount% gtr 0 (
        echo Failed to publish: %failureCount% packages
    )
    echo Feed: %NuGetFeedUrl%
) else (
    echo.
    echo Skipping publish (use --skip-publish:false to enable publishing)
)

REM Clean up
if "%SkipCleanup%"=="false" (
    echo.
    echo Cleaning up...
    if exist "nupkgs" rmdir /s /q "nupkgs"
) else (
    echo.
    echo Skipping cleanup, nupkgs directory retained.
)

echo.
echo Done!

endlocal