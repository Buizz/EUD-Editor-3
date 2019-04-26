Imports System.IO
Imports System.Xml
Imports System.Reflection
Imports System.Windows.Threading
Imports ICSharpCode.AvalonEdit.Folding
Imports ICSharpCode.AvalonEdit.Highlighting
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion

Partial Public Class CodeEditor
    Private Sub InitTextEditor()
        Dim customHighlighting As IHighlightingDefinition
        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3.EpsHighlightingDark.xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting("EpsHighlightingDark", {".eps"}, customHighlighting)
        TextEditor.SyntaxHighlighting = customHighlighting



        Dim foldingUpdateTimer As DispatcherTimer = New DispatcherTimer()
        foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2)
        AddHandler foldingUpdateTimer.Tick, AddressOf foldingUpdateTimer_Tick
        foldingUpdateTimer.Start()

        FoldingManager = FoldingManager.Install(TextEditor.TextArea)
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



    Private LastStr As String
    Private FuncName As String
    Private ArgumentCount As Integer
    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        LastStr = ""
        FuncName = ""
        ArgumentCount = 0

        Dim index As Integer = 0
        Dim bracketCount As Integer = 0
        Dim GetFuncName As Boolean = False
        While ((TextEditor.SelectionStart - index) > 0)
            Dim MidStr As String = Mid(TextEditor.Text, TextEditor.SelectionStart - index, 1)

            If GetFuncName Then
                FuncName = MidStr & FuncName
            End If
            Select Case MidStr
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
                    If GetFuncName Then
                        Exit While
                    End If
                Case ","
                    ArgumentCount += 1
            End Select


            LastStr = MidStr & LastStr

            index += 1
        End While




        Log.Text = LastStr & " " & TextEditor.SelectionStart & vbCrLf & "함수이름 : " & FuncName & "     함수 인자 번호 : " & ArgumentCount





        'ShowCompletion(e.Text, False)
    End Sub





    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If e.Text = vbTab Then
                completionWindow.CompletionList.RequestInsertion(e)
            ElseIf Not Char.IsLetterOrDigit(e.Text(0)) Then
                completionWindow.Close()
            End If
        End If
    End Sub

    Private Sub ShowCompletion(ByVal enteredText As String, ByVal controlSpace As Boolean)
        If Not controlSpace Then
            Debug.WriteLine("Code Completion: TextEntered: " & enteredText)
        Else
            Debug.WriteLine("Code Completion: Ctrl+Space")
        End If


        If completionWindow Is Nothing Then
            completionWindow = New CompletionWindow(TextEditor.TextArea)
            completionWindow.CloseWhenCaretAtBeginning = controlSpace
            'completionWindow.StartOffset -= 1


            Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData

            For i = 0 To 10
                data.Add(New DataEditCompletionData("EudEditor 3", New TextBlock(), TextEditor, DataEditCompletionData.EIconType.Funcname))
            Next

            completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
            completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")
            'If results.TriggerWordLength > 0 Then
            '    completionWindow.CompletionList.SelectItem(results.TriggerWord)
            'End If

            completionWindow.Show()
            AddHandler completionWindow.Closed, Sub()
                                                    completionWindow = Nothing
                                                End Sub
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
