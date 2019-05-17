Partial Class PluginWindow
    Private DragSelectindex As Integer
    Private DragSelectItem As ListBoxItem
    Private DragSelctData As BuildData.EdsBlock.EdsBlockItem
    Private IsClick As Boolean
    Private DragPos As Point
    Private IsTopSelect As Boolean

    Private Sub EdsText_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragImage.Margin = New Thickness(e.GetPosition(EdsText).X, e.GetPosition(EdsText).Y, 0, 0)

        IsClick = True
    End Sub

    Private Sub EdsText_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        For i = 0 To EdsText.Items.Count - 1
            DSelectBorder(i)
        Next
        For i = 0 To EdsText.Items.Count - 1
            Dim RealHeight As Integer = CType(EdsText.Items(i), ListBoxItem).ActualHeight
            Dim InsertIndex As Integer = i
            Dim LastPos As Point = e.GetPosition(EdsText.Items(i))

            If LastPos.Y > 0 And RealHeight / 2 > LastPos.Y Then
                SelectTopBorder(InsertIndex)
                SelectDownBorder(InsertIndex - 1)
            ElseIf LastPos.Y > RealHeight / 2 And RealHeight > LastPos.Y Then
                SelectDownBorder(InsertIndex)
                SelectTopBorder(InsertIndex + 1)
            End If

        Next



        If IsClick Then '만약 클릭 중 일경우
            '아이템이 선택 중일 경우. 그 아이템을 드래그 아이템으로 지정.
            If DragSelectItem Is Nothing Then
                If EdsText.SelectedItem IsNot Nothing Then '드래그 아이템이 지정되어 있지 않을 경우
                    DragSelectindex = EdsText.SelectedIndex
                    DragSelectItem = EdsText.SelectedItem
                    DragSelctData = pjData.EdsBlock.Blocks(DragSelectindex)
                    DragImage.Width = DragSelectItem.ActualWidth
                    DragImage.Height = DragSelectItem.ActualHeight

                    Dim listboxitem As New ListBoxItem
                    listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
                    listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

                    listboxitem.Padding = New Thickness(-2)

                    'Dim ListItem As New RequireListBoxItem(RequireList(DragSelectindex))

                    listboxitem.Content = New PluginItem(DragSelectindex)


                    'listboxitem.Content = ListItem

                    DragImage.Child = listboxitem
                End If
            Else '드래그 중
                DragImage.Visibility = Visibility.Visible
                DragImage.Margin = New Thickness(e.GetPosition(EdsText).X + 5, e.GetPosition(EdsText).Y + 5, 0, 0)

                If EdsText.SelectedItem IsNot Nothing Then
                    Dim RealHeight As Integer = CType(EdsText.SelectedItem, ListBoxItem).ActualHeight
                    Dim InsertIndex As Integer = EdsText.SelectedIndex
                    Dim LastPos As Point = e.GetPosition(EdsText.SelectedItem)


                    If LastPos.Y < RealHeight / 2 Then
                        IsTopSelect = True
                    Else
                        IsTopSelect = False
                    End If
                Else
                    IsTopSelect = True
                End If
            End If
        End If
    End Sub

    Private Sub EdsText_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        IsClick = False
        DragImage.Visibility = Visibility.Collapsed

        If DragSelectItem Is Nothing Then
            Exit Sub
        End If


        '===========================================================================
        Dim InsertIndex As Integer = EdsText.SelectedIndex

        If Not IsTopSelect Then
            InsertIndex += 1
        End If


        'Dim listboxitem As New ListBoxItem
        'listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        'listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        'listboxitem.Padding = New Thickness(-2)

        Dim tItem As BuildData.EdsBlock.EdsBlockItem = DragSelctData.Clone

        'Dim ListItem As New RequireListBoxItem(tItem)

        'listboxitem.Content = ListItem
        ''만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

        pjData.EdsBlock.Blocks.Insert(InsertIndex, tItem)
        'EdsText.Items.Insert(InsertIndex, listboxitem)


        pjData.EdsBlock.Blocks.Remove(DragSelctData)
        'EdsText.Items.Remove(DragSelectItem)


        ItemRefresh()
        '    ListBoxItem.IsSelected = True
        'CType(DragSelectItem.Content, PluginItem).Refresh()
        'CType(CType(EdsText.Items(InsertIndex), ListBoxItem).Content, PluginItem).Refresh()


        DragSelectItem = Nothing
        ''pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()

    End Sub

    Private Sub ItemRefresh()
        For i = 0 To EdsText.Items.Count - 1
            CType(CType(EdsText.Items(i), ListBoxItem).Content, PluginItem).Refresh()
        Next
    End Sub


    Private Sub SelectTopBorder(index As Integer)
        If EdsText.Items.Count > index And index >= 0 Then
            Dim RListbox As PluginItem = CType(EdsText.Items(index), ListBoxItem).Content
            RListbox.SelectTopBorder()
        End If
    End Sub
    Private Sub SelectDownBorder(index As Integer)
        If EdsText.Items.Count > index And index >= 0 Then
            Dim RListbox As PluginItem = CType(EdsText.Items(index), ListBoxItem).Content
            RListbox.SelectDownBorder()
        End If
    End Sub
    Private Sub DSelectBorder(index As Integer)
        If EdsText.Items.Count > index And index >= 0 Then
            Dim RListbox As PluginItem = CType(EdsText.Items(index), ListBoxItem).Content
            RListbox.DSelectBorder()
        End If
    End Sub
End Class
