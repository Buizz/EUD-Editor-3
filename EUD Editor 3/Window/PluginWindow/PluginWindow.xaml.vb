Public Class PluginWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ControlBar.HotkeyInit(Me)

        EdsText.Items.Clear()

        Topmost = pgData.Setting(ProgramData.TSetting.PluginSettingTopMost)

        For i = 0 To pjData.EdsBlock.Blocks.Count - 1
            Dim items As New ListBoxItem
            items.HorizontalContentAlignment = HorizontalAlignment.Stretch
            items.VerticalContentAlignment = VerticalAlignment.Stretch

            items.Padding = New Thickness(-2)


            items.Content = New PluginItem(i)
            EdsText.Items.Add(items)
        Next
    End Sub

    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        CloseToolWindow()
    End Sub

    Private Sub NewItem_Click(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        Dim items As New ListBoxItem
        items.HorizontalContentAlignment = HorizontalAlignment.Stretch
        items.VerticalContentAlignment = VerticalAlignment.Stretch

        items.Padding = New Thickness(-2)


        'items.Padding = New Thickness(10)
        'items.Margin = New Thickness(5)



        Dim InsertIndex As Integer = MenuSelectIndex
        If Not IsTopSelect Then
            InsertIndex += 1
        End If

        pjData.EdsBlock.Blocks.Insert(InsertIndex, New BuildData.EdsBlock.EdsBlockItem(BuildData.EdsBlockType.UserPlugin))


        items.Content = New PluginItem(pjData.EdsBlock.Blocks.Count - 1) 'pjData.EdsBlock.BlocksName(i) & pjData.EdsBlock.BlocksStr(i)


        EdsText.Items.Add(items)

        ItemRefresh()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        EdsText.Items.RemoveAt(EdsText.Items.Count - 1)
        pjData.EdsBlock.Blocks.RemoveAt(MenuSelectIndex)

        ItemRefresh()
    End Sub

    Private MenuSelectIndex As Integer
    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If EdsText.SelectedItem IsNot Nothing Then
            MenuSelectIndex = EdsText.SelectedIndex
            If pjData.EdsBlock.Blocks(EdsText.SelectedIndex).BType = BuildData.EdsBlockType.UserPlugin Then
                DeleteItem.IsEnabled = True
            Else
                DeleteItem.IsEnabled = False
            End If
        End If
    End Sub

    Private Sub EdsText_MouseUp(sender As Object, e As MouseButtonEventArgs)
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

    End Sub

End Class
