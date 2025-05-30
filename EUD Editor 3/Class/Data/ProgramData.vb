﻿Imports System.IO
Imports MaterialDesignThemes.Wpf

Public Class ProgramData
    '<주 버전>.<부 버전>.<빌드 번호>.<수정>
    Public ReadOnly Property Version As System.Version = Reflection.Assembly.GetExecutingAssembly().GetName().Version '"0.0.1"
    Public ReadOnly Property RecommendeuddraftVersion As New System.Version(0, 10, 1, 6)


    Public Sub SetLanguage(LanName As String)
        Dim dict As New ResourceDictionary()
        'Load the Language file corresponding to the user's OS, default to en-US
        Try
            dict.Source = New Uri("Language\" & LanName & ".xaml", UriKind.Relative)
        Catch ex As Exception
            dict.Source = New Uri("Language\en-US.xaml", UriKind.Relative)
            LanName = "en-US"
        End Try
        Application.Current.Resources.MergedDictionaries.Add(dict)
        Setting(ProgramData.TSetting.Language) = LanName
    End Sub



    '사용자 지정 컬러
    Public Property PFiledDefault As Color
    Public Property PFiledEditColor As Color
    Public Property PFiledMapEditColor As Color
    Public Property PFiledFlagColor As Color

    Private DarkBackGroundColor As Color = Color.FromRgb(48, 48, 48)
    Private LightBackGroundColor As Color = Color.FromRgb(255, 255, 255)


    Private Function colorConbersion(ARGB As Color, RGB As Color) As Color
        Dim alpha As Double = ARGB.A / 256
        Dim alphai As Double = (1 - alpha)

        Return Color.FromArgb(255, alphai * RGB.R + alpha * ARGB.R,
                             alphai * RGB.G + alpha * ARGB.G,
                             alphai * RGB.B + alpha * ARGB.B)
    End Function

    Public ReadOnly Property FiledDefault As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledDefault, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledDefault, LightBackGroundColor)
            End If
        End Get
    End Property
    Public ReadOnly Property FiledEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledEditColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledEditColor, LightBackGroundColor)
            End If
        End Get
    End Property

    Public ReadOnly Property FiledMapEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledMapEditColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledMapEditColor, LightBackGroundColor)
            End If
        End Get
    End Property
    Public ReadOnly Property FiledFalgColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledFlagColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledFlagColor, LightBackGroundColor)
            End If
        End Get
    End Property


    Public Property IsCompilng As Boolean
    Public Property isEddCompile As Boolean





    Private pgsetting As IniClass
    Public Sub New()
        pgsetting = New IniClass(Tool.GetSettingFile)

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.CDLanguageChange)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CDLanguageChange) = True
        End Try
        Try
            Dim uint As UInt32 = Setting(ProgramData.TSetting.CDLanguage)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CDLanguage) = 0
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.DataEditorTopMost)
        Catch ex As Exception
            Setting(ProgramData.TSetting.DataEditorTopMost) = False
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.CheckReg)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CheckReg) = True
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.TriggerEditrTopMost)
        Catch ex As Exception
            Setting(ProgramData.TSetting.TriggerEditrTopMost) = False
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.PluginSettingTopMost)
        Catch ex As Exception
            Setting(ProgramData.TSetting.PluginSettingTopMost) = False
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.CheckUpdate)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CheckUpdate) = False
        End Try

        If Not IsNumeric(Setting(ProgramData.TSetting.Graphic)) Then
            Setting(ProgramData.TSetting.Graphic) = 1
        End If

        Setting(ProgramData.TSetting.TEFontSize) = 16


        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.DonateMsg)
        Catch ex As Exception
            Setting(ProgramData.TSetting.DonateMsg) = False
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.MuteSound)
        Catch ex As Exception
            Setting(ProgramData.TSetting.MuteSound) = True
        End Try


        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.TestCodeEditorUse)
        Catch ex As Exception
            Setting(ProgramData.TSetting.TestCodeEditorUse) = False
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.TECEUseSmoothScrool)
        Catch ex As Exception
            Setting(ProgramData.TSetting.TECEUseSmoothScrool) = True
        End Try

        IsCompilng = False
        'Lan = New Language(Setting(TSetting.language))

    End Sub



    Public Enum TSetting
        euddraft = 0
        starcraft = 1
        Language = 2
        Theme = 3
        CDLanguage = 4

        PrimaryHueLightBrush = 5
        PrimaryHueLightForegroundBrush = 6
        PrimaryHueMidBrush = 7
        PrimaryHueMidForegroundBrush = 8
        PrimaryHueDarkBrush = 9

        PrimaryHueDarkForegroundBrush = 10
        SecondaryAccentBrush = 11
        SecondaryAccentForegroundBrush = 12
        DefaultData = 13
        MapEditorData = 14

        EditedData = 15
        CheckedData = 16
        CDLanguageChange = 17
        DataEditorTopMost = 18
        CheckReg = 19

        TriggerEditrTopMost = 20
        PluginSettingTopMost = 21
        CheckUpdate = 22
        Graphic = 23

        TEFontSize = 24
        SavePath = 25
        OpenPath = 26
        SaveMapPath = 27
        OpenMapPath = 28

        DonateMsg = 29

        MuteSound = 30
        TestCodeEditorUse = 31
        TECEUseSmoothScrool = 32
    End Enum
    Private settingstr() As String = {"euddraft.exe", "StarCraft.exe", "Lanuage", "Theme", "CDLanuage",
    "PrimaryHueLightBrush", "PrimaryHueLightForegroundBrush", "PrimaryHueMidBrush", "PrimaryHueMidForegroundBrush", "PrimaryHueDarkBrush",
    "PrimaryHueDarkForegroundBrush", "SecondaryAccentBrush", "SecondaryAccentForegroundBrush", "DefaultData", "MapEditorData",
    "EditedData", "CheckedData", "CDLanuageChange", "DataEditorTopMost", "CheckReg",
    "TriggerEditrTopMost", "PluginSettingTopMost", "CheckUpdate", "Graphic", "TEFontSize", "SavePath", "OpenPath", "SaveMapPath", "OpenMapPath", "DonateMsg", "MuteSound",
    "TestCodeEditorUse", "TECEUseSmoothScrool"}
    Public Property Setting(key As TSetting) As String
        Get
            Return pgsetting.SettingData(settingstr(key))
        End Get
        Set(value As String)
            pgsetting.SettingData(settingstr(key)) = value
        End Set
    End Property
    Public Sub SaveSetting()
        Setting(ProgramData.TSetting.DefaultData) = pgData.PFiledDefault.ToString()
        Setting(ProgramData.TSetting.MapEditorData) = pgData.PFiledMapEditColor.ToString()
        Setting(ProgramData.TSetting.EditedData) = pgData.PFiledEditColor.ToString()
        Setting(ProgramData.TSetting.CheckedData) = pgData.PFiledFlagColor.ToString()




        Setting(ProgramData.TSetting.PrimaryHueLightBrush) = ctheme.PrimaryHueLight.ToString
        Setting(ProgramData.TSetting.PrimaryHueMidBrush) = ctheme.PrimaryHueMid.ToString
        Setting(ProgramData.TSetting.PrimaryHueDarkBrush) = ctheme.PrimaryHueDark.ToString
        Setting(ProgramData.TSetting.PrimaryHueLightForegroundBrush) = ctheme.PrimaryHueLightForeground.ToString
        Setting(ProgramData.TSetting.PrimaryHueMidForegroundBrush) = ctheme.PrimaryHueMidForeground.ToString
        Setting(ProgramData.TSetting.PrimaryHueDarkForegroundBrush) = ctheme.PrimaryHueDarkForeground.ToString
        Setting(ProgramData.TSetting.SecondaryAccentBrush) = ctheme.SecondaryMid.ToString
        Setting(ProgramData.TSetting.SecondaryAccentForegroundBrush) = ctheme.SecondaryMidForeground.ToString




        pgsetting.WriteIni()
    End Sub
End Class
