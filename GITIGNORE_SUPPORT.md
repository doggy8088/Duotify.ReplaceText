# .gitignore 支援說明

## 功能概述

ReplaceText 現在支援自動讀取並套用 `.gitignore` 規則, 避免意外轉換不應該處理的檔案。

## 工作原理

1. **自動尋找**: 程式會從指定的目錄開始, 往上層目錄搜尋 `.gitignore` 檔案
2. **載入規則**: 找到 `.gitignore` 後, 會載入所有有效的規則 (自動忽略空行和註解)
3. **套用過濾**: 在掃描檔案時, 會檢查每個檔案是否符合 `.gitignore` 規則
4. **自動排除**: 符合規則的檔案會被自動跳過, 不會進行編碼轉換

## 支援的 .gitignore 語法

程式使用 `Ignore` NuGet 套件, 完整支援 Git 的 `.gitignore` 規則:

- ✅ 萬用字元模式 (如 `*.dll`, `*.exe`)
- ✅ 目錄規則 (如 `bin/`, `obj/`)
- ✅ 否定規則 (如 `!important.dll`)
- ✅ 雙星號遞迴 (如 `**/node_modules/`)
- ✅ 相對路徑與絕對路徑規則
- ✅ 註解與空行自動忽略

## 使用範例

### 範例 1: 基本使用

```bash
# 程式會自動尋找並套用 .gitignore 規則
ReplaceText.exe /T g:\Projects\MyProject
```

輸出:

```
已載入 .gitignore 規則: g:\Projects\MyProject\.gitignore
共載入 150 條規則

預計要處理 25 個檔案
...
```

### 範例 2: 詳細模式查看忽略的檔案

```bash
# 使用 /V 參數可以看到哪些檔案被 .gitignore 忽略
ReplaceText.exe /T /V g:\Projects\MyProject
```

輸出:

```
已載入 .gitignore 規則: g:\Projects\MyProject\.gitignore
共載入 150 條規則

已忽略 (gitignore): \bin\Debug\MyProject.dll
已忽略 (gitignore): \obj\Debug\MyProject.pdb
已忽略 (gitignore): \node_modules\package\index.js
...
```

### 範例 3: 沒有 .gitignore 的情況

```bash
ReplaceText.exe /T g:\Temp\TestFolder
```

輸出:

```
未找到 .gitignore 檔案,將處理所有符合條件的檔案

預計要處理 150 個檔案
...
```

## .gitignore 範例

典型的 Visual Studio 專案 `.gitignore`:

```gitignore
# Build results
[Bb]in/
[Oo]bj/
[Dd]ebug/
[Rr]elease/

# User-specific files
*.suo
*.user
*.userosscache

# Build outputs
*.dll
*.exe
*.pdb

# NuGet
*.nupkg
packages/

# Visual Studio
.vs/
```

## 常見問題

### Q: 如果我不想使用 .gitignore 怎麼辦?

A: 程式會自動檢測, 如果沒有找到 `.gitignore` 檔案, 會正常處理所有符合條件的檔案。

### Q: 我可以暫時停用 .gitignore 嗎?

A: 目前沒有停用選項, 但您可以暫時重新命名 `.gitignore` 檔案 (如改名為 `.gitignore.bak`)。

### Q: .gitignore 會影響單一檔案處理嗎?

A: 會的, 即使指定單一檔案, 程式仍會檢查該檔案是否被 `.gitignore` 規則忽略。

### Q: 程式會尋找多遠的上層目錄?

A: 程式會一直往上尋找到磁碟根目錄, 直到找到第一個 `.gitignore` 檔案為止。

### Q: 如果有多個 .gitignore 檔案怎麼辦?

A: 目前僅載入找到的第一個 `.gitignore` 檔案 (最接近處理目錄的那個)。

## 技術細節

- **使用套件**: `Ignore` NuGet package v0.2.1
- **規則引擎**: 完全符合 Git 的 `.gitignore` 規範
- **路徑處理**: 自動將 Windows 路徑分隔符號轉換為 Unix 格式 (符合 Git 標準)
- **效能**: 規則檢查在檔案掃描階段進行, 不影響整體效能

## 相關參數

雖然 `.gitignore` 支援是自動啟用的, 但以下參數可以幫助您了解處理過程:

- `/V` - 詳細輸出模式, 顯示所有被 .gitignore 忽略的檔案
- `/T` - 測試模式, 不實際寫入檔案, 適合檢視哪些檔案會被處理

## 更新歷史

- **v2.1.0** (2025-10-08): 新增 `.gitignore` 支援功能
