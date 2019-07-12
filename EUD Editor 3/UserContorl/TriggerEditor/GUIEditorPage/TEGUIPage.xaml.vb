Public Class TEGUIPage
    Private PTEFile As TEFile
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function

    Public Sub New(tTEFile As TEFile)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile
        Script.LoadScript(tTEFile)
    End Sub


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)


        'SelectItems = New List(Of TreeViewItem)
        'PreviewSelectItems = New List(Of TreeViewItem)

        'For i = 0 To 1
        '    Dim asdf As New CTreeviewItem
        '    MainTreeview.Items.Add(asdf)
        'Next

        '프로젝트 라는 이름의 폴더하나
        '


        'For i = 0 To 10
        '    AddTreeviewItem(MainTreeview)
        'Next

        'For i = 0 To 4
        '    AddTreeviewItem(CType(MainTreeview.Items(2), TreeViewItem))
        'Next
        'For i = 0 To 4
        '    AddTreeviewItem(CType(MainTreeview.Items(2).Items(2), TreeViewItem))
        'Next
    End Sub

    Public Sub SaveData()
        Script.Save()
        '  MsgBox("세이브 : " & TEFile.FileName)
    End Sub
    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        Script.Save()
        '  MsgBox("세이브 : " & TEFile.FileName)
    End Sub


    Private Sub ObjectSelector_ItemSelect(sender As Object, e As RoutedEventArgs)
        Script.AddItemClick(sender.ToString)
    End Sub
End Class
