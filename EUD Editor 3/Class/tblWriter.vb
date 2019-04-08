Imports System.IO
Imports System.Text.RegularExpressions

Public Class tblWriter
    Public Shared Sub WriteTbl(tblStrings As String(), filename As String)
        Dim fs As New FileStream(filename, FileMode.Create)
        Dim bw As New BinaryWriter(fs)

        Dim tblCount As UInt16 = tblStrings.Count

        'tbl의 수를 쓴다
        bw.Write(tblCount)

        Dim header(tblCount - 1) As UInt16

        fs.Position = tblCount * 2 + 2
        For i = 0 To tblCount - 1
            header(i) = fs.Position


            Dim rgx As New Regex("<([^<>])+>", RegexOptions.IgnoreCase)
            Dim tempstr As String = tblStrings(i)

            '==================문자 변형========================
            Dim MatchPass As Integer = 0

            Dim TempChar As String = "ᚏ"
            Dim SpecialKeys As New List(Of String)
            Dim SpecialKeyPos As New List(Of Integer)
            Dim OriginalKeys As New List(Of String)

            Dim Matchs As Text.RegularExpressions.MatchCollection = rgx.Matches(tempstr)
            'Rgx들을 문자 치환하고 해당 번지를 돌려주자.
            '만약 Virtual안에 있으면 해당 번호로 돌려줌.
            For k = 0 To Matchs.Count - 1
                Dim tMatchs As Text.RegularExpressions.MatchCollection = rgx.Matches(tempstr)

                Dim pureStr As String = Mid(tMatchs(MatchPass).Value, 2, tMatchs(MatchPass).Value.Length - 2)
                Dim ResultStr As String = pureStr

                Dim PassFlag As Boolean = False
                '값의 순수성 검사.(Virtual KEY문자이거나 16진수 이거나 판단)
                For keys = 0 To SCConst.ASCIICount
                    If scData.ASCIICode(keys) = pureStr Then 'Virtual KEY일 경우
                        ResultStr = Hex(keys).PadLeft(2, "0")
                        PassFlag = True
                    End If
                Next
                If Not PassFlag Then
                    Try
                        Dim Isnum As Long = "&H" & pureStr '16진수인지 판별
                        ResultStr = Hex(Isnum).PadLeft(2, "0")
                        PassFlag = True
                    Catch ex As Exception

                    End Try
                End If
                If Not PassFlag Then
                    MatchPass += 1
                    Continue For
                End If

                OriginalKeys.Add(tMatchs(MatchPass).Value)
                SpecialKeys.Add(ResultStr)
                SpecialKeyPos.Add(tMatchs(MatchPass).Index)
                tempstr = Replace(tempstr, tMatchs(MatchPass).Value, TempChar, 1, 1)
            Next
            '별문자완성


            For k = 0 To SpecialKeyPos.Count - 1
                tempstr = Replace(tempstr, TempChar, Chr("&H" & SpecialKeys(k)), 1, 1)
            Next


            'If rgx.Match(tempstr).Success Then
            '    Dim Matches As MatchCollection = rgx.Matches(tempstr)
            '    For k = 0 To rgx.Matches(tempstr).Count - 1
            '        Dim Match As Match = Matches(k)


            '        '찾은 문자 판별

            '        Dim RealChar As Byte

            '        Try
            '            RealChar = "&H" & Mid(Match.Value, 2, Match.Value.Length - 2)
            '        Catch ex As Exception
            '            Continue For
            '        End Try


            '        tempstr = tempstr.Replace(Match.Value, Chr(RealChar))
            '    Next
            'End If


            '==================문자 변형========================


            bw.Write(Text.Encoding.Default.GetBytes(tempstr))
            bw.Write(CByte(0))
        Next

        fs.Position = 2
        For i = 0 To tblCount - 1
            bw.Write(header(i))
        Next


        bw.Close()
        fs.Close()
    End Sub

End Class
