#!/usr/bin/env bash
# 03-replace-one.sh
# 替換字串並轉換編碼

set -euo pipefail

if [ $# -lt 3 ]; then
    echo "Usage: $0 <target-path> <old-text> <new-text>"
    exit 1
fi

replacetext "$1" "$2" "$3"
