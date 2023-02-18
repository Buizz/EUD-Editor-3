Public Class DonateControl
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Process.Start("https://www.patreon.com/bing_su?fan_landing=true")
    End Sub

    Private Sub Checking_Unchecked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.DonateMsg) = Checking.IsChecked
    End Sub

    Private Sub Checking_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.DonateMsg) = Checking.IsChecked
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        Checking.IsChecked = pgData.Setting(ProgramData.TSetting.DonateMsg)
    End Sub

    Private Sub CoffeeButton_Click(sender As Object, e As RoutedEventArgs)
        Process.Start("https://www.patreon.com/bing_su?fan_landing=true")
    End Sub
End Class
