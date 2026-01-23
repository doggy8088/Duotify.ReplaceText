#!/usr/bin/env bash
# 10-batch-mapping.sh
# 批次 Mapping 替換：從 JSON 規則檔執行多組字串替換
#
# 用法：
#   bash ./10-batch-mapping.sh ./mapping.sample.json
#
# 需要 jq：sudo apt install jq 或 brew install jq
# 規則檔格式請參考 mapping.sample.json

set -euo pipefail

if [ $# -lt 1 ]; then
    echo "Usage: $0 <mapping-json>"
    exit 1
fi

MAPPING_JSON="$1"

if [ ! -f "$MAPPING_JSON" ]; then
    echo "❌ 找不到規則檔：$MAPPING_JSON" >&2
    exit 1
fi

if ! command -v jq >/dev/null 2>&1; then
    echo "❌ 需要 jq 來解析 JSON。請安裝：sudo apt install jq 或 brew install jq" >&2
    exit 1
fi

TARGET=$(jq -r '.target' "$MAPPING_JSON")
FLAGS=$(jq -r '.flags | join(" ")' "$MAPPING_JSON")
TOTAL=$(jq '.replacements | length' "$MAPPING_JSON")

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  批次 Mapping 替換"
echo "═══════════════════════════════════════════════════════════"
echo ""
echo "📁 目標: $TARGET"
echo "🏷️  旗標: $FLAGS"
echo "📋 規則數: $TOTAL"
echo ""

for i in $(seq 0 $((TOTAL - 1))); do
    OLD=$(jq -r ".replacements[$i].old" "$MAPPING_JSON")
    NEW=$(jq -r ".replacements[$i].new" "$MAPPING_JSON")
    COUNT=$((i + 1))

    echo "[$COUNT/$TOTAL] \"$OLD\" → \"$NEW\""
    echo "  ➡️  replacetext $FLAGS $TARGET \"$OLD\" \"$NEW\""

    # shellcheck disable=SC2086
    if replacetext $FLAGS "$TARGET" "$OLD" "$NEW"; then
        echo "  ✅ 完成"
    else
        echo "  ⚠️  執行失敗"
    fi
    echo ""
done

echo "═══════════════════════════════════════════════════════════"
echo "  ✅ 批次替換完成！共 $TOTAL 組規則"
echo "═══════════════════════════════════════════════════════════"
