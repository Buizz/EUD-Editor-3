Imports System.IO
Imports System.Xml
Imports System.Reflection
Imports System.Windows.Threading
Imports ICSharpCode.AvalonEdit.Folding
Imports ICSharpCode.AvalonEdit.Highlighting
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion

Partial Public Class CodeEditor
    Private TEFile As TEFile
    Public Sub Init(tTEFile As TEFile)
        TEFile = tTEFile
    End Sub


    '외부 참조 파일들을 저장해야됨.
    '사용하지 않는거로 추정되면 삭제도 해야됨
    '즉 관리하는게 필요



    Private foldingUpdateTimer As DispatcherTimer
    Private Sub InitTextEditor()
        Dim SPanel As Search.SearchPanel = Search.SearchPanel.Install(TextEditor)
        SPanel.MarkerBrush = New SolidColorBrush(Color.FromArgb(120, 125, 125, 125))
        SPanel.Localization = New CodeSearchLocalization



        Dim customHighlighting As IHighlightingDefinition
        Dim highlightName As String = ""

        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            highlightName = "EpsHighlightingDark"
        Else
            highlightName = "EpsHighlightingLight"
        End If

        ExternFiles = New List(Of ExternFile)


        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3." & highlightName & ".xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting(highlightName, {".eps"}, customHighlighting)
        TextEditor.SyntaxHighlighting = customHighlighting


        LocalFunc = New CFunc

        foldingUpdateTimer = New DispatcherTimer()
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

        AddHandler TextEditor.TextArea.LostKeyboardFocus, AddressOf TextEditorLostFocus

        AddHandler TextEditor.TextArea.PreviewKeyDown, AddressOf TextEditorPreviewKey
        AddHandler TextEditor.TextArea.PreviewKeyUp, AddressOf TextEditorPreviewKey
        AddHandler TextEditor.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler TextEditor.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
    End Sub

    Private Sub TextEditorLostFocus(sender As Object, e As EventArgs)
        TooltipHide()
    End Sub




    Private FoldingManager As FoldingManager
    Private FoldingStrategy As EPSFoldingStrategy

    Private Sub foldingUpdateTimer_Tick(sender As Object, e As EventArgs)
        If FoldingStrategy IsNot Nothing Then
            FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document)
        End If
        ExternerLoader()
    End Sub

    Private ToolTipUpdateTimer As DispatcherTimer

    Private ToolTipCounter As Integer = 0
    Private Sub ToolTipPosUpdateTimer_Tick(sender As Object, e As EventArgs)
        MoveToolTipBox()
        If ToolTipCounter = 0 Then
            ToolTipUpdateTimer.Stop()
            ToolTipUpdateTimer = Nothing
        Else
            ToolTipCounter -= 1
        End If
    End Sub


    '현재 위치로 부터 뒤로 간다.
    '만약 엔터가 나오면 얆짤없이 끝 End

    '이름()

    '괄호를 입력하는 순간 뒤에 )이거 추가됨!


    'asdfg(adsf, 123, 344, 312, 33, 11







    Private Enum SpecialFlag
        None
        IsFuncNameWrite
        IsFuncDefWrite
        IsImportWrite
        Extern
    End Enum


    Private OrginXPos As Integer
    Private OrginYPos As Integer
    Private Function ShowFuncTooltip(FuncName As String, ArgumentIndex As Integer, Startindex As Integer) As Boolean
        Dim funArgument As Border = Nothing


        Dim pieceStr() As String = FuncName.Split(".")


        Dim NameSpace_ As String = pieceStr.First
        Dim FuncrName As String = pieceStr.Last
        If FuncName.IndexOf(".") >= 0 Then
            Dim VarName As String = pieceStr.First

            If pieceStr.Length > 2 Then
                VarName = pieceStr(pieceStr.Length - 2)
            End If
            For i = 0 To LocalFunc.VariableCount - 1
                If VarName = LocalFunc.GetVariableNames(i) Then
                    Dim VarType As String = LocalFunc.GetVariableType(i)
                    If VarType.IndexOf("(") >= 0 Then
                        'Object나 함수인 경우
                        Dim ObjectName As String = VarType.Split("(").First


                        '다른거 다 조사하기
                        For k = 0 To LocalFunc.ObjectCount - 1
                            If ObjectName = LocalFunc.GetObject(k).ObjName Then
                                funArgument = LocalFunc.GetObject(k).Functions.GetPopupToolTip(FuncrName, ArgumentIndex)
                                Exit For
                            End If
                        Next

                        For k = 0 To Tool.TEEpsDefaultFunc.ObjectCount - 1
                            If ObjectName = Tool.TEEpsDefaultFunc.GetObject(k).ObjName Then
                                funArgument = Tool.TEEpsDefaultFunc.GetObject(k).Functions.GetPopupToolTip(FuncrName, ArgumentIndex)
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next





            For i = 0 To ExternFiles.Count - 1
                If ExternFiles(i).nameSpaceName = NameSpace_ Then
                    funArgument = ExternFiles(i).Funcs.GetPopupToolTip(FuncrName, ArgumentIndex)
                    'If funArgument Is Nothing Then
                    '    funArgument = ExternFunc.GetPopupToolTip(FuncName, ArgumentIndex)
                    'End If
                    Exit For
                End If
            Next

        Else
            funArgument = LocalFunc.GetPopupToolTip(FuncName, ArgumentIndex)
            'If funArgument Is Nothing Then
            '    funArgument = ExternFunc.GetPopupToolTip(FuncName, ArgumentIndex)
            'End If
            If funArgument Is Nothing Then
                funArgument = Tool.TEEpsDefaultFunc.GetPopupToolTip(FuncName, ArgumentIndex)
            End If
        End If






        If funArgument IsNot Nothing Then
            PopupToolTip.tBorder.Child = funArgument

            'ToltipBorder.Child = funArgument
            ' m_toolTip.Content = funArgument
            If PopupToolTip.Visibility = Visibility.Hidden Then
                Dim StartPostion As TextViewPosition = TextEditor.TextArea.Caret.Position
                'MsgBox(StartPostion.VisualColumn)
                StartPostion.VisualColumn -= Startindex
                'MsgBox(StartPostion.VisualColumn)

                'If StartPostion.Line > 1 Then
                '    StartPostion.Line -= 1
                'End If


                Dim p As Point = TextEditor.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)
                OrginXPos = p.X + 36
                OrginYPos = p.Y - 5 - TextEditor.VerticalOffset
                'ToltipBorder.Margin = New Thickness(p.X + 36, p.Y - 5 - TextEditor.VerticalOffset, 0, 0)
                TooltipShow()

                If ToolTipUpdateTimer Is Nothing Then
                    ToolTipCounter = 100
                    ToolTipUpdateTimer = New DispatcherTimer()
                    ToolTipUpdateTimer.Interval = TimeSpan.FromMilliseconds(10)
                    AddHandler ToolTipUpdateTimer.Tick, AddressOf ToolTipPosUpdateTimer_Tick
                    ToolTipUpdateTimer.Start()
                Else
                    ToolTipCounter += 10
                End If

            End If

            MoveToolTipBox()
            Return True
        Else
            TooltipHide()
        End If
        Return False
    End Function

    Private PopupToolTip As PopupToolTip
    Private Sub TooltipShow()
        PopupToolTip.Visibility = Visibility.Visible
    End Sub
    Private Sub TooltipHide()
        If PopupToolTip.Visibility = Visibility.Visible Then
            PopupToolTip.Visibility = Visibility.Hidden
        End If


    End Sub

    Private Sub MoveToolTipBox()
        If PopupToolTip.Visibility = Visibility.Visible Then
            If completionWindow IsNot Nothing Then
                Dim StartPostion As TextViewPosition = TextEditor.TextArea.Caret.Position
                Dim p As Point = TextEditor.TextArea.TextView.GetVisualPosition(StartPostion, Rendering.VisualYPosition.LineTop)

                Dim CaretPos As Integer = p.Y - TextEditor.VerticalOffset
                Dim copPos As Integer = completionWindow.Top - TextEditor.PointToScreen(New Point(0, 0)).Y

                If CaretPos > copPos Then
                    PopupToolTip.Left = -TextEditor.PointFromScreen(New Point(0, 0)).X + OrginXPos
                    PopupToolTip.Top = -TextEditor.PointFromScreen(New Point(0, 0)).Y + OrginYPos + 23
                    'ToltipBorder.Margin = New Thickness(ToltipBorder.Margin.Left, OrginYPos + 23, 0, 0)
                Else
                    PopupToolTip.Left = -TextEditor.PointFromScreen(New Point(0, 0)).X + OrginXPos
                    PopupToolTip.Top = -TextEditor.PointFromScreen(New Point(0, 0)).Y + OrginYPos - (PopupToolTip.ActualHeight - 4)
                    'ToltipBorder.Margin = New Thickness(ToltipBorder.Margin.Left, OrginYPos - (ToltipBorder.ActualHeight - 4), 0, 0)
                End If


            Else
                PopupToolTip.Left = -TextEditor.PointFromScreen(New Point(0, 0)).X + OrginXPos
                PopupToolTip.Top = -TextEditor.PointFromScreen(New Point(0, 0)).Y + OrginYPos - (PopupToolTip.ActualHeight - 4)
            End If
        End If
    End Sub

    Private LocalFunc As CFunc

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
                    If Flag = SpecialFlag.Extern Then
                        If LastStr.Split(".").Last.Length = 0 Then
                            completionWindow.StartOffset -= 1
                        Else
                            completionWindow.StartOffset -= LastStr.Split(".").Last.Length
                        End If
                    Else
                        completionWindow.StartOffset -= LastStr.Length
                    End If
                End If

                Select Case Flag
                    Case SpecialFlag.None
                        LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet, LastStr)
                    Case SpecialFlag.IsFuncNameWrite
                        LoadFuncNameData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsFuncDefWrite
                        LoadFuncWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsImportWrite
                        LoadImportWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.Extern
                        LoaddotData(TextEditor, completionWindow.CompletionList.CompletionData, LastStr)
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
                    If Flag = SpecialFlag.Extern Then
                        completionWindow.CompletionList.SelectItem(LastStr.Split(".").Last)
                    Else
                        completionWindow.CompletionList.SelectItem(LastStr)
                    End If
                End If

                If completionWindow.CompletionList.SelectedItem Is Nothing Then
                    completionWindow.Close()
                    'completionWindow.Visibility = Visibility.Collapsed
                End If
            ElseIf enteredText = " " Or enteredText = vbTab Or enteredText = "," Or enteredText = "(" Or enteredText = "." Then 'IsFirstArgumnet And FuncNameas.Trim <> "" Then '
                completionWindow = New CompletionWindow(TextEditor.TextArea)
                completionWindow.CloseWhenCaretAtBeginning = controlSpace


                If LastStr.Length > 0 Then
                    'completionWindow.StartOffset -= LastStr.Length
                End If


                Select Case Flag
                    Case SpecialFlag.None
                        LoadData(TextEditor, completionWindow.CompletionList.CompletionData, FuncNameas, ArgumentCount, IsFirstArgumnet, LastStr)
                    Case SpecialFlag.IsFuncNameWrite
                        LoadFuncNameData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsFuncDefWrite
                        LoadFuncWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.IsImportWrite
                        LoadImportWriteData(TextEditor, completionWindow.CompletionList.CompletionData)
                    Case SpecialFlag.Extern
                        LoaddotData(TextEditor, completionWindow.CompletionList.CompletionData, LastStr)
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
                    If Flag = SpecialFlag.Extern Then
                        completionWindow.CompletionList.SelectItem(LastStr.Split(".").Last.Replace("""", "").Replace("'", ""))
                    Else
                        completionWindow.CompletionList.SelectItem(LastStr.Replace("""", "").Replace("'", ""))
                    End If

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

    Private IsOpencompletionWindow As Boolean
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
                TooltipHide()
            End If
        End If


        If e.Key = Key.Tab And e.IsUp And Not IsOpencompletionWindow Then
            If TextEditor.SelectedText = "" Then
                textEditor_KeyboardInput(vbTab)
            End If
        End If

        If e.IsDown Then
            IsOpencompletionWindow = completionWindow IsNot Nothing
        ElseIf e.IsUp Then
            IsOpencompletionWindow = completionWindow IsNot Nothing
        End If

        'If PopupToolTip.Visibility = Visibility.Visible Then
        '    If e.Key = Key.Right Or e.Key = Key.Left Then
        '        ShowFuncTooltip(FuncName, ArgumentIndex, FunctionStartOffset)
        '    End If
        'End If




        If e.Key = Key.Back Then
            pjData.SetDirty(True)
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
                Dim tailchar As String

                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = " "
                End Try

                If tailchar.Trim = "" Then
                    TextEditor.SelectedText = ")"
                    TextEditor.SelectionLength = 0
                End If
            Case "{"
                Dim tailchar As String

                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = " "
                End Try

                If tailchar.Trim = "" Then
                    TextEditor.SelectedText = "}"
                    TextEditor.SelectionLength = 0
                End If
            Case "["
                Dim tailchar As String

                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = " "
                End Try

                If tailchar.Trim = "" Then
                    TextEditor.SelectedText = "]"
                    TextEditor.SelectionLength = 0
                End If
            Case ")", "}", "]"
                Dim tailchar As String
                Dim ttailchar As String

                Try
                    tailchar = TextEditor.Text.Chars(TextEditor.SelectionStart)
                Catch ex As IndexOutOfRangeException
                    tailchar = ""
                End Try
                Try
                    ttailchar = TextEditor.Text.Chars(TextEditor.SelectionStart + 1)
                Catch ex As IndexOutOfRangeException
                    ttailchar = ""
                End Try

                If keys = tailchar And ttailchar.Trim = "" Then
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
