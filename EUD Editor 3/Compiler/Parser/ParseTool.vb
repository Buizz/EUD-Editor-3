Partial Public Class Parser
    '네임스페이스를 얻는 함수

    Public NameSpaces As List(Of String)
    Public Function GetNameSpace() As List(Of String)
        If NameSpaces Is Nothing Then
            NameSpaces = New List(Of String)
            For i = 0 To MainCode.Items.Count - 1
                If MainCode.Items(i).BType = CodeType.CODE_IMPORT Then
                    NameSpaces.Add(MainCode.Items(i).Value2)
                End If
            Next
        End If
        Return NameSpaces
    End Function
End Class
