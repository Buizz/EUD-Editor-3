Module GlobalObj


    Public pgData As ProgramData
    Public pjData As ProjectData
    Public scData As StarCraftData



    Public DataEditorForm As DataEditor
    Public SettiingForm As SettingWindows



    Public Sub InitProgram()
        Tool.Init()

        pgData = New ProgramData
        scData = New StarCraftData

        pjData = New ProjectData


        '세팅파일
        If pgData.Setting(ProgramData.TSetting.euddraft) = Nothing Then
            pgData.SaveSetting()
        End If
    End Sub

    Public Sub ShutDownProgram()
        pgData.SaveSetting()
    End Sub
End Module
