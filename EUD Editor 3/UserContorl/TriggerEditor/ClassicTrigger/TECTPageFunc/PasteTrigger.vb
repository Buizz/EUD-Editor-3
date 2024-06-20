Imports Newtonsoft.Json

Partial Public Class TECTPage

    Private LastCheck As String
    Private LastAble As Boolean
    Public Function IsPasteAble() As Boolean
        Dim PasteText As String = My.Computer.Clipboard.GetText

        If PasteText = LastCheck Then
            Return LastAble
        Else
            LastCheck = PasteText
        End If


        'Dim Triggers As New List(Of Trigger)
        'For i = 0 To SelectList.Count - 1
        '    Triggers.Add(GetTrg(SelectList(i)))
        'Next

        Try
            Dim Triggers As List(Of Trigger) = JsonConvert.DeserializeObject(PasteText, GetType(List(Of Trigger)))

            If Triggers Is Nothing Then
                Return False
            End If

            If Triggers.Count = 0 Then
                LastAble = False
                Return False
            Else
                If Triggers.First.CName <> "Trigger" Then
                    LastAble = False
                    Return False
                End If
            End If
        Catch ex As Exception
            LastAble = False
            Return False
        End Try

        LastAble = True
        Return True
    End Function


    Private Sub TriggerPasteBtn_Click(sender As Object, e As RoutedEventArgs)
        PasteTrigger()
    End Sub
    Private Sub PasteTrigger()
        If Not IsPasteAble() Then
            Return
        End If

        Dim PasteText As String = My.Computer.Clipboard.GetText
        Dim Triggers As List(Of Trigger) = JsonConvert.DeserializeObject(PasteText, GetType(List(Of Trigger)))

        Dim InsertPos As Integer = -1

        If TListBox.SelectedIndex <> -1 Then
            Dim SelectList As New List(Of Trigger)
            For Each sitem In TListBox.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return TListBox.Items.IndexOf(x).CompareTo(TListBox.Items.IndexOf(y))
                            End Function)

            InsertPos = Scripter.TriggerListCollection.IndexOf(SelectList.Last)
            'InsertPos = TListBox.Items.IndexOf(SelectList.Last)
        End If


        'TListBox.SelectedIndex = -1

        'TListBox.SelectedItems.Clear()


        PlayerListReset()
        For i = Triggers.Count - 1 To 0 Step -1
            Dim trg As Trigger = Triggers(i)

            trg.parentscripter = Scripter

            Dim tcheck As Boolean = False
            For k = 0 To 7
                If trg.PlayerEnabled(k) Then
                    tcheck = True
                End If
            Next
            For k = 0 To 6
                If trg.ForceEnabled(k) Then
                    tcheck = True
                End If
            Next
            If Not tcheck Then
                '모든 트리거가 꺼져있어서 추가불가능
                Continue For
            End If




            'Dim cp As Integer = GetPlayerListIndex()



            'Dim ViewPlayer As Boolean
            'If cp = -1 Then
            '    ViewPlayer = False
            'Else
            '    ViewPlayer = trg.PlayerEnabled(cp)
            'End If


            '플레이어가 해당 페이지에 있는지 없는지 여부




            Dim NTrigger As Trigger = trg.DeepCopy
            'Dim tt As ListBoxItem = GetListItem(NTrigger)
            pjData.SetDirty(True)

            If InsertPos = -1 Then
                Scripter.TriggerListCollection.Add(NTrigger)

                'If ViewPlayer Then
                '    TListBox.Items.Add(tt)
                'End If
            Else
                Scripter.TriggerListCollection.Insert(InsertPos + 1, NTrigger)

                'If ViewPlayer Then
                '    TListBox.Items.Insert(InsertPos + 1, tt)
                'End If
            End If
            TListBox.SelectedItems.Add(NTrigger)

        Next



    End Sub
End Class
