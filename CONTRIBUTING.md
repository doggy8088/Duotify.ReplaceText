# 貢獻指南

感謝您考慮為 ReplaceText 專案做出貢獻！

## 如何貢獻

### 回報問題

如果您發現了 bug 或有功能建議，請：

1. 檢查 [Issues](https://github.com/doggy8088/ReplaceText/issues) 確認問題尚未被回報
2. 建立新的 Issue，提供詳細資訊：
   - 問題描述
   - 重現步驟
   - 預期行為
   - 實際行為
   - 環境資訊（作業系統、.NET 版本等）

### 提交程式碼

1. Fork 此專案
2. 建立您的功能分支 (`git checkout -b feature/AmazingFeature`)
3. 確保程式碼符合專案風格（執行 `dotnet format`）
4. 提交您的變更 (`git commit -m 'Add some AmazingFeature'`)
5. 推送到分支 (`git push origin feature/AmazingFeature`)
6. 建立 Pull Request

### 程式碼風格

本專案使用 `.editorconfig` 定義程式碼風格。在提交前：

```bash
# 自動格式化程式碼
dotnet format

# 驗證程式碼格式
dotnet format --verify-no-changes
```

### 開發流程

1. **設定開發環境**

   ```bash
   git clone https://github.com/doggy8088/ReplaceText.git
   cd ReplaceText
   dotnet restore
   ```

2. **建構專案**

   ```bash
   dotnet build
   ```

3. **執行程式**

   ```bash
   dotnet run --project ReplaceText -- /T
   ```

4. **使用建構腳本**

   ```powershell
   # 完整建構
   .\build.ps1 -Configuration Release

   # 格式化程式碼
   .\build.ps1 -Format

   # 驗證格式
   .\build.ps1 -Verify

   # 發布所有平台
   .\build.ps1 -Publish
   ```

### Commit 訊息規範

我們遵循 [Conventional Commits](https://www.conventionalcommits.org/) 規範：

- `feat:` 新功能
- `fix:` 錯誤修復
- `docs:` 文件更新
- `style:` 程式碼格式調整（不影響功能）
- `refactor:` 重構（不修改功能）
- `perf:` 效能改善
- `test:` 測試相關
- `chore:` 建構流程或輔助工具變更

範例：

```
feat: add support for UTF-16 encoding
fix: correct GBK character detection
docs: update README with new examples
```

### Pull Request 指南

在提交 PR 前，請確保：

- [ ] 程式碼已通過建構 (`dotnet build`)
- [ ] 程式碼格式已驗證 (`dotnet format --verify-no-changes`)
- [ ] 已更新相關文件
- [ ] PR 描述清楚說明變更內容
- [ ] 已測試變更功能

### 需要協助？

如有任何問題，歡迎在 Issue 中詢問或聯繫維護者。

## 行為準則

### 我們的承諾

為了營造開放且友善的環境，我們承諾讓所有人都能無騷擾地參與本專案。

### 我們的標準

正面行為包括：

- 使用友善和包容的語言
- 尊重不同的觀點和經驗
- 優雅地接受建設性批評
- 專注於對社群最有利的事
- 對其他社群成員表現同理心

### 執行

不當行為可向專案維護者回報。所有投訴都會被審查和調查。

## 授權

提交貢獻即表示您同意您的貢獻將依照本專案相同的授權條款授權。
