Public Class PluginWindow
    'https://cafe.naver.com/edac/78006
    'https://cafe.naver.com/edac/78598



    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ControlBar.HotkeyInit(Me)

        EdsText.Items.Clear()

        Topmost = pgData.Setting(ProgramData.TSetting.PluginSettingTopMost)

        For i = 0 To pjData.EdsBlock.Blocks.Count - 1
            Dim items As New ListBoxItem

            AddHandler items.MouseMove, New MouseEventHandler(Sub(s As Object, ee As MouseEventArgs)
                                                                  Hoverindex = EdsText.Items.IndexOf(s)
                                                              End Sub)


            items.HorizontalContentAlignment = HorizontalAlignment.Stretch
            items.VerticalContentAlignment = VerticalAlignment.Stretch

            items.Padding = New Thickness(-2)


            items.Content = New PluginItem(i)
            EdsText.Items.Add(items)
        Next


        Dim euddraftPath As String = pgData.Setting(ProgramData.TSetting.euddraft)
        If My.Computer.FileSystem.FileExists(euddraftPath) Then
            Dim pluginPath As String = IO.Path.GetDirectoryName(euddraftPath) & "\plugins"

            NewItem.Items.Clear()
            If True Then
                Dim mitem As New MenuItem
                mitem.Header = Tool.GetText("emptytext")
                mitem.Tag = ""
                AddHandler mitem.Click, AddressOf NewItem_Click

                NewItem.Items.Add(mitem)
            End If


            For Each plugins As String In My.Computer.FileSystem.GetFiles(pluginPath)
                Dim filename As String = IO.Path.GetFileNameWithoutExtension(plugins)

                Dim mitem As New MenuItem
                mitem.Header = filename
                mitem.Tag = filename
                AddHandler mitem.Click, AddressOf NewItem_Click

                NewItem.Items.Add(mitem)
            Next
        End If
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

        Dim edsitem As New BuildData.EdsBlock.EdsBlockItem(BuildData.EdsBlockType.UserPlugin)


        Dim btn As MenuItem = sender
        Dim initText As String = btn.Tag
        If initText <> "" Then
            edsitem.Texts = "[" & initText & "]"
        End If


        pjData.EdsBlock.Blocks.Insert(InsertIndex, edsitem)


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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'https://cafe.naver.com/edac/78006
        Process.Start("https://cafe.naver.com/edac/78006")
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Process.Start("https://cafe.naver.com/edac/78598")

        'https://cafe.naver.com/edac/78598
    End Sub
End Class
