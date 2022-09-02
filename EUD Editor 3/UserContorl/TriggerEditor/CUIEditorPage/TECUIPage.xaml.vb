Imports Newtonsoft.Json.Linq

Public Class TECUIPage
    Private PTEFile As TEFile
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function


    Public Sub RefreshData()
        Dim TString As String = PTEFile.RefreshData()
        If TString <> "" Then
            If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "True" Then
                NewTextEditor.SetFilePath = PTEFile.FileName
                NewTextEditor.Text = TString
            Else
                TextEditor.ExternerLoader()
            End If
        End If
    End Sub


    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim window As New TextEditorWindow(NewTextEditor.SelectedText)
        window.ShowDialog()

        NewTextEditor.SelectedText = window.TextString
    End Sub
    Public Sub New(tTEFile As TEFile, Optional highLightLine As Integer = -1)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile



        If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "True" Then
            TextEditor.Visibility = Visibility.Collapsed
            NewTextEditor.AddCustomMenuBtn("텍스트 에디터 열기", "Ctrl+T", Key.LeftCtrl, Key.T, New RoutedEventHandler(AddressOf MenuItem_Click))
            NewTextEditor.SetImportManager(Tool.EpsImportManager)
            NewTextEditor.SetImportManager(Tool.LuaImportManager)
            NewTextEditor.SetFilePath = PTEFile.GetPullPath()
            NewTextEditor.Text = CType(TEFile.Scripter, CUIScriptEditor).StringText
            NewTextEditor.OptionFilePath = Tool.GetCodeEditorSettingFolder
            NewTextEditor.LoadOption()

            If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
                NewTextEditor.IsDark = True
            Else
                NewTextEditor.IsDark = False
            End If
        Else
            NewTextEditor.Visibility = Visibility.Collapsed
            TextEditor.Init(tTEFile)
            TextEditor.Text = CType(TEFile.Scripter, CUIScriptEditor).StringText

            If highLightLine > -1 Then
                TextEditor.LineHightLight(highLightLine)
            End If
        End If
    End Sub


    Public Sub SaveData()
        If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "True" Then
            CType(TEFile.Scripter, CUIScriptEditor).StringText = NewTextEditor.Text
        Else
            CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
        End If
        TEFile.LastDataRefresh()
    End Sub



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        If TEFile.FileType = TEFile.EFileType.CUIEps Then

            If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "False" Then
                CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
            End If
        End If
    End Sub

    Private Sub TextEditor_Text_Change(sender As Object, e As EventArgs)
        pjData.SetDirty(True)

        Tool.EpsImportManager.CachedContainerRemove(PTEFile.GetPullPath())

        If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "False" Then
            CType(TEFile.Scripter, CUIScriptEditor).StringText = NewTextEditor.Text
        End If
    End Sub
End Class

