Imports System.IO
Imports KopiLua

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

        Dim mainfile As TEFile = Nothing

        For i = 0 To tTEFile.FileCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Files(i).RealFileName

            If tTEFile.Files(i).Scripter.IsMain Then
                mainfile = tTEFile.Files(i)
                Continue For
            End If

            Try
                Dim fs As New FileStream(filePath, FileMode.Create)
                Dim sw As New StreamWriter(fs)

                sw.Write(tTEFile.Files(i).Scripter.GetFileText(tTEFile.Files(i).FileName))

                sw.Close()
                fs.Close()
            Catch ex As Exception

            End Try
        Next

        If mainfile IsNot Nothing Then
            Try
                Dim filePath As String = FolderPath & "\" & mainfile.RealFileName
                Dim fs As New FileStream(filePath, FileMode.Create)
                Dim sw As New StreamWriter(fs)

                sw.Write(mainfile.Scripter.GetFileText(mainfile.FileName))

                sw.Close()
                fs.Close()
            Catch ex As Exception

            End Try
        End If



        For i = 0 To tTEFile.FolderCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Folders(i).FileName
            My.Computer.FileSystem.CreateDirectory(filePath)
            CreateTEFile(filePath, tTEFile.Folders(i))
        Next
    End Sub
End Class
