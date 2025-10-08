# 如何貢獻

感謝您對 `Duotify.ReplaceText` 的興趣！本文件說明如何貢獻程式碼、回報問題與提交 Pull Request (PR)。

基本原則

- 請保持尊重、建設性與專業。若專案另有 `CODE_OF_CONDUCT`，請遵守其規範。

回報問題 (Issue)

1. 在提交 issue 前，請先搜尋現有 issue 與 `docs/` 中的相關文件 (例如 `docs/QUICKSTART.md`、`docs/DOTNET_GLOBAL_TOOL_SETUP.md`)，確認問題尚未被回報或有相關討論。
2. 新增 issue 時請提供：重現步驟、預期行為、實際行為、平台與版本資訊 (例如 Windows 版本、.NET SDK 版本)。必要時附上小型重現範例或輸出截圖／錯誤訊息。

開發流程 (Fork → Branch → PR)

1. Fork 本專案並在本機 clone：

   - 建議使用 PowerShell (本專案多數貢獻者以 Windows / PowerShell 開發)：

     - `git clone <your-fork-url>`

     - `git remote add upstream https://github.com/doggy8088/ReplaceText.git`

2. 建立分支：

   - 分支命名建議：`feature/<短描述>`、`fix/<短描述>`、`docs/<短描述>`。

3. 在分支上完成變更，確保變更單一、敘述清楚。

建立與執行 (在專案根目錄)

- 建置：
  - `dotnet build ReplaceText.sln -c Debug`

- 執行主程式：
  - `dotnet run --project ReplaceText -- -h` (或依需要帶入參數)

- 若專案包含測試，請執行：
  - `dotnet test`

- 若提供自動化建置腳本，可使用：
  - PowerShell: `.\build.ps1` (視腳本參數而定)

程式碼風格與品質

- 請遵守 .NET 的一般程式碼風格與可讀性原則。
- 若可能，請新增或更新對應的單元測試以涵蓋修正或新功能。
- 使用 `dotnet format` 或相同工具保持格式一致 (若未安裝，請先安裝相依工具)。

提交紀錄 (Commit)

- 使用具描述性的提交訊息。建議採用 Conventional Commits (例如 `feat: 新增 X`、`fix: 修正 Y`、`docs: 更新說明`)。

Pull Request 要求

- PR 請對應單一議題 (issue)，並在 PR 描述中說明：
  - 為何需要此變更
  - 變更內容的重點 (包含範例或輸出)
  - 若有相依的外部變更或注意事項請一併說明
- 所有自動化檢查 (CI) 須通過後才會進行合併審核。

授權與簽署

- 送出 PR 即表示您同意以本專案的授權條款提供貢獻 (請參照專案根目錄的 `LICENSE`)。如需其他法律文件 (例如 CLA)，會另行說明。

聯絡與支援

- 如需即時討論，請在 issue 中標註維護者或使用專案提供之通訊管道 (若有)。

致謝

感謝您的時間與貢獻！任何改善專案的建議或修正都非常歡迎。
