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


            Dim rgx As New Regex("<([A-Za-z0-9])+>", RegexOptions.IgnoreCase)
            Dim tempstr As String = tblStrings(i)

            If rgx.Match(tempstr).Success Then
                Dim Matches As MatchCollection = rgx.Matches(tempstr)
                For k = 0 To rgx.Matches(tempstr).Count - 1
                    Dim Match As Match = Matches(k)

                    Dim RealChar As Byte

                    Try
                        RealChar = "&H" & Mid(Match.Value, 2, Match.Value.Length - 2)
                    Catch ex As Exception
                        Continue For
                    End Try


                    tempstr = tempstr.Replace(Match.Value, Chr(RealChar))
                Next
            End If



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
