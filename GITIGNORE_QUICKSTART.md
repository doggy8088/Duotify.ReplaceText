# 🚀 .gitignore 功能快速指南

## 一分鐘了解新功能

ReplaceText 現在會**自動排除** `.gitignore` 中定義的檔案, 讓您的專案更安全!

## 📖 使用方式

### 基本使用 (自動啟用)

```bash
# 就像以前一樣使用,程式會自動套用 .gitignore 規則
replacetext /T /path/to/project
```

### 看看哪些檔案被忽略

```bash
# 使用 /V 參數查看詳細資訊
replacetext /T /V /path/to/project
```

## 📊 效果對比

### 之前 (沒有 .gitignore 支援)

```
預計要處理 150 個檔案
ooooooooooooooooooooooooooo...
(包含 bin/, obj/, *.dll, *.exe 等不該轉換的檔案)
```

### 現在 (有 .gitignore 支援)

```
已載入 .gitignore 規則: /path/to/.gitignore
共載入 204 條規則

預計要處理 4 個檔案
.... [4/4]
(只處理真正的原始碼檔案!)
```

## ✨ 自動忽略的檔案範例

如果您有標準的 Visual Studio `.gitignore`:

- ❌ `bin/Debug/MyApp.dll` - 建構產物
- ❌ `obj/Debug/MyApp.pdb` - 偵錯符號
- ❌ `packages/Newtonsoft.Json.dll` - NuGet 套件
- ❌ `.vs/config.json` - IDE 設定
- ✅ `src/Program.cs` - 原始碼 (會處理)
- ✅ `README.md` - 文件 (會處理, 如果使用 /M)

## 🎯 常見情境

### 情境 1: 轉換 Visual Studio 專案

```bash
# 程式會自動跳過 bin/, obj/, *.dll 等
replacetext /T g:\Projects\MyApp
```

輸出:

```
已載入 .gitignore 規則: g:\Projects\MyApp\.gitignore
共載入 204 條規則
預計要處理 25 個檔案  (不包含建構產物!)
```

### 情境 2: 轉換 Node.js 專案

```bash
# 程式會自動跳過 node_modules/
replacetext /T /M /path/to/node-project
```

輸出:

```
已載入 .gitignore 規則: /path/to/node-project/.gitignore
共載入 50 條規則
預計要處理 100 個檔案  (不包含 node_modules!)
```

### 情境 3: 沒有 .gitignore 的專案

```bash
replacetext /T /path/to/legacy-project
```

輸出:

```
未找到 .gitignore 檔案,將處理所有符合條件的檔案
預計要處理 200 個檔案
```

## 💡 提示

1. **無需設定**: 功能自動啟用, 不需要任何參數
2. **向上搜尋**: 程式會從目標目錄往上尋找 `.gitignore`
3. **Git 標準**: 完整支援 Git 的 `.gitignore` 語法
4. **詳細模式**: 使用 `/V` 可查看哪些檔案被忽略

## 📚 更多資訊

- [完整功能說明](GITIGNORE_SUPPORT.md)
- [功能介紹](FEATURE_GITIGNORE.md)
- [實作總結](IMPLEMENTATION_SUMMARY.md)

---

**現在就試試看吧!** 🎉
