Public Class CodeEditor
    Public Property Text As String
        Get
            Return TextEditor.Text
        End Get
        Set(value As String)
            TextEditor.Text = value
        End Set
    End Property

    Public Event TextChange As RoutedEventHandler


    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        InitTextEditor()
    End Sub

    Private Sub TextEditor_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        TooltipHide()
    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        foldingUpdateTimer.Stop()
        PopupToolTip.Close()
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        PopupToolTip = New PopupToolTip
        PopupToolTip.Show()

        PopupToolTip.Visibility = Visibility.Hidden
    End Sub

    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        'TooltipHide()
    End Sub

    Private Sub TextEditor_TextChanged(sender As Object, e As EventArgs)
        RaiseEvent TextChange(Me, Nothing)
    End Sub
End Class
