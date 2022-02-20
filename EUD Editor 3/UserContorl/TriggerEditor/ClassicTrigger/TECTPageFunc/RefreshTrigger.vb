Partial Public Class TECTPage


    Public Sub RefershPlayerFliter()



        TListBox.Items.Filter = Function(trig As Trigger)
                                    Dim rval As Boolean = False

                                    Dim Player As Integer = GetPlayerListIndex()

                                    If Player = -1 Then
                                        Return False
                                    End If

                                    '해당 플레이어가 ON일경우 True
                                    If trig.PlayerEnabled(Player) Then
                                        rval = True
                                    End If


                                    Return rval
                                End Function



    End Sub

    Public Sub RefreshTriggerPage()
        'Dim Player As Integer = GetPlayerListIndex()

        'Scripter.TriggerList

        TListBox.ItemsSource = Scripter.TriggerListCollection
        'TListBox.Items.Clear()

        'If Player <> -1 Then
        '    For i = 0 To Scripter.TriggerList.Count - 1
        '        If Scripter.TriggerList(i).PlayerEnabled(Player) Then
        '            TListBox.Items.Add(GetListItem(Scripter.TriggerList(i)))
        '        End If
        '    Next
        'End If

    End Sub
End Class
