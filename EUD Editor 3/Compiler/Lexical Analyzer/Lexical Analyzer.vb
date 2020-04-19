Imports System.IO
Imports System.Text.RegularExpressions

Public Class Lexical_Analyzer
    Public Function DoWork(str As String) As List(Of Token)
        'function a(asd){}
        Dim tokenlist As New List(Of Token)

        Dim tk As New Tokenizer(str & " ")


        While (Not tk.IsEnd)
            Dim c As String = tk.NextChar
            While (c.Trim = "" And Not tk.IsEnd)
                c = tk.NextChar
            End While
            If tk.IsEnd Then
                Exit While
            End If

            If IsNumeric(c) Then
                c = tk.NextChar
                If c = "x" Then
                    'hex값
                    Dim v As String = ("&H" & tk.NextHex())
                    tokenlist.Add(New Token(Token.TokenType.TOKEN_NUMBER, v))
                Else
                    tk.Back()
                    tk.Back()
                    Dim v As String = tk.NextDigt()
                    tokenlist.Add(New Token(Token.TokenType.TOKEN_NUMBER, v))
                End If
            ElseIf IsLetter(c) Then
                Dim letter As String = c & tk.NextBlock.Trim

                Select Case letter
                    Case "import"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IMPORT))
                    Case "as"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_AS))
                    Case "var"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_VAR))
                    Case "const"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CONST))
                    Case "static"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_STATIC))
                    Case "function"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FUNCTION))
                    Case "object"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_OBJECT))
                    Case "if"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IF))
                    Case "else"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_ELSE))
                    Case "for"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FOR))
                    Case "foreach"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FOREACH))
                    Case "while"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_WHILE))
                    Case "switch"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_SWITCH))
                    Case "case"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CASE))
                    Case "break"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BREAK))
                    Case "continue"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CONTINUE))
                    Case "return"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RETURN))
                    Case Else
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IDENTIFIER, letter))
                End Select
            Else
                Select Case c
                    Case "("
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LPAREN))
                    Case ")"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RPAREN))
                    Case "["
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LBRACKET))
                    Case "]"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RBRACKET))
                    Case "{"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LSQBRACKET))
                    Case "}"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RSQBRACKET))

                    Case "."
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_PERIOD))
                    Case "?"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_QMARK))
                    Case ","
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_COMMA))
                    Case ":"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_COLON))
                    Case ";"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_SEMICOLON))
                    Case "~"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITNOT))
                    Case "+"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_PLUS))
                        Else
                            c = tk.NextChar()
                            If c = "+" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_INC))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IADD))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_PLUS))
                                tk.Back()
                            End If
                        End If
                    Case "-"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MINUS))
                        Else
                            c = tk.NextChar()
                            If c = "-" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_DEC))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_ISUB))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MINUS))
                                tk.Back()
                            End If
                        End If
                    Case "*"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MULTIPLY))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IMUL))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MULTIPLY))
                                tk.Back()
                            End If
                        End If
                    Case "/"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_DIVIDE))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IDIV))
                            ElseIf c = "*" Then
                                Dim commentstr As String = ""

                                '주석시작
                                While (True)
                                    c = tk.NextChar()
                                    commentstr = commentstr & c
                                    If c = "*" Then
                                        c = tk.NextChar()
                                        If c = "/" Then
                                            tokenlist.Add(New Token(Token.TokenType.TOKEN_LINECOMMENT, commentstr))
                                            Exit While
                                        Else
                                            commentstr = commentstr & "*" & c
                                        End If
                                    End If
                                End While

                            ElseIf c = "/" Then
                                '주석시작
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LINECOMMENT, tk.NextLine()))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_DIVIDE))
                                tk.Back()
                            End If
                        End If
                    Case "%"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MOD))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IMOD))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MOD))
                                tk.Back()
                            End If
                        End If
                    Case "<"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_LT))
                        Else
                            c = tk.NextChar()
                            If c = "<" Then
                                If tk.IsEnd Then
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_BITLSHIFT))
                                Else
                                    c = tk.NextChar()
                                    If c = "=" Then
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_ILSHIFT))
                                    Else
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITLSHIFT))
                                        tk.Back()
                                    End If
                                End If
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LE))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LT))
                                tk.Back()
                            End If
                        End If
                    Case ">"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_GT))
                        Else
                            c = tk.NextChar()
                            If c = ">" Then
                                If tk.IsEnd Then
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_BITRSHIFT))
                                Else
                                    c = tk.NextChar()
                                    If c = "=" Then
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IRSHIFT))
                                    Else
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITRSHIFT))
                                        tk.Back()
                                    End If
                                End If
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_GE))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_GT))
                                tk.Back()
                            End If
                        End If
                    Case "&"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITAND))
                        Else
                            c = tk.NextChar()
                            If c = "&" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LAND))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITAND))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITAND))
                                tk.Back()
                            End If
                        End If
                    Case "|"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITOR))
                        Else
                            c = tk.NextChar()
                            If c = "|" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LOR))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITOR))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITOR))
                                tk.Back()
                            End If
                        End If
                    Case "="
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_ASSIGN))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_EQ))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_ASSIGN))
                                tk.Back()
                            End If
                        End If
                    Case "^"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITXOR))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITXOR))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITXOR))
                                tk.Back()
                            End If
                        End If
                    Case "!"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_LNOT))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_NE))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LNOT))
                                tk.Back()
                            End If
                        End If
                    Case """"
                        Dim textstr As String = ""
                        While (c <> vbCr And c <> vbLf)
                            c = tk.NextChar()

                            If c = """" Then
                                tk.Back()
                                tk.Back()
                                c = tk.NextChar()
                                If c = "/" Then
                                    tk.NextChar()
                                    c = """"
                                Else
                                    tk.NextChar()
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_STRING, textstr))
                                    Exit While
                                End If
                            End If
                            textstr = textstr & c
                        End While
                    Case "'"
                        Dim textstr As String = ""
                        While (c <> vbCr And c <> vbLf)
                            c = tk.NextChar()

                            If c = "'" Then
                                tk.Back()
                                tk.Back()
                                c = tk.NextChar()
                                If c = "/" Then
                                    tk.NextChar()
                                    c = "'"
                                Else
                                    tk.NextChar()
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_STRING, textstr))
                                    Exit While
                                End If
                            End If
                            textstr = textstr & c
                        End While
                    Case Else
                End Select
            End If
        End While

        Return tokenlist
    End Function

    Public Shared Function IsLetter(str As String) As Boolean
        Dim rex As New Regex("[a-zA-Z_]+")
        Return rex.Match(str).Success
    End Function
    Public Shared Function IsHexLetter(str As String) As Boolean
        Dim rex As New Regex("[a-fA-f0-9]+")
        Return rex.Match(str).Success
    End Function
    '연산자
    '숫자
    '문자열
    '한라인 주석
    '두라인 주석
    '식별자
    '키워드
End Class
