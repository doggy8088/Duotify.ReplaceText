# 01-convert-only.ps1
# 轉換目錄中所有檔案為 UTF-8 with BOM（預設行為）

param(
    [Parameter(Mandatory = $true)]
    [string]$Target
)

replacetext $Target
