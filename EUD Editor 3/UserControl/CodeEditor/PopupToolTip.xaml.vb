Public Class PopupToolTip
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        ShowActivated = False
        Focusable = False
        ShowInTaskbar = False
        IsEnabled = False
    End Sub
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)


    End Sub
End Class
