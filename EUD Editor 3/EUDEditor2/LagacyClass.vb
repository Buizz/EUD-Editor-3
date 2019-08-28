Imports System.Text.RegularExpressions

Public Class LagacyClass
    Public ReadOnly Property LagacySaveLoad As LagacySaveLoad


    Public Sub New()
        LagacySaveLoad = New LagacySaveLoad
    End Sub

    Public Shared Function STRdecToHec(inputstr As String) As String
        '<십진수>를 <16진수>로 바꾸자
        Dim tRegex As New Regex("<\d+>")

        Dim matches As Match = tRegex.Match(inputstr)

        While (matches.Success)
            Dim number As Integer = Mid(matches.Value, 2, matches.Value.Length - 2)

            Dim hexnum As String = Hex(number).ToUpper



            inputstr = Replace(inputstr, matches.Value, "<ꊛꊛ" & hexnum & "ꊛꊛ>", 1, 1)


            matches = tRegex.Match(inputstr)
        End While


        inputstr = inputstr.Replace("ꊛ", "")

        Return inputstr
    End Function
End Class
