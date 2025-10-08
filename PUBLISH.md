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
# 解除安裝舊版本（如果有）
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

- **主版本號 (MAJOR)**: 不相容的 API 變更
- **次版本號 (MINOR)**: 向下相容的功能新增
- **修訂號 (PATCH)**: 向下相容的錯誤修正

#### 範例：

- `1.0.0` → `2.0.0`: 重大變更，不向下相容
- `2.0.0` → `2.1.0`: 新增功能，向下相容
- `2.1.0` → `2.1.1`: 錯誤修正，向下相容

### 版本號更新指南

#### 何時增加主版本號 (MAJOR)

- 移除或重新命名命令列參數
- 改變預設行為
- 移除支援的檔案格式
- 移除支援的編碼格式
- .NET 目標框架的重大升級（例如 .NET 6 → .NET 8）

#### 何時增加次版本號 (MINOR)

- 新增命令列參數
- 新增支援的檔案格式
- 新增支援的編碼格式
- 效能改善
- 新增功能但保持向下相容

#### 何時增加修訂號 (PATCH)

- 錯誤修正
- 文件更新
- 小型效能優化
- 依賴套件更新（不影響功能）

### 版本號設定位置

版本號需要在以下位置更新：

1. **ReplaceText.csproj** - 主要版本號
   ```xml
   <Version>2.0.1</Version>
   <AssemblyVersion>2.0.1.0</AssemblyVersion>
   <FileVersion>2.0.1.0</FileVersion>
   ```

2. **Git Tag** - 用於觸發 CD
   ```bash
   git tag v2.0.1
   ```

## 常見問題

### Q1: 發佈失敗，顯示 "409 Conflict" 錯誤

**原因**: 該版本號已經存在於 NuGet.org

**解決方法**:
1. 更新版本號（不能覆蓋已發佈的版本）
2. 刪除舊標籤: `git tag -d v2.0.0 && git push origin :refs/tags/v2.0.0`
3. 建立新標籤: `git tag v2.0.1 && git push origin v2.0.1`

### Q2: GitHub Actions 顯示 "unauthorized" 錯誤

**原因**: NuGet API Key 無效或已過期

**解決方法**:
1. 前往 [NuGet.org API Keys](https://www.nuget.org/account/apikeys)
2. 重新產生 API Key
3. 更新 GitHub Secret `NUGET_API_KEY`

### Q3: 套件發佈成功但無法搜尋到

**原因**: NuGet.org 的搜尋索引需要時間更新

**解決方法**:
- 等待 5-10 分鐘
- 使用直接連結: `https://www.nuget.org/packages/ReplaceText`
- 嘗試清除本機 NuGet 快取: `dotnet nuget locals all --clear`

### Q4: 如何撤銷已發佈的版本？

**注意**: NuGet.org 不允許刪除已發佈的套件，只能 "Unlist"（取消列出）

**步驟**:
1. 登入 [NuGet.org](https://www.nuget.org/)
2. 前往 `Manage Packages` → 選擇 `ReplaceText`
3. 選擇要取消列出的版本
4. 點擊 `Unlist`

> ⚠️ **重要**: Unlist 後的版本仍可透過直接連結或已知版本號安裝，但不會出現在搜尋結果中。

### Q5: 如何測試 CD workflow 而不實際發佈？

**方法 1**: 使用分支而非標籤
```bash
# 推送到非主分支
git checkout -b test-publish
git push origin test-publish

# 手動觸發 workflow（選擇 test-publish 分支）
# 這會產生開發版本號，不會發佈到 NuGet.org
```

**方法 2**: 修改 workflow，加入測試步驟
```yaml
- name: 🧪 Dry Run - 測試打包
  run: |
    dotnet pack ${{ env.PROJECT_PATH }} \
      -c Release \
      -p:PackageVersion=${{ steps.get_version.outputs.VERSION }}-test
```

### Q6: 如何發佈到私有 NuGet Feed？

修改 `.github/workflows/cd.yml` 中的發佈步驟：

```yaml
- name: 🚀 發佈到私有 NuGet Feed
  run: |
    dotnet nuget push "${{ env.PACKAGE_OUTPUT_DIR }}/*.nupkg" \
      --api-key ${{ secrets.PRIVATE_NUGET_API_KEY }} \
      --source https://your-private-feed.com/v3/index.json \
      --skip-duplicate
```

並新增對應的 GitHub Secret: `PRIVATE_NUGET_API_KEY`

## 發佈檢查清單

在發佈新版本前，請確認：

- [ ] 所有測試通過
- [ ] 程式碼已合併到 `main` 分支
- [ ] `CHANGELOG.md` 已更新
- [ ] 版本號已正確更新在 `.csproj` 檔案中
- [ ] `README.md` 的範例和說明是最新的
- [ ] 本機測試安裝成功
- [ ] GitHub Actions workflow 設定正確
- [ ] NuGet API Key 有效且權限正確
- [ ] Git 標籤命名正確（v + 版本號）

## 相關資源

- [NuGet.org 官方文件](https://docs.microsoft.com/nuget/)
- [.NET Global Tools 指南](https://docs.microsoft.com/dotnet/core/tools/global-tools)
- [GitHub Actions 文件](https://docs.github.com/actions)
- [語義化版本控制](https://semver.org/)
- [專案 README](./README.md)
- [安裝指南](./INSTALL.md)
- [變更記錄](./CHANGELOG.md)

## 聯絡資訊

如有任何問題或建議，請透過以下方式聯絡：

- GitHub Issues: https://github.com/doggy8088/ReplaceText/issues
- Email: will@miniasp.com

---

最後更新: 2025-10-08
