Partial Public Class TriggerEditControl
    Private Sub New_Click(sender As Object, e As RoutedEventArgs)
        AddFunc()

    End Sub

    Private Sub AddFunc()
        Dim tlist As ListBox
        Dim openType As TriggerCodeEditControl.OpenType
        If GetPageIndex = 1 Then
            tlist = cList
            openType = TriggerCodeEditControl.OpenType.Contidion
        Else
            tlist = aList
            openType = TriggerCodeEditControl.OpenType.Action
        End If

        IsEditOpen = False
        If tlist.SelectedIndex <> -1 Then
            LastSelectListBoxIndex = tlist.SelectedIndex
        Else
            LastSelectListBoxIndex = tlist.Items.Count
        End If



        TriggerCodeEdit.OpenEdit(openType)
    End Sub
End Class
