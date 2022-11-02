Partial Public Class TECTPage
    Private Sub DeleteTriggerBtn_Click(sender As Object, e As RoutedEventArgs)
        DeleteTrigger()
    End Sub

    Public Sub DeleteTrigger()
        If TListBox.SelectedItem IsNot Nothing Then
            pjData.SetDirty(True)
            Dim listboxindex As Integer = TListBox.SelectedIndex

            Dim SelectedItems As New List(Of Trigger)

            For Each sitem In TListBox.SelectedItems
                SelectedItems.Add(sitem)
            Next


            For Each sitem In SelectedItems
                Dim trg As Trigger = sitem

                Scripter.TriggerListCollection.Remove(trg)
                'TListBox.Items.Remove(sitem)
            Next




            If TListBox.Items.Count > listboxindex Then
                TListBox.SelectedIndex = listboxindex
            Else
                TListBox.SelectedIndex = TListBox.Items.Count - 1
            End If
        End If
        PlayerListReset()
    End Sub
End Class
