Public Class GUI_ArgEditorWindow
    Private gw As GUI_GrayWindow
    Public Sub New(graywindow As GUI_GrayWindow)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        gw = graywindow

    End Sub



    Private Sub Window_Closed(sender As Object, e As EventArgs)
        gw.Close()
    End Sub

    Private Sub OkBtn_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

End Class
