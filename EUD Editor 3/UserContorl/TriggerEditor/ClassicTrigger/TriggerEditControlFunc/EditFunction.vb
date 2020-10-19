Partial Public Class TriggerEditControl
    Private Sub Edit_Click(sender As Object, e As RoutedEventArgs)
        EditFunc()
    End Sub

    Private Sub EditFunc()
        Dim tlist As ListBox
        Dim openType As TriggerCodeEditControl.OpenType
        Dim ttriglist As List(Of TriggerCodeBlock)
        If GetPageIndex = 1 Then
            tlist = cList
            ttriglist = ptrg.Condition
            openType = TriggerCodeEditControl.OpenType.Contidion
        Else
            tlist = aList
            ttriglist = ptrg.Actions
            openType = TriggerCodeEditControl.OpenType.Action
        End If

        IsEditOpen = True
        If tlist.SelectedIndex <> -1 Then
            LastSelectListBoxIndex = tlist.SelectedIndex
        Else
            LastSelectListBoxIndex = tlist.Items.Count
        End If


        Dim editTrg As TriggerCodeBlock = ttriglist(LastSelectListBoxIndex).DeepCopy
        TriggerCodeEdit.OpenEdit(openType, TBlock:=editTrg)
    End Sub
End Class
