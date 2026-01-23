# 10-batch-mapping.ps1
# 批次 Mapping 替換：從 JSON 規則檔執行多組字串替換
#
# 用法：
#   pwsh ./10-batch-mapping.ps1 -MappingJson ./mapping.sample.json
#
# 規則檔格式請參考 mapping.sample.json

param(
    [Parameter(Mandatory = $true)]
    [string]$MappingJson
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $MappingJson)) {
    Write-Error "找不到規則檔：$MappingJson"
    exit 1
}

$cfg = Get-Content $MappingJson -Raw | ConvertFrom-Json
$target = $cfg.target
$flags = @($cfg.flags)

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "  批次 Mapping 替換"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host ""
Write-Host "📁 目標: $target"
Write-Host "🏷️  旗標: $($flags -join ' ')"
Write-Host "📋 規則數: $($cfg.replacements.Count)"
Write-Host ""

$count = 0
$total = $cfg.replacements.Count

foreach ($r in $cfg.replacements) {
    $count++
    $old = $r.old
    $new = $r.new

    Write-Host "[$count/$total] `"$old`" → `"$new`""
    Write-Host "  ➡️  replacetext $($flags -join ' ') $target `"$old`" `"$new`""

    & replacetext @flags $target $old $new

    if ($LASTEXITCODE -ne 0) {
        Write-Warning "  ⚠️  執行失敗 (exit code: $LASTEXITCODE)"
    }
    else {
        Write-Host "  ✅ 完成"
    }
    Write-Host ""
}

Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "  ✅ 批次替換完成！共 $total 組規則"
Write-Host "═══════════════════════════════════════════════════════════"
