# ReplaceText - .NET Global Tool 設定完成

✅ **專案已成功設定為 .NET Global Tool！**

## 📦 已完成的工作

### 1. ✅ 專案配置

已更新 `ReplaceText.csproj` 檔案，新增以下設定：

- `<PackAsTool>true</PackAsTool>` - 啟用 .NET Global Tool 打包
- `<ToolCommandName>replacetext</ToolCommandName>` - 設定全域命令名稱
- 完整的 NuGet 套件資訊（描述、標籤、授權等）
- 目前版本：**2.0.0**

### 2. ✅ 建立安裝指南

已建立 `INSTALL.md` 檔案，包含：

- 詳細的安裝步驟（從 NuGet Gallery、本機套件、原始碼）
- 命令列選項說明
- Windows 和 Linux/macOS 使用範例
- 常見問題解答
- 更新與解除安裝說明

### 3. ✅ 建立 CD Workflow

已建立 `.github/workflows/cd.yml`，包含：

- 自動建構與打包
- 自動發佈到 NuGet.org（當推送 v* 標籤時）
- 自動建立 GitHub Release
- 上傳套件作為 Artifacts
- 支援手動觸發

### 4. ✅ 建立發佈指南

已建立 `PUBLISH.md` 檔案，包含：

- NuGet 帳號設定步驟
- GitHub Secret 設定說明
- 本機建構與測試流程
- 手動發佈與自動發佈流程
- 版本管理策略（語義化版本控制）
- 完整的發佈後驗證步驟
- 常見問題與解決方法
- 發佈檢查清單

## 🚀 下一步操作

### 選項 1: 測試本機安裝

```powershell
# 1. 解除安裝舊版本（如果有）
dotnet tool uninstall --global ReplaceText

# 2. 從本機套件安裝
dotnet tool install --global ReplaceText --add-source .\ReplaceText\nupkg

# 3. 測試工具
replacetext

# 4. 實際測試轉換功能
mkdir test-project
cd test-project
echo "測試內容" > test.txt
replacetext /T .
```

### 選項 2: 發佈到 NuGet.org

#### 準備工作

1. **建立 NuGet API Key**
   - 前往 https://www.nuget.org/account/apikeys
   - 建立新的 API Key
   - 複製 API Key

2. **設定 GitHub Secret**
   - 前往 GitHub Repository Settings
   - `Settings` → `Secrets and variables` → `Actions`
   - 建立名為 `NUGET_API_KEY` 的 secret
   - 貼上 NuGet API Key

#### 發佈步驟

```powershell
# 1. 確認所有變更已提交
git add .
git commit -m "feat: configure as .NET Global Tool and add publishing workflows"
git push origin main

# 2. 建立版本標籤（這會觸發自動發佈）
git tag v2.0.0
git push origin v2.0.0
```

#### 手動發佈（替代方式）

如果您想要手動發佈：

```powershell
dotnet nuget push .\ReplaceText\nupkg\ReplaceText.2.0.0.nupkg `
  --api-key YOUR_NUGET_API_KEY `
  --source https://api.nuget.org/v3/index.json
```

## 📝 重要文件

| 檔案 | 說明 |
|------|------|
| `INSTALL.md` | 使用者安裝與使用指南 |
| `PUBLISH.md` | 開發者發佈指南 |
| `.github/workflows/cd.yml` | 自動發佈 workflow |
| `ReplaceText.csproj` | 專案設定檔（含 NuGet 資訊） |

## 🔍 驗證套件內容

已產生的套件位於：`G:\Projects\ReplaceText\ReplaceText\nupkg\ReplaceText.2.0.0.nupkg`

您可以使用以下命令檢查套件內容：

```powershell
# 解壓縮套件到臨時目錄
Expand-Archive -Path .\ReplaceText\nupkg\ReplaceText.2.0.0.nupkg -DestinationPath .\temp-nupkg -Force

# 查看內容
Get-ChildItem -Recurse .\temp-nupkg
```

## 📚 使用範例

安裝後的使用方式：

```powershell
# 轉換目錄中所有檔案為 UTF-8
replacetext C:\MyProject

# 測試模式（不實際修改）
replacetext /T C:\MyProject

# 包含文字檔案
replacetext /M C:\MyProject

# 詳細輸出
replacetext /V /F C:\MyProject

# 字串替換
replacetext C:\MyProject "舊文字" "新文字"

# GBK 優先模式
replacetext /GBK C:\MyProject
```

## ⚙️ 版本管理

### 當前版本
- **Version**: 2.0.0
- **AssemblyVersion**: 1.0.0.0
- **FileVersion**: 1.0.0.0

### 更新版本號時需要修改

1. `ReplaceText/ReplaceText.csproj` 中的 `<Version>` 標籤
2. Git 標籤（例如：`git tag v2.0.1`）
3. 可選：更新 `<AssemblyVersion>` 和 `<FileVersion>`

### 版本號規則

遵循語義化版本控制 (Semantic Versioning)：

- **主版本號**: 不相容的 API 變更
- **次版本號**: 向下相容的功能新增
- **修訂號**: 向下相容的錯誤修正

## ⚠️ 注意事項

1. **NuGet 套件一旦發佈就無法刪除**，只能取消列出（Unlist）
2. **相同版本號無法重複發佈**，請確保版本號遞增
3. **API Key 請妥善保管**，不要提交到程式碼庫
4. **發佈到 NuGet.org 後需要幾分鐘**才會出現在搜尋結果中
5. **GitHub Actions 需要正確設定 Secret** 才能自動發佈

## 📞 需要協助？

- 📖 詳細安裝說明：查看 `INSTALL.md`
- 🚀 發佈流程說明：查看 `PUBLISH.md`
- 🐛 問題回報：https://github.com/doggy8088/ReplaceText/issues
- 📧 Email: will@miniasp.com

## ✅ 檢查清單

發佈前請確認：

- [ ] 已在本機測試安裝成功
- [ ] 已建立 NuGet API Key
- [ ] 已設定 GitHub Secret (`NUGET_API_KEY`)
- [ ] 版本號已更新
- [ ] README.md 內容最新
- [ ] 所有變更已提交到 Git
- [ ] 已建立並推送版本標籤（如要自動發佈）

---

**建立時間**: 2025-10-08  
**工具版本**: 2.0.0  
**目標框架**: .NET 8.0
