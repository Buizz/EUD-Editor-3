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
        End If

        '[EUDEditor.py]

        '[TriggerEditor.eps]

        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy

        Dim filestreama As New FileStream(EdsFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub
End Class
