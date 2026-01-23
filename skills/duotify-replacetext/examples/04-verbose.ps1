# 04-verbose.ps1
# 詳細輸出模式：列出掃描檔案 + 顯示完整路徑

param(
    [Parameter(Mandatory = $true)]
    [string]$Target
)

replacetext /V /F $Target
