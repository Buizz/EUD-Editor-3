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
        Dim highlightName As String = ""

        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            highlightName = "EpsHighlightingDark"
        Else
            highlightName = "EpsHighlightingLight"
        End If




        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3." & highlightName & ".xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting(highlightName, {".eps"}, customHighlighting)
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

        'TextEditor.Options.ShowSpaces = True
        'TextEditor.Options.ShowTabs = True
        'TextEditor.Options.HighlightCurrentLine = True
        'TextEditor.Options.ShowColumnRuler = True

        AddHandler TextEditor.TextArea.PreviewKeyDown, AddressOf TextEditorPreviewKey
        AddHandler TextEditor.TextArea.PreviewKeyUp, AddressOf TextEditorPreviewKey
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

    Private ToolTipUpdateTimer As DispatcherTimer

    Private Sub ToolTipPosUpdateTimer_Tick(sender As Object, e As EventArgs)
        MoveToolTipBox()
        ToolTipUpdateTimer.Stop()
        ToolTipUpdateTimer = Nothing
    End Sub


    '현재 위치로 부터 뒤로 간다.
    '만약 엔터가 나오면 얆짤없이 끝 End

    '이름()

    '괄호를 입력하는 순간 뒤에 )이거 추가됨!


    'asdfg(adsf, 123, 344, 312, 33, 11



    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        pjData.SetDirty(True)

        Dim MainStr As String = TextEditor.Document.Text
        Dim SelectStart As Integer = TextEditor.SelectionStart


        Dim LastStr As String = ""
        Dim TypingStr As String = ""
        Dim TypingStrSave As String = ""
        Dim CheckChar As Boolean = False
        Dim FuncName As String = ""
        Dim CheckFunctionName As String = ""
        Dim ArgumentIndex As Integer
        Dim AnotherCharCount As Integer
        Dim IsFirstArgumnet As Boolean = False


        Dim index As Integer = 0
        Dim FunctionStartOffset As Integer = 0
        Dim bracketCount As Integer = 0
        Dim GetFuncName As Boolean = False
        Dim CheckFunction As Boolean = False
        Dim IsFunctionSpace As Boolean = False
        Dim IsQuotes As Boolean = False
        Dim QuotesCount As Integer
        While ((SelectStart - index) > 0)
            Dim MidStr As String = Mid(MainStr, SelectStart - index, 1)


            Select Case MidStr'이게 뜰때 에러가 아니라 앞에 function이 있는지 확인까지 해야됨. 
                Case vbLf
                    Exit While
                Case "("
                    If ArgumentIndex = 0 And AnotherCharCount = 0 Then
                        IsFirstArgumnet = True
                    End If
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
                Case " ", vbTab
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
                    If Not CheckFunction And Not IsFunctionSpace Then
                        If ArgumentIndex = 0 And AnotherCharCount = 0 Then
                            IsFirstArgumnet = True
                        End If
                        ArgumentIndex += 1
                    End If
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
            If Not Char.IsLetterOrDigit(MidStr) And MidStr <> "_" Then
                CheckChar = True
            End If
            If Not CheckChar Then
                TypingStr = MidStr & LastStr
            End If
            If Not IsFunctionSpace Then
                FunctionStartOffset += 1
            End If

            If MidStr = """" Or MidStr = "'" Then
                QuotesCount += 1
            End If



            LastStr = MidStr & LastStr

            If Not IsQuotes And (MidStr = """" Or MidStr = "'") Then
                TypingStrSave = TypingStr
                TypingStr = LastStr
                IsQuotes = True
            End If


            index += 1
        End While

        Dim IsFuncDefWrite As Boolean = False
        Dim IsFuncNameWrite As Boolean = False
        '이 경우 자동완성을 조금 다르게 해야됨
        If CheckFunctionName = "function" Then
            FuncName = ""
            ArgumentIndex = 0
            IsFuncDefWrite = True
        End If

        If QuotesCount Mod 2 = 0 And IsQuotes Then
            TypingStr = TypingStrSave
        End If





        FuncName = FuncName.Trim

        Log.Text = FunctionStartOffset & "   마지막으로 입력한 글자 : " & TypingStr & vbCrLf & "함수이름 : " & FuncName & "     함수 인자 번호 : " & ArgumentIndex

        Dim strStacks() As String = LastStr.Split(" ")
        If strStacks.Count >= 2 Then
            Dim temp As String = strStacks(strStacks.Count - 2)
            If temp = "function" Then
                IsFuncNameWrite = True
            End If
        End If

        Log.Text = Log.Text & vbCrLf & "IsFuncNameWrite : " & IsFuncNameWrite
        Log.Text = Log.Text & vbCrLf & "IsFuncDefWrite : " & IsFuncDefWrite






        'Dim selectedValues As List(Of InvoiceSOA)
        'selectedValues = DisputeList.FindAll(Function(p) p.ColumnName = "Jewel")
        If Not IsFuncDefWrite And Not IsFuncNameWrite Then
            LocalFunc.LoadFunc(MainStr, SelectStart)
            '외부함수 불러오는건 여기가아님


            AutoInserter(e.Text)

            ShowCompletion(e.Text, False, TypingStr, FuncName, ArgumentIndex, True)

            ShowFuncTooltip(FuncName, ArgumentIndex, FunctionStartOffset)
        ElseIf IsFuncDefWrite Then
            ShowCompletion(e.Text, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.IsFuncDefWrite)
        ElseIf IsFuncNameWrite Then
            ShowCompletion(e.Text, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.IsFuncNameWrite)
        End If

        'Log.Text = Log.Text & vbCrLf & "입력된 함수 갯수 : " & LocalFunc.FuncCount

        'For i = 0 To LocalFunc.FuncCount - 1
        '    Log.Text = Log.Text & vbCrLf & LocalFunc.GetFuncName(i) & " : " & LocalFunc.GetFuncArgument(i)
        'Next
    End Sub


    Private Enum SpecialFlag
        None
        IsFuncNameWrite
        IsFuncDefWrite
    End Enum


    Private OrginYPos As Integer
    Private Function ShowFuncTooltip(FuncName As String, ArgumentIndex As Integer, Startindex As Integer) As Boolean
        Dim funArgument As Border

        funArgument = LocalFunc.GetPopupToolTip(FuncName, ArgumentIndex)
        If funArgument Is Nothing Then
            funArgument = ExternFunc.GetPopupToolTip(FuncName, ArgumentIndex)
        End If
        If funArgument Is Nothing Then
            funArgument = Tool.TEEpsDefaultFunc.GetPopupToolTip(FuncName, ArgumentIndex)
        End If




        If funArgument IsNot Nothing Then
            ToltipBorder.Child = funArgument
            ' m_toolTip.Content = funArgument
            If ToltipBorder.Visibility = Visibility.Hidden Then
                Dim StartPostion As TextViewPosition = TextEditor.TextArea.Caret.Position
                'MsgBox(StartPostion.VisualColumn)
                StartPostion.VisualColumn -= Startindex
                'MsgBox(StartPostion.VisualColumn)

                'If StartPostion.Line > 1 Then
                '    StartPostion.Line -= 1
                'End If


                Dim p As Point = TextEditor.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)
                OrginYPos = p.Y - 5 - TextEditor.VerticalOffset
                ToltipBorder.Margin = New Thickness(p.X + 36, p.Y - 5 - TextEditor.VerticalOffset, 0, 0)
                ToltipBorder.Visibility = Visibility.Visible

                If ToolTipUpdateTimer Is Nothing Then
                    ToolTipUpdateTimer = New DispatcherTimer()
                    ToolTipUpdateTimer.Interval = TimeSpan.FromMilliseconds(1)
                    AddHandler ToolTipUpdateTimer.Tick, AddressOf ToolTipPosUpdateTimer_Tick
                    ToolTipUpdateTimer.Start()
                End If

            End If

            MoveToolTipBox()
            Return True
        Else
            If ToltipBorder.Visibility = Visibility.Visible Then
                ToltipBorder.Visibility = Visibility.Hidden
            End If

        End If
        Return False
    End Function




    Private LocalFunc As CFunc
    Private ExternFunc As CFunc

    Private funcThread As Threading.Thread



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

    Private Sub ShowCompletion(ByVal enteredText As String, ByVal controlSpace As Boolean, LastStr As String, FuncNameas As String, ArgumentCount As Integer, IsFirstArgumnet As Boolean, Optional Flag As SpecialFlag = SpecialFlag.None)
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
                If LastStr.Length = 0 Then
                    completionWindow.StartOffset -= 1
                Else
                    completionWindow.StartOffset -= LastStr.Length
                End If

                Select Case Flag
                    Case SpecialFlag.None
                        LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet)
                    Case SpecialFlag.IsFuncNameWrite
                        LoadFuncNameData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsFuncDefWrite
                        LoadFuncWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                End Select


                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")
                'If results.TriggerWordLength > 0 Then
                '    completionWindow.CompletionList.SelectItem(results.TriggerWord)
                'End If


                completionWindow.SizeToContent = SizeToContent.WidthAndHeight
                completionWindow.MinWidth = 120

                completionWindow.Show()

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
                    'completionWindow.Visibility = Visibility.Collapsed
                End If
            ElseIf enteredText = " " Or enteredText = "," Or enteredText = "(" Then 'IsFirstArgumnet And FuncNameas.Trim <> "" Then '
                completionWindow = New CompletionWindow(TextEditor.TextArea)
                completionWindow.CloseWhenCaretAtBeginning = controlSpace


                If LastStr.Length > 0 Then
                    completionWindow.StartOffset -= LastStr.Length
                End If

                Select Case Flag
                    Case SpecialFlag.None
                        LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet)
                    Case SpecialFlag.IsFuncNameWrite
                        LoadFuncNameData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsFuncDefWrite
                        LoadFuncWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                End Select

                completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
                completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")


                completionWindow.SizeToContent = SizeToContent.WidthAndHeight
                completionWindow.MinWidth = 120


                completionWindow.Show()



                AddHandler completionWindow.LocationChanged, AddressOf OnSizeChange
                'AddHandler completionWindow.PreviewKeyDown, AddressOf completionWindowKeyDown
                AddHandler completionWindow.Closed, Sub()
                                                        completionWindow = Nothing
                                                    End Sub


                If LastStr.Length > 0 Then
                    completionWindow.CompletionList.SelectItem(LastStr.Replace("""", "").Replace("'", ""))
                End If

                'If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
                '    completionWindow.Close()
                'End If
            Else
                Return
            End If
        End If
    End Sub


    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
    Private permissionChar() As String = {" ", "(", ")", "[", "]", "{", "}", vbTab}




    Private Sub OnSizeChange(sender As Object, e As EventArgs)
        MoveToolTipBox()


    End Sub


    Private Sub MoveToolTipBox()
        If ToltipBorder.Visibility = Visibility.Visible Then
            If completionWindow IsNot Nothing Then
                Dim StartPostion As TextViewPosition = TextEditor.TextArea.Caret.Position
                Dim p As Point = TextEditor.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)

                Dim CaretPos As Integer = p.Y - TextEditor.VerticalOffset
                Dim copPos As Integer = completionWindow.Top - TextEditor.PointToScreen(New Point(0, 0)).Y

                If CaretPos > copPos Then
                    ToltipBorder.Margin = New Thickness(ToltipBorder.Margin.Left, OrginYPos + 23, 0, 0)
                Else
                    ToltipBorder.Margin = New Thickness(ToltipBorder.Margin.Left, OrginYPos - (ToltipBorder.ActualHeight - 4), 0, 0)
                End If
            Else
                ToltipBorder.Margin = New Thickness(ToltipBorder.Margin.Left, OrginYPos - (ToltipBorder.ActualHeight - 4), 0, 0)
            End If
        End If
    End Sub


    Private Sub completionWindowKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            If completionWindow IsNot Nothing Then
                If completionWindow.CompletionList.SelectedItem Is Nothing Then
                    completionWindow.Close()

                    TextEditor.SelectedText = vbCrLf
                    TextEditor.SelectionLength -= 1
                    TextEditor.SelectionStart += 2
                End If
            End If

        End If
        'If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
        '    completionWindow.Close()
        'End If
    End Sub


    Private Sub TextEditorPreviewKey(sender As Object, e As KeyEventArgs)
        If completionWindow IsNot Nothing Then
            If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
                completionWindow.Close()
                '    completionWindow.Visibility = Visibility.Collapsed
                '    completionWindow.CompletionList.ListBox.SelectedIndex = -1
                '    completionWindow.CompletionList.ListBox.Visibility = Visibility.Collapsed
                'Else
                '    completionWindow.Visibility = Visibility.Visible
                '    completionWindow.CompletionList.ListBox.Visibility = Visibility.Visible
            End If
        End If
        If e.Key = Key.Up Or e.Key = Key.Down Then
            If completionWindow Is Nothing Then
                If ToltipBorder.Visibility = Visibility.Visible Then
                    ToltipBorder.Visibility = Visibility.Hidden
                End If
            End If
        End If

        If e.IsDown And e.Key = Key.Back Then
            AutoRemover()
        End If
        'If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
        '    completionWindow.Close()
        'End If
    End Sub

    Private Sub AutoInserter(keys As String)
        '/* 쓰면 */ 뒤에 써주는거
        '{ 쓰면 다음 줄에 }
        '( -> )
        '[ -> ]
        '' 쓰면 뒤에 ' 넣어주는거
        '" -> "


        '"의 경우 뒤에 문자가 "일 경우 뒤에 문자를 지운다.
        '앞의 문자가 "일 경우 추가로 저장하지 않는다.




        Select Case keys
            Case """", "'"
                Dim headchar As String
                Dim tailchar As String

                Try
                    headchar = TextEditor.Text.Chars(TextEditor.SelectionStart - 2)
                Catch ex As IndexOutOfRangeException
                    headchar = ""
                End Try
                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = ""
                End Try


                If headchar <> keys And tailchar <> keys Then
                    TextEditor.SelectedText = keys
                    TextEditor.SelectionLength -= 1
                    If headchar = "" Then
                        TextEditor.SelectionStart += 1
                    End If
                Else
                    If tailchar = keys Then
                        TextEditor.SelectionLength += 1
                        TextEditor.SelectedText = ""
                    End If
                End If
            Case "("
                TextEditor.SelectedText = ")"
                TextEditor.SelectionLength = 0
            Case "{"
                TextEditor.SelectedText = "}"
                TextEditor.SelectionLength = 0
            Case "["
                TextEditor.SelectedText = "]"
                TextEditor.SelectionLength = 0
            Case ")", "}", "]"
                Dim tailchar As String

                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = ""
                End Try

                If keys = tailchar Then
                    TextEditor.SelectionLength += 1
                    TextEditor.SelectedText = ""
                End If
        End Select
    End Sub

    Private Sub AutoRemover()
        Dim headchar As String
        Dim tailchar As String

        Try
            headchar = TextEditor.Text.Chars(TextEditor.SelectionStart - 1)
        Catch ex As IndexOutOfRangeException
            headchar = ""
        End Try
        Try
            tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
        Catch ex As IndexOutOfRangeException
            tailchar = ""
        End Try

        If (headchar = """" And tailchar = """") Or (headchar = "'" And tailchar = "'") Or
            (headchar = "(" And tailchar = ")") Or (headchar = "[" And tailchar = "]") Or (headchar = "{" And tailchar = "}") Then
            TextEditor.SelectionLength += 1
            TextEditor.SelectedText = ""
        End If
    End Sub
End Class
