Public Class TEGUIObjectSelecter
    Private LastSelectGroup As String

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        For i = 0 To tescm.dicTriggerScript.Keys.Count - 1
            Dim keyname As String = tescm.dicTriggerScript.Keys(i)
            Dim header As String = Tool.GetText(keyname)



            Dim tlistitem As New ListBoxItem
            tlistitem.Content = header.Split("|").First
            tlistitem.Tag = keyname
            tlistitem.Background = New SolidColorBrush(TriggerScript.GetColor(keyname))
            tlistitem.Foreground = Brushes.FloralWhite

            ToolBoxList.Items.Add(tlistitem)

        Next
        ToolBoxList.SelectedIndex = 0
    End Sub


    Private Sub ToolBoxList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim Selectitem As ListBoxItem = ToolBoxList.SelectedItem

        If Selectitem IsNot Nothing Then
            Dim ScriptGroup As String = Selectitem.Tag
            If LastSelectGroup <> ScriptGroup Then '이전 선택과 다를 경우
                LastSelectGroup = ScriptGroup

                Colorbar.Background = New SolidColorBrush(TriggerScript.GetColor(ScriptGroup))

                ToolBox.Items.Clear()


                For i = 0 To tescm.dicTriggerScript(LastSelectGroup).Count - 1
                    If Not tescm.dicTriggerScript(LastSelectGroup)(i).IsLock Then
                        Dim tlistboxitem As New ListBoxItem
                        tlistboxitem.Content = tescm.dicTriggerScript(LastSelectGroup)(i).SName

                        ToolBox.Items.Add(tlistboxitem)
                    End If
                Next
            End If
        End If
    End Sub



    Public Sub ListRefresh(SelectItem As TriggerScript)
        '선택한 아이템에 따라 표시되는 항목이 바뀜
    End Sub





    Public Event ItemSelect As RoutedEventHandler
    Private Sub ToolBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim Selectitem As ListBoxItem = ToolBox.SelectedItem
        If Selectitem IsNot Nothing Then
            ToolBox.SelectedIndex = -1
            RaiseEvent ItemSelect(Selectitem.Content, e)
        End If
    End Sub


End Class
