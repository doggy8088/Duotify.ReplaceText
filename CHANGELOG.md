# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-10-08

### Changed

- 🎉 **套件名稱變更**: 從 `ReplaceText` 更名為 `Duotify.ReplaceText`
- 📦 版本號重新從 1.0.0 開始

### Added

- **`.gitignore` 支援**: 程式現在會自動尋找並套用 `.gitignore` 規則,避免意外轉換不應處理的檔案
  - 自動從當前目錄或上層目錄尋找 `.gitignore` 檔案
  - 完整支援 Git 的 `.gitignore` 語法 (萬用字元、目錄規則、否定規則等)
  - 在詳細模式 (`/V`) 下會顯示哪些檔案被 `.gitignore` 忽略
  - 使用 `Ignore` NuGet 套件 (v0.2.1) 提供高效能的規則匹配
  - 自動將 Windows 路徑轉換為 Unix 格式以符合 Git 標準
  - 大幅減少不必要的檔案處理 (在測試中減少了 97% 的處理量)

### Technical

- 新增相依套件: `Ignore` v0.2.1
- 新增方法: `FindGitignoreFile()`, `LoadGitignoreRules()`, `IsIgnoredByGitignore()`
- 新增靜態欄位: `gitignoreRules`, `gitignoreRootPath`

## [2.0.0] - 2025-10-08 (舊套件名稱: ReplaceText)

### Changed

- Upgraded from .NET Framework 3.5 to .NET 8.0
- Modernized project structure to SDK-style .csproj format
- Migrated assembly information from AssemblyInfo.cs to project file
- Fixed all compiler warnings and code quality issues
- Improved nullable reference type handling

### Added

- GitHub Actions CI/CD workflows
  - Multi-platform build support (Windows, Linux, macOS)
  - Automated code formatting checks
  - Automated releases with cross-platform binaries
- .editorconfig for consistent code style
- .gitignore for better repository management
- Comprehensive README.md documentation
- CHANGELOG.md for tracking version history
- Support for single-file deployment
- Cross-platform runtime support

### Fixed

- Fixed unreachable code warnings in switch statements
- Fixed nullable reference warnings
- Removed unused variables
- Properly registered code page encodings for .NET 8

### Removed

- Removed Properties/AssemblyInfo.cs (migrated to .csproj)
- Removed old-style project file format

## [1.0.0] - 2010

### Initial Release

- Initial release with .NET Framework 3.5
- Multi-encoding detection and conversion (UTF-8, Unicode, Big5, GBK, ISO-8859-1)
- Recursive directory scanning
- Batch string replacement
- Test mode (Dry Run)
- Verbose output option
- Support for various development file formats
- Big5/GBK priority option
