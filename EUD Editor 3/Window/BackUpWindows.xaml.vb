Public Class BackUpWindows
    Public IsOkay As Boolean = False
    Public SelectBackFile As String




    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim BackUpPath As String = BuildData.BackupFilePath & "\"



        Dim backupFiles As New List(Of IO.FileInfo)
        For Each f As String In IO.Directory.GetFiles(BackUpPath)
            Dim fileinfo As New IO.FileInfo(f)
            backupFiles.Add(fileinfo)
        Next



        backupFiles.Sort(Function(x As IO.FileInfo, y As IO.FileInfo)
                             Return y.LastWriteTime.CompareTo(x.LastWriteTime)
                         End Function)

        For Each b In backupFiles
            BackUpList.Items.Add(New BackUpFileItem(b))
        Next
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        IsOkay = True
        Close()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        IsOkay = False
        Close()
    End Sub


    Private Sub MapList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        SelectBackFile = CType(BackUpList.SelectedItem, BackUpFileItem).BackFileInfo.FullName
        okaybtn.IsEnabled = True
    End Sub
End Class
