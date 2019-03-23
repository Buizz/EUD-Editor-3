Public Class RequireListBoxItem
    Public Sub New(RequireData As CRequireData.RequireBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        If RequireData.HasValue Then
            Textb.Text = RequireData.opText & vbCrLf & RequireData.Value

        Else
            Textb.Text = RequireData.opText

        End If
    End Sub


    Private Sub DockPanel_MouseMove(sender As Object, e As MouseEventArgs)
        If e.GetPosition(Me).Y > Me.ActualHeight / 2 Then
            TopBorder.Visibility = Visibility.Hidden
            DownBorder.Visibility = Visibility.Visible
        Else
            TopBorder.Visibility = Visibility.Visible
            DownBorder.Visibility = Visibility.Hidden
        End If


        'If e.GetPosition(Me).Y > Me.ActualHeight / 3 * 2 Then
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Visible
        'ElseIf e.GetPosition(Me).Y < Me.ActualHeight / 3 Then
        '    TopBorder.Visibility = Visibility.Visible
        '    DownBorder.Visibility = Visibility.Hidden
        'Else
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Hidden
        'End If
        'Textb.Text = e.GetPosition(Me).Y
    End Sub

    Private Sub UserControl_MouseLeave(sender As Object, e As MouseEventArgs)
        TopBorder.Visibility = Visibility.Hidden
        DownBorder.Visibility = Visibility.Hidden
    End Sub
End Class
