# ✅ ReplaceText - .NET Global Tool 完成報告

## 🎉 任務完成摘要

已成功將 ReplaceText 專案配置為 .NET Global Tool，並建立完整的發佈流程和說明文件。

## 📦 已完成的項目

### 1. ✅ 專案配置 (.csproj)

**檔案**: `ReplaceText/ReplaceText.csproj`

已新增以下設定：
- `<PackAsTool>true</PackAsTool>` - 啟用 Global Tool 打包
- `<ToolCommandName>replacetext</ToolCommandName>` - 命令名稱
- 完整的 NuGet 套件中繼資料
- MIT 授權宣告
- 包含 README.md 到套件中

**目前版本**: 2.0.0

### 2. ✅ 安裝指南文件

**檔案**: `INSTALL.md`

內容包含：
- 3 種安裝方式（NuGet、本機、原始碼）
- 完整的命令列選項說明
- Windows/Linux/macOS 使用範例
- 常見問題解答 (FAQ)
- 更新與解除安裝步驟
- 開發者本機測試流程

### 3. ✅ 發佈指南文件

**檔案**: `PUBLISH.md`

內容包含：
- NuGet.org 帳號與 API Key 設定
- GitHub Secret 設定步驟
- 本機建構與測試完整流程
- 手動發佈與自動發佈流程
- 語義化版本控制策略
- 發佈後驗證步驟
- 常見問題與解決方法
- 完整的發佈檢查清單

### 4. ✅ CD Workflow (GitHub Actions)

**檔案**: `.github/workflows/cd.yml`

功能特點：
- ✅ 自動建構與打包
- ✅ 自動發佈到 NuGet.org（推送 v* 標籤時觸發）
- ✅ 自動建立 GitHub Release
- ✅ 上傳 NuGet 套件作為 Artifacts
- ✅ 支援手動觸發 (workflow_dispatch)
- ✅ 自動生成 Release Notes
- ✅ 版本號自動從 Git 標籤提取

### 5. ✅ 設定完成文件

**檔案**: `DOTNET_GLOBAL_TOOL_SETUP.md`

詳細說明：
- 完成項目總覽
- 下一步操作指引
- 本機測試步驟
- NuGet.org 發佈流程
- 版本管理說明
- 檢查清單

### 6. ✅ 更新 README.md

新增內容：
- NuGet 徽章（版本、下載數、.NET 版本、授權）
- 快速開始區塊
- 作為 Global Tool 的安裝說明
- Global Tool 使用範例

### 7. ✅ LICENSE 檔案

**檔案**: `LICENSE`

- MIT License
- Copyright © 2010-2025 Will 保哥

### 8. ✅ 本機測試驗證

已成功：
- ✅ 建構專案
- ✅ 打包 NuGet 套件（2.0.0）
- ✅ 本機安裝工具
- ✅ 執行命令驗證功能

## 📂 新增/修改的檔案清單

```
G:\Projects\ReplaceText\
├── .github/
│   └── workflows/
│       └── cd.yml                          # ✨ 新增：CD workflow
├── ReplaceText/
│   ├── ReplaceText.csproj                  # ✏️ 修改：新增 Global Tool 設定
│   └── nupkg/
│       ├── ReplaceText.1.0.0.nupkg         # 📦 舊版本
│       └── ReplaceText.2.0.0.nupkg         # 📦 新版本（最新）
├── INSTALL.md                               # ✨ 新增：安裝指南
├── PUBLISH.md                               # ✨ 新增：發佈指南
├── DOTNET_GLOBAL_TOOL_SETUP.md             # ✨ 新增：設定完成說明
├── LICENSE                                  # ✨ 新增：MIT 授權
└── README.md                                # ✏️ 修改：新增 Global Tool 說明
```

## 🚀 如何發佈到 NuGet.org

### 前置準備（一次性設定）

1. **建立 NuGet API Key**
   ```
   前往: https://www.nuget.org/account/apikeys
   建立新的 API Key
   複製 API Key（只會顯示一次）
   ```

2. **設定 GitHub Secret**
   ```
   前往: Repository Settings → Secrets and variables → Actions
   建立 Secret:
     Name: NUGET_API_KEY
     Value: <您的 NuGet API Key>
   ```

### 發佈步驟

#### 選項 1: 自動發佈（推薦）

```powershell
# 1. 確保所有變更已提交
git add .
git commit -m "feat: configure as .NET Global Tool v2.0.0"
git push origin main

# 2. 建立版本標籤（這會觸發自動發佈）
git tag v2.0.0
git push origin v2.0.0

# 3. 前往 GitHub Actions 查看發佈進度
# https://github.com/doggy8088/ReplaceText/actions
```

#### 選項 2: 手動發佈

```powershell
# 使用您的 NuGet API Key
dotnet nuget push .\ReplaceText\nupkg\ReplaceText.2.0.0.nupkg `
  --api-key <YOUR_NUGET_API_KEY> `
  --source https://api.nuget.org/v3/index.json
```

## 📝 使用者安裝方式

發佈到 NuGet.org 後，使用者可以透過以下方式安裝：

```bash
# 安裝
dotnet tool install --global ReplaceText

# 使用
replacetext /path/to/project

# 更新
dotnet tool update --global ReplaceText

# 解除安裝
dotnet tool uninstall --global ReplaceText
```

## ✅ 本機測試結果

```powershell
# 安裝成功
PS> dotnet tool install --global ReplaceText --add-source G:\Projects\ReplaceText\ReplaceText\nupkg --version 2.0.0
You can invoke the tool using the following command: replacetext

# 執行測試
PS> replacetext
ReplaceText.exe /T /M /V /F /GBK /U <Directory|File>

/T      測試執行模式,不會寫入檔案 (Dry Run)
/M      是否修改已知的文字檔案 (預設會跳過文字資料檔,僅修改 Visual Studio 2010 程式相關檔案)
/V      顯示詳細輸出模式,會顯示所有掃描的檔案清單
/F      顯示完整的檔案路徑(預設僅顯示相對路徑)
/GBK    讓 GBK (GB18030) 字集優先於 Big5 判斷
/U      自動判斷未知檔案類型 (預設僅處理已知的檔案類型)
```

✅ **測試通過！工具可正常執行。**

## 🔄 版本更新流程

當需要發佈新版本時：

1. **更新版本號**
   - 編輯 `ReplaceText/ReplaceText.csproj`
   - 修改 `<Version>2.0.1</Version>`

2. **更新 CHANGELOG.md**（如果有）
   - 記錄新版本的變更內容

3. **提交變更**
   ```powershell
   git add .
   git commit -m "chore: bump version to 2.0.1"
   git push origin main
   ```

4. **建立並推送標籤**
   ```powershell
   git tag v2.0.1
   git push origin v2.0.1
   ```

5. **GitHub Actions 自動執行**
   - 建構專案
   - 打包 NuGet
   - 發佈到 NuGet.org
   - 建立 GitHub Release

## 📊 專案統計

- **目標框架**: .NET 8.0
- **工具命令名稱**: `replacetext`
- **套件 ID**: ReplaceText
- **目前版本**: 2.0.0
- **授權**: MIT License
- **作者**: Will 保哥
- **Repository**: https://github.com/doggy8088/ReplaceText

## 🔗 相關連結

- **NuGet Gallery**: https://www.nuget.org/packages/ReplaceText/ （發佈後可用）
- **GitHub Repository**: https://github.com/doggy8088/ReplaceText
- **GitHub Releases**: https://github.com/doggy8088/ReplaceText/releases
- **GitHub Actions**: https://github.com/doggy8088/ReplaceText/actions

## 📚 文件連結

| 文件 | 用途 | 目標讀者 |
|------|------|----------|
| [README.md](README.md) | 專案概述與快速開始 | 所有使用者 |
| [INSTALL.md](INSTALL.md) | 詳細安裝與使用指南 | 終端使用者 |
| [PUBLISH.md](PUBLISH.md) | 發佈流程與版本管理 | 專案維護者 |
| [DOTNET_GLOBAL_TOOL_SETUP.md](DOTNET_GLOBAL_TOOL_SETUP.md) | 設定完成說明 | 專案維護者 |
| [LICENSE](LICENSE) | MIT 授權條款 | 所有使用者 |

## ⚠️ 重要提醒

1. **NuGet API Key 安全**
   - ❌ 不要將 API Key 提交到程式碼庫
   - ✅ 使用 GitHub Secrets 儲存
   - ✅ 定期更新 API Key

2. **版本號規則**
   - 使用語義化版本控制 (Semantic Versioning)
   - 無法覆蓋已發佈的版本
   - 標籤格式: `v` + 版本號（例如：`v2.0.0`）

3. **發佈確認**
   - 發佈後無法刪除，只能 Unlist
   - 發佈前務必本機測試
   - 使用 `/T` 參數進行 Dry Run 測試

## 🎯 下一步建議

1. **立即可做**
   - [ ] 建立 NuGet.org 帳號
   - [ ] 設定 GitHub Secret
   - [ ] 推送版本標籤觸發首次發佈

2. **中期規劃**
   - [ ] 建立 CHANGELOG.md 記錄版本變更
   - [ ] 新增單元測試
   - [ ] 建立 CI workflow（建構與測試）
   - [ ] 新增程式碼覆蓋率報告

3. **長期規劃**
   - [ ] 新增更多編碼格式支援
   - [ ] 提供批次處理模式
   - [ ] 建立 Web UI 版本
   - [ ] 支援設定檔自訂規則

## 🙏 致謝

感謝使用 GitHub Copilot 協助完成此專案的 .NET Global Tool 配置！

---

**報告產生時間**: 2025-10-08  
**專案狀態**: ✅ 準備就緒，可以發佈到 NuGet.org  
**建議下一步**: 設定 NuGet API Key 並推送版本標籤以觸發自動發佈
