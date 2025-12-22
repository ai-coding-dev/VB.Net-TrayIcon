# TrayIcon

TrayIcon は、CSV ファイルからメニュー構造を読み込み、Windows のタスクトレイに動的なメニューを生成する軽量ユーティリティです。  
左クリックでテキストコピー、右クリックで URL やファイルパスを実行でき、日常業務の効率化に役立ちます。

---

## ✅ 主な機能

- CSV ファイルからメニューを自動生成
- メインメニュー → サブメニュー → 項目 の 3 階層構造に対応
- 左クリック：`ContentItem` をクリップボードにコピー
- 右クリック：`ContentItem` を既定アプリで実行（URL / ファイルパス）
- メニューの「Reload」で CSV の変更を即時反映
- タスクトレイ常駐型の軽量アプリケーション

---

## ✅ 動作環境

- Windows 10 / 11
- .NET Framework / .NET Runtime（プロジェクト設定に依存）
- CSV ファイル（UTF-8 推奨）

---

## ✅ CSV 仕様

TrayIcon は、以下の 4 列構造の CSV を読み込みます。

| 列名 | 説明 |
|------|------|
| MainMenu | メインメニュー名 |
| SubMenu | サブメニュー名（空欄可） |
| DisplayItem | メニューに表示される文字列 |
| ContentItem | コピーまたは実行される内容 |

### ✅ CSV サンプル

```csv
Tools,Editors,Open Notepad,notepad.exe
Tools,Editors,Open VSCode,"C:\Program Files\Microsoft VS Code\Code.exe"
Links,Search,Google,https://www.google.com
Links,Search,Bing,https://www.bing.com
