Public Class DotPainter
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        ItemList.Items.Clear()
        For i = 0 To pjData.TEData.DotDatas.Count - 1
            Dim tlist As New ListBoxItem

            tlist.Content = pjData.TEData.DotDatas(i).ItemName
            tlist.Tag = pjData.TEData.DotDatas(i)

            ItemList.Items.Add(tlist)
        Next
    End Sub

    Private Sub AddBtn(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = Tool.GetText("IMG Fliter"),
            .Title = Tool.GetText("IMG Select")
            }

        Dim filename As String



        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            filename = opendialog.FileName
        Else
            Return
        End If



        pjData.SetDirty(True)

        Dim dotData As New DotData(filename)
        If Not dotData.LoadSuccess Then
            Return
        End If

        pjData.SetDirty(True)



        'dotData.ItemName = ItemList.Items.Count


        pjData.TEData.DotDatas.Add(dotData)

        Dim tlist As New ListBoxItem

        tlist.Content = dotData.ItemName
        tlist.Tag = dotData

        ItemList.Items.Add(tlist)

        ItemList.SelectedItem = tlist
    End Sub

    Private Sub DeleteBtnClick(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        If ItemList.SelectedIndex <> -1 Then
            Dim tdotdata As DotData = CType(ItemList.SelectedItem, ListBoxItem).Tag

            pjData.TEData.DotDatas.Remove(tdotdata)
            ItemList.Items.Remove(ItemList.SelectedItem)
        End If
    End Sub

    Private Sub ItemList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ItemList.SelectedIndex = -1 Then
            DeleteBtn.IsEnabled = False
            EditPage.Visibility = Visibility.Hidden
            EditPage.disable()
        Else
            DeleteBtn.IsEnabled = True
            EditPage.Visibility = Visibility.Visible
            Dim tdotdata As DotData = CType(ItemList.SelectedItem, ListBoxItem).Tag
            EditPage.init(tdotdata)
        End If
    End Sub
End Class
