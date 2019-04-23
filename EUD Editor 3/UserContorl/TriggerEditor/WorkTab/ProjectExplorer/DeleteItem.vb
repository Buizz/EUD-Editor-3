Partial Public Class ProjectExplorer
    Private Function DeleteItem(Item As TreeViewItem) As Boolean
        Try

            If GetFile(Item).IsTopFolder Then
                Return False
            End If

            Dim CTEFile As TEFile = Item.Tag
            Dim PTEFile As TEFile = CType(Item.Parent, TreeViewItem).Tag
            TECloseTabITem(CTEFile)
            If CTEFile.FileType = TEFile.EFileType.Folder Then
                PTEFile.FolderRemove(CTEFile)
            Else
                PTEFile.FileRemove(CTEFile)
            End If


            If TypeOf Item.Parent Is TreeView Then
                CType(Item.Parent, TreeView).Items.Remove(Item)
            ElseIf TypeOf Item.Parent Is TreeViewItem Then
                CType(Item.Parent, TreeViewItem).Items.Remove(Item)
            End If



            Return True
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error Dont Delete File"), ex.ToString)
            Return False
        End Try
    End Function
End Class
