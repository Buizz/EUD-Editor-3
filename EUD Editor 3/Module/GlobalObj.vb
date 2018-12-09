Module GlobalObj
    Public pgData As ProgramData
    Public pjData As ProjectData
    Public scData As StarCraftData



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
        scData = New StarCraftData
        Try

        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error LoadStarCraftData Fail"), ex.ToString)
            Application.Current.Shutdown()
            Return False
        End Try


        '언어 설정
        If pgData.Setting(ProgramData.TSetting.Language) = Nothing Then
            pgData.Setting(ProgramData.TSetting.Language) = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString()
        End If
        pgData.SetLanguage(pgData.Setting(ProgramData.TSetting.Language))
        'MsgBox(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString())



        '색 설정


        If pgData.Setting(ProgramData.TSetting.Theme) = "Light" Then
            pgData.SetTheme(True)
        ElseIf pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            pgData.SetTheme(False)
        Else
            pgData.Setting(ProgramData.TSetting.Theme) = "Light"
            pgData.SetTheme(True)
        End If




        '세팅파일
        If pgData.Setting(ProgramData.TSetting.euddraft) = Nothing Then
            pgData.SaveSetting()
        End If


        Try
            Tool.SetRegistry()
        Catch ex As Exception

        End Try


        If Environment.GetCommandLineArgs.Count > 1 Then
            Dim filename As String = Environment.GetCommandLineArgs(1)
            ProjectData.Load(filename, pjData)
            'MsgBox(filename & " 다른파일로 열림")
        End If
        Return True
    End Function

    Public Function ShutDownProgram() As Boolean
        Try
            pgData.SaveSetting()
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error SettingSave Fail"))
        End Try
        Return False '종료 허락
    End Function
End Module
