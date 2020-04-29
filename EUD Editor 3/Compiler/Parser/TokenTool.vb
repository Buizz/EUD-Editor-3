Partial Public Class Parser
    Public index As Long = 0
    Private RawList As List(Of Token)


    Public Function PreviousToken() As Token
        If index <> 0 Then
            Return RawList(index - 1)
        Else
            Return Nothing
        End If
    End Function
    Public Sub PreviousTokenDelete()
        RawList.RemoveAt(index - 1)
        index -= 1
    End Sub

    Public Function NextToken() As Token
        Dim rtoken As Token = RawList(index)

        If (rtoken.TType = Token.TokenType.TOKEN_MACRO Or
            rtoken.TType = Token.TokenType.TOKEN_LINECOMMENT Or
            rtoken.TType = Token.TokenType.TOKEN_COMMENT) Then
            While (rtoken.TType = Token.TokenType.TOKEN_MACRO Or
                    rtoken.TType = Token.TokenType.TOKEN_LINECOMMENT Or
                    rtoken.TType = Token.TokenType.TOKEN_COMMENT)
                rtoken = RawList(index)
                index += 1
            End While
        Else
            index += 1
        End If

        Return rtoken
    End Function

    '무조건 넘어감
    Public Function CheckNextToken(type As Token.TokenType) As Token
        Dim rtoken As Token = RawList(index)

        If (rtoken.TType = Token.TokenType.TOKEN_MACRO Or
            rtoken.TType = Token.TokenType.TOKEN_LINECOMMENT Or
            rtoken.TType = Token.TokenType.TOKEN_COMMENT) Then
            While (rtoken.TType = Token.TokenType.TOKEN_MACRO Or
                    rtoken.TType = Token.TokenType.TOKEN_LINECOMMENT Or
                    rtoken.TType = Token.TokenType.TOKEN_COMMENT)
                rtoken = RawList(index)
                index += 1
            End While
        Else
            index += 1
        End If



        If rtoken.TType = type Then
            Return rtoken
        Else
            Throw New Exception("Line : " & rtoken.Ln & " Col : " & rtoken.Col & ", Parse 오류 타입 불일치 " & type.ToString & "가 와야 하지만 " & rtoken.TType.ToString & "가 왔습니다.")
            Return Nothing
        End If
    End Function

    '토큰이 참일 경우 넘어감
    Public Function CheckBlockToken(type As Token.TokenType) As Boolean
        Dim rtoken As Token = RawList(index)



        If rtoken.TType = type Then
            index += 1
            Return True
        End If
        Return False
    End Function

    Public Function CurrentToken() As Token
        Dim rtoken As Token = RawList(index)
        Return rtoken
    End Function
End Class
