Imports ICSharpCode.AvalonEdit
Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json.Linq

Public Class TETextEditorPage
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

    End Sub

    Public Sub RefreshData()
        'Dim TString As String = PTEFile.RefreshData()
        'If TString <> "" Then
        '    If pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = "True" Then
        '        NewTextEditor.SetFilePath = PTEFile.FileName
        '        TextEditor.Text = TString
        '    Else
        '        OldTextEditor.ExternerLoader()
        '    End If
        'End If
    End Sub



    Public Sub New(tTEFile As TEFile, Optional highLightLine As Integer = -1, Optional startoffset As Integer = 0)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile

        TextEditor.Text = TEFile.Scripter.GetStringText()
    End Sub


    Public Sub SaveData()
        CType(TEFile.Scripter, RawTextScriptEditor).StringText = TextEditor.Text

        TEFile.LastDataRefresh()
    End Sub


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        If TEFile.FileType = TEFile.EFileType.CUIEps Then

            CType(TEFile.Scripter, RawTextScriptEditor).StringText = TextEditor.Text

        End If
    End Sub


    Private Sub TextEditor_TextChanged(sender As Object, e As TextChangedEventArgs)
        pjData.SetDirty(True)

    End Sub
End Class

