Imports System.IO

Partial Public Class BuildData



    Private Sub WriteTEFile()
        macro.IsBulid = True


        If Not My.Computer.FileSystem.DirectoryExists(TriggerEditorPath) Then
            My.Computer.FileSystem.CreateDirectory(TriggerEditorPath)
        End If


        CreateTEFile(TriggerEditorPath, pjData.TEData.PFIles)


        Dim filestreama As New FileStream(TriggerEditorPath & "\PluginVariables.py", FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama, Text.Encoding.UTF8)
        strWriter.Write(macro.GetpreVarEPS)
        strWriter.Close()
        filestreama.Close()
        macro.IsBulid = False
    End Sub

    Private Sub CreateTEFile(FolderPath As String, tTEFile As TEFile)

        For i = 0 To tTEFile.FileCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Files(i).RealFileName

            'If pjData.TEData.MainFile Is tTEFile.Files(i) Then
            '    If pjData.TEData.SCArchive.IsUsed Then
            '        WriteSCAScript(FolderPath)
            '    End If
            'End If

            Try
                Dim fs As New FileStream(filePath, FileMode.Create)
                Dim sw As New StreamWriter(fs)

                sw.Write(tTEFile.Files(i).Scripter.GetFileText(tTEFile.Files(i).FileName))

                sw.Close()
                fs.Close()
            Catch ex As Exception

            End Try

        Next

        For i = 0 To tTEFile.FolderCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Folders(i).FileName
            My.Computer.FileSystem.CreateDirectory(filePath)
            CreateTEFile(filePath, tTEFile.Folders(i))
        Next
    End Sub
End Class
