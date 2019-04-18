Public Class PluginWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ControlBar.HotkeyInit(Me)
    End Sub

    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        CloseToolWindow()
    End Sub
End Class
