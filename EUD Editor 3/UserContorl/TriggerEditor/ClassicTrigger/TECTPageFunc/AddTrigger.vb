Partial Public Class TECTPage
    Private Sub NewTriggerBtn_Click(sender As Object, e As RoutedEventArgs)
        AddTrigger()
    End Sub

    Public Sub AddTrigger()
        '원래는 window를 새로 열어야됨
        '편집기 열기

        pjData.SetDirty(True)


        Dim NTrigger As New Trigger
        EditWindow.Visibility = Visibility.Visible

        OpenStroyBoard.Begin(Me)
        Dim TrigEditctrl As New TriggerEditControl(Scripter, NTrigger)
        InputDialog.Content = TrigEditctrl


        AddHandler TrigEditctrl.OkayBtnEvent, Sub()
                                                  BtnRefresh()



                                                  Dim tcheck As Boolean = False
                                                  For k = 0 To 7
                                                      If NTrigger.PlayerEnabled(k) Then
                                                          tcheck = True
                                                      End If
                                                  Next
                                                  If Not tcheck Then
                                                      '모든 트리거가 꺼져있어서 추가불가능
                                                      Return
                                                  End If




                                                  Dim CurrentPage As Integer = GetPlayerListIndex()


                                                  Scripter.TriggerListCollection.Add(NTrigger)

                                                  For i = 0 To NTrigger.PlayerEnabled.Count - 1
                                                      If NTrigger.PlayerEnabled(i) Then
                                                          If CurrentPage <> i Then
                                                              SetPlayerListIndex(i)
                                                              RefreshTriggerPage()
                                                          Else
                                                              TListBox.Items.Add(GetListItem(NTrigger))
                                                              TListBox.SelectedIndex = TListBox.Items.Count - 1
                                                          End If
                                                          Exit For
                                                      End If
                                                  Next



                                                  PlayerListReset()



                                                  'InputDialog.Content = Nothing
                                                  'EditWindow.Visibility = Visibility.Hidden
                                                  CloseStroyBoard.Begin(Me)
                                              End Sub



        AddHandler TrigEditctrl.CancelBtnEvent, Sub()
                                                    BtnRefresh()

                                                    'InputDialog.Content = Nothing
                                                    'EditWindow.Visibility = Visibility.Hidden
                                                    CloseStroyBoard.Begin(Me)
                                                End Sub




    End Sub




    Public Function GetListItem(trg As Trigger) As ListBoxItem
        Dim nTrg As New TriggerBlock(Scripter, trg)


        Dim Listitem As New ListBoxItem

        Listitem.BorderThickness = New Thickness(1)
        Listitem.Background = Application.Current.Resources("MaterialDesignPaper")
        Listitem.HorizontalAlignment = HorizontalAlignment.Stretch
        Listitem.Content = nTrg

        Return Listitem
    End Function
End Class
