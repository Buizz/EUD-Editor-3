Imports System.ComponentModel
Imports System.Runtime.ConstrainedExecution

Public Class Parser
    Public MainCode As New CodeBlock(CodeType.CODE_MAIN)

    Public Enum CodeType
        CODE_MAIN
        CODE_PREFUNCTION
        CODE_FUNCTION
        CODE_PARAM_DCL
        CODE_OBJECT
        CODE_IMPORT
        CODE_DECLARATION '선언문
        CODE_STATEMENT 'if문, 수식
        CODE_COMPOUND_ST '{괄호}
        CODE_EXPRESSION 'expression
        CODE_PRIMARY 'expression
        CODE_OPERATOR


        CODE_IF
        CODE_FOR
        CODE_FOREACH
        CODE_WHILE
        CODE_SWITCH
        CODE_CASE
        CODE_BREAK

        CODE_RETURN

        CODE_VAR
        CODE_STATIC
        CODE_CONST

        PRI_STRING
        PRI_NUMBER
        PRI_USE


        PRI_USEBRACKET
        PRI_USEINDEX


        BOX_FUNCTION_ARG


        BOX_FOREACH_COND

        BOX_OBJECT_METHOD
        BOX_OBJECT_FIELD


        CODE_COMMENTLINE
        CODE_COMMENT
        CODE_MACRO
    End Enum

    '한줄씩 들어감


    '현재 코드를 가르키는 포인터를 만들어 둬야됨.
    'Decode가 작업할때 현재 포인터에다가 아이템을 넣음


    '오류 처리기. 만들어야 됨.

    Public Sub Decoder(CodeType As CodeType, code As CodeBlock)
        Select Case CodeType
            Case CodeType.CODE_MAIN
                Select Case CurrentToken.TType
                    Case Token.TokenType.TOKEN_FUNCTION
                        Decoder(CodeType.CODE_FUNCTION, code)
                    Case Token.TokenType.TOKEN_OBJECT
                        Decoder(CodeType.CODE_OBJECT, code)
                    Case Token.TokenType.TOKEN_IMPORT
                        Decoder(CodeType.CODE_IMPORT, code)
                    Case Token.TokenType.TOKEN_VAR, Token.TokenType.TOKEN_CONST, Token.TokenType.TOKEN_STATIC
                        Decoder(CodeType.CODE_DECLARATION, code)
                    Case Token.TokenType.TOKEN_COMMENT
                        Dim ncode As New CodeBlock(CodeType.CODE_COMMENT)
                        ncode.Value1 = CurrentToken().value
                        code.Items.Add(ncode)
                        index += 1
                    Case Token.TokenType.TOKEN_LINECOMMENT
                        Dim ncode As New CodeBlock(CodeType.CODE_COMMENTLINE)
                        ncode.Value1 = CurrentToken().value
                        code.Items.Add(ncode)
                        index += 1
                    Case Token.TokenType.TOKEN_MACRO
                        Dim ncode As New CodeBlock(CodeType.CODE_MACRO)
                        ncode.Value1 = CurrentToken().value
                        code.Items.Add(ncode)
                        index += 1
                    Case Else
                        Throw New Exception("Line : " & CurrentToken.Ln & " Col : " & CurrentToken.Col & ", Parse 오류 " & CurrentToken.TType.ToString & "는 해석 할 수 없는 단락입니다.")
                End Select
            Case CodeType.CODE_IMPORT
                Dim ncode As New CodeBlock(CodeType.CODE_IMPORT)
                CheckNextToken(Token.TokenType.TOKEN_IMPORT)
                Dim filename As String = CurrentToken().value
                CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER)

                CheckNextToken(Token.TokenType.TOKEN_AS)
                Dim nspace As String = CurrentToken().value
                CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER)
                CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)

                ncode.Value1 = filename
                ncode.Value2 = nspace


                code.Items.Add(ncode)
            Case CodeType.CODE_OBJECT
                Dim ncode As New CodeBlock(CodeType.CODE_OBJECT)
                CheckNextToken(Token.TokenType.TOKEN_OBJECT)

                Dim objname As String = CurrentToken().value
                CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER)
                ncode.Value1 = objname

                Decoder(CodeType.CODE_COMPOUND_ST, ncode)
                CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                code.Items.Add(ncode)
            Case CodeType.CODE_FUNCTION
                Dim ncode As New CodeBlock(CodeType.CODE_FUNCTION)
                Dim argcode As New CodeBlock(CodeType.BOX_FUNCTION_ARG)
                Dim ToolTipToken As Token = PreviousToken()

                ncode.Items.Add(argcode)

                CheckNextToken(Token.TokenType.TOKEN_FUNCTION)

                Dim fname As String = CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER).value


                'MsgBox("함수 명  : " & fname)

                CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                While (Not CheckBlockToken(Token.TokenType.TOKEN_RPAREN))
                    Decoder(CodeType.CODE_PARAM_DCL, argcode)
                    CheckBlockToken(Token.TokenType.TOKEN_COMMA)
                End While

                If CurrentToken.TType = Token.TokenType.TOKEN_LSQBRACKET Then
                    Decoder(CodeType.CODE_COMPOUND_ST, ncode)
                    If ToolTipToken IsNot Nothing Then
                        '툴팁 분석하기.
                        'MsgBox(ToolTipToken.value)



                        'PreviousTokenDelete()
                    End If
                Else
                    CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                    ncode.BType = CodeType.CODE_PREFUNCTION
                End If


                ncode.Value1 = fname

                code.Items.Add(ncode)
            Case CodeType.CODE_PARAM_DCL
                Dim ncode As New CodeBlock(CodeType.CODE_PARAM_DCL)
                Dim argname As String = CurrentToken().value
                Dim argtype As String = ""

                CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER)
                If CurrentToken.TType = Token.TokenType.TOKEN_COLON Then
                    '인자
                    CheckNextToken(Token.TokenType.TOKEN_COLON)


                    argtype = CurrentToken().value
                    CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER)
                ElseIf CurrentToken.TType = Token.TokenType.TOKEN_COMMENT Then
                    '코맨드인자
                    argtype = CurrentToken().value
                    CheckNextToken(Token.TokenType.TOKEN_COMMENT)
                End If
                ncode.Value1 = argname
                ncode.Value2 = argtype

                'MsgBox("인자  : " & argname & ":" & argtype)

                code.Items.Add(ncode)
            Case CodeType.CODE_COMPOUND_ST '{괄호}
                Dim ncode As New CodeBlock(CodeType.CODE_COMPOUND_ST)


                CheckNextToken(Token.TokenType.TOKEN_LSQBRACKET)
                While (Not CheckBlockToken(Token.TokenType.TOKEN_RSQBRACKET))
                    Select Case CurrentToken.TType
                        Case Token.TokenType.TOKEN_VAR, Token.TokenType.TOKEN_CONST, Token.TokenType.TOKEN_STATIC
                            Decoder(CodeType.CODE_DECLARATION, ncode)
                        Case Token.TokenType.TOKEN_FUNCTION
                            Decoder(CodeType.CODE_FUNCTION, ncode)
                        Case Else
                            Decoder(CodeType.CODE_STATEMENT, ncode)
                    End Select
                End While

                code.Items.Add(ncode)
            Case CodeType.CODE_DECLARATION '선언문
                Dim ncode As CodeBlock = Nothing

                Select Case CurrentToken.TType
                    Case Token.TokenType.TOKEN_VAR
                        ncode = New CodeBlock(CodeType.CODE_VAR)
                    Case Token.TokenType.TOKEN_CONST
                        ncode = New CodeBlock(CodeType.CODE_CONST)
                    Case Token.TokenType.TOKEN_STATIC
                        ncode = New CodeBlock(CodeType.CODE_STATIC)
                End Select
                CheckNextToken(CurrentToken.TType)

                '변수 이름
                Dim vname As String = CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER).value
                ncode.Value1 = vname
                If CurrentToken.TType = Token.TokenType.TOKEN_COMMA Then
                    While (CheckBlockToken(Token.TokenType.TOKEN_COMMA))
                        Dim t As String = CheckNextToken(Token.TokenType.TOKEN_IDENTIFIER).value
                        ncode.Value1 = ncode.Value1 & ", " & t
                    End While
                End If



                If CurrentToken.TType = Token.TokenType.TOKEN_SEMICOLON Then
                    '끝
                    CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                ElseIf CurrentToken.TType = Token.TokenType.TOKEN_ASSIGN Then
                    'EXP가 들어감
                    CheckNextToken(Token.TokenType.TOKEN_ASSIGN)

ReLoad:
                    If CurrentToken.TType = Token.TokenType.TOKEN_LBRACKET Then
                        ncode.Value2 = "Array"

                        CheckNextToken(Token.TokenType.TOKEN_LBRACKET)

                        While (Not CheckBlockToken(Token.TokenType.TOKEN_RBRACKET))
                            Decoder(CodeType.CODE_PRIMARY, ncode)
                            CheckBlockToken(Token.TokenType.TOKEN_COMMA)
                        End While
                    Else
                        Decoder(CodeType.CODE_EXPRESSION, ncode)
                    End If

                    If CurrentToken.TType = Token.TokenType.TOKEN_COMMA Then
                        CheckNextToken(Token.TokenType.TOKEN_COMMA)
                        GoTo ReLoad
                    End If




                    CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                    End If

                    code.Items.Add(ncode)
            Case CodeType.CODE_STATEMENT 'if문, 수식
                Dim ncode As New CodeBlock(CodeType.CODE_STATEMENT)

                Select Case CurrentToken.TType
                    Case Token.TokenType.TOKEN_LSQBRACKET
                        'CODE_COMPOUND_ST
                        Decoder(CodeType.CODE_COMPOUND_ST, ncode)
                    Case Token.TokenType.TOKEN_IF
                        'if ( EXP ) STATEMENT else STATEMENT
                        Dim emdcode As New CodeBlock(CodeType.CODE_IF)
                        CheckNextToken(Token.TokenType.TOKEN_IF)
                        CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                        Decoder(CodeType.CODE_EXPRESSION, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                        Decoder(CodeType.CODE_STATEMENT, emdcode)
                        If CurrentToken.TType = Token.TokenType.TOKEN_ELSE Then
                            CheckNextToken(Token.TokenType.TOKEN_ELSE)
                            Decoder(CodeType.CODE_STATEMENT, emdcode)
                        End If
                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_WHILE
                        'while ( EXP ) STATEMENT
                        Dim emdcode As New CodeBlock(CodeType.CODE_WHILE)
                        CheckNextToken(Token.TokenType.TOKEN_WHILE)
                        CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                        Decoder(CodeType.CODE_EXPRESSION, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                        Decoder(CodeType.CODE_STATEMENT, emdcode)
                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_FOREACH
                        'foreach(ptr, epd : LoopNewUnit(0)){}
                        Dim emdcode As New CodeBlock(CodeType.CODE_FOREACH)
                        Dim condition As New CodeBlock(CodeType.BOX_FOREACH_COND)
                        emdcode.Items.Add(condition)

                        CheckNextToken(Token.TokenType.TOKEN_FOREACH)
                        CheckNextToken(Token.TokenType.TOKEN_LPAREN)

                        While (Not CheckBlockToken(Token.TokenType.TOKEN_COLON))
                            Decoder(CodeType.CODE_PRIMARY, condition)
                            CheckBlockToken(Token.TokenType.TOKEN_COMMA)
                        End While

                        Decoder(CodeType.CODE_PRIMARY, condition)
                        CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                        Decoder(CodeType.CODE_STATEMENT, emdcode)

                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_FOR
                        'for ( EXP ; EXP ; EXP ) STATEMENT
                        Dim emdcode As New CodeBlock(CodeType.CODE_FOR)
                        CheckNextToken(Token.TokenType.TOKEN_FOR)
                        CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                        Decoder(CodeType.CODE_DECLARATION, emdcode)
                        Decoder(CodeType.CODE_EXPRESSION, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                        Decoder(CodeType.CODE_EXPRESSION, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                        Decoder(CodeType.CODE_STATEMENT, emdcode)
                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_SWITCH
                        'switch ( CODE_PRIMARY ) COMPOUND_ST
                        Dim emdcode As New CodeBlock(CodeType.CODE_SWITCH)
                        CheckNextToken(Token.TokenType.TOKEN_SWITCH)
                        CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                        Decoder(CodeType.CODE_PRIMARY, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                        Decoder(CodeType.CODE_COMPOUND_ST, emdcode)
                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_CASE
                        'case PRI:
                        Dim emdcode As New CodeBlock(CodeType.CODE_CASE)
                        CheckNextToken(Token.TokenType.TOKEN_CASE)
                        Decoder(CodeType.CODE_PRIMARY, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_COLON)

                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_BREAK
                        'bresk ;
                        Dim emdcode As New CodeBlock(CodeType.CODE_BREAK)
                        ncode.Items.Add(emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_BREAK)
                        CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                    Case Token.TokenType.TOKEN_RETURN
                        Dim emdcode As New CodeBlock(CodeType.CODE_RETURN)
                        CheckNextToken(Token.TokenType.TOKEN_RETURN)
                        Decoder(CodeType.CODE_EXPRESSION, emdcode)
                        CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)
                        ncode.Items.Add(emdcode)
                    Case Token.TokenType.TOKEN_COMMENT
                        Dim emdcode As New CodeBlock(CodeType.CODE_COMMENT)
                        emdcode.Value1 = CurrentToken().value
                        ncode.Items.Add(emdcode)
                        index += 1
                    Case Token.TokenType.TOKEN_LINECOMMENT
                        Dim emdcode As New CodeBlock(CodeType.CODE_COMMENTLINE)
                        emdcode.Value1 = CurrentToken().value
                        ncode.Items.Add(emdcode)
                        index += 1
                    Case Token.TokenType.TOKEN_MACRO
                        Dim emdcode As New CodeBlock(CodeType.CODE_MACRO)
                        emdcode.Value1 = CurrentToken().value
                        ncode.Items.Add(emdcode)
                        index += 1
                        If (CurrentToken().TType = Token.TokenType.TOKEN_SEMICOLON) Then
                            index += 1
                            emdcode.Value2 = "SEMI"
                        End If

                    Case Token.TokenType.TOKEN_IDENTIFIER, Token.TokenType.TOKEN_NUMBER, Token.TokenType.TOKEN_STRING
                        Decoder(CodeType.CODE_EXPRESSION, ncode)
                        CheckNextToken(Token.TokenType.TOKEN_SEMICOLON)

                        'CODE_EXPRESSION   ;
                End Select



                code.Items.Add(ncode)
            Case CodeType.CODE_EXPRESSION 'expression
                'CODE_PRIMARY + 연산자의 묶음
                Dim ncode As New CodeBlock(CodeType.CODE_EXPRESSION)

                While (True)
                    If CurrentToken.IsOperator Then
                        Decoder(CodeType.CODE_OPERATOR, ncode)
                    Else
                        Select Case CurrentToken.TType
                            Case Token.TokenType.TOKEN_IDENTIFIER, Token.TokenType.TOKEN_NUMBER, Token.TokenType.TOKEN_STRING
                                Decoder(CodeType.CODE_PRIMARY, ncode)
                            Case Token.TokenType.TOKEN_LPAREN
                                CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                                Decoder(CodeType.CODE_EXPRESSION, ncode)
                                CheckNextToken(Token.TokenType.TOKEN_RPAREN)
                            Case Token.TokenType.TOKEN_MACRO
                                Dim emdcode As New CodeBlock(CodeType.CODE_MACRO)
                                emdcode.Value1 = CurrentToken().value
                                ncode.Items.Add(emdcode)
                                index += 1
                            Case Else
                                Exit While
                        End Select
                    End If
                End While

                code.Items.Add(ncode)
            Case CodeType.CODE_PRIMARY
                Dim ncode As New CodeBlock(CodeType.CODE_PRIMARY)
                'Number
                '식별자
                '뒤에 (),[]이 올 수 있음.
                Select Case CurrentToken.TType
                    Case Token.TokenType.TOKEN_IDENTIFIER
                        Dim value As String = CurrentToken.value
PRIUse:
                        ncode.BType = CodeType.PRI_USE
                        '변수 호출일수도 있고 함수 사용일 수도 있음
                        NextToken()

                        If CurrentToken.TType = Token.TokenType.TOKEN_LPAREN Then
                            ncode.BType = CodeType.PRI_USEBRACKET
                            CheckNextToken(Token.TokenType.TOKEN_LPAREN)
                            While (Not CheckBlockToken(Token.TokenType.TOKEN_RPAREN))
                                Decoder(CodeType.CODE_EXPRESSION, ncode)
                                CheckBlockToken(Token.TokenType.TOKEN_COMMA)
                            End While


                        ElseIf CurrentToken.TType = Token.TokenType.TOKEN_LBRACKET Then
                            ncode.BType = CodeType.PRI_USEINDEX
                            CheckNextToken(Token.TokenType.TOKEN_LBRACKET)
                            Decoder(CodeType.CODE_EXPRESSION, ncode)
                            CheckBlockToken(Token.TokenType.TOKEN_RBRACKET)
                        ElseIf CurrentToken.TType = Token.TokenType.TOKEN_PERIOD Then
                            value = value & "."
                            NextToken()
                            GoTo PRIUse
                        End If
                        ncode.Value1 = value


                    Case Token.TokenType.TOKEN_NUMBER
                        ncode.BType = CodeType.PRI_NUMBER
                        ncode.Value1 = CurrentToken.value
                        NextToken()
                    Case Token.TokenType.TOKEN_STRING
                        ncode.BType = CodeType.PRI_STRING
                        ncode.Value1 = CurrentToken.value
                        NextToken()
                    Case Else
                End Select

                code.Items.Add(ncode)
            Case CodeType.CODE_OPERATOR
                Dim ncode As New CodeBlock(CodeType.CODE_OPERATOR)

                ncode.Value1 = CurrentToken.ToText
                NextToken()
                code.Items.Add(ncode)
        End Select

    End Sub




    Public Sub New(tlist As List(Of Token))
        RawList = tlist


        'While (index < tlist.Count)
        '    'Decoder(CodeType.CODE_MAIN, MainCode)
        '    MsgBox("타입 : " & tlist(index).TType.ToString & "  값 : " & tlist(index).value)
        '    index += 1
        'End While

        'Dim index As Long = 0

        While (index < tlist.Count)
            Decoder(CodeType.CODE_MAIN, MainCode)
            'MsgBox("타입 : " & tlist(index).TType.ToString & "  값 : " & tlist(index).value)
        End While
    End Sub


    '모든 함수등의 정보를 가지고 있어야 됨











    Private Function CheckFuncComment(funcCode As ScriptBlock, Comment As ScriptBlock) As Boolean
        Dim commentstr As String = Comment.value

        If commentstr.IndexOf("/***") = 0 Then
            If commentstr.IndexOf("***/") = (commentstr.Length - 4) Then
                Return True
            End If
        End If

        Return False
    End Function
    Private Sub GetFuncComment(funcCode As ScriptBlock, Comment As ScriptBlock)
        '두 스크립트를 섞는다
        Dim commentstr As String = Comment.value
        Dim argstr As String = ""
        Dim fname As String = funcCode.value

        commentstr = Mid(commentstr, 5, commentstr.Length - 8).Trim
        '두 스크립트를 비교하여 인코딩 가능하면 True를 반환
        Dim arglist As List(Of ScriptBlock) = tescm.GetFuncArgs(funcCode)
        For i = 0 To arglist.Count - 1
            If i <> 0 Then
                argstr = argstr & ","
            End If

            If tescm.SCValueNoneType.ToList.IndexOf(arglist(i).name) <> -1 Then
                argstr = argstr & arglist(i).value
            Else
                argstr = argstr & arglist(i).value & ":" & arglist(i).name
            End If

        Next


        Dim ftooltip As New FunctionToolTip(commentstr, argstr, fname, False)

        funcCode.SetFuncTooltip(ftooltip.Summary)

        For i = 0 To arglist.Count - 1
            arglist(i).value2 = ftooltip.GetTooltip(i)
        Next
    End Sub
    Public Function GetGUIScripter() As List(Of ScriptBlock)

        Dim scrlist As New List(Of ScriptBlock)
        For i = 0 To MainCode.Items.Count - 1

            Dim scr As ScriptBlock = GetScriptBlock(MainCode.Items(i))

            If i < MainCode.Items.Count - 1 Then
                If scr.ScriptType = ScriptBlock.EBlockType.rawcode Then
                    Dim funcstr As ScriptBlock = GetScriptBlock(MainCode.Items(i + 1))

                    If funcstr.ScriptType = ScriptBlock.EBlockType.fundefine Then
                        If CheckFuncComment(funcstr, scr) Then
                            Continue For
                        End If
                    End If
                End If
            End If
            If i <> 0 Then
                If scr.ScriptType = ScriptBlock.EBlockType.fundefine Then
                    Dim comstr As ScriptBlock = GetScriptBlock(MainCode.Items(i - 1))

                    If comstr.ScriptType = ScriptBlock.EBlockType.rawcode Then
                        If CheckFuncComment(scr, comstr) Then
                            GetFuncComment(scr, comstr)
                        End If
                    End If
                End If
            End If

            If scr IsNot Nothing Then
                scrlist.Add(scr)
            End If
        Next

        Return scrlist
    End Function
    Public Function GetScriptBlock(cblock As CodeBlock) As ScriptBlock
        Dim scritem As ScriptBlock = Nothing

        Select Case cblock.BType
            Case CodeType.CODE_IMPORT
                scritem = New ScriptBlock(ScriptBlock.EBlockType.import, "import", True, False, cblock.Value1 & " as " & cblock.Value2, Nothing)
            Case CodeType.CODE_PREFUNCTION
            Case CodeType.CODE_FUNCTION
                scritem = New ScriptBlock(ScriptBlock.EBlockType.fundefine, "function", True, False, cblock.Value1, Nothing)

                Dim argbox As CodeBlock = cblock.Items.First
                Dim contentbox As CodeBlock = cblock.Items.Last

                Dim arglist As List(Of ScriptBlock) = tescm.GetFuncArgs(scritem)
                Dim contentlist As List(Of ScriptBlock) = tescm.GetFuncContent(scritem)

                For i = 0 To argbox.Items.Count - 1
                    If argbox.Items(i).Value2 = "" Then
                        arglist.Add(New ScriptBlock(ScriptBlock.EBlockType.funargblock, "Number", True, False, argbox.Items(i).Value1, Nothing))
                    Else
                        arglist.Add(New ScriptBlock(ScriptBlock.EBlockType.funargblock, argbox.Items(i).Value2, True, False, argbox.Items(i).Value1, Nothing))
                    End If
                Next

                Dim cbox As ScriptBlock = GetScriptBlock(contentbox)
                For i = 0 To cbox.child.Count - 1
                    contentlist.Add(cbox.child(i))
                Next
            Case CodeType.CODE_OBJECT
                scritem = New ScriptBlock(ScriptBlock.EBlockType.objectdefine, "objectdefine", True, False, cblock.Value1, Nothing)

                Dim compound As CodeBlock = cblock.Items(0)
                For i = 0 To compound.Items.Count - 1
                    Select Case compound.Items(i).BType
                        Case CodeType.CODE_COMMENT

                        Case CodeType.CODE_FUNCTION
                            scritem.child(1).child.Add(GetScriptBlock(compound.Items(i)))
                        Case CodeType.CODE_VAR, CodeType.CODE_CONST, CodeType.CODE_STATIC
                            scritem.child(0).child.Add(GetScriptBlock(compound.Items(i)))
                    End Select

                Next

            Case CodeType.CODE_VAR, CodeType.CODE_CONST, CodeType.CODE_STATIC
                'MsgBox("변수선언 호출")
                scritem = New ScriptBlock(ScriptBlock.EBlockType.vardefine, "vardefine", True, False, cblock.Value1, Nothing)

                Dim vname As String = cblock.Value1
                Select Case cblock.BType
                    Case CodeType.CODE_VAR
                        scritem.value2 = "var"
                    Case CodeType.CODE_CONST
                        scritem.value2 = "const"
                        scritem.flag = True
                    Case CodeType.CODE_STATIC
                        scritem.value2 = "static"
                        scritem.flag = True
                End Select

                If cblock.Value2 = "Array" Then
                    scritem.value = vname
                    scritem.flag = True
                    scritem.value2 = "object"

                    Dim initojb As New ScriptBlock(ScriptBlock.EBlockType.varuse, "EUDArray", True, False, "constructor", Nothing)
                    initojb.flag = True
                    For i = 0 To cblock.Items.Count - 1
                        initojb.AddChild(GetScriptBlock(cblock.Items(i)))
                    Next


                    scritem.AddChild(initojb)
                Else
                    If cblock.Items.Count > 0 Then

                        For p = 0 To cblock.Items.Count - 1

                            Dim initexp As CodeBlock = cblock.Items(p)
                            If initexp.Items.Count = 1 Then
                                Dim tcode As CodeBlock = initexp.Items(0)

                                Select Case tcode.BType
                                    Case CodeType.PRI_USEBRACKET
                                        scritem.flag = True
                                        scritem.value = vname
                                        scritem.value2 = "object"


                                        Dim initojb As New ScriptBlock(ScriptBlock.EBlockType.varuse, "", True, False, "", Nothing)
                                        scritem.AddChild(initojb)
                                        '=====================================================================================
                                        Dim objname As String = tcode.Value1


                                        Dim tstr() As String = objname.Split(".")
                                        If tstr.Count = 1 Then
                                            '.가지고 하는거임
                                            'ex
                                            'name = StringBuffer
                                            'value = constructor
                                            'child = 100(일반 값)
                                            '-> StringBuffer(100)
                                            initojb.name = objname
                                            initojb.value = "constructor"

                                            Select Case initojb.name
                                                Case "EUDArray"
                                                    initojb.flag = False
                                                    Dim ListF As CodeBlock = tcode.Items.First
                                                    ListF = ListF.Items.First
                                                    If IsNumeric(ListF.Value1) Then
                                                        initojb.child.Add(GetScriptBlock(ListF))
                                                    Else
                                                        For i = 0 To ListF.Items.Count - 1
                                                            initojb.child.Add(GetScriptBlock(ListF.Items(i)))
                                                        Next
                                                    End If

                                                Case "VArray"
                                                    initojb.flag = True
                                                    initojb.name = "EUDVArray"
                                                    For i = 0 To tcode.Items.Count - 1
                                                        initojb.child.Add(GetScriptBlock(tcode.Items(i)))
                                                    Next
                                                Case "PVariable"
                                                    initojb.flag = False

                                                    If (tcode.Items.Count <> 0) Then
                                                        Dim ListF As CodeBlock = tcode.Items.First
                                                        ListF = ListF.Items.First
                                                        If IsNumeric(ListF.Value1) Then
                                                            Throw New Exception("P변수 양식이 잘못되었습니다.")
                                                        Else
                                                            For i = 0 To ListF.Items.Count - 1
                                                                initojb.child.Add(GetScriptBlock(ListF.Items(i)))
                                                            Next
                                                        End If
                                                    End If
                                                Case Else
                                                    For i = 0 To tcode.Items.Count - 1
                                                        initojb.child.Add(GetScriptBlock(tcode.Items(i)))
                                                    Next
                                            End Select



                                        Else
                                            If GetNameSpace.IndexOf(tstr.First) = -1 Then
                                                '-> StringBuffer.cast(a[i])
                                                '-> StringBuffer.alloc()
                                                Dim objn As String = ""
                                                Dim methodn As String = ""

                                                For i = 0 To tstr.Count - 1
                                                    If i = tstr.Count - 1 Then
                                                        methodn = tstr(i)
                                                    Else
                                                        If i <> 0 Then
                                                            objn = objn & "."
                                                        End If
                                                        objn = objn & tstr(i)
                                                    End If
                                                Next

                                                Select Case tstr.Last
                                                    Case "cast"
                                                        initojb.name = objn
                                                        initojb.value = "cast"
                                                        For i = 0 To tcode.Items.Count - 1
                                                            initojb.child.Add(GetScriptBlock(tcode.Items(i)))
                                                        Next
                                                    Case "alloc"
                                                        initojb.name = objn
                                                        initojb.value = "alloc"
                                                End Select
                                            Else
                                                '외부 오브젝트
                                                Dim objn As String = ""
                                                Dim methodn As String = ""

                                                For i = 0 To tstr.Count - 1
                                                    If i = tstr.Count - 1 Then
                                                        methodn = tstr(i)
                                                    Else
                                                        If i <> 0 Then
                                                            objn = objn & "."
                                                        End If
                                                        objn = objn & tstr(i)
                                                    End If
                                                Next


                                                If tstr.Count = 2 Then
                                                    initojb.name = objn & "." & methodn
                                                    initojb.value = "constructor"
                                                    For i = 0 To tcode.Items.Count - 1
                                                        initojb.child.Add(GetScriptBlock(tcode.Items(i)))
                                                    Next
                                                Else
                                                    Select Case tstr.Last
                                                        Case "cast"
                                                            initojb.name = objn
                                                            initojb.value = "cast"
                                                            For i = 0 To tcode.Items.Count - 1
                                                                initojb.child.Add(GetScriptBlock(tcode.Items(i)))
                                                            Next
                                                        Case "alloc"
                                                            initojb.name = objn
                                                            initojb.value = "alloc"
                                                    End Select
                                                End If
                                            End If
                                        End If

                                        '=====================================================================================



                                    Case Else
                                        scritem.AddChild(GetScriptBlock(tcode))
                                End Select




                            ElseIf initexp.Items.Count = 2 Then
                                Dim tcode1 As CodeBlock = initexp.Items(0)
                                Dim tcode2 As CodeBlock = initexp.Items(1)

                                If tcode1.Value1 = "EUDVArray" Then
                                    scritem.flag = True
                                    scritem.value = vname
                                    scritem.value2 = "object"

                                    Dim initojb As New ScriptBlock(ScriptBlock.EBlockType.varuse, "EUDVArray", True, False, "constructor", Nothing)

                                    initojb.flag = False


                                    Dim cont As CodeBlock = cblock.Items.First
                                    'Dim vcount As Integer = cont.Items.First.Items.First.Items.First.Value1

                                    Dim countArray As CodeBlock = cont.Items.First
                                    Dim vcount As ScriptBlock = GetScriptBlock(countArray.Items.First)

                                    Dim listArray As CodeBlock = cont.Items.Last
                                    If listArray.Items.Count = 0 Then
                                        initojb.AddChild(vcount)
                                    Else
                                        listArray = listArray.Items.First


                                        For i = 0 To listArray.Items.Count - 1
                                            initojb.AddChild(GetScriptBlock(listArray.Items(i)))
                                        Next
                                    End If





                                    scritem.AddChild(initojb)
                                Else
                                    For i = 0 To initexp.Items.Count - 1
                                        scritem.AddChild(GetScriptBlock(initexp.Items(i)))
                                    Next
                                End If
                            Else

                                For i = 0 To initexp.Items.Count - 1
                                    scritem.AddChild(GetScriptBlock(initexp.Items(i)))
                                Next
                            End If
                        Next

                    End If
                End If


            Case CodeType.CODE_COMPOUND_ST
                'MsgBox("CODE_COMPOUND_ST 호출")
                'None라는건 빈 컨테이너를 뜻함.
                scritem = New ScriptBlock(ScriptBlock.EBlockType.none, "none", True, False, "", Nothing)

                For i = 0 To cblock.Items.Count - 1
                    scritem.child.Add(GetScriptBlock(cblock.Items(i)))
                Next


            Case CodeType.CODE_STATEMENT
                'MsgBox("CODE_STATEMENT 호출")
                Dim tcode As CodeBlock = cblock.Items.First

                Select Case tcode.BType
                    Case CodeType.CODE_COMPOUND_ST
                        'CODE_COMPOUND_ST
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.folder, "folder", True, False, "{ᗋ}", Nothing)
                        Dim cbox As ScriptBlock = GetScriptBlock(tcode)

                        For k = 0 To cbox.child.Count - 1
                            scritem.child.First.child.Add(cbox.child(k))
                        Next
                        'Decoder(CodeType.CODE_COMPOUND_ST, ncode)
                    Case CodeType.CODE_IF
                        'if ( EXP ) STATEMENT else STATEMENT
                        scritem = New ScriptBlock(ScriptBlock.EBlockType._if, "if", True, False, "", Nothing)


                        Dim conditionscr As ScriptBlock = scritem.child(0) 'condition
                        Dim conexp As ScriptBlock = GetScriptBlock(tcode.Items(0)) ' EXP



                        conditionscr.child.Add(conexp)


                        Dim thenscr As ScriptBlock = scritem.child(1) 'then
                        Dim cscr As ScriptBlock = GetScriptBlock(tcode.Items(1))
                        If cscr.ScriptType = ScriptBlock.EBlockType.folder Then
                            '가져온게 폴더일 경우 COMPOUND_ST
                            Dim faction As ScriptBlock = cscr.child.First

                            For i = 0 To faction.child.Count - 1
                                thenscr.child.Add(faction.child(i))
                            Next
                        Else
                            '한 문장일 경우
                            thenscr.child.Add(cscr)
                        End If

                        If tcode.Items.Count = 3 Then
                            scritem.child.Add(New ScriptBlock(ScriptBlock.EBlockType.ifelse, "ifelse", True, False, "", Nothing))

                            Dim elsescr As ScriptBlock = scritem.child(2) 'then
                            Dim tcscr As ScriptBlock = GetScriptBlock(tcode.Items(2))
                            If tcscr.ScriptType = ScriptBlock.EBlockType.folder Then
                                '가져온게 폴더일 경우 COMPOUND_ST
                                Dim faction As ScriptBlock = tcscr.child.First

                                For i = 0 To faction.child.Count - 1
                                    elsescr.child.Add(faction.child(i))
                                Next
                            Else
                                '한 문장일 경우
                                elsescr.child.Add(tcscr)
                            End If
                        End If

                    Case CodeType.CODE_WHILE
                        'while ( EXP ) STATEMENT
                        scritem = New ScriptBlock(ScriptBlock.EBlockType._while, "while", True, False, "", Nothing)


                        Dim conditionscr As ScriptBlock = scritem.child(0) 'condition
                        Dim conexp As ScriptBlock = GetScriptBlock(tcode.Items(0)) ' EXP

                        conditionscr.child.Add(conexp)

                        Dim thenscr As ScriptBlock = scritem.child(1) 'then
                        Dim cscr As ScriptBlock = GetScriptBlock(tcode.Items(1))
                        If cscr.ScriptType = ScriptBlock.EBlockType.folder Then
                            '가져온게 폴더일 경우 COMPOUND_ST
                            Dim faction As ScriptBlock = cscr.child.First

                            For i = 0 To faction.child.Count - 1
                                thenscr.child.Add(faction.child(i))
                            Next
                        Else
                            '한 문장일 경우
                            thenscr.child.Add(cscr)
                        End If
                    Case CodeType.CODE_FOREACH
                        'foreach(ptr, epd : LoopNewUnit(0)){}
                        scritem = New ScriptBlock(ScriptBlock.EBlockType._for, "foreach", True, False, "", Nothing)


                        Dim foreachbox As CodeBlock = tcode.Items(0)
                        Dim func As CodeBlock = foreachbox.Items.Last

                        Dim fortype As String = func.Value1
                        Dim forarg As New List(Of String)
                        If func.Items.Count <> 0 Then
                            For i = 0 To func.Items.Count - 1
                                Dim tscr As ScriptBlock = GetScriptBlock(func.Items(i))

                                forarg.Add(tscr.ValueCoder)
                            Next

                        End If

                        Dim varname As New List(Of String)
                        For i = 0 To foreachbox.Items.Count - 1
                            If i <> foreachbox.Items.Count - 1 Then
                                varname.Add(foreachbox.Items(i).Value1)
                            End If
                        Next
                        Dim rvalue As String = ""
                        Select Case fortype
                            Case "EUDLoopNewUnit", "EUDLoopUnit", "EUDLoopUnit2", "EUDLoopSprite"
                                Dim vname As String = varname(0) & ", " & varname(1)
                                '원래 아무것도 없음
                                rvalue = vname
                            Case "Timeline"
                                Dim vname As String = varname(0)
                                rvalue = vname & "ᗢ" & forarg(0)
                            Case "EUDLoopPlayerUnit"
                                Dim vname As String = varname(0) & ", " & varname(1)
                                rvalue = vname & "ᗢ" & forarg(0)
                            Case "EUDLoopPlayer"
                                Dim vname As String = varname(0)

                                Dim argtest As String = ""
                                For i = 0 To 2
                                    If i <> 0 Then
                                        argtest = argtest & "ᗢ"
                                    End If
                                    If forarg.Count > i Then
                                        argtest = argtest & forarg(i)
                                    Else
                                        argtest = argtest & "None"
                                    End If
                                Next
                                rvalue = vname & "ᗢ" & argtest & "ᗢFalse"
                                'rvalue = vname & "ᗢ" & CType(playerloopcb1.SelectedItem, ComboBoxItem).Tag & "ᗢ" &
                                '    CType(playerloopcb2.SelectedItem, ComboBoxItem).Tag & "ᗢ" &
                                '    CType(playerloopcb3.SelectedItem, ComboBoxItem).Tag & "ᗢ" & playerloopcheckbox.IsChecked
                        End Select





                        Dim cbox As ScriptBlock = GetScriptBlock(tcode.Items(1))
                        If cbox.ScriptType = ScriptBlock.EBlockType.folder Then
                            Dim actionscr As ScriptBlock = cbox.child(0)

                            For i = 0 To actionscr.child.Count - 1
                                scritem.child.First.child.Add(actionscr.child(i))
                            Next
                        Else
                            scritem.child.First.child.Add(cbox)
                        End If

                        scritem.value = fortype & "ᚢ" & rvalue
                    Case CodeType.CODE_FOR
                        'for ( EXP ; EXP ; EXP ) STATEMENT
                        scritem = New ScriptBlock(ScriptBlock.EBlockType._for, "for", True, False, "", Nothing)
                        'type;vvalue    scritem.value 
                        Dim forText As String = ""
                        Dim declarstr As String = ""
                        Dim condstr As String = ""
                        Dim operstr As String = ""



                        Dim declare As ScriptBlock = GetScriptBlock(tcode.Items(0)) '선언문
                        Dim cond As ScriptBlock = GetScriptBlock(tcode.Items(1)) '조건문
                        Dim oper As ScriptBlock = GetScriptBlock(tcode.Items(2)) '연산자
                        declarstr = declare.ValueCoder
                        condstr = cond.ValueCoder
                        operstr = oper.ValueCoder

                        Dim cbox As ScriptBlock = GetScriptBlock(tcode.Items(3))
                        If cbox.ScriptType = ScriptBlock.EBlockType.folder Then
                            Dim actionscr As ScriptBlock = cbox.child(0)

                            For i = 0 To actionscr.child.Count - 1
                                scritem.child.First.child.Add(actionscr.child(i))
                            Next
                        Else
                            scritem.child.First.child.Add(cbox)
                        End If



                        forText = declarstr & " ; " & condstr & " ; " & operstr

                        scritem.value = "UserEditᚢForᗢ" & forText
                    Case CodeType.CODE_SWITCH
                        'switch ( CODE_PRIMARY ) COMPOUNDS_ST
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.switch, "switch", True, False, "", Nothing)

                        scritem.VChild = GetScriptBlock(tcode.Items(0))
                        'scritem.value = tcode.Items(0).Value1

                        Dim cbox As ScriptBlock = GetScriptBlock(tcode.Items(1))
                        For i = 0 To cbox.child.Count - 1
                            scritem.child.Add(cbox.child(i))
                        Next
                    Case CodeType.CODE_CASE
                        'case PRI:
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.switchcase, "switchcase", True, False, tcode.Items.First.Value1, Nothing)
                    Case CodeType.CODE_BREAK
                        'bresk ;
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.break, "break", True, False, "", Nothing)
                    Case CodeType.CODE_RETURN
                        'return EXP ;
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.funreturn, "funreturn", True, False, "", Nothing)
                        Dim childexp As ScriptBlock = GetScriptBlock(tcode.Items(0)) ' EXP
                        scritem.child.First.DuplicationBlock(childexp)

                    Case CodeType.CODE_COMMENT
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "/*" & cblock.Value1 & "*/", Nothing)
                    Case CodeType.CODE_COMMENTLINE
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "//" & cblock.Items.First.Value1, Nothing)
                    Case CodeType.CODE_MACRO
                        If tcode.Value2 = "SEMI" Then
                            scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "<?" & tcode.Value1 & "?>;", Nothing)
                        Else
                            scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "<?" & tcode.Value1 & "?>", Nothing)
                        End If

                    Case CodeType.CODE_EXPRESSION
                        scritem = GetScriptBlock(tcode)
                End Select



            Case CodeType.PRI_USEBRACKET
                '함수, 외부함수, 오브젝트의 필드
                If cblock.Value1.IndexOf(".") = -1 Then
                    '내부함수나 함수입니다.
                    Dim fname As String = cblock.Value1


                    Dim findex As Integer = Tool.TEEpsDefaultFunc.SearchFunc(fname)
                    If findex <> -1 Then
                        Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(findex)
                        Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.action, fname, True, False, "", Nothing, False)
                        tscr.child.Clear()
                        tscr.name = fname

                        For i = 0 To cblock.Items.Count - 1
                            tscr.child.Add(GetScriptBlock(cblock.Items(i)))
                        Next
                        Select Case functooltip.Type
                            Case FunctionToolTip.FType.Act
                                'action
                                tscr.ScriptType = ScriptBlock.EBlockType.action
                            Case FunctionToolTip.FType.Cond
                                'condition
                                tscr.ScriptType = ScriptBlock.EBlockType.condition
                            Case FunctionToolTip.FType.Func
                                'plibfun
                                tscr.ScriptType = ScriptBlock.EBlockType.plibfun
                        End Select

                        scritem = tscr
                    Else
                        'funuse
                        Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.funuse, fname, True, False, "", Nothing, False)
                        tscr.child.Clear()
                        tscr.name = fname
                        For i = 0 To cblock.Items.Count - 1
                            tscr.child.Add(GetScriptBlock(cblock.Items(i)))
                        Next

                        scritem = tscr
                    End If



                    'macrofun
                Else
                    '외부함수, 매서드입니다.
                    Dim nspace As String = cblock.Value1.Split(".").First
                    Dim tastr() As String = cblock.Value1.Split(".")

                    If GetNameSpace.IndexOf(nspace) = -1 Then
                        'varuse
                        Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, cblock.Value1.Split(".").First, True, False, cblock.Value1.Split(".").Last, Nothing, False)
                        tscr.value2 = "method"
                        scritem = tscr
                        For i = 0 To cblock.Items.Count - 1
                            tscr.child.Add(GetScriptBlock(cblock.Items(i)))
                        Next
                    Else
                        If tastr.Count = 2 Then
                            'externfun
                            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.externfun, cblock.Value1, True, False, "", Nothing, False)
                            tscr.child.Clear()
                            tscr.name = cblock.Value1

                            For i = 0 To cblock.Items.Count - 1
                                tscr.child.Add(GetScriptBlock(cblock.Items(i)))
                            Next
                            scritem = tscr
                        Else
                            'varuse
                            Dim objname As String = ""
                            Dim filename As String = ""
                            For k = 0 To tastr.Count - 1
                                If k = tastr.Count - 1 Then
                                    filename = tastr(k)
                                Else
                                    If k <> 0 Then
                                        objname = objname & "."
                                    End If
                                    objname = objname & tastr(k)
                                End If
                            Next
                            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, objname, True, False, filename, Nothing, False)
                            tscr.value2 = "method"
                            scritem = tscr
                        End If
                    End If


                End If


            Case CodeType.PRI_USEINDEX
                '오브젝트
                Dim tastr() As String = cblock.Value1.Split(".")
                Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, cblock.Value1, True, False, "!index", Nothing)
                tscr.child.Add(GetScriptBlock(cblock.Items(0)))

                scritem = tscr
            Case CodeType.PRI_USE
                If cblock.Value1.IndexOf(".") = -1 Then
                    '내부 파일의 변수 일 경우
                    Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, cblock.Value1, True, False, "", Nothing)
                    tscr.value2 = "value"
                    scritem = tscr

                Else
                    Dim nspace As String = cblock.Value1.Split(".").First
                    If GetNameSpace.IndexOf(nspace) <> -1 Then
                        '외부파일의 변수(.앞에 글자가 네임스페이스에 있을 경우
                        Dim tastr() As String = cblock.Value1.Split(".")

                        If tastr.Count = 2 Then
                            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, cblock.Value1, True, False, "", Nothing)
                            tscr.value2 = "value"
                            scritem = tscr
                        Else
                            Dim objname As String = ""
                            Dim filename As String = ""
                            For k = 0 To tastr.Count - 1
                                If k = tastr.Count - 1 Then
                                    filename = tastr(k)
                                Else
                                    If k <> 0 Then
                                        objname = objname & "."
                                    End If
                                    objname = objname & tastr(k)
                                End If
                            Next
                            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, objname, True, False, filename, Nothing)
                            tscr.value2 = "fields"
                            scritem = tscr
                        End If
                    Else
                        '내부파일의 변수의 오브젝트의 필드일 경우(해당 변수가 오브젝트 타입으로 선언된 경우)
                        'fields
                        'method
                        Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.varuse, cblock.Value1.Split(".").First, True, False, cblock.Value1.Split(".").Last, Nothing)
                        tscr.value2 = "fields"
                        scritem = tscr
                    End If
                End If
            Case CodeType.PRI_NUMBER
                scritem = New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", True, False, cblock.Value1, Nothing)
            Case CodeType.PRI_STRING
                scritem = New ScriptBlock(ScriptBlock.EBlockType.constVal, "TrgString", True, False, cblock.Value1, Nothing)
            Case CodeType.CODE_OPERATOR
                scritem = New ScriptBlock(ScriptBlock.EBlockType.sign, "sign", True, False, cblock.Value1, Nothing)


            Case CodeType.CODE_EXPRESSION
                'MsgBox("CODE_EXPRESSION 호출")
                Dim scrlist As New List(Of ScriptBlock)

                For i = 0 To cblock.Items.Count - 1
                    Dim tcode As CodeBlock = cblock.Items(i)

                    Select Case tcode.BType
                        Case CodeType.PRI_USEBRACKET, CodeType.PRI_USEINDEX, CodeType.PRI_STRING, CodeType.PRI_NUMBER, CodeType.CODE_OPERATOR, CodeType.PRI_USE
                            scrlist.Add(GetScriptBlock(tcode))
                        Case CodeType.CODE_EXPRESSION
                            scrlist.Add(New ScriptBlock(ScriptBlock.EBlockType.sign, "sign", True, False, "(", Nothing))
                            Dim tscritem As ScriptBlock = GetScriptBlock(tcode)
                            For k = 0 To tscritem.child.Count - 1
                                scrlist.Add(tscritem.child(k))
                            Next
                            scrlist.Add(New ScriptBlock(ScriptBlock.EBlockType.sign, "sign", True, False, ")", Nothing))
                        Case CodeType.CODE_MACRO
                            scrlist.Add(New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "<?" & cblock.Items(i).Value1 & "?>", Nothing))

                    End Select
                Next


                If scrlist.Count = 1 Then
                    If scrlist(0).ScriptType <> ScriptBlock.EBlockType.varuse Then
                        scritem = scrlist(0)
                    Else
                        scritem = New ScriptBlock(ScriptBlock.EBlockType.exp, "expression", True, False, "", Nothing)

                        For i = 0 To scrlist.Count - 1
                            scritem.child.Add(scrlist(i))
                        Next
                    End If
                Else
                    scritem = New ScriptBlock(ScriptBlock.EBlockType.exp, "expression", True, False, "", Nothing)

                    For i = 0 To scrlist.Count - 1
                        scritem.child.Add(scrlist(i))
                    Next
                End If




                '마지막에 설정

            Case CodeType.CODE_COMMENT
                scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "/*" & cblock.Value1 & "*/", Nothing)
            Case CodeType.CODE_COMMENTLINE
                scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "//" & cblock.Items.First.Value1, Nothing)
            Case CodeType.CODE_MACRO
                scritem = New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "<?" & cblock.Value1 & "?>", Nothing)
        End Select


        Return scritem
    End Function



    Public Sub GetCUIScripter()

    End Sub



End Class
