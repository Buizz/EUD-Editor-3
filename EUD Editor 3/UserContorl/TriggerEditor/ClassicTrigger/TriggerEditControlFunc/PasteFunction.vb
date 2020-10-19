Imports Newtonsoft.Json

Partial Public Class TriggerEditControl

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
            Dim Triggers As List(Of TriggerCodeBlock) = JsonConvert.DeserializeObject(PasteText, GetType(List(Of TriggerCodeBlock)))

            If Triggers.Count = 0 Then
                LastAble = False
                Return False
            Else
                If Triggers.First.CName <> "TriggerCodeBlock" Then
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
    Private Sub Paste_Click(sender As Object, e As RoutedEventArgs)
        PasteFunc()
    End Sub

    Private Sub PasteFunc()
        If Not IsPasteAble() Then
            Return
        End If

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





        Dim PasteText As String = My.Computer.Clipboard.GetText
        Dim TriggerCodes As List(Of TriggerCodeBlock) = JsonConvert.DeserializeObject(PasteText, GetType(List(Of TriggerCodeBlock)))

        Dim InsertPos As Integer = -1

        If tlist.SelectedIndex <> -1 Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In tlist.SelectedItems
                SelectList.Add(sitem)
            Next

            SelectList.Sort(Function(x, y)
                                Return tlist.Items.IndexOf(x).CompareTo(tlist.Items.IndexOf(y))
                            End Function)
            InsertPos = tlist.Items.IndexOf(SelectList.Last)
        End If


        tlist.SelectedIndex = -1



        For i = TriggerCodes.Count - 1 To 0 Step -1
            Dim trg As TriggerCodeBlock = TriggerCodes(i)


            Dim NCode As TriggerCodeBlock = trg.DeepCopy
            Dim tt As New ListBoxItem
            tt.Content = New ListItemCodeBlock(NCode)


            pjData.SetDirty(True)

            If InsertPos = -1 Then
                ttriglist.Add(NCode)
                tlist.Items.Add(tt)
            Else
                ttriglist.Insert(InsertPos + 1, NCode)
                tlist.Items.Insert(InsertPos + 1, tt)
            End If
            tt.IsSelected = True
        Next
    End Sub
End Class
