Public Class MacroFuncSetting
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        MouseLocationCB.Items.Clear()

        Dim loclist As List(Of String) = GetArgList("TrgLocation").ToList
        loclist.Insert(0, Tool.GetLanText("None"))

        For i = 0 To loclist.Count - 1
            Dim cbitem As New ComboBoxItem
            cbitem.Content = loclist(i)
            cbitem.Tag = loclist(i)

            MouseLocationCB.Items.Add(cbitem)

            If loclist(i) = pjData.TEData.MouseLocation Then
                MouseLocationCB.SelectedIndex = i
            End If
        Next
        If MouseLocationCB.SelectedIndex = -1 Then
            MouseLocationCB.SelectedIndex = 0
        End If


        ChatEventCB.IsChecked = pjData.TEData.UseChatEvent
        MSQCCB.IsChecked = pjData.TEData.UseMSQC


        addrTextBox.Text = Hex(pjData.TEData.__addr__).ToUpper
        ptrAddrTextBox.Text = Hex(pjData.TEData.__ptrAddr__).ToUpper
        patternAddrTextBox.Text = Hex(pjData.TEData.__patternAddr__).ToUpper
        lenAddrTextBox.Text = Hex(pjData.TEData.__lenAddr__).ToUpper
    End Sub



    Private Sub ChatEventCB_Checked(sender As Object, e As RoutedEventArgs)
        pjData.TEData.UseChatEvent = ChatEventCB.IsChecked
    End Sub

    Private Sub ChatEventCB_Unchecked(sender As Object, e As RoutedEventArgs)
        pjData.TEData.UseChatEvent = ChatEventCB.IsChecked
    End Sub

    Private Sub MSQCCB_Checked(sender As Object, e As RoutedEventArgs)
        pjData.TEData.UseMSQC = MSQCCB.IsChecked
    End Sub

    Private Sub MSQCCB_Unchecked(sender As Object, e As RoutedEventArgs)
        pjData.TEData.UseMSQC = MSQCCB.IsChecked
    End Sub

    Private Sub addrTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            Dim v As UInteger = "&H" & addrTextBox.Text
            pjData.TEData.__addr__ = v
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ptrAddrTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            Dim v As UInteger = "&H" & ptrAddrTextBox.Text
            pjData.TEData.__ptrAddr__ = v
        Catch ex As Exception
        End Try
    End Sub

    Private Sub patternAddrTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            Dim v As UInteger = "&H" & patternAddrTextBox.Text
            pjData.TEData.__patternAddr__ = v
        Catch ex As Exception
        End Try
    End Sub

    Private Sub lenAddrTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            Dim v As UInteger = "&H" & lenAddrTextBox.Text
            pjData.TEData.__lenAddr__ = v
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MouseLocationCB_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If MouseLocationCB.SelectedIndex = 0 Then
            pjData.TEData.MouseLocation = ""
        Else
            pjData.TEData.MouseLocation = CType(MouseLocationCB.SelectedItem, ComboBoxItem).Tag
        End If
    End Sub
End Class
