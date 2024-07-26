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


            'Dim rgx As New Regex("<[^>\\]*(?:\\.[^>\\]*)*>", RegexOptions.IgnoreCase)
            Dim tempstr As String = StrExecuter(tblStrings(i))
            tempstr = StringTool.ChangeSlash(tempstr)

            Dim en As Text.Encoding = Text.Encoding.GetEncoding(949, New Text.EncoderExceptionFallback(), New Text.DecoderExceptionFallback())
            'Dim en As Text.Encoding = Text.Encoding.UTF8


            Try
                Dim barray() As Byte
                barray = en.GetBytes(tempstr)
                bw.Write(barray)
            Catch ex As Text.EncoderFallbackException
                bw.Write(Text.Encoding.UTF8.GetBytes(tempstr))
                bw.Write(CByte(&HE2))
                bw.Write(CByte(&H80))
                bw.Write(CByte(&H89))
            End Try



            'bw.Write(CByte(&HE2))
            'bw.Write(CByte(&H80))
            'bw.Write(CByte(&H89))
            'bw.Write(Text.Encoding.UTF8.GetBytes(StringTool.ChangeSlash(tempstr)))


            bw.Write(CByte(0))
        Next

        fs.Position = 2
        For i = 0 To tblCount - 1
            bw.Write(header(i))
        Next


        bw.Close()
        fs.Close()
    End Sub

    Public Shared Function StrExecuter(str As String, Optional DeleteOChar As Boolean = False) As String
        Dim rgx As New Regex("<[^>\\]*(?:\\.[^>\\]*)*>", RegexOptions.IgnoreCase)
        Dim tempstr As String = str

        '==================문자 변형========================
        Dim MatchPass As Integer = 0

        Dim TempChar As String = "ᚏ"
        Dim SpecialKeys As New List(Of String)
        Dim SpecialKeyPos As New List(Of Integer)
        Dim OriginalKeys As New List(Of String)

        Dim Matches As Text.RegularExpressions.MatchCollection = rgx.Matches(tempstr)
        'Rgx들을 문자 치환하고 해당 번지를 돌려주자.
        '만약 Virtual안에 있으면 해당 번호로 돌려줌.
        For k = 0 To Matches.Count - 1
            Dim tMatches As Text.RegularExpressions.MatchCollection = rgx.Matches(tempstr)

            Dim pureStr As String = Mid(tMatches(MatchPass).Value, 2, tMatches(MatchPass).Value.Length - 2)
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

            OriginalKeys.Add(tMatches(MatchPass).Value)
            SpecialKeys.Add(ResultStr)
            SpecialKeyPos.Add(tMatches(MatchPass).Index)
            tempstr = Replace(tempstr, tMatches(MatchPass).Value, TempChar, 1, 1)
        Next
        '별문자완성


        For k = 0 To SpecialKeyPos.Count - 1
            If DeleteOChar Then
                tempstr = Replace(tempstr, TempChar, "", 1, 1)
            Else
                tempstr = Replace(tempstr, TempChar, Chr("&H" & SpecialKeys(k)), 1, 1)
            End If
        Next
        Return tempstr
    End Function

End Class
