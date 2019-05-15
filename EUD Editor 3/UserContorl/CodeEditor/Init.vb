Imports System.IO
Imports System.Xml
Imports System.Reflection
Imports System.Windows.Threading
Imports ICSharpCode.AvalonEdit.Folding
Imports ICSharpCode.AvalonEdit.Highlighting
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports System.Text.RegularExpressions

Partial Public Class CodeEditor
    Private Sub InitTextEditor()
        Dim customHighlighting As IHighlightingDefinition
        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3.EpsHighlightingDark.xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting("EpsHighlightingDark", {".eps"}, customHighlighting)
        TextEditor.SyntaxHighlighting = customHighlighting


        LocalFunc = New CFunc
        ExternFunc = New CFunc


        Dim foldingUpdateTimer As DispatcherTimer = New DispatcherTimer()
        foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2)
        AddHandler foldingUpdateTimer.Tick, AddressOf foldingUpdateTimer_Tick
        foldingUpdateTimer.Start()

        FoldingManager = FoldingManager.Install(TextEditor.TextArea)

        TextEditor.SetValue(FoldingMargin.FoldingMarkerBrushProperty, Brushes.LightGray)
        TextEditor.SetValue(FoldingMargin.SelectedFoldingMarkerBrushProperty, Brushes.LightPink)
        TextEditor.SetValue(FoldingMargin.SelectedFoldingMarkerBackgroundBrushProperty, Brushes.LightGray)

        FoldingStrategy = New EPSFoldingStrategy()
        FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document)



        AddHandler TextEditor.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler TextEditor.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
    End Sub

    Private FoldingManager As FoldingManager
    Private FoldingStrategy As EPSFoldingStrategy

    Private Sub foldingUpdateTimer_Tick(sender As Object, e As EventArgs)
        If FoldingStrategy IsNot Nothing Then
            FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document)
        End If
    End Sub




    '현재 위치로 부터 뒤로 간다.
    '만약 엔터가 나오면 얆짤없이 끝 End

    '이름()

    '괄호를 입력하는 순간 뒤에 )이거 추가됨!


    'asdfg(adsf, 123, 344, 312, 33, 11



    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        Dim MainStr As String = TextEditor.Document.Text
        Dim SelectStart As Integer = TextEditor.SelectionStart


        Dim LastStr As String = ""
        Dim FuncName As String = ""
        Dim CheckFunctionName As String = ""
        Dim ArgumentIndex As Integer
        Dim AnotherCharCount As Integer
        Dim IsFirstArgumnet As Boolean = False


        Dim index As Integer = 0
        Dim bracketCount As Integer = 0
        Dim GetFuncName As Boolean = False
        Dim CheckFunction As Boolean = False
        Dim IsFunctionSpace As Boolean = False
        While ((SelectStart - index) > 0)
            Dim MidStr As String = Mid(MainStr, SelectStart - index, 1)


            Select Case MidStr'이게 뜰때 에러가 아니라 앞에 function이 있는지 확인까지 해야됨. 
                Case vbLf
                    Exit While
                Case "("
                    If GetFuncName Then
                        Exit While
                    Else
                        If bracketCount > 0 Then
                            bracketCount -= 1
                        Else
                            GetFuncName = True
                        End If
                    End If
                Case ")"
                    If GetFuncName Then
                        Exit While
                    Else
                        bracketCount += 1
                    End If
                Case " "
                    If CheckFunction Then
                        If GetFuncName Then
                            Exit While
                        End If
                    Else
                        If GetFuncName Then
                            IsFunctionSpace = True
                        End If
                    End If
                Case ","
                    If ArgumentIndex = 0 And AnotherCharCount = 0 Then
                        IsFirstArgumnet = True
                    End If
                    ArgumentIndex += 1
                Case Else
                    If GetFuncName Then
                        If IsFunctionSpace Then
                            CheckFunction = True
                        End If
                    End If

                    If index <> 0 Then
                        AnotherCharCount += 1
                    End If
            End Select
            If GetFuncName And Not IsFunctionSpace Then
                If MidStr <> "(" Then
                    FuncName = MidStr & FuncName
                End If
            End If
            If CheckFunction Then
                CheckFunctionName = MidStr & CheckFunctionName
            End If


            LastStr = MidStr & LastStr

            index += 1
        End While
        If CheckFunctionName = "function" Then
            FuncName = ""
            ArgumentIndex = 0
        End If



        Log.Text = LastStr & " " & SelectStart & vbCrLf & "함수이름 : " & FuncName & "     함수 인자 번호 : " & ArgumentIndex & vbCrLf & IsFirstArgumnet & vbCr & CheckFunctionName

        'Dim selectedValues As List(Of InvoiceSOA)
        'selectedValues = DisputeList.FindAll(Function(p) p.ColumnName = "Jewel")

        LocalFunc.LoadFunc(MainStr)

        ShowCompletion(e.Text, False, FuncName, ArgumentIndex, IsFirstArgumnet)

        ShowFuncTooltip(FuncName, ArgumentIndex, index)




        'Log.Text = Log.Text & vbCrLf & "입력된 함수 갯수 : " & LocalFunc.FuncCount

        'For i = 0 To LocalFunc.FuncCount - 1
        '    Log.Text = Log.Text & vbCrLf & LocalFunc.GetFuncName(i) & " : " & LocalFunc.GetFuncArgument(i)
        'Next
    End Sub

    Private Sub ShowFuncTooltip(FuncName As String, ArgumentIndex As Integer, Startindex As Integer)
        Dim funArgument As Border = LocalFunc.GetToolTip(FuncName, ArgumentIndex)
        If funArgument IsNot Nothing Then
            ToltipBorder.Child = funArgument
            ' m_toolTip.Content = funArgument
            If ToltipBorder.Visibility = Visibility.Hidden Then
                Dim StartPostion As TextViewPosition = TextEditor.TextArea.Caret.Position
                StartPostion.VisualColumn -= Startindex

                If StartPostion.Line > 1 Then
                    StartPostion.Line -= 1
                End If


                Dim p As Point = TextEditor.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)
                ToltipBorder.Margin = New Thickness(p.X + 36, p.Y - 5 - TextEditor.VerticalOffset, 0, 0)
                ToltipBorder.Visibility = Visibility.Visible
            End If


        Else
            If ToltipBorder.Visibility = Visibility.Visible Then
                ToltipBorder.Visibility = Visibility.Hidden
            End If

        End If
    End Sub




    Private LocalFunc As CFunc
    Private ExternFunc As CFunc

    Private funcThread As Threading.Thread



    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If e.Text = vbTab Then
                completionWindow.CompletionList.RequestInsertion(e)
            ElseIf Not Char.IsLetterOrDigit(e.Text(0)) Then
                completionWindow.Close()
            End If
        End If
    End Sub

    Private Sub ShowCompletion(ByVal enteredText As String, ByVal controlSpace As Boolean, FuncNameas As String, ArgumentCount As Integer, IsFirstArgumnet As Boolean)
        If Not controlSpace Then
            Debug.WriteLine("Code Completion: TextEntered: " & enteredText)
        Else
            Debug.WriteLine("Code Completion: Ctrl+Space")
        End If




        If completionWindow Is Nothing Then
            If Char.IsLetterOrDigit(enteredText) Or enteredText = "_" Then
                completionWindow = New CompletionWindow(TextEditor.TextArea)
                completionWindow.CloseWhenCaretAtBeginning = controlSpace

                '만약 enteredText로 인해 CompletionData가 남아있을 경우 -1을 한다.
                '아니면 -1을 하지 않고 SelectItem을 하지 않는다.

                '리스트에 엔터텍스트가 포함되어있을 경우 넣으면서 -1한다. 아니면 건들지 않는다.
                completionWindow.StartOffset -= 1

                LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet)


                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")
                'If results.TriggerWordLength > 0 Then
                '    completionWindow.CompletionList.SelectItem(results.TriggerWord)
                'End If


                completionWindow.Width = 400


                completionWindow.Show()
                AddHandler completionWindow.Closed, Sub()
                                                        completionWindow = Nothing
                                                    End Sub
                completionWindow.CompletionList.SelectItem(enteredText)

                If completionWindow.CompletionList.SelectedItem Is Nothing Then
                    completionWindow.Visibility = Visibility.Collapsed
                End If
            ElseIf enteredText = " " Or enteredText = "," Then
                completionWindow = New CompletionWindow(TextEditor.TextArea)
                completionWindow.CloseWhenCaretAtBeginning = controlSpace


                LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet)


                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")

                completionWindow.Width = 400


                completionWindow.Show()
                AddHandler completionWindow.Closed, Sub()
                                                        completionWindow = Nothing
                                                    End Sub
            Else
                Return
            End If
        Else
            If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
                completionWindow.Visibility = Visibility.Collapsed
            Else
                completionWindow.Visibility = Visibility.Visible
            End If
        End If
    End Sub


    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
    Private permissionChar() As String = {" ", "(", ")", "[", "]", "{", "}", vbTab}

    Private Sub completionWindowKeyDown(sender As Object, e As KeyEventArgs)
        If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
            completionWindow.Close()
        End If
    End Sub
End Class
