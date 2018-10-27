Module GlobalObj
    Public pgData As ProgramData
    Public pjData As ProjectData
    Public scData As StarCraftData



    Public DataEditorForm As DataEditor
    Public SettiingForm As SettingWindows



    Public Function InitProgram() As Boolean
        Tool.Init()

        Try
            pgData = New ProgramData
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error ProgramInit Fail"), ex.ToString)
            Application.Current.Shutdown()
            Return False
        End Try

        Try
            scData = New StarCraftData
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error LoadStarCraftData Fail"), ex.ToString)
            Application.Current.Shutdown()
            Return False
        End Try




        '세팅파일
        If pgData.Setting(ProgramData.TSetting.euddraft) = Nothing Then
            pgData.SaveSetting()
        End If
        Return True
    End Function

    Public Sub ShutDownProgram()
        Try
            pgData.SaveSetting()
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error SettingSave Fail"))
        End Try

    End Sub
End Module
