
---

# 🇨🇳 **README.md（中文版本）**

```markdown
# TrayIcon

TrayIcon 是一个轻量级的 Windows 托盘工具，可根据 CSV 文件动态生成菜单。  
左键复制文本，右键打开 URL 或文件路径，大幅提升日常工作效率。

---

## ✅ 功能特点

- 从 CSV 文件自动生成托盘菜单
- 支持三级结构：主菜单 → 子菜单 → 项目
- 左键：复制 `ContentItem` 到剪贴板
- 右键：使用默认应用程序执行 `ContentItem`
- “Reload” 可立即重新加载 CSV
- 常驻系统托盘，资源占用极低

---

## ✅ 运行环境

- Windows 10 / 11
- .NET Framework / .NET Runtime（依项目设置而定）
- CSV 文件（建议 UTF-8）

---

## ✅ CSV 格式说明

TrayIcon 读取以下四列结构的 CSV：

| 列名 | 说明 |
|------|------|
| MainMenu | 主菜单名称 |
| SubMenu | 子菜单名称（可为空） |
| DisplayItem | 菜单显示文本 |
| ContentItem | 要复制或执行的内容 |

### ✅ CSV 示例

```csv
Tools,Editors,Open Notepad,notepad.exe
Tools,Editors,Open VSCode,"C:\Program Files\Microsoft VS Code\Code.exe"
Links,Search,Google,https://www.google.com
Links,Search,Bing,https://www.bing.com
