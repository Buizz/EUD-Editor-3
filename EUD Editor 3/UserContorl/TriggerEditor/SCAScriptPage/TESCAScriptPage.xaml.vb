Imports Newtonsoft.Json.Linq

Public Class TESCAScriptPage
    Private PTEFile As TEFile
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function


    Public Sub Deactivated()
        If NewTextEditor.Visibility <> Visibility.Collapsed Then
            NewTextEditor.Deactivated()
        End If
    End Sub

    Public Sub RefreshData()
        Dim TString As String = PTEFile.RefreshData()
        If TString <> "" Then
            NewTextEditor.SetFilePath = PTEFile.FileName
            NewTextEditor.Text = TString
        End If
    End Sub


    Public Sub New(tTEFile As TEFile, Optional highLightLine As Integer = -1)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile

        NewTextEditor.SetFilePath = PTEFile.GetPullPath()
        NewTextEditor.SetImportManager(Tool.SCAScriptImportManager)
        NewTextEditor.Text = CType(TEFile.Scripter, SCAScriptEditor).StringText
        NewTextEditor.OptionFilePath = Tool.GetCodeEditorSettingFolder
        NewTextEditor.LoadOption("Lua")
        NewTextEditor.TabSizeTextBoxRefresh()

        Dim foldedList As List(Of Integer) = CType(TEFile.Scripter, SCAScriptEditor).foldedData
        If foldedList IsNot Nothing Then
            NewTextEditor.LoadFolding(foldedList)
        End If

        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            NewTextEditor.IsDark = True
        Else
            NewTextEditor.IsDark = False
        End If
    End Sub


    Public Sub SaveData()
        CType(TEFile.Scripter, SCAScriptEditor).StringText = NewTextEditor.Text
        TEFile.LastDataRefresh()
    End Sub



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        If TEFile.FileType = TEFile.EFileType.CUIEps Then


            CType(TEFile.Scripter, SCAScriptEditor).foldedData = NewTextEditor.SaveFolding()
        End If
    End Sub

    Private Sub TextEditor_Text_Change(sender As Object, e As EventArgs)
        pjData.SetDirty(True)
        'Tool.EpsImportManager.CachedContainerRemove(PTEFile.GetPullPath())

        CType(TEFile.Scripter, SCAScriptEditor).StringText = NewTextEditor.Text
        'If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "True" Then
        'End If
    End Sub
End Class

