Partial Public Class TEGUIPage
    Private Function DeleteItem(Item As TreeViewItem) As Boolean
        If TypeOf Item.Parent Is TreeView Then
            CType(Item.Parent, TreeView).Items.Remove(Item)
        ElseIf TypeOf Item.Parent Is TreeViewItem Then
            CType(Item.Parent, TreeViewItem).Items.Remove(Item)
        End If
        Return True
    End Function
End Class
