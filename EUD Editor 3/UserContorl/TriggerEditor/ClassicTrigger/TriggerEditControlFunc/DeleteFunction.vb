Partial Public Class TriggerEditControl
    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
        DeleteFunc()
    End Sub

    Private Sub DeleteFunc()
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



        If tlist.SelectedIndex <> -1 Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In tlist.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return tlist.Items.IndexOf(x).CompareTo(tlist.Items.IndexOf(y))
                            End Function)



            Dim listboxindex As Integer = tlist.SelectedIndex



            For i = 0 To SelectList.Count - 1
                Dim tCodeBlock As ListItemCodeBlock = CType(SelectList(i), ListBoxItem).Content

                Dim ListIndex As Integer = tlist.Items.IndexOf(SelectList(i))
                Dim CodeBlockIndex As Integer = ttriglist.IndexOf(tCodeBlock._tcode)

                tlist.Items.RemoveAt(ListIndex)
                ttriglist.RemoveAt(CodeBlockIndex)
            Next

            If tlist.Items.Count > listboxindex Then
                tlist.SelectedIndex = listboxindex
            Else
                tlist.SelectedIndex = tlist.Items.Count - 1
            End If
        End If
    End Sub
End Class
