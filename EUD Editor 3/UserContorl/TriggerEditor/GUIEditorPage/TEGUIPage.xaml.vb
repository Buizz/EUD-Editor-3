Public Class TEGUIPage



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        SelectItems = New List(Of TreeViewItem)
        PreviewSelectItems = New List(Of TreeViewItem)

        'For i = 0 To 1
        '    Dim asdf As New CTreeviewItem
        '    MainTreeview.Items.Add(asdf)
        'Next



        For i = 0 To 10
            AddTreeviewItem(MainTreeview)
        Next

        For i = 0 To 4
            AddTreeviewItem(CType(MainTreeview.Items(2), TreeViewItem))
        Next
        For i = 0 To 4
            AddTreeviewItem(CType(MainTreeview.Items(2).Items(2), TreeViewItem))
        Next
    End Sub











    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)

    End Sub


End Class
