Public Class SCAWorkSpace


    'sb.AppendLine("const ws = Db(" & EntryPoint.Count * 4 + '엔트리포인트
    '8 * (CommandLength + SpaceLength) + '일반 값
    '(FastLoadCommandLength + SpaceLength) + '패스트 로드
    'FuncCommandLength + FuncLength * 4 + 'FuncLoad
    'FuncLength * 4 + 'FuncLoad
    'SCAScriptVarCount * 4 &'FuncReturnTable

    Public Spaces As New List(Of Space)


    Public Sub AddSpace(key As String, datalen As Long)
        Spaces.Add(New Space(key, datalen))
    End Sub

    Public Function GetSpaceStartOffset(key As String) As Long
        Dim rval As Long = 0

        Dim i As Integer = 0
        While Spaces(i).KEY <> key
            rval += Spaces(i).DATALEN
            i += 1
        End While

        Return rval
    End Function

    Public Function GetAllCapacity() As Long
        Dim rval As Long = 0
        For Each i In Spaces
            rval += i.DATALEN
        Next
        Return rval
    End Function


    Public Class Space
        Public KEY As String
        Public DATALEN As Long

        Public Sub New(KEY As String, DATALEN As Long)
            Me.KEY = KEY
            Me.DATALEN = DATALEN
        End Sub

    End Class
End Class
