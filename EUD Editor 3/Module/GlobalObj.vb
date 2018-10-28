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

        Try
            Tool.SetRegistry()
        Catch ex As Exception

        End Try


        If Environment.GetCommandLineArgs.Count > 1 Then
            Dim filename As String = Environment.GetCommandLineArgs(1)
            ProjectData.Load(filename, pjData)
            'MsgBox(filename & " 다른파일로 열림")
        End If

        'MsgBox(Environment.GetCommandLineArgs(1))



        'AppDomain.CreateDomain("")
        'MsgBox(My.Computer.Registry.ClassesRoot.CreateSubKey("e3s\shell\open\command").GetValue(""))
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
