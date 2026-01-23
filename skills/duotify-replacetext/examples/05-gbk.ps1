# 05-gbk.ps1
# GBK 優先模式：適用於簡體中文檔案

param(
    [Parameter(Mandatory = $true)]
    [string]$Target
)

replacetext /GBK $Target
