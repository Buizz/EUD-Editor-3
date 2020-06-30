Public Class SampleMapSetting
    Public IsOkay As Boolean = False
    Public SelectSampleMap As String

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        '샘플 맵을 읽어온다.
        For Each Files As String In My.Computer.FileSystem.GetFiles(Tool.SampleDataFolderPath)
            Dim sampleList As New SampleMapList(Files)

            If sampleList.IsMapLoading Then
                MapList.Items.Add(sampleList)
            End If
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
        SelectSampleMap = CType(MapList.SelectedItem, SampleMapList).CurrentMapPath
        okaybtn.IsEnabled = True
    End Sub
End Class
