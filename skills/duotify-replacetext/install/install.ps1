#requires -Version 5.1
# Duotify.ReplaceText - 安裝腳本 (Windows PowerShell)
$ErrorActionPreference = "Stop"

function Require-DotNet8 {
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw "找不到 dotnet，請先安裝 .NET 8 SDK 以上版本。"
    }
    $ver = (& dotnet --version).Trim()
    $major = [int]($ver.Split('.')[0])
    if ($major -lt 8) {
        throw "dotnet 版本過低：$ver，請升級到 .NET 8 以上。"
    }
    Write-Host "✅ .NET SDK: $ver"
}

function Install-Or-Update {
    $list = & dotnet tool list -g 2>$null
    if ($list -match "Duotify\.ReplaceText") {
        Write-Host "🔄 更新 Duotify.ReplaceText..."
        & dotnet tool update --global Duotify.ReplaceText
    }
    else {
        Write-Host "📦 安裝 Duotify.ReplaceText..."
        & dotnet tool install --global Duotify.ReplaceText
    }
}

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "  Duotify.ReplaceText - 安裝程式"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host ""

Require-DotNet8
Install-Or-Update

Write-Host ""
Write-Host "🔍 驗證安裝..."
& replacetext --help | Select-Object -First 5 | Out-Host

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "  ✅ 安裝完成！"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host ""
Write-Host "使用方式："
Write-Host "  replacetext /path/to/project          # 轉換為 UTF-8"
Write-Host "  replacetext /T /path/to/project       # 測試模式（不修改檔案）"
Write-Host ""
