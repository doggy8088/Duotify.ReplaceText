# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

## [1.1.2] - 2025-10-28

### Added

- 新增 `.razor` 檔案類型支援 (Blazor 元件)
- 新增多種程式語言檔案格式支援:
  - **Web 前端**: TypeScript (.ts, .tsx), JSX (.jsx), Vue (.vue), Svelte (.svelte), CSS 預處理器 (.scss, .sass, .less)
  - **後端語言**: Python (.py, .pyw), Java (.java), Go (.go), Rust (.rs), PHP (.php, .phtml), Ruby (.rb, .erb), Swift (.swift), Kotlin (.kt, .kts), Scala (.scala)
  - **系統程式語言**: C/C++ (.cpp, .c, .h, .hpp, .cc, .cxx, .hxx), Objective-C (.m, .mm)
  - **腳本語言**: Shell (.sh, .bash, .zsh), PowerShell (.ps1, .psm1), Perl (.pl, .pm), Lua (.lua), R (.r, .R)
  - **其他**: Dart (.dart), Groovy/Gradle (.groovy, .gradle), SQL (.sql)
  - **JavaScript 模組**: ES 模組 (.mjs), CommonJS (.cjs)
- 新增文字檔案格式: .gitignore, .editorconfig

## [1.1.0] - 2025-10-09

### Added

- 透過 [UTF.Unknown](https://github.com/CharsetDetector/UTF-unknown) 自動偵測未知編碼

## [1.0.0] - 2025-10-08

### Changed

- 🎉 **套件名稱變更**: 從 `ReplaceText` 更名為 `Duotify.ReplaceText`
- 專案從 .NET Framework 3.5 升級到 .NET 8.0，現代化為 SDK-style .csproj，並將組件資訊從 AssemblyInfo.cs 移至專案檔
- 修正所有編譯器警告與程式碼品質問題，改善可為 null 參考型別處理

### Added

- **`.gitignore` 支援**:
  - 程式會自動從當前或上層目錄尋找 `.gitignore` 檔案並套用規則
  - 完整支援 Git 的 `.gitignore` 語法（萬用字元、目錄規則、否定規則等）
  - 在詳細模式 (`/V`) 下會顯示哪些檔案被 `.gitignore` 忽略
  - 使用 `Ignore` NuGet 套件 (v0.2.1) 提供高效能的規則匹配
  - 自動將 Windows 路徑轉換為 Unix 格式以符合 Git 標準
  - 大幅減少不必要的檔案處理（測試中減少約 97% 的處理量）
- GitHub Actions CI/CD 工作流程
  - 多平台建置支援（Windows、Linux、macOS）
  - 自動化程式碼格式化檢查與靜態檢查
  - 自動化發佈（包含跨平台二進位檔）
- 新增 `.editorconfig`、`.gitignore`（倉儲管理）、完整 README.md 文件，以及 CHANGELOG.md
- 支援單一檔案部署與跨平台執行支援

### Technical

- 新增相依套件: `Ignore` v0.2.1
- 新增方法: `FindGitignoreFile()`, `LoadGitignoreRules()`, `IsIgnoredByGitignore()`
- 新增靜態欄位: `gitignoreRules`, `gitignoreRootPath`
- 將組件資訊由 Properties/AssemblyInfo.cs 移除並遷移至 .csproj
- 現代化專案結構，移除舊式專案檔格式

### Fixed

- 修正 switch 陳述式中無法到達的程式碼警告
- 已正確註冊 .NET 8 的程式碼頁編碼
- 修正 nullable 參考型別相關警告
- 移除未使用的變數

### Removed

- 移除 Properties/AssemblyInfo.cs（已遷移至 .csproj）
- 移除舊式專案檔格式，改用 SDK-style .csproj

## [0.1.0] - 2010

### Initial Release

- Initial release with .NET Framework 3.5
- Multi-encoding detection and conversion (UTF-8, Unicode, Big5, GBK, ISO-8859-1)
- Recursive directory scanning
- Batch string replacement
- Test mode (Dry Run)
- Verbose output option
- Support for various development file formats
- Big5/GBK priority option
