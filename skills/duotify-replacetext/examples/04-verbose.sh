#!/usr/bin/env bash
# 04-verbose.sh
# 詳細輸出模式：列出掃描檔案 + 顯示完整路徑

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <target-path>"
    exit 1
fi

replacetext /V /F "$1"
