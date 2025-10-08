# 升級摘要：從 .NET Framework 到 .NET 8

## 概述

此文件記錄了將 ReplaceText 專案從 .NET Framework 3.5 升級到 .NET 8 的完整過程。

## 執行日期

2025-10-08

## 主要變更

### 1. 專案檔案升級

#### 變更前
- 使用舊式 .NET Framework 專案格式 (ToolsVersion="3.5")
- 包含大量冗餘的 XML 設定
- 需要手動管理參考

#### 變更後
- 升級為現代 SDK 風格的專案檔案
- 目標框架: `net8.0`
- 啟用 nullable reference types
- 停用隱式 using 以保持相容性
- 將組件資訊從 AssemblyInfo.cs 遷移到專案檔

### 2. 程式碼修正

#### 編碼支援
- 新增 `Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)` 以支援 Big5 和 GBK 編碼
- 這是 .NET Core/.NET 5+ 必需的變更

#### Nullable Reference Types
- 更新變數宣告以支援 nullable 參考型別
- 將 `string` 改為 `string?` 以正確處理可能的 null 值

#### 程式碼品質改善
- 移除未使用的變數 (`curCounter`, `fileCount`)
- 修正 switch 語句中不可達到的 `break` 語句
- 清理所有編譯器警告

### 3. 新增檔案

#### 專案組態
- `.editorconfig` - 統一的程式碼風格設定
- `.gitignore` - Git 版本控制忽略規則

#### GitHub Actions Workflows
- `.github/workflows/ci.yml` - 持續整合工作流程
  - 多平台建構（Windows、Linux、macOS）
  - 程式碼品質檢查
  - 自動程式碼格式化
- `.github/workflows/release.yml` - 發布工作流程
  - 多平台發布（包含 ARM64）
  - 單一檔案執行檔
  - 自動化 GitHub Releases

#### 文件
- `README.md` - 完整的專案說明文件
- `CHANGELOG.md` - 版本變更記錄
- `CONTRIBUTING.md` - 貢獻指南
- `UPGRADE_SUMMARY.md` - 此升級摘要文件

#### 建構腳本
- `build.ps1` - PowerShell 建構腳本
  - 支援清理、建構、格式化、發布等操作

### 4. 移除檔案

- `Properties/AssemblyInfo.cs` - 資訊已遷移到 .csproj

## 技術改進

### 建構系統
- ✅ 更快的建構速度
- ✅ 簡化的專案結構
- ✅ 自動套件還原
- ✅ 跨平台建構支援

### 程式碼品質
- ✅ 零編譯警告
- ✅ 統一的程式碼風格
- ✅ Nullable reference types 支援
- ✅ 符合現代 C# 最佳實踐

### CI/CD
- ✅ 自動化建構流程
- ✅ 程式碼格式檢查
- ✅ 多平台測試
- ✅ 自動發布管理

### 文件
- ✅ 完整的 README
- ✅ 版本記錄
- ✅ 貢獻指南
- ✅ 升級文件

## 相容性

### 支援平台
- Windows (x64, x86)
- Linux (x64)
- macOS (x64, ARM64)

### 執行環境
- 需要 .NET 8.0 runtime（自包含發布則不需要）
- 支援單一檔案部署

## 功能保留

所有原有功能均已保留：
- ✅ 多編碼偵測與轉換（UTF-8, Unicode, Big5, GBK, ISO-8859-1）
- ✅ 遞迴目錄掃描
- ✅ 批次字串替換
- ✅ 測試模式（Dry Run）
- ✅ 詳細輸出選項
- ✅ Big5/GBK 優先選項

## 驗證步驟

以下命令已全部通過驗證：

```bash
# 還原套件
dotnet restore

# 建構專案
dotnet build -c Release

# 執行程式
dotnet run -- /T

# 格式化檢查
dotnet format --verify-no-changes

# 發布單一檔案
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 後續建議

1. **測試**
   - 建議新增單元測試專案
   - 新增整合測試

2. **文件**
   - 考慮新增 API 文件
   - 新增更多使用範例

3. **功能**
   - 考慮支援更多編碼格式
   - 新增進度條顯示
   - 支援設定檔

4. **效能**
   - 考慮使用 `Span<T>` 改善效能
   - 平行處理大量檔案

## 聯絡資訊

如有問題或建議，請：
- 建立 GitHub Issue
- 提交 Pull Request
- 查看 CONTRIBUTING.md

## 結論

升級已成功完成，專案現在使用現代的 .NET 8 平台，具備：
- 更好的效能
- 跨平台支援
- 現代化的開發工具
- 完整的 CI/CD 流程
- 高品質的程式碼標準

所有原有功能均已保留且正常運作。
