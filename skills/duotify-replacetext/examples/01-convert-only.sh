#!/usr/bin/env bash
# 01-convert-only.sh
# 轉換目錄中所有檔案為 UTF-8 with BOM（預設行為）

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <target-path>"
    exit 1
fi

replacetext "$1"
