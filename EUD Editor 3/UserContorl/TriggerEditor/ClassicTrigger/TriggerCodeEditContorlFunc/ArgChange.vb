Partial Public Class TriggerCodeEditControl
    Private Function NewArgBtn(argIndex As Integer, tCode As TriggerCodeBlock) As Button
        Dim tBtn As New Button


        tBtn.Tag = argIndex
        tBtn.Content = tCode.LoadedArgString(argIndex)

        Dim targindex As Integer = argIndex
        AddHandler tBtn.Click, Sub(sender As Button, e As RoutedEventArgs)
                                   Dim pos As Point = tBtn.PointToScreen(New Point(0, 0))
                                   'TriggerArgsIndex = targindex

                                   'Dim ArgChanger As New TriggerEditValueSelecterWindow(tCode, argIndex, pos)
                                   AddHandler TriggerArgsEdit.ValueChange, Sub(_sender As Object, _e As RoutedEventArgs)
                                                                               tCode.LoadArgText(MainScriptEditor)

                                                                               tBtn.Content = tCode.LoadedArgString(targindex)
                                                                               BtnRefresh()
                                                                           End Sub
                                   Tool.OpenArgWindow(MainScriptEditor, tCode, targindex, pos, OtherPage, _Loc:=tLoc & SelectTBlock.FName & "->", ButtonHeight:=tBtn.Height)
                               End Sub
        Return tBtn
    End Function


    Private Sub FuncRefresh()
        TriggerPanel.Children.Clear()
        SummaryPanel.Children.Clear()

        '함수를 재정렬하여 쓴다.
        Dim t As TriggerFunction = SelectTBlock.GetCodeFunction(MainScriptEditor)
        Dim isCmpTrigger As Boolean
        Dim IsEmpty As Boolean = False

        If t Is Nothing Then
            '없는 함수
            '그냥 인자들만 출력시킨다.
            isCmpTrigger = False
            IsEmpty = True
        Else
            isCmpTrigger = t.IsCmpTrigger
        End If

        SelectTBlock.LoadArgText(MainScriptEditor)

        If isCmpTrigger Then
            If t.SortArgList.Count = 0 Then
                Dim ttb As New Label
                ttb.VerticalContentAlignment = VerticalAlignment.Center

                If Not IsEmpty Then
                    ttb.Content = t.FSummary
                End If

                TriggerPanel.Children.Add(ttb)
            Else
                '트리거
                For i = 0 To t.SortArgList.Count - 1
                    Dim tstr As String = t.SortArgList(i)

                    If TriggerFunction.IsArg(tstr) Then
                        Dim tindex As Integer = TriggerFunction.GetArgIndex(tstr)
                        Dim ttb As Button = NewArgBtn(tindex, SelectTBlock)

                        TriggerPanel.Children.Add(ttb)
                    Else
                        Dim ttb As New Label
                        ttb.VerticalContentAlignment = VerticalAlignment.Center


                        ttb.Content = tstr

                        TriggerPanel.Children.Add(ttb)
                    End If
                Next
            End If


        Else
            '인자들을 나열한다.
            For i = 0 To SelectTBlock.Args.Count - 1
                Dim ttb As Button = NewArgBtn(i, SelectTBlock)

                TriggerPanel.Children.Add(ttb)
            Next

            If True Then
                '설명
                Dim ttb As New Label
                ttb.VerticalContentAlignment = VerticalAlignment.Center

                If Not IsEmpty Then
                    ttb.Content = t.FSummary
                End If

                SummaryPanel.Children.Add(ttb)
            End If

        End If



        'SelectTBlock
    End Sub
End Class
