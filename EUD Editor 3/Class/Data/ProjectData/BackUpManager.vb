Module BackUpManager

    Public BackupCount As Integer = 5
    Public BackUpFileName As String = "back"
    Public Sub BackUpFile(SavePath As String)
        Dim BackUpPath As String = BuildData.BackupFilePath & "\"

        Dim backupFiles() As String = IO.Directory.GetFiles(BackUpPath)


        Dim BackUpFile As String = ""
        If backupFiles.Length < BackupCount Then
            '단순 파일 추가
            BackUpFile = BackUpPath & BackUpFileName & backupFiles.Length
        Else
            '가장 예전파일에 덮어쓰기
            Dim dd As Date = Now
            For Each f As String In backupFiles
                Dim fileinfo As New IO.FileInfo(f)
                If fileinfo.LastWriteTime < dd Then
                    dd = fileinfo.LastWriteTime
                    BackUpFile = f
                End If
            Next
        End If

        Try
            IO.File.Copy(SavePath, BackUpFile, True)
        Catch ex As Exception

        End Try
    End Sub

End Module
