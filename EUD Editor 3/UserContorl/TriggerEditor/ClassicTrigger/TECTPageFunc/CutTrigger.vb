Partial Public Class TECTPage
    Private Sub TriggerCutBtn_Click(sender As Object, e As RoutedEventArgs)
        CutTrigger()
    End Sub
    Private Sub CutTrigger()
        CopyTrigger()
        DeleteTrigger()
    End Sub
End Class
