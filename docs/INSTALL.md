# Duotify.ReplaceText 安裝指南

ReplaceText 現在可以作為 .NET Global Tool 安裝，讓您可以在任何地方使用 `replacetext` 命令。

## 前置需求

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 或更高版本

## 安裝方式

### 方式 1：從 NuGet Gallery 安裝 (推薦)

```bash
dotnet tool install --global Duotify.ReplaceText
```

### 方式 2：從本機 NuGet 套件安裝

如果您有本機建構的 NuGet 套件檔案：

```bash
dotnet tool install --global Duotify.ReplaceText --add-source ./nupkg
```

### 方式 3：從原始碼建構並安裝

```bash
# 1. 複製專案
git clone https://github.com/doggy8088/Duotify.ReplaceText.git
cd ReplaceText

# 2. 建構並打包
dotnet pack -c Release

# 3. 安裝
dotnet tool install --global Duotify.ReplaceText --add-source ./ReplaceText/nupkg
```

## 驗證安裝

安裝完成後，您可以在任何目錄執行以下命令來驗證安裝：

```bash
replacetext
```

您應該會看到說明訊息顯示。

## 基本使用

### 轉換目錄中所有檔案為 UTF-8

```bash
replacetext /path/to/your/project
```

### 測試模式 (不實際修改檔案)

```bash
replacetext /T /path/to/your/project
```

### 替換文字並轉換編碼

```bash
replacetext /path/to/your/project "舊文字" "新文字"
```

### 詳細輸出模式

```bash
replacetext /V /F /path/to/your/project
```

### Windows 特定範例

```powershell
# 轉換整個專案
replacetext C:\Projects\MyProject

# 包含文字檔案
replacetext /M C:\Projects\MyProject

# GBK 優先模式(處理簡體中文)
replacetext /GBK C:\Projects\MyProject
```

### Linux/macOS 範例

```bash
# 轉換整個專案
replacetext ~/projects/myproject

# 包含文字檔案
replacetext -m ~/projects/myproject

# 自動判斷未知檔案類型
replacetext -u ~/projects/myproject
```

## 命令列選項

| 選項             | 說明                                               |
| ---------------- | -------------------------------------------------- |
| `/T` 或 `-t`     | 測試執行模式，不會寫入檔案 (Dry Run)               |
| `/M` 或 `-m`     | 修改已知的文字檔案 (預設會跳過 .txt、.md 等資料檔) |
| `/V` 或 `-v`     | 顯示詳細輸出模式，會顯示所有掃描的檔案清單         |
| `/F` 或 `-f`     | 顯示完整的檔案路徑 (預設僅顯示相對路徑)            |
| `/GBK` 或 `-gbk` | 讓 GBK (GB18030) 字集優先於 Big5 判斷              |
| `/U` 或 `-u`     | 自動判斷未知檔案類型 (預設僅處理已知的檔案類型)    |

## 更新工具

```bash
dotnet tool update --global Duotify.ReplaceText
```

## 解除安裝

```bash
dotnet tool uninstall --global Duotify.ReplaceText
```

## 查看已安裝的全域工具

```bash
dotnet tool list --global
```

## 常見問題

### Q: 為什麼執行 `replacetext` 命令時找不到命令？

**A**: 請確認以下幾點：

1. 已安裝 .NET SDK 8.0 或更高版本
2. .NET 工具路徑已加入到系統 PATH 環境變數中

預設工具路徑：

- Windows: `%USERPROFILE%\.dotnet\tools`
- Linux/macOS: `$HOME/.dotnet/tools`

### Q: 如何查看目前安裝的版本？

**A**: 使用以下命令：

```bash
dotnet tool list --global | findstr ReplaceText    # Windows
dotnet tool list --global | grep ReplaceText       # Linux/macOS
```

### Q: 可以安裝特定版本嗎？

**A**: 可以，使用 `--version` 參數：

```bash
dotnet tool install --global Duotify.ReplaceText --version 1.0.0
```

### Q: 安裝失敗怎麼辦？

**A**: 常見解決方法：

1. 確認 .NET SDK 版本：`dotnet --version`
2. 清除 NuGet 快取：`dotnet nuget locals all --clear`
3. 使用 `--verbosity detailed` 查看詳細錯誤訊息：

   ```bash
   dotnet tool install --global Duotify.ReplaceText --verbosity detailed
   ```

## 開發者資訊

### 本機開發與測試

如果您正在開發或修改 ReplaceText：

```bash
# 1. 建構專案
dotnet build -c Release

# 2. 打包為 NuGet 套件
dotnet pack -c Release

# 3. 解除安裝舊版本(如果存在)
dotnet tool uninstall --global Duotify.ReplaceText

# 4. 從本機安裝新版本
dotnet tool install --global Duotify.ReplaceText --add-source ./ReplaceText/nupkg

# 5. 測試
replacetext /T /path/to/test/project
```

### 建立本機 NuGet 來源

您也可以建立本機 NuGet 來源：

```bash
# 1. 建立本機來源目錄
mkdir ~/local-nuget-source

# 2. 複製套件到該目錄
cp ./ReplaceText/nupkg/*.nupkg ~/local-nuget-source/

# 3. 從本機來源安裝
dotnet tool install --global Duotify.ReplaceText --add-source ~/local-nuget-source
```

## 支援的檔案類型

預設支援以下檔案格式：

### 程式碼檔案

- .cs, .vb, .js, .vbs, .jsl, .as

### Web 檔案

- .html, .htm, .cshtml, .vbhtml, .aspx, .ascx, .ashx, .master, .asp, .asa, .asax, .asmx, .css

### 配置檔案

- .config, .xml, .xsd, .xsl, .xslt, .settings

### 專案檔案

- .sln, .csproj, .vbproj, .wdproj

### 資料檔案 (需使用 `/M` 參數)

- .txt, .md, .log, .sql, .csv, .json, .yml, .yaml

## 更多資訊

- [專案首頁](https://github.com/doggy8088/Duotify.ReplaceText)
- [問題回報](https://github.com/doggy8088/Duotify.ReplaceText/issues)
- [變更記錄](https://github.com/doggy8088/Duotify.ReplaceText/blob/main/CHANGELOG.md)

## 授權

MIT License - 詳見 [LICENSE](/LICENSE) 檔案
