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

        Select Case sb.ScriptType
            Case ScriptBlock.EBlockType.rawcode
                tAddText(lnlines, "코드 : " & vbCrLf, Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case ScriptBlock.EBlockType.import
                Dim tstr() As String = sb.value.Replace(" as ", "ᗋ").Split("ᗋ")
                Dim name As String = tstr.First.Trim
                Dim tag As String = tstr.Last.Trim


                tAddText(lnlines, "임포트 : ", Nothing)
                tAddText(lnlines, name, tescm.HighlightBrush)
                tAddText(lnlines, "을 ", Nothing)
                tAddText(lnlines, tag, tescm.HighlightBrush)
                tAddText(lnlines, "로 불러옵니다.", Nothing)
            Case ScriptBlock.EBlockType.vardefine
                Dim vname As String = sb.value

                If sb.flag Then
                    If sb.value2 = "object" Then
                        tAddText(lnlines, "오브젝트 ", Nothing)
                    ElseIf sb.value2 = "const" Then
                        tAddText(lnlines, "상수 변수 ", Nothing)
                    End If
                Else
                    tAddText(lnlines, "변수 ", Nothing)
                End If

                tAddText(lnlines, vname, tescm.HighlightBrush)

                If sb.child.Count = 0 Then
                    tAddText(lnlines, "를 선언합니다.", Nothing)
                Else
                    tAddText(lnlines, "를 ", Nothing)

                    tAddText(lnlines, sb.child(0).ValueCoder, tescm.HighlightBrush)

                    tAddText(lnlines, "의 초기값", tescm.HighlightBrush)
                    tAddText(lnlines, "으로 선언합니다.", Nothing)
                End If
            Case ScriptBlock.EBlockType.varuse
                tAddText(lnlines, "변수 ", Nothing)
                tAddText(lnlines, sb.name, tescm.HighlightBrush)
                tAddText(lnlines, "를 사용합니다.", Nothing)
            Case ScriptBlock.EBlockType.objectdefine
                tAddText(lnlines, "객체정의 : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case ScriptBlock.EBlockType.objectfields
                textblock.Text = "필드"
            Case ScriptBlock.EBlockType.objectmethod
                textblock.Text = "메서드"
            Case ScriptBlock.EBlockType.fundefine
                tAddText(lnlines, "함수정의 : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
            Case ScriptBlock.EBlockType.funargs
                textblock.Text = "인자"
            Case ScriptBlock.EBlockType.funcontent
                textblock.Text = "내용"
            Case ScriptBlock.EBlockType._if
                textblock.Text = "If"
            Case ScriptBlock.EBlockType._elseif
                textblock.Text = "else If"
            Case ScriptBlock.EBlockType.ifcondition
                textblock.Text = "Condition"
            Case ScriptBlock.EBlockType.ifthen
                textblock.Text = "Then"
            Case ScriptBlock.EBlockType.ifelse
                textblock.Text = "Else"
            Case ScriptBlock.EBlockType._for
                tAddText(lnlines, "For ", Nothing)
                tAddText(lnlines, sb.ForCoder, tescm.HighlightBrush)

                'Dim funargs As List(Of ScriptBlock) = sb.child.First.child
                'For i = 0 To funargs.Count - 1
                '    If i <> 0 Then
                '        tAddText(lnlines, " ; ", Nothing)
                '    End If
                '    tAddText(lnlines, funargs(i).value, tescm.HighlightBrush)
                'Next
            Case ScriptBlock.EBlockType.foraction
                textblock.Text = "액션"
            Case ScriptBlock.EBlockType._while
                textblock.Text = "While"
            Case ScriptBlock.EBlockType.whilecondition
                textblock.Text = "Condition"
            Case ScriptBlock.EBlockType.whileaction
                textblock.Text = "Action"
            Case ScriptBlock.EBlockType.switch
                tAddText(lnlines, "Switch : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
                'For i = 0 To sb.child.Count - 1
                '    If sb.child(i).name = "switchvar" Then
                '        tAddText(lnlines, sb.child(i).value, tescm.HighlightBrush)
                '    End If
                'Next
            Case ScriptBlock.EBlockType.switchcase
                tAddText(lnlines, "Case : ", Nothing)
                tAddText(lnlines, sb.value, tescm.HighlightBrush)
                tAddText(lnlines, "   Break : ", Nothing)
                tAddText(lnlines, sb.flag, tescm.HighlightBrush)
            Case ScriptBlock.EBlockType._or
                tAddText(lnlines, "논리연산 또는", Nothing)
            Case ScriptBlock.EBlockType._and
                tAddText(lnlines, "논리연산 그리고", Nothing)
            Case ScriptBlock.EBlockType.folder
                'flag true = iscondition


                Dim temp As String() = sb.value.Split("ᗋ")

                Dim head As String = temp.First
                Dim tail As String = temp.Last


                tAddText(lnlines, head, tescm.HighlightBrush)
                tAddText(lnlines, " / ", Nothing)
                tAddText(lnlines, tail, tescm.HighlightBrush)
            Case ScriptBlock.EBlockType.folderaction
                tAddText(lnlines, "액션", Nothing)
            Case ScriptBlock.EBlockType.plibfun, ScriptBlock.EBlockType.funuse, ScriptBlock.EBlockType.action, ScriptBlock.EBlockType.condition, ScriptBlock.EBlockType.externfun
                sb.FuncCoder(lnlines)
            Case ScriptBlock.EBlockType.exp
                tAddText(lnlines, "수식 : ", Nothing)
                sb.ExpCoder(lnlines)
            Case Else
                If sb.IsArgument Then
                    sb.ArgCoder(lnlines)
                Else
                    tAddText(lnlines, sb.ValueCoder(), tescm.HighlightBrush)
                End If
        End Select
    End Sub



End Class
