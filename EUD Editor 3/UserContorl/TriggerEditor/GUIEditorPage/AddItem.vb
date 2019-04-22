Partial Public Class TEGUIPage
    Private Function MoveItem(FriendItem As TreeViewItem, Item As TreeViewItem, Optional IsTop As Boolean = False) As Boolean
        Dim itemC As ItemCollection = Nothing

        If TypeOf FriendItem.Parent Is TreeView Then
            itemC = CType(FriendItem.Parent, TreeView).Items
        ElseIf TypeOf FriendItem.Parent Is TreeViewItem Then
            itemC = CType(FriendItem.Parent, TreeViewItem).Items
        End If



        Dim ItemIndex As Integer = itemC.IndexOf(FriendItem)


        If IsTop Then
            itemC.Insert(ItemIndex, Item)
        Else
            itemC.Insert(ItemIndex + 1, Item)
        End If


        Return True
    End Function



    Private Sub AddTreeviewItem(parent As TreeViewItem)
        Dim items As New TreeViewItem
        Dim textb As New TextBlock
        textb.Text = "안녕 " & parent.Items.Count
        items.Header = textb
        items.Tag = "안녕 " & parent.Items.Count



        items.Background = Brushes.AliceBlue


        AddHandler items.PreviewMouseLeftButtonDown, AddressOf ListClick
        AddHandler items.PreviewMouseMove, AddressOf MainTreeviewItme_PreviewMouseMove
        AddHandler items.PreviewMouseLeftButtonUp, AddressOf MainTreeviewItme_PreviewMouseUp



        parent.Items.Add(items)
    End Sub
    Private Sub AddTreeviewItem(parent As TreeView)
        Dim items As New TreeViewItem
        Dim textb As New TextBlock
        textb.Text = "안녕 " & parent.Items.Count
        items.Header = textb
        items.Tag = "안녕 " & parent.Items.Count



        items.Background = Brushes.AliceBlue


        AddHandler items.PreviewMouseLeftButtonDown, AddressOf ListClick
        AddHandler items.PreviewMouseMove, AddressOf MainTreeviewItme_PreviewMouseMove
        AddHandler items.PreviewMouseLeftButtonUp, AddressOf MainTreeviewItme_PreviewMouseUp


        parent.Items.Add(items)
    End Sub
End Class
