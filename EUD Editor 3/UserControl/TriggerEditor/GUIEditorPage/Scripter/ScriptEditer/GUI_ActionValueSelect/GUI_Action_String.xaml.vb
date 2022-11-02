Public Class GUI_Action_String
    Public Event SelectEvent As RoutedEventHandler

    Public Sub New(val As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        If IsDefaultValue(val) Then
            MainTB.Text = ""
        Else
            MainTB.Text = val
        End If
        isLoad = True
    End Sub
    Private Function IsDefaultValue(str As String) As Boolean
        Dim strs() As String = str.Split(";")
        If strs.Length <> 2 Then
            Return False
        Else
            If strs.First = "defaultvalue" Then
                Return True
            End If
        End If
        Return False
    End Function

    Private isLoad As Boolean = False
    Private Sub MainTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        If isLoad Then
            RaiseEvent SelectEvent(MainTB.Text, e)
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        TextEditorOpen()
    End Sub
    Private Sub TextEditorOpen()
        MainTB.IsEnabled = False
        Dim TEditor As New TextEditorWindow(MainTB.Text)

        TEditor.ShowDialog()
        MainTB.Text = TEditor.TextString
        MainTB.IsEnabled = True
    End Sub
End Class
