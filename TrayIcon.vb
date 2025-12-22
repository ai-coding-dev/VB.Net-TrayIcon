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