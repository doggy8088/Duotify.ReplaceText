#!/usr/bin/env bash
# 02-dryrun.sh
# Dry Run 模式：預覽變更但不修改檔案

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <target-path>"
    exit 1
fi

replacetext /T "$1"
