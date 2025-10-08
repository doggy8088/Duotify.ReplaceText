````markdown
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

... (content unchanged) ...

## 相關參數

- `/V` - 詳細輸出模式, 顯示所有被 .gitignore 忽略的檔案
- `/T` - 測試模式, 不實際寫入檔案, 適合檢視哪些檔案會被處理

## 更新歷史

- **v2.1.0** (2025-10-08): 新增 `.gitignore` 支援功能

## 📚 相關文件

- [完整 .gitignore 支援文件](GITIGNORE_SUPPORT.md)
- [安裝指南](INSTALL.md)
- [快速開始](QUICKSTART.md)

---

**版本**: 2.1.0

````
