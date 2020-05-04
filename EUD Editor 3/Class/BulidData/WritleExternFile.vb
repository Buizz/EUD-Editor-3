Imports System.IO

Partial Public Class BuildData
    Public Sub WriteExternFIle()
        Dim folderpath As String = GetTriggerEditorFolderPath

        Dim copypath As String = TriggerEditorPath & "\TriggerEditor"

        If Not My.Computer.FileSystem.DirectoryExists(copypath) Then
            My.Computer.FileSystem.CreateDirectory(copypath)
        End If


        For Each files As String In My.Computer.FileSystem.GetFiles(folderpath)
            Dim fileinfo As FileInfo = My.Computer.FileSystem.GetFileInfo(files)
            If fileinfo.Extension = "eps" Then
                My.Computer.FileSystem.CopyFile(files, copypath & "\" & fileinfo.Name, True)
            End If
        Next
    End Sub
End Class
