Imports Newtonsoft.Json

Partial Public Class TriggerEditControl
    Private Sub Copy_Click(sender As Object, e As RoutedEventArgs)
        CopyFunc()
    End Sub

    Private Sub CopyFunc()
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



            Dim TriggerCodes As New List(Of TriggerCodeBlock)
            For i = 0 To SelectList.Count - 1
                Dim tListCodeBlock As ListItemCodeBlock = CType(SelectList(i), ListBoxItem).Content
                Dim tCodeBlock As TriggerCodeBlock = tListCodeBlock._tcode
                TriggerCodes.Add(tCodeBlock)
            Next



            Dim TextStr As String = JsonConvert.SerializeObject(TriggerCodes)
            My.Computer.Clipboard.SetText(TextStr)
        End If
        ButtonRefresh()
    End Sub
End Class
