# 快速開始指南

## 安裝與設定

### 必要條件

- .NET 8.0 SDK（開發用）或 Runtime（執行用）
- 下載：https://dotnet.microsoft.com/download/dotnet/8.0

### 選項 1：使用發行版本（推薦）

1. 從 [Releases](https://github.com/doggy8088/ReplaceText/releases) 下載適合您作業系統的版本
2. 解壓縮檔案
3. 直接執行 `ReplaceText.exe`（Windows）或 `ReplaceText`（Linux/macOS）

### 選項 2：從原始碼建構

```bash
git clone https://github.com/doggy8088/ReplaceText.git
cd ReplaceText
dotnet build -c Release
```

執行檔位置：`ReplaceText/bin/Release/net8.0/ReplaceText.dll`

## 基本使用

### 1. 查看說明

```bash
ReplaceText.exe
# 或
dotnet run --project ReplaceText
```

### 2. 轉換目錄中所有檔案為 UTF-8

```bash
# 測試模式（不會實際修改檔案）
ReplaceText.exe /T C:\MyProject

# 實際執行
ReplaceText.exe C:\MyProject
```

### 3. 替換文字並轉換編碼

```bash
ReplaceText.exe C:\MyProject "舊文字" "新文字"
```

### 4. 詳細輸出

```bash
ReplaceText.exe /V /F C:\MyProject
```

## 常見使用場景

### 場景 1：Visual Studio 專案編碼轉換

```bash
# 測試執行，查看會影響哪些檔案
ReplaceText.exe /T /V C:\Projects\MyVSProject

# 確認後執行轉換
ReplaceText.exe C:\Projects\MyVSProject
```

### 場景 2：包含文字檔案的轉換

```bash
# 預設會跳過 .txt 和 .csv 檔案
# 使用 /M 選項來包含這些檔案
ReplaceText.exe /M C:\MyData
```

### 場景 3：處理簡體中文檔案（GBK）

```bash
# 讓 GBK 編碼優先於 Big5 判斷
ReplaceText.exe /GBK C:\ChineseProject
```

### 場景 4：批次重構程式碼

```bash
# 將專案中的 "oldClassName" 全部改為 "newClassName"
ReplaceText.exe C:\Projects\MyApp "oldClassName" "newClassName"
```

## 選項組合使用

### 安全執行流程

```bash
# 步驟 1：測試執行 + 詳細輸出
ReplaceText.exe /T /V C:\MyProject

# 步驟 2：確認無誤後正式執行
ReplaceText.exe C:\MyProject
```

### 完整選項範例

```bash
# 測試 + 包含文字檔 + 詳細輸出 + 完整路徑 + GBK 優先
ReplaceText.exe /T /M /V /F /GBK C:\MyProject
```

## 支援的檔案類型

### 預設處理的檔案

- 程式碼：`.cs`, `.js`, `.vb`, `.html`, `.css` 等
- 專案：`.sln`, `.csproj`, `.vbproj` 等
- 設定：`.config`, `.xml`, `.json` 等

### 需要 /M 選項的檔案

- `.txt`
- `.csv`

## 注意事項

1. **備份重要檔案**：建議在執行前備份專案
2. **先測試執行**：使用 `/T` 選項先查看會影響哪些檔案
3. **編碼偵測**：工具會自動偵測檔案編碼，但複雜情況可能需要手動檢查
4. **版本控制**：在 Git 或其他版本控制系統中執行更安全

## 疑難排解

### 問題：程式無法執行

```bash
# 檢查 .NET 版本
dotnet --version

# 應該是 8.0.x
```

### 問題：部分檔案未被處理

- 檢查檔案是否在忽略清單中（.bat, .svn, zh-cn 等）
- 使用 `/V` 選項查看詳細輸出

### 問題：編碼判斷錯誤

- 對於簡體中文檔案，嘗試使用 `/GBK` 選項
- 對於特殊情況，可能需要手動處理

## 進階使用

### 使用 PowerShell 建構腳本

```powershell
# 完整建構
.\build.ps1 -Configuration Release

# 格式化程式碼
.\build.ps1 -Format

# 發布所有平台
.\build.ps1 -Publish

# 清理 + 建構 + 發布
.\build.ps1 -Clean -Publish
```

### 從原始碼執行

```bash
dotnet run --project ReplaceText -- /T C:\MyProject
```

## 更多資源

- [完整文件](README.md)
- [貢獻指南](CONTRIBUTING.md)
- [變更記錄](CHANGELOG.md)
- [升級摘要](UPGRADE_SUMMARY.md)

## 需要協助？

- 提交 [Issue](https://github.com/doggy8088/ReplaceText/issues)
- 查看 [文件](README.md)
- 聯繫維護者
