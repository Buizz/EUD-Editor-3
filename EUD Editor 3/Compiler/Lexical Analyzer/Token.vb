Public Class Token
    '플래그 타입
    Public Enum TokenType

        'Keywords
        TOKEN_IMPORT
        TOKEN_AS
        TOKEN_VAR
        TOKEN_CONST
        TOKEN_STATIC
        TOKEN_FUNCTION
        TOKEN_OBJECT
        TOKEN_L2V
        TOKEN_ONCE
        TOKEN_IF
        TOKEN_ELSE
        TOKEN_FOR
        TOKEN_FOREACH
        TOKEN_WHILE
        TOKEN_SWITCH
        TOKEN_CASE
        TOKEN_BREAK
        TOKEN_CONTINUE
        TOKEN_RETURN

        'Identifiers
        TOKEN_IDENTIFIER

        'value
        TOKEN_NUMBER
        TOKEN_STRING
        TOKEN_LINECOMMENT
        TOKEN_COMMENT
        TOKEN_MACRO

        TOKEN_OPERATOR1
        'Operators
        TOKEN_INC '++
        TOKEN_DEC '--
        TOKEN_IADD '+=
        TOKEN_ISUB '-=
        TOKEN_IMUL '*=
        TOKEN_IDIV '/=
        TOKEN_IMOD '%=
        TOKEN_ILSHIFT '<<=
        TOKEN_IRSHIFT '>>=
        TOKEN_IBITAND '&=
        TOKEN_IBITOR '|=
        TOKEN_IBITXOR '^=
        TOKEN_ASSIGN '=

        TOKEN_PLUS '+
        TOKEN_MINUS '-
        TOKEN_MULTIPLY '*
        TOKEN_DIVIDE '/
        TOKEN_MOD '%
        TOKEN_BITLSHIFT '<<
        TOKEN_BITRSHIFT '>>
        TOKEN_BITAND '&
        TOKEN_BITOR '|
        TOKEN_BITXOR '^
        TOKEN_BITNOT '~
        TOKEN_LAND '&&
        TOKEN_LOR '||
        TOKEN_LNOT '!
        TOKEN_EQ '==
        TOKEN_LE '<=
        TOKEN_GE '>=
        TOKEN_LT '<
        TOKEN_GT '>
        TOKEN_NE '!=

        TOKEN_OPERATOR2

        'Other tokens
        TOKEN_LPAREN '(
        TOKEN_RPAREN ')
        TOKEN_LBRACKET '[
        TOKEN_RBRACKET ']
        TOKEN_LSQBRACKET '{
        TOKEN_RSQBRACKET '}
        TOKEN_PERIOD '.
        TOKEN_QMARK '?
        TOKEN_COMMA ',
        TOKEN_COLON ':
        TOKEN_SEMICOLON ';
    End Enum
    Public TType As TokenType

    Public value As String
    Public Ln As Long
    Public Col As Long


    Public Function ToText() As String
        Select Case TType
            Case TokenType.TOKEN_INC
                Return "++"
            Case TokenType.TOKEN_DEC
                Return "--"
            Case TokenType.TOKEN_IADD
                Return "+="
            Case TokenType.TOKEN_ISUB
                Return "-="
            Case TokenType.TOKEN_IMUL
                Return "*="
            Case TokenType.TOKEN_IDIV
                Return "/="
            Case TokenType.TOKEN_IMOD
                Return "%="
            Case TokenType.TOKEN_ILSHIFT
                Return "<<="
            Case TokenType.TOKEN_IRSHIFT
                Return ">>="
            Case TokenType.TOKEN_IBITAND
                Return "&="
            Case TokenType.TOKEN_IBITOR
                Return "|="
            Case TokenType.TOKEN_IBITXOR
                Return "^="
            Case TokenType.TOKEN_ASSIGN
                Return "="
            Case TokenType.TOKEN_PLUS
                Return "+"
            Case TokenType.TOKEN_MINUS
                Return "-"
            Case TokenType.TOKEN_MULTIPLY
                Return "*"
            Case TokenType.TOKEN_DIVIDE
                Return "/"
            Case TokenType.TOKEN_MOD
                Return "%"
            Case TokenType.TOKEN_BITLSHIFT
                Return "<<"
            Case TokenType.TOKEN_BITRSHIFT
                Return ">>"
            Case TokenType.TOKEN_BITAND
                Return "&"
            Case TokenType.TOKEN_BITOR
                Return "|"
            Case TokenType.TOKEN_BITXOR
                Return "^"
            Case TokenType.TOKEN_BITNOT
                Return "~"
            Case TokenType.TOKEN_LAND
                Return "&&"
            Case TokenType.TOKEN_LOR
                Return "||"
            Case TokenType.TOKEN_LNOT
                Return "!"
            Case TokenType.TOKEN_EQ
                Return "=="
            Case TokenType.TOKEN_LE
                Return "<="
            Case TokenType.TOKEN_GE
                Return ">="
            Case TokenType.TOKEN_LT
                Return "<"
            Case TokenType.TOKEN_GT
                Return ">"
            Case TokenType.TOKEN_NE
                Return "!="
        End Select
    End Function


    Public ReadOnly Property IsOperator As Boolean
        Get
            Return TokenType.TOKEN_OPERATOR1 < TType And TType < TokenType.TOKEN_OPERATOR2
        End Get
    End Property


    Public Sub New(_ttype As TokenType, tk As Tokenizer, Optional v As String = "")
        TType = _ttype
        Ln = tk.line
        Col = tk.col

        value = v
    End Sub
End Class