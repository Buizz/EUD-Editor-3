Partial Public Class TECTPage
    Private Sub NewTriggerBtn_Click(sender As Object, e As RoutedEventArgs)
        AddTrigger()
    End Sub

    Public Sub AddTrigger()
        '원래는 window를 새로 열어야됨
        Dim nTrg As New TriggerBlock


        Dim Listitem As New ListBoxItem

        Listitem.BorderThickness = New Thickness(1)
        Listitem.Background = Application.Current.Resources("MaterialDesignPaper")

        Listitem.HorizontalAlignment = HorizontalAlignment.Stretch


        Listitem.Content = nTrg

        TListBox.Items.Add(Listitem)

    End Sub
End Class
