#!/usr/bin/env pwsh
# Build script for ReplaceText project

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [Parameter(Mandatory=$false)]
    [switch]$Clean,

    [Parameter(Mandatory=$false)]
    [switch]$Publish,

    [Parameter(Mandatory=$false)]
    [switch]$Format,

    [Parameter(Mandatory=$false)]
    [switch]$Verify
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

Write-Host "ReplaceText Build Script" -ForegroundColor Cyan
Write-Host "========================" -ForegroundColor Cyan
Write-Host ""

# Clean
if ($Clean) {
    Write-Host "Cleaning..." -ForegroundColor Yellow
    dotnet clean -c $Configuration
    if (Test-Path "publish") {
        Remove-Item -Recurse -Force "publish"
    }
    Write-Host "Clean complete!" -ForegroundColor Green
    Write-Host ""
}

# Format
if ($Format) {
    Write-Host "Formatting code..." -ForegroundColor Yellow
    Push-Location ReplaceText
    dotnet format
    Pop-Location
    Write-Host "Format complete!" -ForegroundColor Green
    Write-Host ""
}

# Verify format
if ($Verify) {
    Write-Host "Verifying code format..." -ForegroundColor Yellow
    Push-Location ReplaceText
    dotnet format --verify-no-changes
    Pop-Location
    Write-Host "Verification complete!" -ForegroundColor Green
    Write-Host ""
}

# Restore
Write-Host "Restoring dependencies..." -ForegroundColor Yellow
dotnet restore
Write-Host "Restore complete!" -ForegroundColor Green
Write-Host ""

# Build
Write-Host "Building ($Configuration)..." -ForegroundColor Yellow
dotnet build -c $Configuration --no-restore
Write-Host "Build complete!" -ForegroundColor Green
Write-Host ""

# Publish
if ($Publish) {
    Write-Host "Publishing binaries..." -ForegroundColor Yellow

    $runtimes = @(
        @{Name="win-x64"; DisplayName="Windows x64"},
        @{Name="win-x86"; DisplayName="Windows x86"},
        @{Name="linux-x64"; DisplayName="Linux x64"},
        @{Name="osx-x64"; DisplayName="macOS x64"},
        @{Name="osx-arm64"; DisplayName="macOS ARM64"}
    )

    foreach ($runtime in $runtimes) {
        Write-Host "  Publishing $($runtime.DisplayName)..." -ForegroundColor Cyan
        dotnet publish ReplaceText/ReplaceText.csproj `
            -c $Configuration `
            -r $runtime.Name `
            --self-contained `
            -p:PublishSingleFile=true `
            -p:IncludeNativeLibrariesForSelfExtract=true `
            -o "publish/$($runtime.Name)"
    }

    Write-Host "Publish complete!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Published files are in the 'publish' directory" -ForegroundColor Cyan
}

Write-Host "All done! ✅" -ForegroundColor Green
