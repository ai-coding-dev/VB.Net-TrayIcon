
---

# 🇺🇸 **README.md（English Version）**

```markdown
# TrayIcon

TrayIcon is a lightweight Windows tray utility that dynamically generates menu items based on CSV files.  
Left-click copies text to the clipboard, and right-click launches URLs or file paths, making daily workflow more efficient.

---

## ✅ Features

- Automatically generates tray menus from CSV files
- Supports a 3-level structure: Main Menu → Sub Menu → Items
- Left-click: Copy `ContentItem` to clipboard
- Right-click: Execute `ContentItem` using the default application
- “Reload” menu updates changes instantly
- Lightweight and always running in the system tray

---

## ✅ Requirements

- Windows 10 / 11
- .NET Framework / .NET Runtime (depending on project settings)
- CSV files (UTF-8 recommended)

---

## ✅ CSV Format

TrayIcon reads CSV files with the following 4-column structure:

| Column | Description |
|--------|-------------|
| MainMenu | Main menu name |
| SubMenu | Sub menu name (optional) |
| DisplayItem | Text displayed in the menu |
| ContentItem | Value to copy or execute |

### ✅ Sample CSV

```csv
Tools,Editors,Open Notepad,notepad.exe
Tools,Editors,Open VSCode,"C:\Program Files\Microsoft VS Code\Code.exe"
Links,Search,Google,https://www.google.com
Links,Search,Bing,https://www.bing.com
