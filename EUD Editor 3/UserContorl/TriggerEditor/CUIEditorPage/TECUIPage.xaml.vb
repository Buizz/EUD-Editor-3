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
            TextEditor.Text = TString
            TextEditor.ExternerLoader()
        End If
    End Sub



    Public Sub New(tTEFile As TEFile, Optional highLightLine As Integer = -1)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile
        TextEditor.Init(tTEFile)
        TextEditor.Text = CType(TEFile.Scripter, CUIScriptEditor).StringText

        If highLightLine > -1 Then
            TextEditor.LineHightLight(highLightLine)
        End If
    End Sub


    Public Sub SaveData()
        CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
        TEFile.LastDataRefresh()
    End Sub



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        If TEFile.FileType = TEFile.EFileType.CUIEps Then
            CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
        End If
    End Sub
End Class

