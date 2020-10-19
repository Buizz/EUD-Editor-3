Partial Public Class TECTPage
    Private Sub MoveItem(Item As ListBoxItem, MoveCount As Integer)
        Dim trg As Trigger = GetTrg(Item)

        Dim cindex As Integer = TListBox.Items.IndexOf(Item)
        Dim ctriggerindex As Integer = Scripter.TriggerList.IndexOf(trg)


        Dim cTargetindex As Integer = cindex + MoveCount
        Dim Targettrg As Trigger = GetTrg(TListBox.Items(cTargetindex))
        Dim cTargettriggerindex As Integer = Scripter.TriggerList.IndexOf(Targettrg)






        TListBox.Items.RemoveAt(cindex)
        Scripter.TriggerList.RemoveAt(ctriggerindex)


        TListBox.Items.Insert(cTargetindex, Item)
        Scripter.TriggerList.Insert(cTargettriggerindex, trg)

        Item.IsSelected = True
    End Sub


    Private Sub TriggerUpBtn_Click(sender As Object, e As RoutedEventArgs)
        If TListBox.SelectedItem IsNot Nothing Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In TListBox.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return TListBox.Items.IndexOf(x).CompareTo(TListBox.Items.IndexOf(y))
                            End Function)

            For i = 0 To SelectList.Count - 1
                MoveItem(SelectList(i), -1)
            Next


            'Dim trg As Trigger = GetTrg(SelectList.First)
            'Dim startindex As Integer = TListBox.Items.IndexOf(SelectList.First)
            'Dim starttriggerindex As Integer = Scripter.TriggerList.IndexOf(trg)

            'DeleteTrigger()
            'TListBox.SelectedIndex = -1

            'For i = SelectList.Count - 1 To 0 Step -1
            '    Dim ctrg As Trigger = GetTrg(SelectList(i))
            '    Dim tt As ListBoxItem = GetListItem(ctrg)


            '    TListBox.Items.Insert(startindex - 1, tt)
            '    Scripter.TriggerList.Insert(starttriggerindex - 1, ctrg)


            '    tt.IsSelected = True
            'Next
        End If
    End Sub

    Private Sub TriggerDownBtn_Click(sender As Object, e As RoutedEventArgs)
        If TListBox.SelectedItem IsNot Nothing Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In TListBox.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return TListBox.Items.IndexOf(x).CompareTo(TListBox.Items.IndexOf(y))
                            End Function)


            For i = SelectList.Count - 1 To 0 Step -1
                MoveItem(SelectList(i), 1)
            Next
            'Dim trg As Trigger = GetTrg(SelectList.First)
            'Dim startindex As Integer = TListBox.Items.IndexOf(SelectList.First)
            'Dim starttriggerindex As Integer = Scripter.TriggerList.IndexOf(trg)

            'DeleteTrigger()
            'TListBox.SelectedIndex = -1

            'For i = SelectList.Count - 1 To 0 Step -1
            '    Dim ctrg As Trigger = GetTrg(SelectList(i))
            '    Dim tt As ListBoxItem = GetListItem(ctrg)


            '    TListBox.Items.Insert(startindex + 1, tt)
            '    Scripter.TriggerList.Insert(starttriggerindex + 1, ctrg)


            '    tt.IsSelected = True
            'Next
        End If
    End Sub
End Class
