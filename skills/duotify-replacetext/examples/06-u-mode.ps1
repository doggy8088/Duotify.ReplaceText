# 06-u-mode.ps1
# 自動判斷未知副檔名或無副檔名檔案

param(
    [Parameter(Mandatory = $true)]
    [string]$Target
)

replacetext /U $Target
