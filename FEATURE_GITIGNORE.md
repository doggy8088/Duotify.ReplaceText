# ✨ 新功能: .gitignore 支援

## 📋 功能說明

ReplaceText 現在會自動尋找並套用 `.gitignore` 規則,確保不會意外轉換不應該處理的檔案!

## 🎯 解決的問題

在之前的版本中,ReplaceText 可能會轉換一些不應該處理的檔案,例如:
- 建構產物 (`bin/`, `obj/`, `*.dll`, `*.exe`)
- 套件檔案 (`*.nupkg`, `node_modules/`)
- IDE 設定檔 (`.vs/`, `.vscode/`)
- 暫存檔案 (`*.cache`, `*.tmp`)

現在,只要您的專案有 `.gitignore` 檔案,這些檔案都會被自動排除!

## 🚀 使用方式

### 無需任何額外設定!

```bash
# 程式會自動尋找並套用 .gitignore
replacetext /T /path/to/project
```

### 輸出範例

```
已載入 .gitignore 規則: C:\Projects\MyApp\.gitignore
共載入 204 條規則

預計要處理 25 個檔案
ooooooooooo.........ooo.... [25/25]
```

### 詳細模式 (查看哪些檔案被忽略)

```bash
replacetext /T /V /path/to/project
```

輸出:
```
已載入 .gitignore 規則: C:\Projects\MyApp\.gitignore
共載入 204 條規則

已忽略 (gitignore): \bin\Debug\MyApp.dll
已忽略 (gitignore): \obj\Debug\MyApp.pdb
已忽略 (gitignore): \node_modules\express\index.js
...

預計要處理 25 個檔案
[1/25] \src\Program.cs (BIG5 -> UTF-8)
[2/25] \src\Utils.cs 檔案為 UTF-8 編碼,直接跳過
...
```

## 🔧 技術實作

### 自動尋找邏輯

程式會從指定目錄開始,往上層目錄搜尋 `.gitignore`:

```
C:\Projects\MyApp\src\components\  (起始位置)
  ↓ 沒找到
C:\Projects\MyApp\src\
  ↓ 沒找到
C:\Projects\MyApp\
  ✓ 找到 .gitignore!
```

### 支援的規則

使用 `Ignore` NuGet 套件,完整支援 Git 規範:

- ✅ `bin/` - 目錄規則
- ✅ `*.dll` - 萬用字元
- ✅ `!important.config` - 否定規則
- ✅ `**/node_modules/` - 遞迴目錄
- ✅ `# 註解` - 自動忽略
- ✅ 空行 - 自動忽略

### 路徑處理

```csharp
// Windows 路徑
G:\Projects\MyApp\bin\Debug\MyApp.dll

// 轉換為相對路徑
bin/Debug/MyApp.dll  (相對於 .gitignore 位置)

// 轉換為 Unix 格式 (Git 標準)
bin/Debug/MyApp.dll

// 套用規則檢查
gitignoreRules.IsIgnored("bin/Debug/MyApp.dll")  // true
```

## 📊 效能影響

- ✅ **極小影響**: 規則檢查在檔案掃描階段進行
- ✅ **一次載入**: .gitignore 僅載入一次
- ✅ **快速比對**: 使用高效的模式匹配演算法

## 🧪 測試結果

### 測試專案: ReplaceText 本身

**沒有 .gitignore 支援時**:
```
預計要處理 150+ 個檔案
(包含所有 bin/, obj/, *.dll, *.exe 等)
```

**有 .gitignore 支援時**:
```
已載入 .gitignore 規則: G:\Projects\ReplaceText\.gitignore
共載入 204 條規則

預計要處理 4 個檔案
(只處理真正的原始碼檔案!)
```

**效果**: 減少了 **97%** 的不必要處理!

## 💡 最佳實踐

### 1. 確保專案有 .gitignore

如果您的專案使用 Git,通常已經有 `.gitignore` 檔案。
如果沒有,可以從 GitHub 的模板產生器取得:

- [Visual Studio .gitignore](https://github.com/github/gitignore/blob/main/VisualStudio.gitignore)
- [Node.js .gitignore](https://github.com/github/gitignore/blob/main/Node.gitignore)

### 2. 使用測試模式驗證

在實際轉換前,先用 `/T` 參數檢查:

```bash
replacetext /T /V /path/to/project
```

### 3. 檢查忽略清單

如果發現某些檔案不該被忽略,檢查您的 `.gitignore`:

```bash
# 使用 /V 參數查看所有被忽略的檔案
replacetext /T /V /path/to/project | findstr "已忽略"
```

## 📝 常見問題

### Q: 如果沒有 .gitignore 會怎樣?

A: 程式會顯示提示並照常處理所有符合條件的檔案:

```
未找到 .gitignore 檔案,將處理所有符合條件的檔案
```

### Q: 可以停用 .gitignore 檢查嗎?

A: 目前沒有停用選項,因為這是為了保護您的專案。如需暫時停用,可以重新命名 `.gitignore` 檔案。

### Q: 會處理 .gitignore 檔案本身嗎?

A: 不會,`.gitignore` 通常不在預設的處理副檔名清單中 (除非使用 `/M` 或 `/MO` 參數)。

### Q: 支援巢狀的 .gitignore 嗎?

A: 目前僅載入找到的第一個 `.gitignore` (最接近處理目錄的那個),與 Git 的行為略有不同。

## 🔄 版本資訊

- **版本**: 2.1.0+
- **相依套件**: `Ignore` v0.2.1
- **相容性**: 向下相容,不影響現有功能

## 📚 相關文件

- [完整 .gitignore 支援文件](GITIGNORE_SUPPORT.md)
- [安裝指南](INSTALL.md)
- [快速開始](QUICKSTART.md)

---

**享受更安全、更智慧的檔案處理體驗!** 🎉
