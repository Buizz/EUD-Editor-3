Imports System.Windows.Threading
Imports System.Windows.Interop
Imports System.Net
Imports System.IO

Module GlobalObj
    Public pgData As ProgramData
    Public pjData As ProjectData
    Public scData As StarCraftData


    Public Lagacy As LagacyClass


    Public ProjectControlBinding As MainMenuBinding

    Public SettiingForm As SettingWindows


    Public tescm As GUIScriptManager
    Public macro As MacroManager


    Public ctheme As CustomTheme

    Public SCATEFile As TEFile
    Public Function UpdateCheck() As Boolean
        If pgData.Setting(ProgramData.TSetting.CheckUpdate) Then
            Dim data As String = ""
            Try
                With CreateObject("WinHttp.WinHttpRequest.5.1")
                    .Open("GET", "https://raw.githubusercontent.com/Buizz/EUD-Editor-3/master/EUD%20Editor%203/Version.txt")
                    .Send
                    .WaitForResponse

                    data = .ResponseText

                End With
            Catch ex As Exception
                data = "Error"
            End Try

            Dim version As String

            Dim lines() As String = data.Trim.Split(vbLf)

            If lines.Count > 1 Then
                version = lines(0).Trim
                If pgData.Version.ToString <> version Then
                    Return True
                Else
                    Return False
                End If
            End If
            'WindowMenu.Setting()
            'MsgBox(data)
        End If
        Return False
    End Function











    Public Function InitProgramDatas() As Boolean
        Try
            scData = New StarCraftData
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error LoadStarCraftData Fail"), ex.ToString)
            Application.Current.Shutdown()
            Return False
        End Try


        If Environment.GetCommandLineArgs.Count > 1 Then
            Dim filename As String = Environment.GetCommandLineArgs(1)
            ProjectData.Load(filename, pjData)
            'MsgBox(filename & " 다른파일로 열림")
        End If
        Return True
    End Function
    Public Function InitProgram() As Boolean
        Try
            My.Computer.FileSystem.DeleteFile(System.AppDomain.CurrentDomain.BaseDirectory & "EUDEditorDownloader.exe")
        Catch ex As Exception

        End Try

        'Try
        '    tescm = New ScriptManager
        'Catch ex As Exception
        '    Tool.ErrorMsgBox(Tool.GetText("Error TriggerScript Init Fail"), ex.ToString)
        '    Application.Current.Shutdown()
        '    Return False
        'End Try
        Try
            pgData = New ProgramData
            Lagacy = New LagacyClass
            tescm = New GUIScriptManager
            ctheme = New CustomTheme
            macro = New MacroManager
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error ProgramInit Fail"), ex.ToString)
            Application.Current.Shutdown()
            Return False
        End Try

        '언어 설정
        If pgData.Setting(ProgramData.TSetting.Language) = Nothing Then
            pgData.Setting(ProgramData.TSetting.Language) = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString()
        End If
        pgData.SetLanguage(pgData.Setting(ProgramData.TSetting.Language))
        'MsgBox(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString())

        Tool.Init()



        ProjectControlBinding = New MainMenuBinding


        '색 설정
        If pgData.Setting(ProgramData.TSetting.Theme) = "Light" Then
            ctheme.IsLight = True
        ElseIf pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            ctheme.IsLight = False
        Else
            pgData.Setting(ProgramData.TSetting.Theme) = "Light"
            ctheme.IsLight = True
        End If



        Dim colorStr As String = pgData.Setting(ProgramData.TSetting.DefaultData)
        If colorStr Is Nothing Then
            pgData.PFiledDefault = ColorConverter.ConvertFromString("#60DDCDCD")
        Else
            pgData.PFiledDefault = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.MapEditorData)
        If colorStr Is Nothing Then
            pgData.PFiledMapEditColor = ColorConverter.ConvertFromString("#60A1C7FF")
        Else
            pgData.PFiledMapEditColor = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.EditedData)
        If colorStr Is Nothing Then
            pgData.PFiledEditColor = ColorConverter.ConvertFromString("#60FFA3FA")
        Else
            pgData.PFiledEditColor = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.CheckedData)
        If colorStr Is Nothing Then
            pgData.PFiledFalgColor = ColorConverter.ConvertFromString("#80FF8F6A")
        Else
            pgData.PFiledFalgColor = ColorConverter.ConvertFromString(colorStr)
        End If


        ctheme.InitColor()
        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueLightBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueLight = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueMidBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueMid = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueDarkBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueDark = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueLightForegroundBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueLightForeground = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueMidForegroundBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueMidForeground = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.PrimaryHueDarkForegroundBrush)
        If colorStr IsNot Nothing Then
            ctheme.PrimaryHueDarkForeground = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.SecondaryAccentBrush)
        If colorStr IsNot Nothing Then
            ctheme.SecondaryMid = ColorConverter.ConvertFromString(colorStr)
        End If

        colorStr = pgData.Setting(ProgramData.TSetting.SecondaryAccentForegroundBrush)
        If colorStr IsNot Nothing Then
            ctheme.SecondaryMidForeground = ColorConverter.ConvertFromString(colorStr)
        End If
        ctheme.ApplyTheme()



        '세팅파일
        If pgData.Setting(ProgramData.TSetting.euddraft) = Nothing Then
            pgData.SaveSetting()
        End If



        Return True
    End Function

    Public Function ShutDownProgram() As Boolean
        'MsgBox("프로그램종료")
        Try
            pgData.SaveSetting()
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error SettingSave Fail"))
        End Try
        Return False '종료 허락
    End Function
End Module
