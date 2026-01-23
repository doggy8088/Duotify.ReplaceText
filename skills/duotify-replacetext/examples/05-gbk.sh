#!/usr/bin/env bash
# 05-gbk.sh
# GBK 優先模式：適用於簡體中文檔案

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <target-path>"
    exit 1
fi

replacetext /GBK "$1"
