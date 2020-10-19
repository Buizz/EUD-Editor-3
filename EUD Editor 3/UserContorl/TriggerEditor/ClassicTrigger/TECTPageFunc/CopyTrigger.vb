Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Newtonsoft.Json

Partial Public Class TECTPage
    Private Sub TriggerCopyBtn_Click(sender As Object, e As RoutedEventArgs)
        CopyTrigger()
    End Sub
    Public Sub CopyTrigger()
        '바로 복사해서 붙여넣는다.

        If TListBox.SelectedItem IsNot Nothing Then
            Dim SelectList As New List(Of ListBoxItem)
            For Each sitem In TListBox.SelectedItems
                SelectList.Add(sitem)
            Next


            SelectList.Sort(Function(x, y)
                                Return TListBox.Items.IndexOf(x).CompareTo(TListBox.Items.IndexOf(y))
                            End Function)

            Dim Triggers As New List(Of Trigger)
            For i = 0 To SelectList.Count - 1
                Triggers.Add(GetTrg(SelectList(i)))
            Next



            Dim TextStr As String = JsonConvert.SerializeObject(Triggers)
            My.Computer.Clipboard.SetText(TextStr)

            'Dim stream As MemoryStream = New MemoryStream()
            'Dim formatter As BinaryFormatter = New BinaryFormatter()
            'formatter.Serialize(stream, Triggers)
            'stream.Position = 0


            'Dim bytes() As Byte = stream.ToArray()
            'Dim TextStr As String = Convert.ToBase64String(bytes)
            'My.Computer.Clipboard.SetText(TextStr)

            'MsgBox(TextStr)


            'Dim rScriptBlock As Trigger = formatter.Deserialize(stream)



            'TListBox.SelectedIndex = -1
            'For Each sitem In SelectList
            '    Dim trg As Trigger = GetTrg(sitem)


            '    Dim NTrigger As Trigger = trg.DeepCopy
            '    Dim tt As ListBoxItem = GetListItem(NTrigger)
            '    pjData.SetDirty(True)

            '    Scripter.TriggerList.Add(NTrigger)
            '    TListBox.Items.Add(tt)
            '    tt.IsSelected = True
            'Next
            BtnRefresh()
        End If
    End Sub
End Class
