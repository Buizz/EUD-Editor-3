Partial Public Class GUIScriptEditorUI
    Private Function f_DeleteItem(titem As TreeViewItem)
        '아이템을 지움

        Dim scr As ScriptBlock = titem.Tag
        If scr.IsDeleteAble Then
            If scr.Parent Is Nothing Then
                Script.items.Remove(scr)
            Else
                scr.Parent.child.Remove(scr)
            End If


            If titem.Parent.GetType Is GetType(TreeViewItem) Then
                Dim pitem As TreeViewItem = titem.Parent
                pitem.Items.Remove(titem)
            Else
                Dim pitem As TreeView = titem.Parent
                pitem.Items.Remove(titem)
            End If
        Else
            Return False
        End If




        pjData.SetDirty(True)
        Return True
    End Function
End Class
