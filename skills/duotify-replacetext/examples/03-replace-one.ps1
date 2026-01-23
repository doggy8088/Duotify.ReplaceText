# 03-replace-one.ps1
# 替換字串並轉換編碼

param(
    [Parameter(Mandatory = $true)]
    [string]$Target,

    [Parameter(Mandatory = $true)]
    [string]$OldText,

    [Parameter(Mandatory = $true)]
    [string]$NewText
)

replacetext $Target $OldText $NewText
