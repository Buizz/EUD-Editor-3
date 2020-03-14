Public Class ScriptTreeviewitem
    Private sb As ScriptBlock
    Public Sub New(tsb As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        sb = tsb

        Refresh()
    End Sub

    Private Sub tAddText(Inline As InlineCollection, text As String, brush As Brush)
        Dim Run As New Run(text)
        If brush IsNot Nothing Then
            Run.Foreground = brush
        End If
        Inline.Add(Run)
    End Sub

    Public Sub Refresh()
        Dim lnlines As InlineCollection = textblock.Inlines
        lnlines.Clear()

        Select Case sb.name
            Case "rawcode"
                textblock.Text = sb.value
            Case "import"
                Dim tstr() As String = sb.value.Split(" as ")
                Dim name As String = tstr.First.Trim
                Dim tag As String = tstr.Last.Trim


                tAddText(lnlines, "임포트 : ", Nothing)
                tAddText(lnlines, name, tescm.HighlightBrush)
                tAddText(lnlines, "을 ", Nothing)
                tAddText(lnlines, tag, tescm.HighlightBrush)
                tAddText(lnlines, "로 불러옵니다.", Nothing)
            Case "var"
                Dim tstr() As String = sb.value.Split("=")

                Dim vname As String = tstr.First.Trim
                Dim initval As String = tstr.Last.Trim

                If sb.flag Then
                    tAddText(lnlines, "상수 변수 ", Nothing)
                Else
                    tAddText(lnlines, "변수 ", Nothing)
                End If

                tAddText(lnlines, vname, tescm.HighlightBrush)

                If tstr.Length = 1 Then
                    tAddText(lnlines, "를 선언합니다.", Nothing)
                Else
                    tAddText(lnlines, "를 ", Nothing)
                    tAddText(lnlines, initval, tescm.HighlightBrush)
                    tAddText(lnlines, "의 초기값", tescm.HighlightBrush)
                    tAddText(lnlines, "으로 선언합니다.", Nothing)
                End If
            Case "function"
                tAddText(lnlines, "함수정의 : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
                'tAddText(lnlines, " 인자 : ", Nothing)

                'Dim funargs As List(Of ScriptBlock) = sb.child.First.child
                'For i = 0 To funargs.Count - 1
                '    If i <> 0 Then
                '        tAddText(lnlines, ", ", Nothing)
                '    End If
                '    tAddText(lnlines, funargs(i).value, tescm.HighlightBrush)
                'Next
            Case "funargs"
                textblock.Text = "인자"
            Case "funcontent"
                textblock.Text = "내용"
            Case "if"
                textblock.Text = "If"
            Case "elseif"
                textblock.Text = "else If"
            Case "ifcondition"
                textblock.Text = "Condition"
            Case "ifthen"
                textblock.Text = "Then"
            Case "ifelse"
                textblock.Text = "Else"
            Case "for"
                tAddText(lnlines, "For ", Nothing)

                Dim funargs As List(Of ScriptBlock) = sb.child.First.child
                For i = 0 To funargs.Count - 1
                    If i <> 0 Then
                        tAddText(lnlines, " ; ", Nothing)
                    End If
                    tAddText(lnlines, funargs(i).value, tescm.HighlightBrush)
                Next
            Case "forinit"
                tAddText(lnlines, "초기식 ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case "forcondition"
                tAddText(lnlines, "조건식 ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case "forincre"
                tAddText(lnlines, "증감식 ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case "forcontent"
                textblock.Text = "조건문"
            Case "foraction"
                textblock.Text = "액션"
            Case "while"
                textblock.Text = "While"
            Case "whilecondition"
                textblock.Text = "Condition"
            Case "whileaction"
                textblock.Text = "Action"
            Case "switch"
                tAddText(lnlines, "Switch : ", Nothing)
                For i = 0 To sb.child.Count - 1
                    If sb.child(i).name = "switchvar" Then
                        tAddText(lnlines, sb.child(i).value, tescm.HighlightBrush)
                    End If
                Next
            Case "switchvar"
                tAddText(lnlines, "조건변수 : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case "switchcase"
                tAddText(lnlines, "Case : ", Nothing)
                tAddText(lnlines, sb.value, Nothing)
            Case "or"
                tAddText(lnlines, "또는", Nothing)
            Case "and"
                tAddText(lnlines, "그리고", Nothing)
            Case "folder"
                'flag true = iscondition


                Dim temp As String() = sb.value.Split("////")

                Dim head As String = temp.First
                Dim tail As String = temp.Last


                tAddText(lnlines, head, tescm.HighlightBrush)
                tAddText(lnlines, " / ", Nothing)
                tAddText(lnlines, tail, tescm.HighlightBrush)
            Case "folderaction"
                tAddText(lnlines, "액션", Nothing)
            Case Else
                If sb.value = "" Then

                    sb.FuncCoder(lnlines)

                Else
                    '값
                    If sb.IsArgument Then
                        sb.ArgCoder(lnlines)
                    Else
                        tAddText(lnlines, sb.ValueCoder(), tescm.HighlightBrush)
                    End If
                End If
        End Select
    End Sub



End Class
