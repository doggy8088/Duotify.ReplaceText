#!/usr/bin/env bash
# Duotify.ReplaceText - 安裝腳本 (Linux/macOS)
set -euo pipefail

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  Duotify.ReplaceText - 安裝程式"
echo "═══════════════════════════════════════════════════════════"
echo ""

# 檢查 .NET SDK
if ! command -v dotnet >/dev/null 2>&1; then
    echo "❌ 找不到 dotnet，請先安裝 .NET 8 SDK 以上版本。" >&2
    exit 1
fi

ver="$(dotnet --version)"
major="${ver%%.*}"
if [ "${major}" -lt 8 ]; then
    echo "❌ dotnet 版本過低：${ver}，請升級到 .NET 8 以上。" >&2
    exit 1
fi
echo "✅ .NET SDK: ${ver}"

# 安裝或更新
if dotnet tool list -g 2>/dev/null | grep -qi "Duotify\.ReplaceText"; then
    echo "🔄 更新 Duotify.ReplaceText..."
    dotnet tool update --global Duotify.ReplaceText
else
    echo "📦 安裝 Duotify.ReplaceText..."
    dotnet tool install --global Duotify.ReplaceText
fi

# 確保 PATH 包含 .NET tools
export PATH="$PATH:$HOME/.dotnet/tools"

echo ""
echo "🔍 驗證安裝..."
replacetext --help 2>/dev/null | head -5 || true

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  ✅ 安裝完成！"
echo "═══════════════════════════════════════════════════════════"
echo ""
echo "使用方式："
echo "  replacetext /path/to/project          # 轉換為 UTF-8"
echo "  replacetext /T /path/to/project       # 測試模式（不修改檔案）"
echo ""
echo "注意：若 replacetext 命令找不到，請執行："
echo "  export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
echo ""
