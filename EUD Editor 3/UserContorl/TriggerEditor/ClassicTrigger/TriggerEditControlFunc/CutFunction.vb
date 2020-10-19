Partial Public Class TriggerEditControl
    Private Sub Cut_Click(sender As Object, e As RoutedEventArgs)
        CutFunc()
    End Sub

    Private Sub CutFunc()
        CopyFunc()
        DeleteFunc()
    End Sub
End Class
