Partial Public Class GUIScriptEditorUI
    Private Function f_DeleteItem(titem As TreeViewItem)
        '아이템을 지움

        Dim scr As ScriptBlock = titem.Tag
        If scr.IsDeleteAble Then
            SelectedList.Remove(titem)
            AddDeleteTask(titem)
            If scr.Parent Is Nothing Then
                Script.RemoveItems(scr)
            Else
                scr.Parent.child.Remove(scr)
            End If


            If titem.Parent.GetType Is GetType(TreeViewItem) Then
                Dim pitem As TreeViewItem = titem.Parent
                pitem.Items.Remove(titem)
            ElseIf titem.Parent.GetType Is GetType(TreeView) Then
                Dim pitem As TreeView = titem.Parent
                pitem.Items.Remove(titem)
            End If
        Else
            Return False
        End If
        TEGUIPage.ObjectSelector.RefreshCurrentList()
        'MsgBox("아이템삭제")

        pjData.SetDirty(True)
        Return True
    End Function
End Class
