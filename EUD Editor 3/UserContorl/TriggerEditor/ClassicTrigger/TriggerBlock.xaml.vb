Imports MaterialDesignThemes.Wpf

Public Class TriggerBlock

    Public trg As Trigger
    Private scripter As ScriptEditor

    Public Sub New(_scripter As ScriptEditor, _trg As Trigger)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        trg = _trg
        scripter = _scripter
        Refresh()
    End Sub

    Public Sub Refresh()
        TriggerEnabledCB.IsChecked = trg.IsEnabled
        TriggerPreservedCB.IsChecked = trg.IsPreserved

        If trg.CommentStr <> "" Then
            CommentTB.Text = trg.CommentStr
            CommentTB.Visibility = Visibility.Visible
            ContentPanel.Visibility = Visibility.Collapsed
        Else
            CommentTB.Visibility = Visibility.Collapsed
            ContentPanel.Visibility = Visibility.Visible

            ConditionPanel.Children.Clear()
            For i = 0 To trg.Condition.Count - 1
                Dim titem As New ListItemCodeBlock(scripter, trg.Condition(i))
                ConditionPanel.Children.Add(titem)
            Next
            If trg.Condition.Count = 0 Then
                Dim tlabel As New Label
                tlabel.Content = "조건이 없습니다."

                ConditionPanel.Children.Add(tlabel)
            End If



            ActionPanel.Children.Clear()
            For i = 0 To trg.Actions.Count - 1
                Dim titem As New ListItemCodeBlock(scripter, trg.Actions(i))
                ActionPanel.Children.Add(titem)
            Next
            If trg.Actions.Count = 0 Then
                Dim tlabel As New Label
                tlabel.Content = "액션이 없습니다."

                ActionPanel.Children.Add(tlabel)
            End If



        End If


        PlayerList.Children.Clear()

        For i = 0 To 7
            If trg.PlayerEnabled(i) Then
                Dim tlabel As New Label
                tlabel.Content = "P" & i + 1

                PlayerList.Children.Add(tlabel)


                'Dim mchip As New Chip
                'mchip.Background = Application.Current.Resources("PrimaryHueMidBrush")
                'mchip.IsHitTestVisible = False

                'mchip.Content = "P" & i + 1

                'PlayerList.Children.Add(mchip)
            End If
        Next
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        trg.IsEnabled = True
    End Sub

    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        trg.IsEnabled = False
    End Sub

    Private Sub PreservedCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        trg.IsPreserved = True
    End Sub

    Private Sub PreservedCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        trg.IsPreserved = False
    End Sub
End Class
