````markdown
# 套件更名說明

## 📦 套件名稱變更

從 **ReplaceText** 更名為 **Duotify.ReplaceText**

## 📊 版本資訊

- **新套件名稱**: `Duotify.ReplaceText`
- **新版本號**: `1.0.0`
- **舊套件名稱**: `ReplaceText` (已棄用)
- **舊版本號**: `2.0.0`

## 🔄 遷移指南

### 解除安裝舊套件

```bash
dotnet tool uninstall --global ReplaceText
```

### 安裝新套件

```bash
dotnet tool install --global Duotify.ReplaceText
```

### 命令列工具名稱

⚠️ **重要**: 命令列工具名稱保持不變,仍然是 `replacetext`

```bash
# 這個命令不變
replacetext /path/to/project
```

## 📝 變更清單

### 專案檔案

- ✅ `ReplaceText/ReplaceText.csproj`
  - PackageId: `ReplaceText` → `Duotify.ReplaceText`
  - Version: `2.0.0` → `1.0.0`
  - Description: 加入 `.gitignore` 功能說明
  - PackageTags: 加入 `duotify`
  - PackageReleaseNotes: 更新為新版本說明

### CI/CD 設定

- ✅ `.github/workflows/cd.yml`
  - GitHub Release 標題: `ReplaceText` → `Duotify.ReplaceText`
  - 安裝命令: `dotnet tool install --global ReplaceText` → `dotnet tool install --global Duotify.ReplaceText`
  - 更新命令: `dotnet tool update --global ReplaceText` → `dotnet tool update --global Duotify.ReplaceText`
  - NuGet 套件連結更新

### 文件更新

- ✅ `README.md`
  - NuGet badges 更新
  - 快速開始指令更新
  - 安裝/更新/解除安裝指令更新
  - 升級記錄新增 v1.0.0 說明

- ✅ `INSTALL.md`
  - 標題更新為 `Duotify.ReplaceText 安裝指南`
  - 所有安裝指令更新
  - 更新/解除安裝指令更新
  - 範例版本號更新

- ✅ `CHANGELOG.md`
  - 新增 v1.0.0 版本記錄
  - 說明套件名稱變更
  - 標註舊版本為 ReplaceText 套件

## 🎯 為什麼要更名?

1. **品牌統一**: 將工具納入 Duotify 品牌體系
2. **命名空間管理**: 避免與其他同名套件衝突
3. **版本重置**: 從 1.0.0 開始,配合新功能發佈

## ✨ v1.0.0 新功能

- 🚫 **新增 .gitignore 自動支援**
  - 自動尋找並套用 .gitignore 規則
  - 避免意外轉換建構產物、套件檔案等
  - 大幅減少不必要的檔案處理 (測試中減少 97%)

- 📦 **套件資訊更新**
  - 新增 Duotify 品牌標識
  - 更新套件描述和標籤

## 📚 相關連結

- [NuGet Gallery - Duotify.ReplaceText](https://www.nuget.org/packages/Duotify.ReplaceText/)
- [GitHub Repository](https://github.com/doggy8088/ReplaceText)
- [安裝指南](./INSTALL.md)
- [更新日誌](../CHANGELOG.md)

## ⚠️ 注意事項

1. **命令列工具名稱不變**: 仍然使用 `replacetext` 命令
2. **需要手動遷移**: 舊套件不會自動更新到新套件
3. **功能完全相容**: 所有功能和參數保持不變
4. **版本號重置**: 新套件從 1.0.0 開始

## 🆘 需要協助?

如果在遷移過程中遇到問題,請至 [GitHub Issues](https://github.com/doggy8088/ReplaceText/issues) 回報。

---

**更新日期**: 2025-10-08

````
