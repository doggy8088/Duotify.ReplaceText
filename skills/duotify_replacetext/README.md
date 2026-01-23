# Duotify.ReplaceText Skill

[![NuGet](https://img.shields.io/nuget/v/Duotify.ReplaceText.svg)](https://www.nuget.org/packages/Duotify.ReplaceText/)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> **安全的、可重複、可被 AI / Agent 呼叫的文字轉碼與批次替換能力**

本 Skill 是 [Duotify.ReplaceText](https://github.com/doggy8088/Duotify.ReplaceText) .NET Global Tool 的智慧型封裝，專為 AI Agent、MCP Server、CI/CD Pipeline 設計。

## 🎯 Skill 定位

| 層級 | 責任 |
|------|------|
| ReplaceText 原專案 | 編碼偵測、UTF-8 BOM 轉換、字串替換、`.gitignore` 套用、效能 |
| **本 Skill** | 安裝管理、版本控管、模式封裝、批次規則、dry-run 預設、安全護欄、AI 友善介面 |

## 🚀 快速開始

### 1. 安裝 Skill

```bash
# 使用 Python
pip install -e skills/duotify_replacetext

# 或直接執行
python skills/duotify_replacetext/src/duotify_replacetext.py install
```

### 2. 檢查環境

```bash
duotify-replacetext doctor
```

### 3. 執行轉換（預設為安全的 Dry Run 模式）

```bash
# 預覽變更（不實際修改）
duotify-replacetext run --target ./MyProject

# 確認執行
duotify-replacetext run --target ./MyProject --apply
```

## 📖 命令說明

### `install` - 安裝工具

```bash
duotify-replacetext install
```

自動安裝或更新 `Duotify.ReplaceText` .NET Global Tool。

### `doctor` - 環境檢查

```bash
duotify-replacetext doctor
```

檢查：
- ✅ .NET SDK 版本
- ✅ ReplaceText 工具安裝狀態
- ✅ 工具版本

### `run` - 執行轉換

```bash
duotify-replacetext run [options]
```

#### 參數

| 參數 | 說明 | 預設值 |
|------|------|--------|
| `--target` | 目標目錄或檔案路徑 | *必填* |
| `--old` | 要替換的原始字串 | - |
| `--new` | 替換後的新字串 | - |
| `--mode` | 處理模式 (M/MO/mo) | - |
| `--dry-run` | 測試模式 | `true` |
| `--apply` | 確認執行 | `false` |
| `--verbose` | 詳細輸出 | `false` |
| `--full-path` | 完整路徑 | `false` |
| `--gbk` | GBK 優先 | `false` |
| `--unknown` | 自動判斷未知檔案 | `false` |
| `--backup` | 建立備份 | `false` |
| `--output` | 輸出格式 (text/json) | `text` |

#### 使用範例

```bash
# 1. 預覽轉換（預設 dry-run）
duotify-replacetext run --target ./MyProject

# 2. 實際執行轉換
duotify-replacetext run --target ./MyProject --apply

# 3. 字串替換
duotify-replacetext run --target ./MyProject --old "http://" --new "https://" --apply

# 4. GBK 優先模式（處理簡體中文）
duotify-replacetext run --target ./MyProject --gbk --apply

# 5. 僅處理文字檔案
duotify-replacetext run --target ./MyProject --mode MO --apply

# 6. 詳細輸出
duotify-replacetext run --target ./MyProject --verbose --full-path

# 7. 進階：直接使用 CLI 旗標 (passthrough)
duotify-replacetext run --target ./MyProject -- /MO /GBK /V /F
```

## 🛡️ 安全設計

### 預設 Dry Run

Skill 預設為**安全模式**（dry-run），不會實際修改任何檔案：

```bash
# 這只會預覽，不會修改
duotify-replacetext run --target ./MyProject

# 要實際執行，必須明確加上 --apply
duotify-replacetext run --target ./MyProject --apply
```

### 備份功能（v2）

```bash
# 在修改前自動備份到 .replacetext-backup/
duotify-replacetext run --target ./MyProject --apply --backup
```

## 📦 批次替換（v2）

使用 JSON 規格檔進行多組替換：

```bash
duotify-replacetext run --spec ./mapping.json
```

`mapping.json` 範例：

```json
{
  "target": "./MyProject",
  "options": {
    "dryRun": false,
    "flags": ["MO", "GBK"]
  },
  "replacements": [
    { "old": "http://", "new": "https://" },
    { "old": "localhost", "new": "127.0.0.1" }
  ]
}
```

## 🤖 AI / Agent 整合

### JSON 輸出

```bash
duotify-replacetext run --target ./MyProject --output json
```

輸出範例：

```json
{
  "success": true,
  "mode": "dry-run",
  "target": "./MyProject",
  "filesScanned": 150,
  "filesChanged": 23,
  "summary": {
    "encodingConverted": 20,
    "stringsReplaced": 3
  }
}
```

### Exit Codes

| Code | 說明 |
|------|------|
| 0 | 成功 |
| 1 | 一般錯誤 |
| 2 | 工具未安裝 |
| 3 | 目標路徑不存在 |
| 4 | 權限不足 |

## 📁 檔案結構

```text
skills/duotify_replacetext/
├── README.md              # 本文件
├── skill.yaml             # MCP / Agent 用定義檔
├── scripts/
│   ├── install.ps1        # Windows 安裝腳本
│   └── install.sh         # Linux/macOS 安裝腳本
├── src/
│   └── duotify_replacetext.py  # Skill 主程式
└── examples/
    ├── simple.json        # 簡單替換範例
    └── mapping.json       # 批次替換範例
```

## 🔧 CLI 旗標對照表

| Skill 參數 | CLI 旗標 | 說明 |
|------------|----------|------|
| `--dry-run` | `/T` | 測試模式 |
| `--mode M` | `/M` | 修改已知文字檔 |
| `--mode MO` | `/MO` | 僅處理 TextExtensions |
| `--verbose` | `/V` | 詳細輸出 |
| `--full-path` | `/F` | 完整路徑 |
| `--gbk` | `/GBK` | GBK 優先 |
| `--unknown` | `/U` | 自動判斷未知檔案 |

## 📋 系統需求

- .NET 8.0 SDK 或更高版本
- Python 3.9+ （執行 Skill）

## 📄 授權

MIT License - 與原專案相同

## 🔗 相關連結

- [原專案 README](../../README.md)
- [NuGet 套件](https://www.nuget.org/packages/Duotify.ReplaceText/)
- [GitHub](https://github.com/doggy8088/Duotify.ReplaceText)
