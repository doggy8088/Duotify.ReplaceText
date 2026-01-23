#!/bin/bash
# Duotify.ReplaceText Skill - Linux/macOS 安裝腳本
# 安裝或更新 ReplaceText .NET Global Tool

set -e

# 顏色定義
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# 輸出函式
step() {
    echo -e "${CYAN}➡️  $1${NC}"
}

success() {
    echo -e "${GREEN}✅ $1${NC}"
}

warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

error() {
    echo -e "${RED}❌ $1${NC}"
}

# 解析參數
FORCE=false
while [[ $# -gt 0 ]]; do
    case $1 in
        -f|--force)
            FORCE=true
            shift
            ;;
        *)
            shift
            ;;
    esac
done

# 標題
echo ""
echo -e "${CYAN}═══════════════════════════════════════════════════════════${NC}"
echo -e "${CYAN}  Duotify.ReplaceText Skill - 安裝程式${NC}"
echo -e "${CYAN}═══════════════════════════════════════════════════════════${NC}"
echo ""

# Step 1: 檢查 .NET SDK
step "檢查 .NET SDK..."

if ! command -v dotnet &> /dev/null; then
    error "找不到 .NET SDK"
    echo "  請前往 https://dotnet.microsoft.com/download/dotnet/8.0 下載安裝"
    echo ""
    echo "  快速安裝（Linux/macOS）："
    echo "    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest"
    exit 2
fi

DOTNET_VERSION=$(dotnet --version 2>/dev/null)
MAJOR_VERSION=$(echo "$DOTNET_VERSION" | cut -d. -f1)

if [ "$MAJOR_VERSION" -lt 8 ]; then
    error ".NET SDK 版本過舊: $DOTNET_VERSION"
    echo "  需要 .NET 8.0 或更高版本"
    echo "  請前往 https://dotnet.microsoft.com/download/dotnet/8.0 下載"
    exit 2
fi

success ".NET SDK 版本: $DOTNET_VERSION"

# Step 2: 檢查現有安裝
step "檢查 ReplaceText 安裝狀態..."

TOOL_LIST=$(dotnet tool list -g 2>/dev/null || echo "")

if echo "$TOOL_LIST" | grep -qi "duotify.replacetext"; then
    CURRENT_VERSION=$(echo "$TOOL_LIST" | grep -i "duotify.replacetext" | awk '{print $2}')
    warning "ReplaceText 已安裝，版本: $CURRENT_VERSION"

    if [ "$FORCE" = true ]; then
        step "強制更新中..."
        if dotnet tool update --global Duotify.ReplaceText; then
            success "更新成功！"
        else
            error "更新失敗"
            exit 1
        fi
    else
        echo "  使用 -f 或 --force 參數可強制更新"

        step "檢查是否有新版本..."
        UPDATE_RESULT=$(dotnet tool update --global Duotify.ReplaceText 2>&1)

        if echo "$UPDATE_RESULT" | grep -qi "already installed\|已經是最新"; then
            success "已是最新版本"
        else
            success "已更新至最新版本"
        fi
    fi
else
    step "安裝 Duotify.ReplaceText..."
    if dotnet tool install --global Duotify.ReplaceText; then
        success "安裝成功！"
    else
        error "安裝失敗"
        exit 1
    fi
fi

# Step 3: 確保 PATH 包含 .NET tools
DOTNET_TOOLS_PATH="$HOME/.dotnet/tools"
if [[ ":$PATH:" != *":$DOTNET_TOOLS_PATH:"* ]]; then
    warning ".NET tools 路徑可能不在 PATH 中"
    echo "  請將以下內容加入您的 shell 設定檔 (~/.bashrc, ~/.zshrc 等)："
    echo ""
    echo "    export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
    echo ""
fi

# Step 4: 驗證安裝
step "驗證安裝..."

# 確保可以找到 replacetext
export PATH="$PATH:$HOME/.dotnet/tools"

if command -v replacetext &> /dev/null; then
    if replacetext --help &> /dev/null; then
        success "ReplaceText 可正常執行"
    else
        warning "replacetext 命令可能需要重新開啟終端機才能使用"
    fi
else
    warning "replacetext 命令可能需要重新開啟終端機才能使用"
    echo "  或執行: export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
fi

# 完成
echo ""
echo -e "${GREEN}═══════════════════════════════════════════════════════════${NC}"
echo -e "${GREEN}  安裝完成！${NC}"
echo -e "${GREEN}═══════════════════════════════════════════════════════════${NC}"
echo ""
echo "使用方式："
echo "  replacetext /path/to/project          # 轉換為 UTF-8"
echo "  replacetext /T /path/to/project       # 測試模式（不修改檔案）"
echo ""
echo "更多資訊請參閱："
echo "  https://github.com/doggy8088/Duotify.ReplaceText"
echo ""

exit 0
