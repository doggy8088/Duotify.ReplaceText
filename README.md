# ReplaceText

[![NuGet](https://img.shields.io/nuget/v/ReplaceText.svg)](https://www.nuget.org/packages/ReplaceText/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ReplaceText.svg)](https://www.nuget.org/packages/ReplaceText/)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

一個強大的文字編碼轉換與字串替換工具，專為處理多種編碼格式的文字檔案而設計。

現在可作為 .NET Global Tool 安裝，讓您在任何地方使用 `replacetext` 命令！

## 🚀 快速開始

```bash
# 1. 安裝工具
dotnet tool install --global ReplaceText

# 2. 轉換專案中的所有檔案為 UTF-8
replacetext /path/to/your/project

# 3. 完成！
```

📖 **更多使用方式請參閱 [安裝指南](INSTALL.md)**

## 功能特點

- 🔄 自動偵測並轉換多種編碼格式（UTF-8、Unicode、Big5、GBK、ISO-8859-1）
- 🔍 遞迴掃描目錄中的所有支援檔案
- ✏️ 批次字串替換功能
- 🎯 支援多種開發相關檔案格式（.cs, .js, .html, .config 等）
- 🧪 測試模式（Dry Run）可在不修改檔案的情況下預覽變更
- 📊 詳細的輸出選項以追蹤處理進度

## 系統需求

- .NET 8.0 或更高版本

## 安裝

### 方式 1：安裝為 .NET Global Tool（推薦）

需要 [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 或更高版本。

```bash
dotnet tool install --global ReplaceText
```

安裝後可在任何位置使用 `replacetext` 命令：

```bash
replacetext /path/to/your/project
```

更新工具：

```bash
dotnet tool update --global ReplaceText
```

解除安裝：

```bash
dotnet tool uninstall --global ReplaceText
```

📖 **詳細安裝說明請參閱 [INSTALL.md](INSTALL.md)**

### 方式 2：從原始碼建構

```bash
git clone https://github.com/doggy8088/ReplaceText.git
cd ReplaceText
dotnet build -c Release
```

### 方式 3：使用發行版本

從 [Releases](https://github.com/doggy8088/ReplaceText/releases) 頁面下載適合您作業系統的版本。

## 使用方法

### 作為 Global Tool 使用（推薦）

```bash
# 轉換目錄中所有檔案為 UTF-8
replacetext /path/to/your/project

# 測試模式（不實際修改檔案）
replacetext /T /path/to/your/project

# 替換字串並轉換編碼
replacetext /path/to/your/project "oldText" "newText"

# 詳細輸出模式
replacetext /V /F /path/to/your/project

# GBK 優先模式（處理簡體中文）
replacetext /GBK /path/to/your/project
```

### 作為獨立執行檔使用

```
ReplaceText.exe [選項] <目錄|檔案> [舊字串] [新字串]
```

### 選項

- `/T` - 測試執行模式，不會寫入檔案 (Dry Run)
- `/M` - 修改已知的文字檔案（預設會跳過 .txt 和 .csv 檔案）
- `/V` - 顯示詳細輸出模式，會顯示所有掃描的檔案清單
- `/F` - 顯示完整的檔案路徑（預設僅顯示相對路徑）
- `/GBK` - 讓 GBK (GB18030) 字集優先於 Big5 判斷
- `/U` - 自動判斷未知檔案類型，嘗試以文字方式處理沒有副檔名或非典型副檔名的檔案（若非文字檔則會跳過）

### 使用範例

#### 1. 將目錄中所有檔案轉換為 UTF-8

```bash
ReplaceText.exe C:\MyProject
```

#### 2. 測試模式檢查將要變更的檔案

```bash
ReplaceText.exe /T C:\MyProject
```

#### 3. 替換字串並轉換編碼

```bash
ReplaceText.exe C:\MyProject "oldText" "newText"
```

#### 4. 詳細輸出模式

```bash
ReplaceText.exe /V /F C:\MyProject
```

#### 5. GBK 優先模式

```bash
ReplaceText.exe /GBK C:\MyProject
```

#### 6. 自動判斷未知檔案類型（/U）

當目錄中存在沒有副檔名或副檔名不常見的檔案時，使用 `/U` 可以讓工具嘗試以文字方式判斷並處理這些檔案（若判定為二進位檔案則會跳過）：

```bash
ReplaceText.exe /U C:\MyProject
```

## 支援的檔案格式

預設支援以下檔案格式：

- **程式碼**: .cs, .js, .vb, .vbs, .jsl, .as
- **Web**: .html, .htm, .cshtml, .vbhtml, .aspx, .ascx, .ashx, .master, .asp, .asa, .asax, .asmx, .css
- **配置**: .config, .xml, .xsd, .xsl, .xslt
- **專案**: .sln, .csproj, .vbproj, .wdproj
- **資料**: .resx, .edmx, .dbml, .rdlc
- **其他**: .settings, .cd, .wsf, .sitemap, .skin, .browser, .disco, .wsdl, .discomap, .webinfo

使用 `/M` 選項可額外處理：
- .txt
- .csv

## 開發

### 建構專案

```bash
dotnet build
```

### 執行測試

```bash
dotnet test
```

### 程式碼格式化

```bash
dotnet format
```

### 檢查程式碼格式

```bash
dotnet format --verify-no-changes
```

## CI/CD

本專案使用 GitHub Actions 進行持續整合和部署：

- ✅ 自動建構（Windows、Linux、macOS）
- ✅ 程式碼格式檢查
- ✅ 自動發布多平台二進位檔案

## 授權

MIT License

Copyright (c) 2010-2025 Will 保哥 (doggy8088)

完整授權條款請參閱 [LICENSE](LICENSE)

## 升級記錄

### v2.0.0 (2025)
- ✨ 升級至 .NET 8.0
- 🔧 現代化專案結構（SDK 風格的 .csproj）
- 🤖 新增 GitHub Actions CI/CD
- 📝 新增 .editorconfig 以確保程式碼品質
- 🎯 支援多平台發布（Windows、Linux、macOS）
- 🐛 修正所有編譯警告
- 📦 支援單一檔案發布

## 貢獻

歡迎提交 Issue 和 Pull Request！

## 相關連結

- [專案首頁](https://github.com/doggy8088/ReplaceText)
- [問題回報](https://github.com/doggy8088/ReplaceText/issues)
- [發行版本](https://github.com/doggy8088/ReplaceText/releases)
