# ReplaceText

一個強大的文字編碼轉換與字串替換工具，專為處理多種編碼格式的文字檔案而設計。

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

### 從原始碼建構

```bash
git clone https://github.com/doggy8088/ReplaceText.git
cd ReplaceText
dotnet build -c Release
```

### 使用發行版本

從 [Releases](https://github.com/doggy8088/ReplaceText/releases) 頁面下載適合您作業系統的版本。

## 使用方法

```
ReplaceText.exe [選項] <目錄|檔案> [舊字串] [新字串]
```

### 選項

- `/T` - 測試執行模式，不會寫入檔案 (Dry Run)
- `/M` - 修改已知的文字檔案（預設會跳過 .txt 和 .csv 檔案）
- `/V` - 顯示詳細輸出模式，會顯示所有掃描的檔案清單
- `/F` - 顯示完整的檔案路徑（預設僅顯示相對路徑）
- `/GBK` - 讓 GBK (GB18030) 字集優先於 Big5 判斷

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

Copyright © Microsoft 2010

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
