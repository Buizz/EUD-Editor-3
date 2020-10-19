Partial Public Class TECTPage
    Public Sub RefreshTriggerPage()
        Dim Player As Integer = GetPlayerListIndex()




        TListBox.Items.Clear()

        If Player <> -1 Then
            For i = 0 To Scripter.TriggerList.Count - 1
                If Scripter.TriggerList(i).PlayerEnabled(Player) Then
                    TListBox.Items.Add(GetListItem(Scripter.TriggerList(i)))
                End If
            Next
        End If

    End Sub
End Class
