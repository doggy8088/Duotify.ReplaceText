# 02-dryrun.ps1
# Dry Run 模式：預覽變更但不修改檔案

param(
    [Parameter(Mandatory = $true)]
    [string]$Target
)

replacetext /T $Target
