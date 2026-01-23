# Duotify.ReplaceText Skill - Windows 安裝腳本
# 安裝或更新 ReplaceText .NET Global Tool

param(
    [switch]$Force,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"

# 顏色輸出函式
function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

function Write-Step {
    param([string]$Message)
    Write-ColorOutput "➡️  $Message" "Cyan"
}

function Write-Success {
    param([string]$Message)
    Write-ColorOutput "✅ $Message" "Green"
}

function Write-Warning {
    param([string]$Message)
    Write-ColorOutput "⚠️  $Message" "Yellow"
}

function Write-Error {
    param([string]$Message)
    Write-ColorOutput "❌ $Message" "Red"
}

# 標題
Write-Host ""
Write-ColorOutput "═══════════════════════════════════════════════════════════" "Cyan"
Write-ColorOutput "  Duotify.ReplaceText Skill - 安裝程式" "Cyan"
Write-ColorOutput "═══════════════════════════════════════════════════════════" "Cyan"
Write-Host ""

# Step 1: 檢查 .NET SDK
Write-Step "檢查 .NET SDK..."

try {
    $dotnetVersion = dotnet --version 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet 命令執行失敗"
    }

    $majorVersion = [int]($dotnetVersion.Split('.')[0])
    if ($majorVersion -lt 8) {
        Write-Error ".NET SDK 版本過舊: $dotnetVersion"
        Write-Host "  需要 .NET 8.0 或更高版本"
        Write-Host "  請前往 https://dotnet.microsoft.com/download/dotnet/8.0 下載"
        exit 2
    }
    Write-Success ".NET SDK 版本: $dotnetVersion"
}
catch {
    Write-Error "找不到 .NET SDK"
    Write-Host "  請前往 https://dotnet.microsoft.com/download/dotnet/8.0 下載安裝"
    exit 2
}

# Step 2: 檢查現有安裝
Write-Step "檢查 ReplaceText 安裝狀態..."

$toolList = dotnet tool list -g 2>$null
$isInstalled = $toolList | Select-String -Pattern "duotify.replacetext" -Quiet

if ($isInstalled) {
    $currentVersion = ($toolList | Select-String -Pattern "duotify.replacetext").ToString().Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[1]
    Write-Warning "ReplaceText 已安裝，版本: $currentVersion"

    if ($Force) {
        Write-Step "強制更新中..."
        dotnet tool update --global Duotify.ReplaceText
        if ($LASTEXITCODE -eq 0) {
            Write-Success "更新成功！"
        }
        else {
            Write-Error "更新失敗"
            exit 1
        }
    }
    else {
        Write-Host "  使用 -Force 參數可強制更新"

        # 嘗試更新
        Write-Step "檢查是否有新版本..."
        $updateResult = dotnet tool update --global Duotify.ReplaceText 2>&1
        if ($updateResult -match "已經是最新版本" -or $updateResult -match "is already installed") {
            Write-Success "已是最新版本"
        }
        else {
            Write-Success "已更新至最新版本"
        }
    }
}
else {
    Write-Step "安裝 Duotify.ReplaceText..."
    dotnet tool install --global Duotify.ReplaceText
    if ($LASTEXITCODE -eq 0) {
        Write-Success "安裝成功！"
    }
    else {
        Write-Error "安裝失敗"
        exit 1
    }
}

# Step 3: 驗證安裝
Write-Step "驗證安裝..."

try {
    $helpOutput = replacetext --help 2>$null
    if ($LASTEXITCODE -eq 0 -or $helpOutput) {
        Write-Success "ReplaceText 可正常執行"
    }
    else {
        throw "執行失敗"
    }
}
catch {
    Write-Warning "replacetext 命令可能需要重新開啟終端機才能使用"
    Write-Host "  請關閉並重新開啟 PowerShell / Terminal"
}

# 完成
Write-Host ""
Write-ColorOutput "═══════════════════════════════════════════════════════════" "Green"
Write-ColorOutput "  安裝完成！" "Green"
Write-ColorOutput "═══════════════════════════════════════════════════════════" "Green"
Write-Host ""
Write-Host "使用方式："
Write-Host "  replacetext /path/to/project          # 轉換為 UTF-8"
Write-Host "  replacetext /T /path/to/project       # 測試模式（不修改檔案）"
Write-Host ""
Write-Host "更多資訊請參閱："
Write-Host "  https://github.com/doggy8088/Duotify.ReplaceText"
Write-Host ""

exit 0
