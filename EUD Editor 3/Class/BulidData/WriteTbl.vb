Imports System.IO

Partial Public Class BuildData
    Private Sub WriteTbl()
        Dim tbls(SCtbltxtCount - 1) As String

        Dim Lasttbls As Integer
        For i = 0 To SCtbltxtCount - 1
            tbls(i) = pjData.BuildStat_txt(i)
            If Not pjData.BuildStat_txtIsDefault(i) Then
                Lasttbls = i + 1
            End If
        Next



        If Lasttbls > 0 Then
            tblWriter.WriteTbl(tbls, tblFilePath)
        Else
            tblWriter.WriteTbl({}, tblFilePath)
        End If
    End Sub
End Class
