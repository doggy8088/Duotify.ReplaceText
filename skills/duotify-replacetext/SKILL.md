# Skill: Duotify.ReplaceText（ReplaceText）

## 目的

此 Skill 用於在專案/目錄中進行：

1. 文字檔案編碼自動偵測與轉換（預設轉為 UTF-8 with BOM）
2. 批次字串替換（可選）
3. 遞迴掃描目錄，並自動套用 .gitignore 規則以避免處理不該處理的檔案

底層工具採用 .NET 8+ 的 Global Tool：`Duotify.ReplaceText`，安裝後使用 `replacetext` 命令。

---

## 系統需求

- .NET SDK 8.0 以上
- Windows / Linux / macOS 皆可

---

## 安裝（自動化）

### Windows（PowerShell）

```powershell
pwsh ./install/install.ps1
```

### Linux/macOS（bash）

```bash
bash ./install/install.sh
```

---

## 快速開始（常用情境）

> 所有範例都可參考 `examples/` 資料夾的腳本。

### 1) 轉換目錄中所有檔案為 UTF-8 with BOM（預設行為）

```bash
replacetext /path/to/your/project
```

### 2) Dry Run（不修改檔案，先預覽）

```bash
replacetext /T /path/to/your/project
```

### 3) 替換字串並轉換編碼

```bash
replacetext /path/to/your/project "oldText" "newText"
```

### 4) 詳細輸出（列出掃描檔案 + 顯示完整路徑）

```bash
replacetext /V /F /path/to/your/project
```

### 5) GBK 優先模式（簡體中文常用）

```bash
replacetext /GBK /path/to/your/project
```

### 6) 僅處理 TextExtensions 清單內的文字檔（/MO，會隱含 /M）

```bash
replacetext /MO /path/to/your/project
# 或使用簡短別名
replacetext -mo /path/to/your/project
```

### 7) 自動判斷未知副檔名或無副檔名檔案（/U）

```bash
replacetext /U /path/to/your/project
```

---

## 參數摘要

| 參數 | 說明 |
|------|------|
| `/T` | Dry Run（不寫入） |
| `/MO` 或 `-mo` | 僅處理 TextExtensions（隱含 `/M`） |
| `/M` | 額外處理常見文字檔（預設會跳過 .txt/.csv） |
| `/V` | 詳細輸出（列出掃描檔案） |
| `/F` | 顯示完整路徑 |
| `/GBK` | GBK 優先於 Big5 判斷 |
| `/U` | 未知檔案類型也嘗試以文字方式處理（非文字會跳過） |

---

## 建議操作流程（避免翻車）

1. **先用 `/T` Dry Run 檢查**（尤其是大 repo）
2. 必要時加 `/V /F` 做可追溯的清單輸出
3. 若是中文檔案混雜，依情況加 `/GBK`
4. 若只想處理文字檔（.md/.txt/.json/.yml 等），用 `/MO`

---

## 批次 Mapping 替換（進階）

當你需要一次執行多組字串替換時，可使用 `examples/10-batch-mapping.ps1` 或 `10-batch-mapping.sh`。

### 規則檔格式（mapping.sample.json）

```json
{
  "target": "./MyProject",
  "flags": ["/MO", "/T"],
  "replacements": [
    { "old": "http://", "new": "https://" },
    { "old": "localhost", "new": "127.0.0.1" }
  ]
}
```

### 執行方式

```powershell
# Windows
pwsh ./examples/10-batch-mapping.ps1 -MappingJson ./examples/mapping.sample.json

# Linux/macOS
bash ./examples/10-batch-mapping.sh ./examples/mapping.sample.json
```

---

## 支援的檔案格式

預設支援以下檔案格式：

- **.NET/Visual Studio**: .cs, .vb, .vbs, .cshtml, .vbhtml, .razor, .aspx, .ascx, .ashx, .master, .asmx, .resx, .settings, .edmx, .dbml, .rdlc, .sln, .csproj, .vbproj, .wdproj
- **Web (JavaScript/TypeScript)**: .js, .jsx, .ts, .tsx, .mjs, .cjs, .html, .htm, .css, .scss, .sass, .less, .vue, .svelte
- **配置與資料**: .config, .xml, .xsd, .xsl, .xslt, .sitemap, .skin, .browser, .disco, .wsdl, .discomap, .webinfo, .cd, .wsf
- **Classic ASP**: .asp, .asa, .asax
- **ActionScript**: .as, .jsl
- **Python**: .py, .pyw
- **Java**: .java
- **C/C++**: .cpp, .c, .h, .hpp, .cc, .cxx, .hxx
- **Go**: .go
- **Rust**: .rs
- **PHP**: .php, .phtml
- **Ruby**: .rb, .erb
- **Swift**: .swift
- **Kotlin**: .kt, .kts
- **Scala**: .scala
- **Shell**: .sh, .bash, .zsh
- **PowerShell**: .ps1, .psm1
- **Dart**: .dart
- **Objective-C**: .m, .mm
- **R**: .r, .R
- **SQL**: .sql
- **Perl**: .pl, .pm
- **Lua**: .lua
- **Groovy/Gradle**: .groovy, .gradle

使用 `/M` 選項可額外處理：

- .txt, .md, .log, .csv, .ini, .json, .yml, .yaml, .properties, .toml, .env, .lock, .conf, .cfg, .gitignore, .editorconfig

---

## 相關連結

- [NuGet 套件](https://www.nuget.org/packages/Duotify.ReplaceText/)
- [GitHub 專案](https://github.com/doggy8088/Duotify.ReplaceText)
- [原專案 README](../../README.md)
