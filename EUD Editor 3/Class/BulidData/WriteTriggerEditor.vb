Imports System.IO

Partial Public Class BuildData
    Private Sub WriteSCAScript(filePath As String)

        Dim fs As New FileStream(EudPlibFilePath & "\scatempfile", FileMode.Create)
        Dim sw As New StreamWriter(fs)
        sw.Write(pjData.TEData.SCArchive.MapName)
        sw.Close()
        fs.Close()


        fs = New FileStream(filePath & "\SCArchive.eps", FileMode.Create)
        sw = New StreamWriter(fs)

        sw.Write(GetSCAEps)

        sw.Close()
        fs.Close()
    End Sub


    Private Sub WriteTEFile()
        If Not My.Computer.FileSystem.DirectoryExists(TriggerEditorPath) Then
            My.Computer.FileSystem.CreateDirectory(TriggerEditorPath)
        End If


        CreateTEFile(TriggerEditorPath, pjData.TEData.PFIles)
    End Sub

    Private Sub CreateTEFile(FolderPath As String, tTEFile As TEFile)

        For i = 0 To tTEFile.FileCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Files(i).RealFileName

            If pjData.TEData.MainFile Is tTEFile.Files(i) Then
                If pjData.TEData.SCArchive.IsUsed Then
                    WriteSCAScript(FolderPath)
                End If
            End If


            Dim fs As New FileStream(filePath, FileMode.Create)
            Dim sw As New StreamWriter(fs)

            sw.Write(tTEFile.Files(i).Scripter.GetFileText)

            sw.Close()
            fs.Close()
        Next

        For i = 0 To tTEFile.FolderCount - 1
            Dim filePath As String = FolderPath & "\" & tTEFile.Folders(i).FileName
            My.Computer.FileSystem.CreateDirectory(filePath)
            CreateTEFile(filePath, tTEFile.Folders(i))
        Next
    End Sub
End Class
