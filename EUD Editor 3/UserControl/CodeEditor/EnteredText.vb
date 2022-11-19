Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Windows.Threading

Partial Public Class CodeEditor

    Private FuncName As String = ""
    Private ArgumentIndex As Integer
    Private FunctionStartOffset As Integer = 0
    Private Sub textEditor_KeyboardInput(etext As String)
        pjData.SetDirty(True)
        Dim MainStr As String = TextEditor.Document.Text
        Dim SelectStart As Integer = TextEditor.SelectionStart


        Dim LastStr As String = ""
        Dim TypingStr As String = ""
        Dim TypingStrSave As String = ""
        Dim CheckChar As Boolean = False
        FuncName = ""
        ArgumentIndex = 0
        FunctionStartOffset = 0
        Dim CheckFunctionName As String = ""
        Dim AnotherCharCount As Integer
        Dim IsFirstArgument As Boolean = False


        Dim index As Integer = 0
        Dim bracketCount As Integer = 0
        Dim GetFuncName As Boolean = False
        Dim CheckFunction As Boolean = False
        Dim IsFunctionSpace As Boolean = False
        Dim IsQuotes As Boolean = False
        Dim QuotesCount As Integer


        Dim remainText As String = "<?" & Mid(MainStr, SelectStart)

        Dim regex As New Regex("(<\?|<\?php)(.+?)\?>", RegexOptions.Multiline Or RegexOptions.Singleline Or RegexOptions.ExplicitCapture)
        Dim IsInline As Boolean = False

        Dim regexmatch As Match = regex.Match(remainText)
        If regexmatch.Success Then
            If regexmatch.Index = 0 Then
                Dim startText As String = Mid(MainStr, 1, SelectStart) & "?>"

                Dim matchex As MatchCollection = regex.Matches(startText)

                If matchex.Count > 0 Then
                    Dim lastMatch As Match = matchex(matchex.Count - 1)

                    If lastMatch.Success Then
                        Dim startIndex As Long = lastMatch.Index
                        Dim endIndex As Long = startIndex + lastMatch.Value.Length

                        If endIndex = startText.Length Then
                            IsInline = True
                        End If
                    End If
                End If
            End If
        End If

        While ((SelectStart - index) > 0)
            Dim MidStr As String = Mid(MainStr, SelectStart - index, 1)


            Select Case MidStr'이게 뜰때 에러가 아니라 앞에 function이 있는지 확인까지 해야됨. 
                Case vbLf
                    Exit While
                Case "("
                    If ArgumentIndex = 0 And AnotherCharCount = 0 Then
                        IsFirstArgument = True
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
                    If Not CheckFunction And Not IsFunctionSpace Then
                        If ArgumentIndex = 0 And AnotherCharCount = 0 Then
                            IsFirstArgument = True
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

            If IsNumeric(MidStr) Then
                If index = 0 Then
                    Return
                End If
            End If



            If GetFuncName And Not IsFunctionSpace Then
                If MidStr <> "(" Then
                    FuncName = MidStr & FuncName
                End If
            End If
            If CheckFunction Then
                CheckFunctionName = MidStr & CheckFunctionName
            End If
            If Not Char.IsLetterOrDigit(MidStr) And MidStr <> "/" And MidStr <> "_" And MidStr <> "." Then
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
        Dim IsImportWrite As Boolean = False
        Dim CompletionPath As Boolean = False
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


        Dim strStacks() As String = LastStr.Split(" ")
        If strStacks.Count >= 2 Then
            Dim temp As String = strStacks(strStacks.Count - 2)
            Select Case temp
                Case "function"
                    IsFuncNameWrite = True
                Case "import"
                    IsImportWrite = True
            End Select
        End If
        If strStacks.First = "import" Then
            If Not IsImportWrite Then
                CompletionPath = True
            End If
        End If
        'Log.Text = FunctionStartOffset & "   마지막으로 입력한 글자 : " & TypingStr & vbCrLf & "함수이름 : " & FuncName & "     함수 인자 번호 : " & ArgumentIndex

        'Log.Text = Log.Text & vbCrLf & "IsFuncNameWrite : " & IsFuncNameWrite
        'Log.Text = Log.Text & vbCrLf & "IsFuncDefWrite : " & IsFuncDefWrite


        'Log.Text = Log.Text & vbCrLf & "LastStr.IndexOf : " & TypingStr.IndexOf(".")


        AutoInserter(etext)


        Dim background As New BackgroundWorker

        AddHandler background.DoWork, New DoWorkEventHandler(Sub(sender As Object, e As DoWorkEventArgs)
                                                                 Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                                                                             If Not CompletionPath Then
                                                                                                                                 If TypingStr.IndexOf(".") >= 0 Then
                                                                                                                                     LocalFunc.Init()
                                                                                                                                     LocalFunc.LoadFunc(MainStr, SelectStart)
                                                                                                                                     '외부함수 불러오는건 여기가아님

                                                                                                                                     ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.Extern)
                                                                                                                                 Else
                                                                                                                                     If Not IsFuncDefWrite And Not IsFuncNameWrite And Not IsImportWrite Then
                                                                                                                                         If IsInline Then
                                                                                                                                             ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.lnlineCode)
                                                                                                                                             ShowFuncTooltip(FuncName, ArgumentIndex, FunctionStartOffset, True)
                                                                                                                                         Else
                                                                                                                                             LocalFunc.Init()
                                                                                                                                             LocalFunc.LoadFunc(MainStr, SelectStart)
                                                                                                                                             '외부함수 불러오는건 여기가아님


                                                                                                                                             ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True)

                                                                                                                                             ShowFuncTooltip(FuncName, ArgumentIndex, FunctionStartOffset)
                                                                                                                                         End If
                                                                                                                                     ElseIf IsImportWrite Then
                                                                                                                                         ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.IsImportWrite)
                                                                                                                                     ElseIf IsFuncDefWrite Then
                                                                                                                                         ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.IsFuncDefWrite)
                                                                                                                                     ElseIf IsFuncNameWrite Then
                                                                                                                                         ShowCompletion(etext, False, TypingStr, FuncName, ArgumentIndex, True, SpecialFlag.IsFuncNameWrite)
                                                                                                                                     End If
                                                                                                                                 End If
                                                                                                                             End If
                                                                                                                         End Sub))

                                                             End Sub)


        background.RunWorkerAsync()
        'Dim selectedValues As List(Of InvoiceSOA)
        'selectedValues = DisputeList.FindAll(Function(p) p.ColumnName = "Jewel")


        'Log.Text = Log.Text & vbCrLf & "입력된 함수 갯수 : " & LocalFunc.FuncCount

        'For i = 0 To LocalFunc.FuncCount - 1
        '    Log.Text = Log.Text & vbCrLf & LocalFunc.GetFuncName(i) & " : " & LocalFunc.GetFuncArgument(i)
        'Next
    End Sub





    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        If TextEditor.IsReadOnly Then
            Return
        End If
        textEditor_KeyboardInput(e.Text)
    End Sub
End Class
