Partial Public Class ProjectData
    Public Sub InitSetting()
        Dim tTEFile As New TEFile("main", TEFile.EFileType.ClassicTrigger)



        SaveData.TEData.PFIles.FileAdd(tTEFile)
        SaveData.TEData.MainFile = tTEFile

        SaveData.TEData.LastOpenTabs.Items.Add(tTEFile)
    End Sub
End Class
