Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Interop

Public Class Lexical_Analyzer
    Public Function DoWork(str As String) As List(Of Token)
        'function a(asd){}
        Dim tokenlist As New List(Of Token)

        Dim tk As New Tokenizer(str & " ")

        Dim IsDollar As Boolean = False


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
                    Dim v As String = ("0x" & tk.NextHex())
                    tokenlist.Add(New Token(Token.TokenType.TOKEN_NUMBER, tk, v))
                Else
                    tk.Back()
                    tk.Back()
                    Dim v As String = tk.NextDigt()
                    tokenlist.Add(New Token(Token.TokenType.TOKEN_NUMBER, tk, v))
                End If
            ElseIf IsLetter(c) Then
                Dim letter As String = c & tk.NextBlock.Trim

                Select Case letter
                    Case "import"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IMPORT, tk))
                    Case "as"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_AS, tk))
                    Case "var"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_VAR, tk))
                    Case "const"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CONST, tk))
                    Case "static"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_STATIC, tk))
                    Case "function"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FUNCTION, tk))
                    Case "object"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_OBJECT, tk))
                    Case "if"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IF, tk))
                    Case "else"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_ELSE, tk))
                    Case "for"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FOR, tk))
                    Case "foreach"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_FOREACH, tk))
                    Case "while"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_WHILE, tk))
                    Case "switch"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_SWITCH, tk))
                    Case "case"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CASE, tk))
                    Case "break"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BREAK, tk))
                    Case "continue"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_CONTINUE, tk))
                    Case "return"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RETURN, tk))
                    Case Else
                        If IsDollar Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_IDENTIFIER, tk, "$" & letter))
                            IsDollar = False
                        Else
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_IDENTIFIER, tk, letter))
                        End If
                End Select
            Else
                Select Case c
                    Case "$"
                        IsDollar = True
                        'tokenlist.Add(New Token(Token.TokenType.TOKEN_DOLLAR, tk))
                    Case "("
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LPAREN, tk))
                    Case "("
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LPAREN, tk))
                    Case ")"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RPAREN, tk))
                    Case "["
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LBRACKET, tk))
                    Case "]"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RBRACKET, tk))
                    Case "{"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_LSQBRACKET, tk))
                    Case "}"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_RSQBRACKET, tk))
                    Case "."
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_PERIOD, tk))
                    Case "?"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_QMARK, tk))
                    Case ","
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_COMMA, tk))
                    Case ":"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_COLON, tk))
                    Case ";"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_SEMICOLON, tk))
                    Case "~"
                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITNOT, tk))
                    Case "+"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_PLUS, tk))
                        Else
                            c = tk.NextChar()
                            If c = "+" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_INC, tk))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IADD, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_PLUS, tk))
                                tk.Back()
                            End If
                        End If
                    Case "-"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MINUS, tk))
                        Else
                            c = tk.NextChar()
                            If c = "-" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_DEC, tk))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_ISUB, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MINUS, tk))
                                tk.Back()
                            End If
                        End If
                    Case "*"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MULTIPLY, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IMUL, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MULTIPLY, tk))
                                tk.Back()
                            End If
                        End If
                    Case "/"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_DIVIDE, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IDIV, tk))
                            ElseIf c = "*" Then
                                Dim commentstr As String = ""

                                '주석시작
                                While (True)
                                    c = tk.NextChar()
                                    If c = "*" Then
                                        c = tk.NextChar()
                                        If c = "/" Then
                                            tokenlist.Add(New Token(Token.TokenType.TOKEN_COMMENT, tk, commentstr))
                                            Exit While
                                        Else
                                            commentstr = commentstr & c
                                        End If
                                    End If
                                    commentstr = commentstr & c
                                End While

                            ElseIf c = "/" Then
                                '주석시작
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LINECOMMENT, tk, tk.NextLine()))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_DIVIDE, tk))
                                tk.Back()
                            End If
                        End If
                    Case "%"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MOD, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IMOD, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_MOD, tk))
                                tk.Back()
                            End If
                        End If
                    Case "<"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_LT, tk))
                        Else
                            c = tk.NextChar()

                            If c = "?" Then
                                Dim commentstr As String = ""

                                '주석시작
                                While (True)
                                    c = tk.NextChar()
                                    If c = "?" Then
                                        c = tk.NextChar()
                                        If c = ">" Then
                                            tokenlist.Add(New Token(Token.TokenType.TOKEN_MACRO, tk, commentstr))
                                            Exit While
                                        Else
                                            commentstr = commentstr & "?" & c
                                        End If
                                    End If
                                    commentstr = commentstr & c
                                End While


                            ElseIf c = "<" Then
                                If tk.IsEnd Then
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_BITLSHIFT, tk))
                                Else
                                    c = tk.NextChar()
                                    If c = "=" Then
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_ILSHIFT, tk))
                                    Else
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITLSHIFT, tk))
                                        tk.Back()
                                    End If
                                End If
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LE, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LT, tk))
                                tk.Back()
                            End If
                        End If
                    Case ">"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_GT, tk))
                        Else
                            c = tk.NextChar()
                            If c = ">" Then
                                If tk.IsEnd Then
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_BITRSHIFT, tk))
                                Else
                                    c = tk.NextChar()
                                    If c = "=" Then
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_IRSHIFT, tk))
                                    Else
                                        tokenlist.Add(New Token(Token.TokenType.TOKEN_BITRSHIFT, tk))
                                        tk.Back()
                                    End If
                                End If
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_GE, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_GT, tk))
                                tk.Back()
                            End If
                        End If
                    Case "&"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITAND, tk))
                        Else
                            c = tk.NextChar()
                            If c = "&" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LAND, tk))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITAND, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITAND, tk))
                                tk.Back()
                            End If
                        End If
                    Case "|"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITOR, tk))
                        Else
                            c = tk.NextChar()
                            If c = "|" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LOR, tk))
                            ElseIf c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITOR, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITOR, tk))
                                tk.Back()
                            End If
                        End If
                    Case "="
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_ASSIGN, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_EQ, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_ASSIGN, tk))
                                tk.Back()
                            End If
                        End If
                    Case "^"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_BITXOR, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_IBITXOR, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_BITXOR, tk))
                                tk.Back()
                            End If
                        End If
                    Case "!"
                        If tk.IsEnd Then
                            tokenlist.Add(New Token(Token.TokenType.TOKEN_LNOT, tk))
                        Else
                            c = tk.NextChar()
                            If c = "=" Then
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_NE, tk))
                            Else
                                tokenlist.Add(New Token(Token.TokenType.TOKEN_LNOT, tk))
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
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_STRING, tk, textstr))
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
                                    tokenlist.Add(New Token(Token.TokenType.TOKEN_STRING, tk, textstr))
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
        Dim rex As New Regex("[ㄱ-ㅎ|ㅏ-ㅣ|가-힣a-zA-Z_]+")
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
