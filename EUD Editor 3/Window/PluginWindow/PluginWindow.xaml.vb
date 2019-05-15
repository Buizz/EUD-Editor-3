Public Class PluginWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ControlBar.HotkeyInit(Me)

        EdsText.Items.Clear()

        Topmost = pgData.Setting(ProgramData.TSetting.PluginSettingTopMost)

        For i = 0 To pjData.EdsBlock.BlocksLen - 1
            Dim items As New ListBoxItem
            items.Content = pjData.EdsBlock.BlocksName(i) & pjData.EdsBlock.BlocksStr(i)
            'items.Padding = New Thickness(10)
            'items.Margin = New Thickness(5)

            EdsText.Items.Add(items)



            Dim items2 As New ListBoxItem
            items2.Height = 12

            Dim sep As New Separator
            sep.Height = 10
            items2.Content = sep

            'items2.Background = New SolidColorBrush(Color.FromArgb(200, 128, 128, 128))

            EdsText.Items.Add(items2)
        Next


    End Sub

    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        CloseToolWindow()
    End Sub


End Class
