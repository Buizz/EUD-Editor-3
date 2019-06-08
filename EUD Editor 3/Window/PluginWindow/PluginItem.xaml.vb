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
        If pjData.EdsBlock.Blocks(index).BType = BuildData.EdsBlockType.UserPlugin Then
            Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Book
            TextTitle.Visibility = Visibility.Collapsed
            Textb.Visibility = Visibility.Collapsed
            Textbox.Visibility = Visibility.Visible

            Textbox.Text = pjData.EdsBlock.Blocks(index).Texts
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



    Private Sub Textbox_TextChanged(sender As Object, e As TextChangedEventArgs)
        pjData.SetDirty(True)
        If pjData.EdsBlock.Blocks(index).BType = BuildData.EdsBlockType.UserPlugin Then
            pjData.EdsBlock.Blocks(index).Texts = Textbox.Text
        End If
    End Sub
End Class
