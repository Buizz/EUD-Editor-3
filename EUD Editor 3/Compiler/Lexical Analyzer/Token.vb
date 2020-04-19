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

    Public Sub New(_ttype As TokenType, Optional v As String = "")
        TType = _ttype
        value = v
    End Sub
End Class