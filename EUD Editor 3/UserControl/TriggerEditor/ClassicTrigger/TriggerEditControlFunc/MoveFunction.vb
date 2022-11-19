Partial Public Class TriggerEditControl


    Private Sub MoveItem(IsAction As Boolean, Item As ListBoxItem, MoveCount As Integer)
        Dim tlist As ListBox
        Dim ttriglist As List(Of TriggerCodeBlock)
        If IsAction Then
            tlist = aList
            ttriglist = ptrg.Actions
        Else
            tlist = cList
            ttriglist = ptrg.Condition
        End If

        Dim tCodeBlock As ListItemCodeBlock = Item.Content



        Dim trg As TriggerCodeBlock = tCodeBlock._tcode

        Dim cindex As Integer = tlist.Items.IndexOf(Item)
        Dim ctriggerindex As Integer = ttriglist.IndexOf(trg)



        tlist.Items.RemoveAt(cindex)
        ttriglist.RemoveAt(ctriggerindex)


        tlist.Items.Insert(cindex + MoveCount, Item)
        ttriglist.Insert(ctriggerindex + MoveCount, trg)

        Item.IsSelected = True
    End Sub

    Private Sub Up_Click(sender As Object, e As RoutedEventArgs)
        Dim tlist As ListBox
        Dim ttriglist As List(Of TriggerCodeBlock)
        Dim IsAction As Boolean
        If GetPageIndex = 1 Then
            tlist = cList
            ttriglist = ptrg.Condition
            IsAction = False
        Else
            tlist = aList
            ttriglist = ptrg.Actions
            IsAction = True
        End If


        If tlist.SelectedItem IsNot Nothing Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In tlist.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return tlist.Items.IndexOf(x).CompareTo(tlist.Items.IndexOf(y))
                            End Function)

            For i = 0 To SelectList.Count - 1
                MoveItem(IsAction, SelectList(i), -1)
            Next
        End If
    End Sub
    Private Sub Down_Click(sender As Object, e As RoutedEventArgs)
        Dim tlist As ListBox
        Dim ttriglist As List(Of TriggerCodeBlock)
        Dim IsAction As Boolean
        If GetPageIndex = 1 Then
            tlist = cList
            ttriglist = ptrg.Condition
            IsAction = False
        Else
            tlist = aList
            ttriglist = ptrg.Actions
            IsAction = True
        End If


        If tlist.SelectedItem IsNot Nothing Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In tlist.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return tlist.Items.IndexOf(x).CompareTo(tlist.Items.IndexOf(y))
                            End Function)

            For i = 0 To SelectList.Count - 1
                MoveItem(IsAction, SelectList(i), +1)
            Next
        End If
    End Sub
End Class
