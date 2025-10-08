````markdown
# ReplaceText 發佈指南

本文件說明如何將 ReplaceText 發佈到 NuGet Gallery。

## 📋 目錄

- [前置準備](#前置準備)
- [本機建構與測試](#本機建構與測試)
- [手動發佈流程](#手動發佈流程)
- [自動發佈流程](#自動發佈流程-推薦)
- [發佈後驗證](#發佈後驗證)
- [版本管理策略](#版本管理策略)
- [常見問題](#常見問題)

## 前置準備

### 1. 建立 NuGet 帳號

1. 前往 [NuGet.org](https://www.nuget.org/) 註冊帳號
2. 登入後，前往 [API Keys](https://www.nuget.org/account/apikeys) 頁面
3. 點擊 "Create" 建立新的 API Key
   - **Key Name**: `ReplaceText-GitHub-Actions`（或任何您喜歡的名稱）
   - **Glob Pattern**: `ReplaceText`
   - **Scopes**: 選擇 `Push` 和 `Push new packages and package versions`
   - **Expiration**: 建議設定為 365 天
4. 複製產生的 API Key（只會顯示一次，請妥善保存）

### 2. 設定 GitHub Repository Secret

1. 前往您的 GitHub repository 設定頁面
2. 選擇 `Settings` → `Secrets and variables` → `Actions`
3. 點擊 `New repository secret`
4. 建立以下 secret：
   - **Name**: `NUGET_API_KEY`
   - **Value**: 貼上您在 NuGet.org 建立的 API Key

### 3. 安裝必要工具

```bash
# 安裝 .NET SDK 8.0 或更高版本
# 下載位置: https://dotnet.microsoft.com/download/dotnet/8.0

# 驗證安裝
dotnet --version
```

## 本機建構與測試

在發佈前，請先在本機進行完整測試：

### 1. 清理舊的建構檔案

```bash
# Windows (PowerShell)
Remove-Item -Recurse -Force .\ReplaceText\bin\, .\ReplaceText\obj\, .\ReplaceText\nupkg\ -ErrorAction SilentlyContinue

# Linux/macOS
rm -rf ./ReplaceText/bin ./ReplaceText/obj ./ReplaceText/nupkg
```

### 2. 還原相依套件

```bash
dotnet restore ReplaceText/ReplaceText.csproj
```

### 3. 建構專案

```bash
dotnet build ReplaceText/ReplaceText.csproj -c Release
```

### 4. 打包 NuGet 套件

```bash
dotnet pack ReplaceText/ReplaceText.csproj -c Release -o ./ReplaceText/nupkg
```

### 5. 檢查套件內容

```bash
# Windows (PowerShell)
Expand-Archive -Path .\ReplaceText\nupkg\ReplaceText.*.nupkg -DestinationPath .\temp-nupkg -Force
Get-ChildItem -Recurse .\temp-nupkg

# Linux/macOS
unzip -l ./ReplaceText/nupkg/ReplaceText.*.nupkg
```

### 6. 本機測試安裝

```bash
# 解除安裝舊版本（如果已安裝）
dotnet tool uninstall --global ReplaceText

# 從本機套件安裝
dotnet tool install --global ReplaceText --add-source ./ReplaceText/nupkg

# 測試工具是否正常運作
replacetext

# 建立測試目錄進行實際測試
mkdir test-project
cd test-project
echo "測試內容" > test.txt
replacetext /T .
```

## 手動發佈流程

如果您想要手動發佈套件到 NuGet.org：

### 1. 更新版本號

編輯 `ReplaceText/ReplaceText.csproj`，更新 `<Version>` 標籤：

```xml
<Version>2.0.1</Version>
```

### 2. 建構並打包

```bash
dotnet pack ReplaceText/ReplaceText.csproj -c Release -o ./ReplaceText/nupkg
```

### 3. 發佈到 NuGet.org

```bash
dotnet nuget push ./ReplaceText/nupkg/ReplaceText.*.nupkg \
  --api-key YOUR_NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### 4. 驗證發佈

前往 [NuGet.org](https://www.nuget.org/packages/ReplaceText) 查看套件是否已成功發佈。

> ⚠️ **注意**: 套件發佈後可能需要幾分鐘才會顯示在搜尋結果中。

## 自動發佈流程（推薦）

使用 GitHub Actions 自動化發佈流程是最推薦的方式。

### 發佈步驟

#### 1. 更新版本號

編輯 `ReplaceText/ReplaceText.csproj`，更新版本號：

```xml
<Version>2.0.1</Version>
```

#### 2. 提交變更

```bash
git add ReplaceText/ReplaceText.csproj
git commit -m "chore: bump version to 2.0.1"
git push origin main
```

#### 3. 建立並推送版本標籤

```bash
# 建立標籤（版本號前加 v）
git tag v2.0.1

# 推送標籤到 GitHub
git push origin v2.0.1
```

#### 4. GitHub Actions 自動執行

推送標籤後，GitHub Actions 會自動：
1. ✅ 建構專案
2. ✅ 執行測試（如果有）
3. ✅ 打包 NuGet 套件
4. ✅ 發佈到 NuGet.org
5. ✅ 建立 GitHub Release
6. ✅ 上傳套件作為 Release Assets

#### 5. 監控發佈進度

1. 前往 GitHub repository 的 `Actions` 頁籤
2. 查看 "CD - Publish to NuGet" workflow 的執行狀態
3. 如果發生錯誤，檢查執行記錄

### 手動觸發 Workflow

您也可以手動觸發發佈流程（不需要建立標籤）：

1. 前往 GitHub repository 的 `Actions` 頁籤
2. 選擇 "CD - Publish to NuGet" workflow
3. 點擊 `Run workflow` 按鈕
4. 選擇分支後執行

> ⚠️ **注意**: 手動觸發時會使用開發版本號 (例如: 1.0.0-dev.123)

## 發佈後驗證

### 1. 檢查 NuGet.org

前往 [NuGet.org](https://www.nuget.org/packages/ReplaceText) 確認：
- ✅ 版本號正確
- ✅ 套件描述完整
- ✅ 專案連結正確
- ✅ 下載數開始累積

### 2. 測試從 NuGet 安裝

```bash
# 解除安裝舊版本
dotnet tool uninstall --global ReplaceText

# 等待幾分鐘讓套件索引更新

# 從 NuGet.org 安裝新版本
dotnet tool install --global ReplaceText

# 驗證版本
dotnet tool list --global | findstr ReplaceText  # Windows
dotnet tool list --global | grep ReplaceText     # Linux/macOS

# 測試功能
replacetext
```

### 3. 檢查 GitHub Release

確認 GitHub Release 頁面：
- ✅ Release 已建立
- ✅ Release Notes 完整
- ✅ NuGet 套件已附加為 Assets

### 4. 更新說明文件

發佈新版本後，建議更新以下文件：
- `CHANGELOG.md` - 記錄變更內容
- `README.md` - 更新版本號和新功能說明
- `INSTALL.md` - 更新安裝範例（如有需要）

## 版本管理策略

### 語義化版本控制 (Semantic Versioning)

本專案遵循 [SemVer 2.0.0](https://semver.org/) 規範：

```
主版本號.次版本號.修訂號 (MAJOR.MINOR.PATCH)
```

... (content unchanged) ...

## 相關資源

- [NuGet.org 官方文件](https://docs.microsoft.com/nuget/)
- [.NET Global Tools 指南](https://docs.microsoft.com/dotnet/core/tools/global-tools)
- [GitHub Actions 文件](https://docs.github.com/actions)
- [語義化版本控制](https://semver.org/)
- [專案 README](../README.md)
- [安裝指南](./INSTALL.md)
- [變更記錄](../CHANGELOG.md)

## 聯絡資訊

如有任何問題或建議，請透過以下方式聯絡：

- GitHub Issues: https://github.com/doggy8088/Duotify.ReplaceText/issues
- Email: will@miniasp.com

---

最後更新: 2025-10-08

````
