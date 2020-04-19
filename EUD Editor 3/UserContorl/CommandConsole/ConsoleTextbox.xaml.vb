Imports ICSharpCode.AvalonEdit

Public Class ConsoleTextbox

    Private LuaManager As LuaManager
    Private completion As ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        AddHandler ConsoleText.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler ConsoleText.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
        ConsoleText.Clear()

        LuaManager = New LuaManager(ConsoleLog)
    End Sub

    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow


    Private permissionChar() As String = {" ", "(", ")", "[", "]", "{", "}", vbTab}




    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        EnteredText(e.Text)
    End Sub


    Private Sub EnteredText(e As String)
        Dim MainStr As String = ConsoleText.Document.Text
        Dim SelectStart As Integer = ConsoleText.SelectionStart


        Dim LastStr As String = ""



        Dim index As Integer = 0
        While (SelectStart - index) > 0
            Dim MidStr As String = Mid(MainStr, SelectStart - index, 1)

            Dim ArgumentIndex As Integer = 0
            Select Case MidStr'이게 뜰때 에러가 아니라 앞에 function이 있는지 확인까지 해야됨. 
                Case vbLf
                    Exit While
            End Select


            LastStr = MidStr & LastStr
            index += 1
        End While

        Dim funcname As String = ""
        Dim argumentcount As Integer
        tooltippanel.Children.Clear()
        If (LastStr.IndexOf("(") >= 0) Then
            Dim temp As String() = LastStr.Split("(")

            funcname = temp(temp.Count - 2).Trim

            argumentcount = LastStr.Split(",").Count - 1


            Dim tooltips As TextBlock = LuaManager.GetTooltipf(funcname)
            If tooltips IsNot Nothing Then
                tooltippanel.Children.Add(tooltips)
            End If
            'tooltiplabel.Content = "함수 이름: " & funcname & vbCrLf & "인자 번호: " & argumentcount & vbCrLf & LuaManager.GetArgumentDefine(funcname, argumentcount)
            'tooltiplabel.Visibility = Visibility.Visible
        Else
            tooltiplabel.Visibility = Visibility.Collapsed
        End If
        ShowCompletion(e, LastStr, funcname, argumentcount)
        MoveToolTipBox()
    End Sub


    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If e.Text(0) = vbCrLf Then

            End If
            'completionWindow.CompletionList.RequestInsertion(e)


            If e.Text(0) = " " Then
                If completionWindow.CompletionList.SelectedItem Is Nothing Then
                    completionWindow.Close()
                End If
            ElseIf Not Char.IsLetterOrDigit(e.Text(0)) Then
                completionWindow.Close()
            End If
        End If
    End Sub


    Private Sub MoveToolTipBox()
        If completionWindow IsNot Nothing Then
            Dim StartPostion As TextViewPosition = ConsoleText.TextArea.Caret.Position
            Dim p As Point = ConsoleText.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)

            Dim CaretPos As Integer = p.Y - ConsoleText.VerticalOffset
            Dim copPos As Integer = completionWindow.Top - ConsoleText.PointToScreen(New Point(0, 0)).Y

            If CaretPos > copPos Then
                DockPanel.SetDock(tooltipstackpanel, Dock.Bottom)
            Else
                DockPanel.SetDock(tooltipstackpanel, Dock.Top)
            End If
        End If
    End Sub
    Private Sub OnSizeChange(sender As Object, e As EventArgs)
        MoveToolTipBox()
    End Sub
    Private Sub ShowCompletion(ByVal enteredText As String, LastStr As String, FuncNameas As String, ArgumentCount As Integer)
        Dim inputkey As String = enteredText

        If inputkey.Length <> 1 Then
            Return
        End If

        If completionWindow Is Nothing Then
            If Char.IsLetterOrDigit(enteredText) Or enteredText = "_" Then
                completionWindow = New ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(ConsoleText.TextArea)
                Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData

                Dim LastSelectStart As Integer = ConsoleText.SelectionStart
                Dim LastSelectLength As Integer = ConsoleText.SelectionLength

                If LastStr.Length = 0 Then
                    completionWindow.StartOffset -= 1
                Else
                    completionWindow.StartOffset -= LastStr.Length
                End If

                LoadAutocmp(data, FuncNameas, ArgumentCount, LastStr)




                'completionWindow.CompletionList.Foreground = Brushes.Black
                completionWindow.CompletionList.ListBox.Foreground = Application.Current.Resources("MaterialDesignBody")
                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")

                completionWindow.SizeToContent = SizeToContent.WidthAndHeight
                completionWindow.MinWidth = 120

                completionWindow.Show()

                'completionWindow.CompletionList.ListBox.Style = Application.Current.Resources("MaterialDesignToolToggleListBox")
                AddHandler completionWindow.LocationChanged, AddressOf OnSizeChange
                AddHandler completionWindow.Closed, Sub()
                                                        completionWindow = Nothing
                                                    End Sub

                If LastStr.Length = 0 Then
                    completionWindow.CompletionList.SelectItem(enteredText)
                Else
                    completionWindow.CompletionList.SelectItem(LastStr)
                End If

                If completionWindow.CompletionList.SelectedItem Is Nothing Then
                    completionWindow.Close()
                End If


            ElseIf enteredText = " " Or enteredText = vbTab Or enteredText = "," Or enteredText = "(" Or enteredText = "." Then
                completionWindow = New ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(ConsoleText.TextArea)
                Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData




                LoadAutocmp(data, FuncNameas, ArgumentCount, LastStr)



                completionWindow.CompletionList.ListBox.Foreground = Application.Current.Resources("MaterialDesignBody")
                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")


                completionWindow.SizeToContent = SizeToContent.WidthAndHeight
                completionWindow.MinWidth = 120


                completionWindow.Show()



                AddHandler completionWindow.LocationChanged, AddressOf OnSizeChange
                AddHandler completionWindow.Closed, Sub()
                                                        completionWindow = Nothing
                                                    End Sub


            End If


        End If


    End Sub


    Private LastCommand As New List(Of String)
    Private LastCommandIndex As Integer = 0
    Private LastCommandChange As Boolean = False

    Private RightShiftDown As Boolean
    Private ClearText As Boolean
    Private Sub ConsoleKeyDown(sender As Object, e As KeyEventArgs) Handles ConsoleText.PreviewKeyDown



        If e.Key = Key.RightShift Then
            RightShiftDown = True
        End If
        If e.Key = Key.Up Then
            LastCommandChange = False
            If LastCommandIndex > 0 Then
                LastCommandIndex -= 1
                ConsoleText.Text = LastCommand(LastCommandIndex)
            End If
        End If
        If e.Key = Key.Down Then
            LastCommandChange = False
            If LastCommandIndex < LastCommand.Count - 1 Then
                LastCommandIndex += 1
                ConsoleText.Text = LastCommand(LastCommandIndex)
            End If
        End If

        If e.Key = Key.Return Then
            If RightShiftDown Then
                If completionWindow IsNot Nothing Then
                    completionWindow.Close()
                End If
                'ConsoleText.SelectedText = vbCrLf
                'ConsoleText.SelectionStart += 1
            Else
                If ConsoleText.Text.Trim <> "" Then
                    ConsoleLog.AppendText(vbCrLf)
                    ConsoleLog.AppendText(ConsoleText.Text)

                    Try
                        If LastCommandIndex = LastCommand.Count - 1 Then
                            '만약 현재 커맨드 인덱스가 마지막위치라면
                            LastCommand.Add(ConsoleText.Text)
                            LastCommandIndex += 1
                        Else
                            LastCommand.Add(ConsoleText.Text)
                            If LastCommandChange Then
                                LastCommandIndex = LastCommand.Count
                            End If
                        End If



                        LuaManager.DoString(ConsoleText.Text)
                    Catch ex As Exception
                        'MsgBox(ex.ToString)
                        ConsoleLog.AppendText(vbCrLf)
                        ConsoleLog.AppendText(ex.Message)
                        'ConsoleLog.AppendText(ex.ToString)
                    End Try

                    ConsoleLog.ScrollToEnd()
                    ConsoleText.Clear()
                    tooltiplabel.Visibility = Visibility.Collapsed
                    ClearText = True
                Else
                    ConsoleText.Clear()
                    tooltiplabel.Visibility = Visibility.Collapsed
                    ClearText = True
                End If
            End If
        Else
            If Not e.Key = Key.Up And Not e.Key = Key.Down Then
                LastCommandChange = True
            End If

        End If
    End Sub
    Private Sub ConsoleKeyUp(sender As Object, e As KeyEventArgs) Handles ConsoleText.KeyUp
        If e.Key = Key.RightShift Then
            RightShiftDown = False
        End If
        If ClearText Then
            ConsoleText.Clear()
            ClearText = False
        End If
        If e.Key = Key.Back Then
            EnteredText(" ")
        End If
    End Sub





    Private Sub OpenFucnFolder(sender As Object, e As RoutedEventArgs)
        Process.Start("explorer.exe", "/root," & LuaManager.LuaFloderPath)
    End Sub

    Private Sub refreshLua(sender As Object, e As RoutedEventArgs)
        LuaManager = New LuaManager(ConsoleLog)
    End Sub

    Private Sub LogClear(sender As Object, e As RoutedEventArgs)
        ConsoleLog.Clear()
    End Sub
End Class
