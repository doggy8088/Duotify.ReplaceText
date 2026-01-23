#!/usr/bin/env bash
# 06-u-mode.sh
# 自動判斷未知副檔名或無副檔名檔案

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <target-path>"
    exit 1
fi

replacetext /U "$1"
