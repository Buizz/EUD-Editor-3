Public Class PluginItem
    Private index As Integer
    Public ReadOnly Property GetIndex As Integer
        Get
            Return index
        End Get
    End Property




    Public Sub New(tIndex As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        index = tIndex

        Refresh()
    End Sub
    Public Sub Refresh()
        If pjData.EdsBlock.Blocks(index).BType = BuildData.EdsBlockType.Etc Then
            Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Book
            TextTitle.Visibility = Visibility.Collapsed
            Textb.Visibility = Visibility.Collapsed
            Textbox.Visibility = Visibility.Visible
        Else
            Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.BookLock
            TextTitle.Visibility = Visibility.Visible
            Textb.Visibility = Visibility.Visible
            Textbox.Visibility = Visibility.Collapsed

            TextTitle.Text = pjData.EdsBlock.Blocks(index).GetEdsName.Trim
            Textb.Text = pjData.EdsBlock.Blocks(index).GetEdsString.Trim
        End If


    End Sub

    Public Sub SelectTopBorder()
        TopBorder.Visibility = Visibility.Visible
        DownBorder.Visibility = Visibility.Hidden
    End Sub
    Public Sub SelectDownBorder()
        TopBorder.Visibility = Visibility.Hidden
        DownBorder.Visibility = Visibility.Visible
    End Sub
    Public Sub DSelectBorder()
        TopBorder.Visibility = Visibility.Hidden
        DownBorder.Visibility = Visibility.Hidden
    End Sub


    Private Sub DockPanel_MouseMove(sender As Object, e As MouseEventArgs)
        'If e.GetPosition(Me).Y > Me.ActualHeight / 2 Then
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Visible
        'Else
        '    TopBorder.Visibility = Visibility.Visible
        '    DownBorder.Visibility = Visibility.Hidden
        'End If


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
        'TopBorder.Visibility = Visibility.Hidden
        'DownBorder.Visibility = Visibility.Hidden
    End Sub
End Class
