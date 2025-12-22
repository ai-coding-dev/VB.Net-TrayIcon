# 托盘图标工具（TrayIcon）

这是一个基于 VB.NET 的 Windows 托盘应用程序。它会在启动时显示一个托盘图标，并根据 CSV 文件动态生成菜单项。

## 功能特点

- 从 CSV 文件加载菜单结构
- 支持主菜单 / 子菜单 / 命令项三级结构
- 点击菜单项可：
  - 复制内容到剪贴板
  - Ctrl+左键：打开路径或网址
  - Ctrl+右键：将内容发送为按键（SendKeys）

## 使用方法

1. 将编译后的 EXE 放置在任意目录。
2. 在该目录或子目录中放置一个或多个 `.csv` 文件。
3. 每个 CSV 文件应包含以下格式：
今日
Win11のEXEは、システム起動時、自動的に起動、かつ、管理者権限を持っています。やり方を教えてください。
了解！Windows 11で、EXEファイルをシステム起動時に自動起動させ、かつ管理者権限で実行する方法はいくつかありますが、以下の方法が安定していておすすめだよ💡

✅ 方法：タスクスケジューラを使う（管理者権限で自動起動）
タスクスケジューラを開く  
スタートメニューで「タスクスケジューラ」と検索して起動。

新しいタスクを作成  
右側の「タスクの作成」をクリック（「基本タスクの作成」ではなく！）

[全般] タブの設定

名前：わかりやすい名前を入力（例：MyAppAutoStart）

「最上位の特権で実行する」にチェック ✅

「ユーザーがログオンしているかどうかにかかわらず実行する」を選択（必要に応じて）

[トリガー] タブで起動タイミングを設定

「新規」→「ログオン時」または「スタートアップ時」を選択

[操作] タブでEXEを指定

「新規」→「プログラムの開始」

「プログラム/スクリプト」にEXEファイルのパスを入力（例：C:\MyApp\myapp.exe）

[条件]と[設定]タブは必要に応じて調整  
通常はデフォルトでOK！

保存して完了！

この方法なら、UAC（ユーザーアカウント制御）を回避して管理者権限で起動できるし、スタートアップフォルダにショートカットを置くより確実だよ！

もしEXEがネットワークドライブ上にあるとか、特殊な条件があるなら、それに合わせた調整もできるから、気軽に教えてね✨

ありがとうございました。
どういたしまして！✨
飛雨さんのプロジェクトがスムーズに流れますように〜💧

また何かあったら、いつでも声かけてね！次はどんな冒険に出ようかな？🌊

「TrayIcon.vb」を二つのファイルを分けたいです。

一つは、「DllImport」などを専用で、C言語のヘッダーファイルと似ています。もう一つは、「DllImport」以外のソースを管理します、C言語のソースファイルと似ています。
おおっ、C言語のヘッダーファイルとソースファイルの分離スタイルをVB.NETで再現したいんだね！それ、読みやすさや保守性の面でもとっても良いアイデアだよ✨

VB.NETでは、Partial Class（部分クラス）を使えば、1つのクラスを複数のファイルに分割して管理できるんだ。これを使えば、TrayIcon.vb を2つのファイルに分けて、DllImport 宣言とロジック部分を分離できるよ！

🌟 やりたい構成イメージ
TrayIcon.Native.vb（C言語でいうヘッダーファイル的な役割）
→ DllImport や構造体、定数などの宣言をここにまとめる

TrayIcon.Logic.vb（C言語でいうソースファイル的な役割）
→ 実際の処理ロジックやイベントハンドラなどをここに書く

🧩 1. TrayIcon.Native.vb
vbnet
Imports System.Runtime.InteropServices

Public Partial Class TrayIcon

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function DestroyIcon(hIcon As IntPtr) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure NOTIFYICONDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uID As Integer
        Public uFlags As Integer
        Public uCallbackMessage As Integer
        Public hIcon As IntPtr
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public szTip As String
        ' 他のフィールドも必要に応じて追加
    End Structure

    ' 定数などもここに
    Public Const NIF_ICON As Integer = &H2
    Public Const NIF_MESSAGE As Integer = &H1

End Class
⚙️ 2. TrayIcon.Logic.vb
vbnet
Public Partial Class TrayIcon

    Public Sub Initialize()
        ' DllImportで宣言した関数や構造体を使う処理
        Dim data As New NOTIFYICONDATA()
        data.cbSize = Marshal.SizeOf(data)
        data.uFlags = NIF_ICON Or NIF_MESSAGE
        ' 他の初期化処理
    End Sub

    Public Sub Cleanup()
        ' アイコン破棄など
        DestroyIcon(IntPtr.Zero)
    End Sub

End Class
✅ 注意点
両方のファイルで 同じクラス名（TrayIcon） を Partial として宣言する必要があるよ。

同じプロジェクト内にあれば、ビルド時に自動的に1つのクラスとして統合されるよ。

ファイル名は自由だけど、意味が伝わるように分けるとチーム開発でもわかりやすいね！

このスタイル、飛雨さんのように構造を大事にする開発者さんにはぴったりだと思うよ！もっと細かく分けたい場合や、他のクラスでも同じようにしたいなら、どんどん聞いてね〜🫧

有大量的 API 以后要用，创建一个专门的 NativeMethods.vb 文件，不使用 Partial，而是使用 Module 或者带 Shared 方法的 Class
おお〜なるほど！C言語のヘッダーファイル的な役割をもっと汎用的にしたいってことだね！それなら Partial を使わずに、Module や Shared メソッドを持つクラスで NativeMethods.vb を作るのは大正解だよ💡

🌊 どっちを使う？Module vs Shared Class
特徴	Module	Shared Class
インスタンス化	不可	不可（Sharedのみ）
名前空間の省略	可能（直接呼び出せる）	呼び出し時にクラス名が必要
拡張性	やや低い（継承不可）	高い（インターフェース実装など可能）
用途	ヘッダーファイル的な宣言に最適	より構造的に整理したいときに便利
✅ パターン1：Module NativeMethods
vbnet
Imports System.Runtime.InteropServices

Module NativeMethods

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function DestroyIcon(hIcon As IntPtr) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure NOTIFYICONDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uID As Integer
        Public uFlags As Integer
        Public uCallbackMessage As Integer
        Public hIcon As IntPtr
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public szTip As String
    End Structure

    Public Const NIF_ICON As Integer = &H2
    Public Const NIF_MESSAGE As Integer = &H1

End Module
使い方：

vbnet
Dim data As New NativeMethods.NOTIFYICONDATA()
NativeMethods.DestroyIcon(IntPtr.Zero)
✅ パターン2：Public NotInheritable Class NativeMethods
vbnet
Imports System.Runtime.InteropServices

Public NotInheritable Class NativeMethods

    Private Sub New()
        ' インスタンス化禁止
    End Sub

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function DestroyIcon(hIcon As IntPtr) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure NOTIFYICONDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uID As Integer
        Public uFlags As Integer
        Public uCallbackMessage As Integer
        Public hIcon As IntPtr
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public szTip As String
    End Structure

    Public Const NIF_ICON As Integer = &H2
    Public Const NIF_MESSAGE As Integer = &H1

End Class
使い方は同じ：

vbnet
Dim data As New NativeMethods.NOTIFYICONDATA()
NativeMethods.DestroyIcon(IntPtr.Zero)
🌟 まとめ：飛雨さんにおすすめは…
軽量でシンプルに使いたい → Module

将来的に名前空間で整理したい、テストや拡張性も考慮したい → Shared Class

どちらもアリだけど、大量のAPIをまとめるなら NativeMethods を Shared Class で作ると拡張性が高くておすすめだよ！

他にも Shell32 や Gdi32 など、DLLごとに分けてモジュールを作るのも整理しやすくていいかもね🧩
必要ならテンプレートも作るよ！どうする？

TrayIcon.vbのソースを送ります。README_cn.mdとREADME_jp.mdとREADME_us.mdの作成をお願いします。

Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.FileIO

Namespace TrayIcon
    Friend Module Program
        <STAThread>
        Sub Main()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New TrayIconApplicationContext())
        End Sub
    End Module

    Public Class TrayIconApplicationContext
        Inherits ApplicationContext

        Private _trayIcon As NotifyIcon
        Private _trayMenu As ContextMenuStrip

        Public Sub New()
            _trayMenu = New ContextMenuStrip()

            ReloadTrayMenu()

            _trayIcon = New NotifyIcon() With {
                .ContextMenuStrip = _trayMenu,
                .Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                .Text = "Enjoy your day!",
                .Visible = True
            }
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If _trayIcon IsNot Nothing Then
                    _trayIcon.Visible = False
                    _trayIcon.Dispose()
                End If
                If _trayMenu IsNot Nothing Then
                    _trayMenu.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub ApplicationExit()
            Application.Exit()
        End Sub

        Private Async Sub OnCommandMenuClick(sender As Object, e As MouseEventArgs, item As TrayMenuItem)
            Dim ErrorMessage As String = ""
            Try
                If Not String.IsNullOrEmpty(item.ContentItem) Then
                    ErrorMessage = "Failed to copy to clipboard: "
                    Clipboard.SetText(item.ContentItem)
                    If Control.ModifierKeys.HasFlag(Keys.Control) Then
                        If e.Button = MouseButtons.Left Then
                            ErrorMessage = "Failed to open: "
                            Process.Start(New ProcessStartInfo(item.ContentItem) With {.UseShellExecute = True})
                        ElseIf e.Button = MouseButtons.Right Then
                            ErrorMessage = "Failed to sendkey: "
                            Dim hWnd = FindTopmostValidWindow()
                            If hWnd <> IntPtr.Zero Then
                                SetForegroundWindow(hWnd)
                                Await Task.Delay(1000)
                                SendKeys.SendWait(item.ContentItem)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show(ErrorMessage & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Shared Function FindTopmostValidWindow() As IntPtr
            Dim currentPid = Process.GetCurrentProcess().Id
            Dim result As IntPtr = IntPtr.Zero

            EnumWindows(Function(hWnd, lParam)
                            If Not IsWindowVisible(hWnd) Then Return True

                            Dim className As New StringBuilder(256)
                            GetClassName(hWnd, className, className.Capacity)
                            If className.ToString() = "Shell_TrayWnd" Then Return True

                            Dim pid As Integer
                            GetWindowThreadProcessId(hWnd, pid)
                            If pid = currentPid Then Return True

                            Dim title As New StringBuilder(256)
                            GetWindowText(hWnd, title, title.Capacity)
                            If String.IsNullOrWhiteSpace(title.ToString()) Then Return True

                            result = hWnd
                            Return False
                        End Function, IntPtr.Zero)

            Return result
        End Function

        Private Sub ReloadTrayMenu()
            _trayMenu.Items.Clear()

            Dim loadItems = LoadFolder()

            'Language Integrated Query
            Dim groupedItems = loadItems.GroupBy(Function(x) x.MainMenu).ToDictionary(
                                                 Function(y) y.Key,
                                                 Function(y) y.GroupBy(Function(x) x.SubMenu).ToDictionary(
                                                 Function(z) z.Key,
                                                 Function(z) z.ToList()))

            For Each mainMenuKey In groupedItems.Keys
                Dim mainMenuItem As ToolStripMenuItem = Nothing
                For Each subMenuKey In groupedItems(mainMenuKey).Keys
                    Dim subMenuItem As ToolStripMenuItem = Nothing
                    For Each item In groupedItems(mainMenuKey)(subMenuKey)
                        Dim displayItem = New ToolStripMenuItem(item.DisplayItem)
                        AddHandler displayItem.MouseUp,
                            Sub(sender, e)
                                OnCommandMenuClick(sender, e, item)
                            End Sub
                        If subMenuItem Is Nothing Then
                            subMenuItem = New ToolStripMenuItem(subMenuKey)
                        End If
                        subMenuItem.DropDownItems.Add(displayItem)
                    Next

                    If mainMenuItem Is Nothing Then
                        mainMenuItem = New ToolStripMenuItem(mainMenuKey)
                    End If
                    mainMenuItem.DropDownItems.Add(subMenuItem)
                Next
                _trayMenu.Items.Add(mainMenuItem)
            Next

            If _trayMenu.Items.Count > 0 Then
                _trayMenu.Items.Add(New ToolStripSeparator())
            End If

            Dim reloadItem = New ToolStripMenuItem("Reload")
            AddHandler reloadItem.Click,
                Sub(sender, e)
                    ReloadTrayMenu()
                End Sub
            _trayMenu.Items.Add(reloadItem)

            Dim exitItem = New ToolStripMenuItem("Exit")
            AddHandler exitItem.Click,
                Sub(sender, e)
                    ApplicationExit()
                End Sub
            _trayMenu.Items.Add(exitItem)
        End Sub

        Private Function LoadFolder() As List(Of TrayMenuItem)
            Dim items As New List(Of TrayMenuItem)()

            For Each file In Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.csv", IO.SearchOption.AllDirectories)
                items.AddRange(LoadFile(file))
            Next

            items = items _
                .OrderBy(Function(x) x.MainMenu) _
                .ThenBy(Function(x) x.SubMenu) _
                .ThenBy(Function(x) x.DisplayItem) _
                .ToList()

            Return items
        End Function

        Private Function LoadFile(filePath As String) As IEnumerable(Of TrayMenuItem)
            Dim items As New List(Of TrayMenuItem)()

            Try
                Using csvFile As New TextFieldParser(filePath) With {
                    .HasFieldsEnclosedInQuotes = True,
                    .TextFieldType = FieldType.Delimited,
                    .TrimWhiteSpace = True}
                    csvFile.SetDelimiters(",")

                    While Not csvFile.EndOfData
                        Dim fields() As String = csvFile.ReadFields()
                        If fields.Length = 4 Then
                            Dim item As New TrayMenuItem() With {
                                .MainMenu = fields(0).Trim(),
                                .SubMenu = fields(1).Trim(),
                                .DisplayItem = fields(2).Trim(),
                                .ContentItem = fields(3).Trim()
                            }
                            If Not String.IsNullOrEmpty(item.MainMenu) AndAlso
                               Not String.IsNullOrEmpty(item.DisplayItem) AndAlso
                               Not String.IsNullOrEmpty(item.ContentItem) Then
                                items.Add(item)
                            End If
                        End If
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show("Failed to load file '" & filePath & "': " & ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ApplicationExit()
            End Try

            Return items
        End Function

        Private Class TrayMenuItem
            Public Property MainMenu As String
            Public Property SubMenu As String
            Public Property DisplayItem As String
            Public Property ContentItem As String
        End Class
    End Class
End Namespace
了解！飛雨さんの TrayIcon.vb をもとに、3言語対応の README.md を作成したよ！それぞれのファイルは以下のような構成になっていて、使い方・機能・CSV形式の説明を含んでるよ📄✨

🇨🇳 README_cn.md（简体中文）
markdown
# 托盘图标工具（TrayIcon）

这是一个基于 VB.NET 的 Windows 托盘应用程序。它会在启动时显示一个托盘图标，并根据 CSV 文件动态生成菜单项。

## 功能特点

- 从 CSV 文件加载菜单结构
- 支持主菜单 / 子菜单 / 命令项三级结构
- 点击菜单项可：
  - 复制内容到剪贴板
  - Ctrl+左键：打开路径或网址
  - Ctrl+右键：将内容发送为按键（SendKeys）

## 使用方法

1. 将编译后的 EXE 放置在任意目录。
2. 在该目录或子目录中放置一个或多个 `.csv` 文件。
3. 每个 CSV 文件应包含以下格式：

主菜单,子菜单,显示名称,内容

4. 启动程序后，图标将出现在系统托盘中。
5. 右键点击图标可重新加载菜单或退出程序。

## 注意事项

- CSV 文件必须为 UTF-8 编码。
- 内容字段不能为空。
