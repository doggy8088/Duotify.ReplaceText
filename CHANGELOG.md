# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-10-08

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
