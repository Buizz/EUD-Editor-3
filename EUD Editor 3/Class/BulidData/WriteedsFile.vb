Imports System.IO
Imports System.Text

Partial Public Class BuildData

    Private Sub WriteedsFile()
        Dim sb As New StringBuilder

        sb.AppendLine("[main]")
        sb.AppendLine("input: " & OpenMapPath)
        sb.AppendLine("output: " & SaveMapPath)
        sb.AppendLine("")
        If My.Computer.FileSystem.FileExists(DatpyFilePath) Then
            sb.AppendLine("[DataEditor.py]")
            sb.AppendLine("[ExtraDataEditor.py]")
            sb.AppendLine("")
        End If

        sb.AppendLine("[dataDumper]")
        If pjData.UseCustomtbl Then
            'tbl 파일 쓰기
            sb.AppendLine(Tool.GetRelativePath(EdsFilePath, tblFilePath) & " : 0x6D5A30, copy")
        End If
        'RequireData 쓰기
        sb.AppendLine(Tool.GetRelativePath(EdsFilePath, requireFilePath) & " : 0x" & Hex(Tool.GetOffset("Vanilla")) & ", copy")


        '"[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\stat_txt.tbl : 0x6D5A30, copy"
        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy


        '[TriggerEditor.eps]


        Dim filestreama As New FileStream(EdsFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub
End Class
